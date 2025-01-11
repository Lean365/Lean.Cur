/**
 * @description 部门实体类
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 部门实体类
/// </summary>
/// <remarks>
/// 该实体类映射到数据库表 lean_dept，用于存储部门相关数据
/// 
/// 数据库映射说明：
/// 1. 表名：lean_dept
/// 2. 主键：Id (自增长)
/// 3. 基础字段：继承自LeanBaseEntity，包含创建时间、创建人、更新时间、更新人等
/// </remarks>
[SugarTable("lean_dept", TableDescription = "部门表")]
public class LeanDept : LeanBaseEntity
{
  /// <summary>
  /// 部门名称
  /// </summary>
  [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string DeptName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [SugarColumn(ColumnName = "english_name", ColumnDescription = "英文名称", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 父部门ID
  /// </summary>
  [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父部门ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? ParentId { get; set; }

  /// <summary>
  /// 祖级列表
  /// </summary>
  [SugarColumn(ColumnName = "ancestors", ColumnDescription = "祖级列表", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public string? Ancestors { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [SugarColumn(ColumnName = "ordernum", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  [SugarColumn(ColumnName = "leader", ColumnDescription = "负责人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [SugarColumn(ColumnName = "phone", ColumnDescription = "联系电话", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  public string? Email { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  public Status Status { get; set; }

  /// <summary>
  /// 部门用户关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserDept.DeptId))]
  public virtual List<LeanUser> Users { get; set; } = new();

  /// <summary>
  /// 子部门
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(ParentId))]
  public virtual List<LeanDept> Children { get; set; } = new();

  /// <summary>
  /// 父部门
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(ParentId))]
  public virtual LeanDept? Parent { get; set; }
}