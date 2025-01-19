using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// API接口权限控制特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LeanApiPermissionAttribute : AuthorizeAttribute
{
  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="permission">权限标识</param>
  public LeanApiPermissionAttribute(string permission)
  {
    Policy = $"Permission_{permission}";
  }
}