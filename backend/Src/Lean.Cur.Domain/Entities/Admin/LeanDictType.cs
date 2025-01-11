using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 字典类型实体
/// </summary>
[SugarTable("lean_dict_type")]
public class LeanDictType : LeanBaseEntity
{
  /// <summary>
  /// 字典名称
  /// </summary>
  [SugarColumn(ColumnDescription = "字典名称")]
  public string DictName { get; set; } = null!;

  /// <summary>
  /// 字典类型
  /// </summary>
  [SugarColumn(ColumnDescription = "字典类型")]
  public string DictType { get; set; } = null!;

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