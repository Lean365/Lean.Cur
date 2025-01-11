using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 用户查询DTO
/// </summary>
public class LeanUserQueryDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  public string? UserName { get; set; }

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  public string? NickName { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus? Status { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long? DeptId { get; set; }

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
/// 用户DTO
/// </summary>
public class LeanUserDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  public string NickName { get; set; } = null!;

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 性别
  /// </summary>
  public Gender Gender { get; set; }

  /// <summary>
  /// 头像URL
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long DeptId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string? DeptName { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long PositionId { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  public string? PositionName { get; set; }

  /// <summary>
  /// 角色ID列表
  /// </summary>
  public List<long> RoleIds { get; set; } = new();

  /// <summary>
  /// 角色名称列表
  /// </summary>
  public List<string> RoleNames { get; set; } = new();

  /// <summary>
  /// 最后登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 最后登录时间
  /// </summary>
  public DateTime? LoginDate { get; set; }

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
  /// 用户类型
  /// </summary>
  public UserType UserType { get; set; }
}

/// <summary>
/// 用户创建DTO
/// </summary>
public class LeanUserCreateDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 密码
  /// </summary>
  public string Password { get; set; } = null!;

  /// <summary>
  /// 昵称
  /// </summary>
  public string NickName { get; set; } = null!;

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 性别
  /// </summary>
  public Gender Gender { get; set; }

  /// <summary>
  /// 头像URL
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long DeptId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long PositionId { get; set; }

  /// <summary>
  /// 角色ID列表
  /// </summary>
  public List<long> RoleIds { get; set; } = new();

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  public UserType UserType { get; set; }
}

/// <summary>
/// 用户更新DTO
/// </summary>
public class LeanUserUpdateDto : LeanUserCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 新密码(为空则不修改密码)
  /// </summary>
  public new string? Password { get; set; }
}

/// <summary>
/// 用户密码重置DTO
/// </summary>
public class LeanUserResetPasswordDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 新密码
  /// </summary>
  public string Password { get; set; } = null!;
}

/// <summary>
/// 用户状态更新DTO
/// </summary>
public class LeanUserStatusUpdateDto
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