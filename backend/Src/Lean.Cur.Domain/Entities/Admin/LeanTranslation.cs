using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 翻译实体
/// </summary>
[SugarTable("lean_translation")]
public class LeanTranslation : LeanBaseEntity
{
  /// <summary>
  /// 语言ID
  /// </summary>
  [SugarColumn(ColumnDescription = "语言ID")]
  public long LanguageId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  [SugarColumn(ColumnDescription = "翻译键")]
  public string TransKey { get; set; } = null!;

  /// <summary>
  /// 翻译值
  /// </summary>
  [SugarColumn(ColumnDescription = "翻译值")]
  public string TransValue { get; set; } = null!;

  /// <summary>
  /// 模块
  /// </summary>
  [SugarColumn(ColumnDescription = "模块")]
  public string Module { get; set; } = null!;

  /// <summary>
  /// 状态
  /// </summary>
  [SugarColumn(ColumnDescription = "状态")]
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 语言
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(LanguageId))]
  public LeanLanguage? Language { get; set; }
}