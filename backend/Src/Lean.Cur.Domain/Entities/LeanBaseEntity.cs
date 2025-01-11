/**
 * @description 实体基类
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using SqlSugar;

namespace Lean.Cur.Domain.Entities;

/// <summary>
/// 实体基类
/// </summary>
/// <remarks>
/// 所有实体类的基类，包含通用字段
/// </remarks>
public abstract class LeanBaseEntity
{
  /// <summary>
  /// 主键ID
  /// </summary>
  [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", IsPrimaryKey = true, IsIdentity = true)]
  public long Id { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false)]
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 创建人ID
  /// </summary>
  [SugarColumn(ColumnName = "create_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false)]
  public long CreateBy { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  [SugarColumn(ColumnName = "update_time", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true)]
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 更新人ID
  /// </summary>
  [SugarColumn(ColumnName = "update_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? UpdateBy { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public string? Remark { get; set; }

  /// <summary>
  /// 是否删除(0:否,1:是)
  /// </summary>
  [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false)]
  public int IsDeleted { get; set; }
}