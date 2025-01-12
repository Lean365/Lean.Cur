using SqlSugar;

namespace Lean.Cur.Domain.Entities;

/// <summary>
/// 实体基类
/// </summary>
/// <remarks>
/// 所有实体类的基类，包含通用字段
///
/// 数据库映射说明：
/// 1. 包含所有实体的基础字段
/// 2. 主键：Id (自增长)
/// 3. 通用字段：创建时间、创建人、更新时间、更新人、备注、是否删除等
/// 
/// 业务规则：
/// 1. 创建时间和创建人必填
/// 2. 更新时间和更新人在修改时必填
/// 3. 删除时间和删除人在删除时必填
/// 4. 默认未删除状态
/// </remarks>
/// <author>CodeGenerator</author>
/// <date>2024-01-17</date>
/// <version>1.0.0</version>
/// <copyright>© 2024 Lean. All rights reserved</copyright>
public abstract class LeanBaseEntity
{
  /// <summary>
  /// 主键ID
  /// </summary>
  [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", IsPrimaryKey = true, IsIdentity = true)]
  public long Id { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：最大500个字符
  /// 2. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public string? Remark { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  /// <remarks>
  /// 1. 记录创建的时间戳
  /// 2. 必填字段
  /// 3. 默认为当前时间
  /// </remarks>
  [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false)]
  public DateTime CreateTime { get; set; } = DateTime.Now;

  /// <summary>
  /// 创建人ID
  /// </summary>
  /// <remarks>
  /// 1. 记录创建人的用户ID
  /// 2. 必填字段
  /// 3. 默认为0（系统创建）
  /// </remarks>
  [SugarColumn(ColumnName = "create_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false)]
  public long CreateBy { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  /// <remarks>
  /// 1. 记录最后一次更新的时间戳
  /// 2. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "update_time", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true)]
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 更新人ID
  /// </summary>
  /// <remarks>
  /// 1. 记录最后一次更新人的用户ID
  /// 2. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "update_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? UpdateBy { get; set; }

  /// <summary>
  /// 是否删除
  /// </summary>
  /// <remarks>
  /// 1. 0：未删除
  /// 2. 1：已删除
  /// 3. 必填字段
  /// 4. 默认为0（未删除）
  /// </remarks>
  [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false)]
  public int IsDeleted { get; set; } = 0;

  /// <summary>
  /// 删除时间
  /// </summary>
  /// <remarks>
  /// 1. 记录删除的时间戳
  /// 2. 可选字段
  /// 3. 仅在IsDeleted=1时有值
  /// </remarks>
  [SugarColumn(ColumnName = "delete_time", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true)]
  public DateTime? DeleteTime { get; set; }

  /// <summary>
  /// 删除人ID
  /// </summary>
  /// <remarks>
  /// 1. 记录删除人的用户ID
  /// 2. 可选字段
  /// 3. 仅在IsDeleted=1时有值
  /// </remarks>
  [SugarColumn(ColumnName = "delete_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? DeleteBy { get; set; }
}