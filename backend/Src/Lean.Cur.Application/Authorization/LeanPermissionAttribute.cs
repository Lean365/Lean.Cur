using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限控制特性
/// </summary>
/// <remarks>
/// 用于标记需要进行权限验证的API方法
/// 权限标识对应菜单的Perms字段
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class LeanPermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="permission">权限标识，对应菜单的Perms字段</param>
    public LeanPermissionAttribute(string permission) : base(permission)
    {
    }
} 