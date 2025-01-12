using Microsoft.Extensions.DependencyInjection;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Application.Services.Auth;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Infrastructure.Services.Admin;
using Lean.Cur.Infrastructure.Services.Auth;
using Lean.Cur.Infrastructure.Services.Logging;

namespace Lean.Cur.Infrastructure.Extensions;

/// <summary>
/// 服务集合扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
  /// <summary>
  /// 注册所有业务服务
  /// </summary>
  /// <param name="services">服务集合</param>
  /// <returns>服务集合</returns>
  public static IServiceCollection RegisterServices(this IServiceCollection services)
  {
    // 认证服务
    services.AddScoped<IAuthService, AuthService>();

    // 用户服务
    services.AddScoped<ILeanUserService, LeanUserService>();

    // 日志服务
    services.AddScoped<ILoginLogService, LoginLogService>();

    return services;
  }
}