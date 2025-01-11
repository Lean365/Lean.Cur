using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 字典类型查询DTO
/// </summary>
public class LeanDictTypeQueryDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  public string? DictName { get; set; }

  /// <summary>
  /// 字典类型
  /// </summary>
  public string? DictType { get; set; }

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
/// 字典类型DTO
/// </summary>
public class LeanDictTypeDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 字典名称
  /// </summary>
  public string DictName { get; set; } = null!;

  /// <summary>
  /// 字典类型
  /// </summary>
  public string DictType { get; set; } = null!;

  /// <summary>
  /// 是否系统内置(0:否,1:是)
  /// </summary>
  public int IsSystem { get; set; }

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
}

/// <summary>
/// 字典类型创建DTO
/// </summary>
public class LeanDictTypeCreateDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  public string DictName { get; set; } = null!;

  /// <summary>
  /// 字典类型
  /// </summary>
  public string DictType { get; set; } = null!;

  /// <summary>
  /// 是否系统内置(0:否,1:是)
  /// </summary>
  public int IsSystem { get; set; }

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
/// 字典类型更新DTO
/// </summary>
public class LeanDictTypeUpdateDto : LeanDictTypeCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}