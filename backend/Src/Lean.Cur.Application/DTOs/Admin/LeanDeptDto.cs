using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 部门查询DTO
/// </summary>
public class LeanDeptQueryDto
{
  /// <summary>
  /// 部门名称
  /// </summary>
  public string? DeptName { get; set; }

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

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
/// 部门DTO
/// </summary>
public class LeanDeptDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string DeptName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 父部门ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 祖级列表
  /// </summary>
  public string? Ancestors { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

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
  /// 子部门
  /// </summary>
  public List<LeanDeptDto> Children { get; set; } = new();
}

/// <summary>
/// 部门创建DTO
/// </summary>
public class LeanDeptCreateDto
{
  /// <summary>
  /// 部门名称
  /// </summary>
  public string DeptName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 父部门ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

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
/// 部门更新DTO
/// </summary>
public class LeanDeptUpdateDto : LeanDeptCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}