using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 角色菜单关联实体
/// </summary>
/// <remarks>
/// 用于维护角色和菜单之间的多对多关系
/// </remarks>
[SugarTable("lean_role_menu")]
public class LeanRoleMenu : LeanBaseEntity
{
    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID")]
    public long RoleId { get; set; }

    /// <summary>
    /// 菜单ID
    /// </summary>
    [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID")]
    public long MenuId { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public LeanRole? Role { get; set; }

    /// <summary>
    /// 菜单信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(MenuId))]
    public LeanMenu? Menu { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    /// <remarks>
    /// 1. 用于存储菜单的权限标识
    /// 2. 可选字段
    /// </remarks>
    [SugarColumn(ColumnName = "permission", ColumnDescription = "权限标识", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    [Description("权限标识")]
    public string? Permission { get; set; }
} 