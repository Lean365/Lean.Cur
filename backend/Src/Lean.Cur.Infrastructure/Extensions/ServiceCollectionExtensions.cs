using Lean.Cur.Application.Services;
using Lean.Cur.Application.Services.Impl;
using Lean.Cur.Domain.Repositories;
using Lean.Cur.Infrastructure.Cache;
using Lean.Cur.Infrastructure.Database;
using Lean.Cur.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lean.Cur.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<LeanDbContext>();
    services.AddScoped(sp => sp.GetRequiredService<LeanDbContext>().GetDatabase());

    services.AddScoped(typeof(ILeanRepository<>), typeof(LeanBaseRepository<>));
    services.AddScoped<ILeanUserRepository, LeanUserRepository>();
    services.AddScoped<ILeanRoleRepository, LeanRoleRepository>();
    services.AddScoped<ILeanPermissionRepository, LeanPermissionRepository>();

    services.AddMemoryCache();
    services.AddSingleton<ILeanCache, LeanMemoryCache>();

    services.AddScoped<ILeanAuthService, LeanAuthService>();
    services.AddScoped<ILeanPermissionService, LeanPermissionService>();

    return services;
  }
}