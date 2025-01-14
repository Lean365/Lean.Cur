using Lean.Cur.Application.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限验证处理器
/// </summary>
/// <remarks>
/// 实现具体的权限验证逻辑
/// 通过用户服务获取用户的菜单权限列表
/// 判断用户是否拥有指定的权限
/// </remarks>
public class LeanPermissionHandler : AuthorizationHandler<LeanPermissionRequirement>
{
    private readonly ILeanUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userService">用户服务</param>
    /// <param name="httpContextAccessor">HTTP上下文访问器</param>
    public LeanPermissionHandler(ILeanUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 处理权限验证
    /// </summary>
    /// <param name="context">授权上下文</param>
    /// <param name="requirement">权限要求</param>
    /// <returns>验证结果</returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LeanPermissionRequirement requirement)
    {
        // 获取当前用户ID
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return;
        }

        // 获取用户菜单权限列表
        var permissions = await _userService.GetUserMenuPermissionsAsync(long.Parse(userId));
        if (permissions == null || !permissions.Any())
        {
            return;
        }

        // 判断用户是否拥有指定权限
        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
} 