using Microsoft.AspNetCore.Authorization;
using Lean.Cur.Common.Configs;
using Lean.Cur.Application.Services.Admin;
using Microsoft.Extensions.Options;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限处理器
/// </summary>
public class LeanPermissionHandler : IAuthorizationHandler
{
  private readonly ILeanUserService _userService;
  private readonly LeanSecuritySettings _securitySettings;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="userService">用户服务</param>
  /// <param name="securitySettings">安全配置</param>
  public LeanPermissionHandler(ILeanUserService userService, IOptions<LeanSecuritySettings> securitySettings)
  {
    _userService = userService;
    _securitySettings = securitySettings.Value;
  }

  /// <summary>
  /// 处理权限验证
  /// </summary>
  /// <param name="context">授权处理上下文</param>
  public async Task HandleAsync(AuthorizationHandlerContext context)
  {
    // 获取当前用户ID
    var userId = context.User.FindFirst("UserId")?.Value;
    if (string.IsNullOrEmpty(userId))
    {
      return;
    }

    // 获取用户角色编码
    var roleCode = await _userService.GetUserRoleCodeAsync(long.Parse(userId));
    if (string.IsNullOrEmpty(roleCode))
    {
      return;
    }

    // 如果是超级管理员，直接通过所有权限验证
    if (roleCode == _securitySettings.Permission?.SuperAdminRoleCode)
    {
      foreach (var requirement in context.PendingRequirements)
      {
        context.Succeed(requirement);
      }
      return;
    }

    // 获取用户权限列表
    var permissions = await _userService.GetUserPermissionsAsync(long.Parse(userId));
    if (permissions == null || !permissions.Any())
    {
      return;
    }

    // 验证每个权限要求
    foreach (var requirement in context.PendingRequirements)
    {
      if (requirement is IAuthorizationRequirement)
      {
        // 获取权限策略名称（即权限标识）
        var policy = context.Resource as string;
        if (string.IsNullOrEmpty(policy))
        {
          continue;
        }

        // 检查用户是否拥有该权限
        if (permissions.Contains(policy))
        {
          context.Succeed(requirement);
        }
      }
    }
  }
}