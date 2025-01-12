using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限要求
/// </summary>
public class LeanPermissionRequirement : IAuthorizationRequirement
{
  /// <summary>
  /// 权限标识
  /// </summary>
  public string Permission { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="permission">权限标识</param>
  public LeanPermissionRequirement(string permission)
  {
    Permission = permission;
  }
}