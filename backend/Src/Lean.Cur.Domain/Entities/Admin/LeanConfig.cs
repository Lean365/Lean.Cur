using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 系统配置实体
/// </summary>
[SugarTable("lean_config")]
public class LeanConfig : LeanBaseEntity
{
  /// <summary>
  /// 配置键
  /// </summary>
  [SugarColumn(ColumnDescription = "配置键")]
  public string ConfigKey { get; set; } = null!;

  /// <summary>
  /// 配置值
  /// </summary>
  [SugarColumn(ColumnDescription = "配置值")]
  public string ConfigValue { get; set; } = null!;

  /// <summary>
  /// 配置组
  /// </summary>
  [SugarColumn(ColumnDescription = "配置组")]
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 是否系统内置(0:否,1:是)
  /// </summary>
  [SugarColumn(ColumnDescription = "是否系统内置(0:否,1:是)")]
  public int IsSystem { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  [SugarColumn(ColumnDescription = "排序号")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [SugarColumn(ColumnDescription = "状态")]
  public LeanStatus Status { get; set; }
}