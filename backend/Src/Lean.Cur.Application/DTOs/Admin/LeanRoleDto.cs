using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 角色查询DTO
/// </summary>
public class LeanRoleQueryDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  public string? RoleName { get; set; }

  /// <summary>
  /// 角色编码
  /// </summary>
  public string? RoleCode { get; set; }

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
/// 角色DTO
/// </summary>
public class LeanRoleDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = null!;

  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; } = null!;

  /// <summary>
  /// 显示顺序
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
  /// 权限ID列表
  /// </summary>
  public List<long> PermissionIds { get; set; } = new();

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 角色创建DTO
/// </summary>
public class LeanRoleCreateDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = null!;

  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; } = null!;

  /// <summary>
  /// 显示顺序
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

  /// <summary>
  /// 权限ID列表
  /// </summary>
  public List<long> PermissionIds { get; set; } = new();

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 角色更新DTO
/// </summary>
public class LeanRoleUpdateDto : LeanRoleCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}

/// <summary>
/// 角色状态更新DTO
/// </summary>
public class LeanRoleStatusUpdateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus Status { get; set; }
}

/// <summary>
/// 角色权限更新DTO
/// </summary>
public class LeanRolePermissionUpdateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 权限ID列表
  /// </summary>
  public List<long> PermissionIds { get; set; } = new();
}

/// <summary>
/// 角色菜单更新DTO
/// </summary>
public class LeanRoleMenuUpdateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}