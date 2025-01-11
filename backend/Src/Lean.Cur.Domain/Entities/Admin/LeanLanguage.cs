using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 语言实体
/// </summary>
[SugarTable("lean_language")]
public class LeanLanguage : LeanBaseEntity
{
  /// <summary>
  /// 语言名称
  /// </summary>
  [SugarColumn(ColumnDescription = "语言名称")]
  public string LanguageName { get; set; } = null!;

  /// <summary>
  /// 语言代码
  /// </summary>
  [SugarColumn(ColumnDescription = "语言代码")]
  public string LanguageCode { get; set; } = null!;

  /// <summary>
  /// 是否默认语言(0:否,1:是)
  /// </summary>
  [SugarColumn(ColumnDescription = "是否默认语言(0:否,1:是)")]
  public int IsDefault { get; set; }

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