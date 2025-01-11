/**
 * @description 权限过滤器
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Api.Filters;

/// <summary>
/// 权限检查特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LeanPermissionAttribute : Attribute
{
  /// <summary>
  /// 权限编码
  /// </summary>
  public string Permission { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="permission">权限编码</param>
  public LeanPermissionAttribute(string permission)
  {
    Permission = permission;
  }
}

/// <summary>
/// 权限过滤器
/// </summary>
public class LeanPermissionFilter : IAsyncAuthorizationFilter
{
  private readonly ILogger<LeanPermissionFilter> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanPermissionFilter(ILogger<LeanPermissionFilter> logger, IHttpContextAccessor httpContextAccessor)
  {
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 权限检查
  /// </summary>
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    try
    {
      // 获取当前请求的权限特性
      var permissionAttribute = context.ActionDescriptor.EndpointMetadata
          .OfType<LeanPermissionAttribute>()
          .FirstOrDefault();

      if (permissionAttribute == null)
      {
        return;
      }

      // 获取当前用户信息
      var userType = GetCurrentUserType();

      // 超级管理员拥有所有权限
      if (userType == UserType.SuperAdmin)
      {
        return;
      }

      // TODO: 从数据库或缓存中获取用户权限列表
      var hasPermission = await CheckUserPermission(permissionAttribute.Permission);

      if (!hasPermission)
      {
        context.Result = new ForbidResult();
        _logger.LogWarning($"用户无权访问: {permissionAttribute.Permission}");
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "权限检查失败");
      context.Result = new StatusCodeResult(500);
    }
  }

  private UserType GetCurrentUserType()
  {
    // TODO: 从Token或Session中获取用户类型
    return UserType.User;
  }

  private Task<bool> CheckUserPermission(string permission)
  {
    // TODO: 实现权限检查逻辑
    return Task.FromResult(true);
  }
}