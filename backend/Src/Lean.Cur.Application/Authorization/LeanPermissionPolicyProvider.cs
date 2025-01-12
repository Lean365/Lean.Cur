using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限策略提供者
/// </summary>
public class LeanPermissionPolicyProvider : IAuthorizationPolicyProvider
{
  private readonly AuthorizationOptions _options;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="options">授权选项</param>
  public LeanPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
  {
    _options = options.Value;
  }

  /// <summary>
  /// 获取默认策略
  /// </summary>
  public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
  {
    return Task.FromResult(_options.DefaultPolicy);
  }

  /// <summary>
  /// 获取回退策略
  /// </summary>
  public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
  {
    return Task.FromResult<AuthorizationPolicy?>(_options.FallbackPolicy);
  }

  /// <summary>
  /// 根据策略名称获取策略
  /// </summary>
  /// <param name="policyName">策略名称</param>
  public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
  {
    // 如果策略已经存在，直接返回
    var policy = _options.GetPolicy(policyName);
    if (policy != null)
    {
      return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    // 创建新的策略
    policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new LeanPermissionRequirement(policyName))
        .Build();

    // 添加到选项中
    _options.AddPolicy(policyName, policy);

    return Task.FromResult<AuthorizationPolicy?>(policy);
  }
}