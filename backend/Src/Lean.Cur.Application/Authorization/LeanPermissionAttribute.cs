using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限特性
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class LeanPermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="permission">权限标识</param>
    public LeanPermissionAttribute(string permission)
    {
        Policy = permission;
    }
} 