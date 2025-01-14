using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

/// <summary>
/// 权限验证要求
/// </summary>
/// <remarks>
/// 定义权限验证的具体要求
/// 包含需要验证的权限标识
/// </remarks>
public class LeanPermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// 权限标识
    /// </summary>
    public string Permission { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="permission">权限标识，对应菜单的Perms字段</param>
    public LeanPermissionRequirement(string permission)
    {
        Permission = permission;
    }
} 