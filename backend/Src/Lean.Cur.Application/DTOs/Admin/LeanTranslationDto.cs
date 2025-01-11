using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 翻译查询DTO
/// </summary>
public class LeanTranslationQueryDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  public long? LanguageId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  public string? TransKey { get; set; }

  /// <summary>
  /// 翻译值
  /// </summary>
  public string? TransValue { get; set; }

  /// <summary>
  /// 模块
  /// </summary>
  public string? Module { get; set; }

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
/// 翻译DTO
/// </summary>
public class LeanTranslationDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 语言ID
  /// </summary>
  public long LanguageId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  public string TransKey { get; set; } = null!;

  /// <summary>
  /// 翻译值
  /// </summary>
  public string TransValue { get; set; } = null!;

  /// <summary>
  /// 模块
  /// </summary>
  public string Module { get; set; } = null!;

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
  /// 语言名称
  /// </summary>
  public string? LanguageName { get; set; }

  /// <summary>
  /// 语言代码
  /// </summary>
  public string? LanguageCode { get; set; }
}

/// <summary>
/// 翻译创建DTO
/// </summary>
public class LeanTranslationCreateDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  public long LanguageId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  public string TransKey { get; set; } = null!;

  /// <summary>
  /// 翻译值
  /// </summary>
  public string TransValue { get; set; } = null!;

  /// <summary>
  /// 模块
  /// </summary>
  public string Module { get; set; } = null!;

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
/// 翻译更新DTO
/// </summary>
public class LeanTranslationUpdateDto : LeanTranslationCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}