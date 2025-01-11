using Lean.Cur.Api.Middlewares;
using Lean.Cur.Application.Authorization;
using Lean.Cur.Infrastructure.Database;
using Lean.Cur.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lean.Cur.Common.Configs;
using Lean.Cur.Api.Filters;
using LeanPermissionAttribute = Lean.Cur.Application.Authorization.LeanPermissionAttribute;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

// 添加配置
var securitySettings = builder.Configuration.GetSection("SecuritySettings").Get<LeanSecuritySettings>();
builder.Services.Configure<LeanSecuritySettings>(builder.Configuration.GetSection("SecuritySettings"));

// 添加 CORS 服务
if (securitySettings?.Cors?.Enabled == true)
{
  builder.Services.AddCors(options =>
  {
    options.AddPolicy("CorsPolicy", policy =>
    {
      var corsSettings = securitySettings.Cors;
      policy.WithOrigins(corsSettings.Origins)
            .WithMethods(corsSettings.Methods)
            .WithHeaders(corsSettings.Headers);

      if (corsSettings.AllowCredentials)
      {
        policy.AllowCredentials();
      }
      else
      {
        policy.DisallowCredentials();
      }
    });
  });
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
      };
    });

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy(LeanPermissionAttribute.POLICY_PREFIX, policy =>
      policy.Requirements.Add(new LeanPermissionRequirement("")));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 添加内存缓存
builder.Services.AddMemoryCache();

// 添加防伪服务
if (securitySettings?.AntiForgery.Enabled == true)
{
  builder.Services.AddAntiforgery(options =>
  {
    options.HeaderName = securitySettings.AntiForgery.HeaderName;
    options.Cookie.Name = securitySettings.AntiForgery.CookieName;
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
  });
}

// 添加过滤器
if (securitySettings?.Permission.Enabled == true)
{
  builder.Services.AddScoped<LeanPermissionFilter>();
}

if (securitySettings?.DataScope.Enabled == true)
{
  builder.Services.AddScoped<LeanDataScopeFilter>();
}

var app = builder.Build();

// 初始化数据库
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<LeanDbContext>();
  dbContext.InitDatabase();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// 添加全局异常处理中间件
app.UseMiddleware<LeanGlobalExceptionMiddleware>();

// 启用 CORS
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// 添加操作日志中间件
app.UseMiddleware<LeanOperationLogMiddleware>();

// 添加安全中间件
if (securitySettings?.RateLimit?.IpRateLimit != null || securitySettings?.RateLimit?.UserRateLimit != null)
{
  app.UseLeanRateLimit();
}

if (securitySettings?.AntiForgery?.Enabled == true)
{
  app.UseLeanAntiForgeryMiddleware();
}

if (securitySettings?.SqlInjection?.Enabled == true)
{
  app.UseMiddleware<LeanSqlInjectionMiddleware>();
}

app.MapControllers();

app.Run();
