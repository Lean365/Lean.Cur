/**
 * @description 岗位实体类
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 岗位实体类
/// </summary>
/// <remarks>
/// 该实体类映射到数据库表 lean_position，用于存储岗位相关数据
/// 
/// 数据库映射说明：
/// 1. 表名：lean_position
/// 2. 主键：Id (自增长)
/// 3. 基础字段：继承自LeanBaseEntity，包含创建时间、创建人、更新时间、更新人等
/// </remarks>
[SugarTable("lean_position", TableDescription = "岗位表")]
public class LeanPosition : LeanBaseEntity
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [SugarColumn(ColumnName = "position_name", ColumnDescription = "岗位名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string PositionName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [SugarColumn(ColumnName = "english_name", ColumnDescription = "英文名称", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 岗位编码
  /// </summary>
  [SugarColumn(ColumnName = "position_code", ColumnDescription = "岗位编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  public string PositionCode { get; set; } = null!;

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
  /// 备注
  /// </summary>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public new string? Remark { get; set; }

  /// <summary>
  /// 岗位用户关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserPosition.PositionId))]
  public virtual List<LeanUser> Users { get; set; } = new();
}