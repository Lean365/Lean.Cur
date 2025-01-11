/**
 * @description 角色实体类
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 角色实体类
/// </summary>
/// <remarks>
/// 该实体类映射到数据库表 lean_role，用于存储角色相关数据
/// 
/// 数据库映射说明：
/// 1. 表名：lean_role
/// 2. 主键：Id (自增长)
/// 3. 基础字段：继承自LeanBaseEntity，包含创建时间、创建人、更新时间、更新人等
/// </remarks>
[SugarTable("lean_role", TableDescription = "角色表")]
public class LeanRole : LeanBaseEntity
{
  /// <summary>
  /// 角色名称
  /// </summary>
  [SugarColumn(ColumnName = "role_name", ColumnDescription = "角色名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string RoleName { get; set; } = null!;

  /// <summary>
  /// 角色编码
  /// </summary>
  [SugarColumn(ColumnName = "role_code", ColumnDescription = "角色编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  public string RoleCode { get; set; } = null!;

  /// <summary>
  /// 显示顺序
  /// </summary>
  [SugarColumn(ColumnName = "ordernum", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  public LeanStatus Status { get; set; } = LeanStatus.Normal;

  /// <summary>
  /// 角色权限关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanRolePermission.RoleId))]
  public virtual List<LeanPermission> Permissions { get; set; } = new();

  /// <summary>
  /// 角色用户关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserRole.RoleId))]
  public virtual List<LeanUser> Users { get; set; } = new();
}