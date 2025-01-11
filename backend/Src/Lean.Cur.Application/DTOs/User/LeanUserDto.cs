using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.DTOs.User;

public class LeanUserCreateDto
{
  public string UserName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string NickName { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public Gender Gender { get; set; } = Gender.Unknown;
  public string? Avatar { get; set; }
  public LeanStatus Status { get; set; } = LeanStatus.Normal;
  public long DeptId { get; set; }
}

public class LeanUserUpdateDto
{
  public long Id { get; set; }
  public string NickName { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public Gender Gender { get; set; }
  public string? Avatar { get; set; }
  public LeanStatus Status { get; set; }
  public long DeptId { get; set; }
}

public class LeanUserQueryDto
{
  public string? UserName { get; set; }
  public string? NickName { get; set; }
  public string? Phone { get; set; }
  public LeanStatus? Status { get; set; }
  public long? DeptId { get; set; }
}

public class LeanUserInfoDto
{
  public long Id { get; set; }
  public string UserName { get; set; } = null!;
  public string NickName { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public Gender Gender { get; set; }
  public string? Avatar { get; set; }
  public LeanStatus Status { get; set; }
  public long DeptId { get; set; }
  public string? LoginIp { get; set; }
  public DateTime? LoginDate { get; set; }
  public List<long> RoleIds { get; set; } = new();
  public List<long> DeptIds { get; set; } = new();
  public List<long> PositionIds { get; set; } = new();
}