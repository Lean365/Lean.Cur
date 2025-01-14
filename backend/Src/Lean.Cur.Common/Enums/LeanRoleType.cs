using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 角色类型枚举
/// </summary>
public enum LeanRoleType
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [Description("系统角色")]
    System = 1,

    /// <summary>
    /// 自定义角色
    /// </summary>
    [Description("自定义角色")]
    Custom = 2
} 