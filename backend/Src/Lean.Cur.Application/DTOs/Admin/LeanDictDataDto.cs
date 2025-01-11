using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 字典数据查询DTO
/// </summary>
public class LeanDictDataQueryDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  public long? DictTypeId { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  public string? DictLabel { get; set; }

  /// <summary>
  /// 字典键值
  /// </summary>
  public string? DictValue { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus? Status { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 字典数据DTO
/// </summary>
public class LeanDictDataDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 字典类型ID
  /// </summary>
  public long DictTypeId { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  public string DictLabel { get; set; } = null!;

  /// <summary>
  /// 字典键值
  /// </summary>
  public string DictValue { get; set; } = null!;

  /// <summary>
  /// 样式类型
  /// </summary>
  public string? CssClass { get; set; }

  /// <summary>
  /// 是否默认(0:否,1:是)
  /// </summary>
  public int IsDefault { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 字典类型名称
  /// </summary>
  public string? DictTypeName { get; set; }

  /// <summary>
  /// 字典类型
  /// </summary>
  public string? DictType { get; set; }
}

/// <summary>
/// 字典数据创建DTO
/// </summary>
public class LeanDictDataCreateDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  public long DictTypeId { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  public string DictLabel { get; set; } = null!;

  /// <summary>
  /// 字典键值
  /// </summary>
  public string DictValue { get; set; } = null!;

  /// <summary>
  /// 样式类型
  /// </summary>
  public string? CssClass { get; set; }

  /// <summary>
  /// 是否默认(0:否,1:是)
  /// </summary>
  public int IsDefault { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }
}

/// <summary>
/// 字典数据更新DTO
/// </summary>
public class LeanDictDataUpdateDto : LeanDictDataCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}