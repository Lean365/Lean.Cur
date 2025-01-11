using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 语言查询DTO
/// </summary>
public class LeanLanguageQueryDto
{
  /// <summary>
  /// 语言名称
  /// </summary>
  public string? LanguageName { get; set; }

  /// <summary>
  /// 语言代码
  /// </summary>
  public string? LanguageCode { get; set; }

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
/// 语言DTO
/// </summary>
public class LeanLanguageDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 语言名称
  /// </summary>
  public string LanguageName { get; set; } = null!;

  /// <summary>
  /// 语言代码
  /// </summary>
  public string LanguageCode { get; set; } = null!;

  /// <summary>
  /// 是否默认语言(0:否,1:是)
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
}

/// <summary>
/// 语言创建DTO
/// </summary>
public class LeanLanguageCreateDto
{
  /// <summary>
  /// 语言名称
  /// </summary>
  public string LanguageName { get; set; } = null!;

  /// <summary>
  /// 语言代码
  /// </summary>
  public string LanguageCode { get; set; } = null!;

  /// <summary>
  /// 是否默认语言(0:否,1:是)
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
/// 语言更新DTO
/// </summary>
public class LeanLanguageUpdateDto : LeanLanguageCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}