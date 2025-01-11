using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 系统配置查询DTO
/// </summary>
public class LeanConfigQueryDto
{
  /// <summary>
  /// 配置键
  /// </summary>
  public string? ConfigKey { get; set; }

  /// <summary>
  /// 配置组
  /// </summary>
  public string? ConfigGroup { get; set; }

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
/// 系统配置DTO
/// </summary>
public class LeanConfigDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 配置键
  /// </summary>
  public string ConfigKey { get; set; } = null!;

  /// <summary>
  /// 配置值
  /// </summary>
  public string ConfigValue { get; set; } = null!;

  /// <summary>
  /// 配置组
  /// </summary>
  public string? ConfigGroup { get; set; }

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
/// 系统配置创建DTO
/// </summary>
public class LeanConfigCreateDto
{
  /// <summary>
  /// 配置键
  /// </summary>
  public string ConfigKey { get; set; } = null!;

  /// <summary>
  /// 配置值
  /// </summary>
  public string ConfigValue { get; set; } = null!;

  /// <summary>
  /// 配置组
  /// </summary>
  public string? ConfigGroup { get; set; }

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
/// 系统配置更新DTO
/// </summary>
public class LeanConfigUpdateDto : LeanConfigCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}