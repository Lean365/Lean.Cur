using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 数据范围枚举
/// </summary>
public enum LeanDataScope
{
    /// <summary>
    /// 全部数据权限
    /// </summary>
    [Description("全部数据权限")]
    All = 1,

    /// <summary>
    /// 自定义数据权限
    /// </summary>
    [Description("自定义数据权限")]
    Custom = 2,

    /// <summary>
    /// 部门数据权限
    /// </summary>
    [Description("部门数据权限")]
    Department = 3,

    /// <summary>
    /// 部门及以下数据权限
    /// </summary>
    [Description("部门及以下数据权限")]
    DepartmentAndBelow = 4,

    /// <summary>
    /// 仅本人数据权限
    /// </summary>
    [Description("仅本人数据权限")]
    Self = 5
} 