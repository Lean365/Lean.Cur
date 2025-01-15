using NLog;
using NLog.Web;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Infrastructure.Interceptors;
using Lean.Cur.Infrastructure.Services.Logging;

// 初始化NLog
var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

try
{
  logger.Info("初始化应用程序...");

  var builder = WebApplication.CreateBuilder(args);

  // 使用NLog作为日志提供程序
  builder.Logging.ClearProviders();
  builder.Host.UseNLog();

  #region 配置文件加载

  // 加载所有配置文件，按照以下顺序：
  // 1. appsettings.json - 基础配置文件
  // 2. appsettings.{环境}.json - 环境特定配置
  // 3. Configs目录下的专项配置文件
  builder.Configuration
      .SetBasePath(builder.Environment.ContentRootPath)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
      .AddJsonFile("Configs/database.json", optional: false, reloadOnChange: true)      // 数据库配置
      .AddJsonFile("Configs/logging.json", optional: false, reloadOnChange: true)       // 日志配置
      .AddJsonFile("Configs/authentication.json", optional: false, reloadOnChange: true) // 身份认证配置
      .AddJsonFile("Configs/cache.json", optional: false, reloadOnChange: true)         // 缓存配置
      .AddJsonFile("Configs/security.json", optional: false, reloadOnChange: true)      // 安全配置
      .AddJsonFile("Configs/excel.json", optional: false, reloadOnChange: true)         // Excel配置
      .AddJsonFile("Configs/cors.json", optional: false, reloadOnChange: true)          // 跨域配置
      .AddEnvironmentVariables();                                                       // 环境变量

  #endregion 配置文件加载

  #region Excel配置

  // 配置EPPlus许可证类型（商业版或非商业版）
  var excelOptions = builder.Configuration.GetSection("Excel").Get<LeanExcelOptions>();
  if (excelOptions == null)
  {
    excelOptions = new LeanExcelOptions
    {
      LicenseContext = LicenseContext.NonCommercial
    };
  }
  ExcelPackage.LicenseContext = excelOptions.LicenseContext;

  // 注册 Excel 配置选项
  builder.Services.AddSingleton(excelOptions);

  // 配置Excel操作相关服务
  builder.Services.AddScoped<LeanExcelHelper>();

  #endregion Excel配置

  #region 日志配置

  // 配置NLog作为日志提供程序
  LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
  builder.Host.UseNLog();

  #endregion 日志配置

  #region 基础服务配置

  // 添加MVC控制器并配置全局过滤器
  builder.Services.AddControllers(options =>
  {
    options.Filters.Add<ApiResultFilter>();  // 添加API统一返回格式过滤器
  });

  // 配置Swagger/OpenAPI文档
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  // 注册HTTP上下文访问器和IP地理位置解析服务
  builder.Services.AddHttpContextAccessor();
  builder.Services.AddSingleton<Ip2RegionHelper>();

  #endregion 基础服务配置

  #region 数据库配置

  // 配置SqlSugar数据库服务
  builder.Services.AddDatabase(builder.Configuration);

  // 注册SQL差异日志服务
  builder.Services.AddSingleton<SqlLogInterceptor>();
  builder.Services.AddScoped<ISqlLogService, SqlLogService>();

  #endregion 数据库配置

  #region Excel服务配置

  // 配置Excel操作相关服务
  builder.Services.Configure<LeanExcelOptions>(builder.Configuration.GetSection("Excel"));
  builder.Services.AddScoped<LeanExcelHelper>();

  #endregion Excel服务配置

  #region 认证和授权配置

  // 配置JWT和滑动验证码选项
  builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
  builder.Services.Configure<SlideVerifyOptions>(builder.Configuration.GetSection("SlideVerify"));

  // 注册认证相关服务
  builder.Services.AddScoped<JwtHelper>();
  builder.Services.AddScoped<SlideVerifyHelper>();
  builder.Services.AddScoped<IAuthService, AuthService>();

  // 配置JWT Bearer认证
  var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
  builder.Services
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,           // 验证颁发者
          ValidateAudience = true,         // 验证接收者
          ValidateLifetime = true,         // 验证过期时间
          ValidateIssuerSigningKey = true, // 验证签名密钥
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
      });

  // 配置授权策略
  builder.Services.AddAuthorization(options =>
  {
    options.AddPolicy("RequireAuthenticated", policy =>
        policy.RequireAuthenticatedUser());
  });

  #endregion 认证和授权配置

  #region 缓存配置

  // 根据配置决定使用Redis缓存还是内存缓存
  var cacheSettings = builder.Configuration.GetSection("LeanCacheSettings").Get<LeanCacheSettings>();
  if (cacheSettings?.UseRedis == true && cacheSettings.Redis != null)
  {
    // 配置Redis分布式缓存
    builder.Services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = cacheSettings.Redis.ConnectionString;
      options.InstanceName = "Lean_";
    });

    // 注册Redis连接复用器
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        ConnectionMultiplexer.Connect(cacheSettings.Redis.ConnectionString));

    // 使用Redis实现的缓存服务
    builder.Services.AddScoped<ILeanCache, LeanRedisCache>();
  }
  else
  {
    // 使用内存缓存
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<ILeanCache, LeanMemoryCache>();
  }

  #endregion 缓存配置

  #region 安全配置

  // 配置密码策略等安全选项
  var securitySettings = builder.Configuration.GetSection("LeanSecuritySettings").Get<LeanSecuritySettings>();
  if (securitySettings != null)
  {
    builder.Services.Configure<LeanSecuritySettings>(builder.Configuration.GetSection("LeanSecuritySettings"));

    // 配置密码复杂度要求
    builder.Services.Configure<Microsoft.AspNetCore.Identity.PasswordOptions>(options =>
    {
      options.RequiredLength = 8;            // 最小长度
      options.RequireDigit = true;          // 要求数字
      options.RequireLowercase = true;      // 要求小写字母
      options.RequireUppercase = true;      // 要求大写字母
      options.RequireNonAlphanumeric = true; // 要求特殊字符
    });
  }

  #endregion 安全配置

  #region 依赖注入服务注册

  // 注册通用仓储
  builder.Services.AddScoped(typeof(ILeanRepository<>), typeof(LeanRepository<>));
  // 注册所有业务服务
  builder.Services.RegisterServices();

  #endregion 依赖注入服务注册

  // 注册审计日志服务
  builder.Services.AddScoped<IAuditLogService, AuditLogService>();

  // 构建应用程序
  var app = builder.Build();

  // 初始化数据库
  using (var scope = app.Services.CreateScope())
  {
    scope.ServiceProvider.InitializeDatabase();
  }

  #region 中间件配置

  // 开发环境启用Swagger
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  // 全局异常处理
  app.UseMiddleware<ExceptionHandlingMiddleware>();
  // HTTPS重定向
  app.UseHttpsRedirection();

  // 配置CORS（跨域资源共享）
  var corsSettings = app.Configuration.GetSection("LeanCorsSettings").Get<LeanCorsSettings>();
  if (corsSettings?.Enabled == true)
  {
    app.UseCors(builder =>
    {
      var corsBuilder = builder
            .WithOrigins(corsSettings.Origins.ToArray())      // 允许的来源
            .WithMethods(corsSettings.Methods.ToArray())      // 允许的HTTP方法
            .WithHeaders(corsSettings.Headers.ToArray());     // 允许的请求头

      // 配置暴露的响应头
      if (corsSettings.ExposedHeaders?.Any() == true)
      {
        corsBuilder.WithExposedHeaders(corsSettings.ExposedHeaders.ToArray());
      }

      // 配置是否允许携带凭证
      if (corsSettings.AllowCredentials)
      {
        corsBuilder.AllowCredentials();
      }
    });
  }

  // 启用身份认证
  app.UseAuthentication();
  // 启用授权
  app.UseAuthorization();

  // 配置路由
  app.MapControllers();

  // 配置安全头
  app.Use(async (context, next) =>
  {
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data: https:; " +
        "font-src 'self' data:; " +
        "connect-src 'self'";

    await next();
  });

  #endregion 中间件配置

  // 启动应用程序
  app.Run();
}
catch (Exception ex)
{
  logger.Error(ex, "程序因未处理的异常而停止");
  throw;
}
finally
{
  LogManager.Shutdown();
}