using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 字典数据实体
/// </summary>
[SugarTable("lean_dict_data")]
public class LeanDictData : LeanBaseEntity
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  [SugarColumn(ColumnDescription = "字典类型ID")]
  public long DictTypeId { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  [SugarColumn(ColumnDescription = "字典标签")]
  public string DictLabel { get; set; } = null!;

  /// <summary>
  /// 字典键值
  /// </summary>
  [SugarColumn(ColumnDescription = "字典键值")]
  public string DictValue { get; set; } = null!;

  /// <summary>
  /// 样式类型
  /// </summary>
  [SugarColumn(ColumnDescription = "样式类型")]
  public string? CssClass { get; set; }

  /// <summary>
  /// 是否默认(0:否,1:是)
  /// </summary>
  [SugarColumn(ColumnDescription = "是否默认(0:否,1:是)")]
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

  /// <summary>
  /// 字典类型
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(DictTypeId))]
  public LeanDictType? DictType { get; set; }
}