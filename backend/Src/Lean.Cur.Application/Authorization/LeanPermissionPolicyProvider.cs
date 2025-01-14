using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限策略提供器
/// </summary>
/// <remarks>
/// 根据权限特性创建权限策略
/// 支持动态添加权限策略
/// </remarks>
public class LeanPermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">授权选项</param>
    public LeanPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    /// <summary>
    /// 获取默认策略
    /// </summary>
    /// <returns>默认策略</returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    /// <summary>
    /// 获取回退策略
    /// </summary>
    /// <returns>回退策略</returns>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    /// <summary>
    /// 根据策略名称获取策略
    /// </summary>
    /// <param name="policyName">策略名称，对应菜单的Perms字段</param>
    /// <returns>权限策略</returns>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // 创建权限要求
        var requirement = new LeanPermissionRequirement(policyName);

        // 创建权限策略
        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(requirement);

        return Task.FromResult<AuthorizationPolicy?>(policy.Build());
    }
} 