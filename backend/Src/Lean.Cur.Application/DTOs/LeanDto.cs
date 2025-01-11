namespace Lean.Cur.Application.DTOs;

#region Auth
public class LeanLoginRequestDto
{
  public string UserName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string? VerifyCode { get; set; }
}

public class LeanLoginResponseDto
{
  public string Token { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
  public LeanUserDto User { get; set; } = null!;
}
#endregion

#region User
public class LeanUserDto
{
  public long Id { get; set; }
  public string UserName { get; set; } = null!;
  public string NickName { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Sex { get; set; }
  public string? Avatar { get; set; }
  public bool Status { get; set; }
  public long DeptId { get; set; }
  public string? LoginIp { get; set; }
  public DateTime? LoginDate { get; set; }
  public List<long> RoleIds { get; set; } = new();
  public List<long> DeptIds { get; set; } = new();
  public List<long> PositionIds { get; set; } = new();
  public List<LeanRoleDto> Roles { get; set; } = new();
  public List<LeanPermissionDto> Permissions { get; set; } = new();
}

public class LeanCreateUserDto
{
  public string UserName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string NickName { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Sex { get; set; }
  public long DeptId { get; set; }
  public List<long> RoleIds { get; set; } = new();
  public List<long> PositionIds { get; set; } = new();
}

public class LeanUpdateUserDto
{
  public string NickName { get; set; } = null!;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string? Sex { get; set; }
  public string? Avatar { get; set; }
  public bool Status { get; set; }
  public long DeptId { get; set; }
  public List<long> RoleIds { get; set; } = new();
  public List<long> PositionIds { get; set; } = new();
}
#endregion

#region Role
public class LeanRoleDto
{
  public long Id { get; set; }
  public string RoleName { get; set; } = null!;
  public string RoleCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; }
  public string? Remark { get; set; }
  public List<long> MenuIds { get; set; } = new();
  public List<long> PermissionIds { get; set; } = new();
  public List<LeanPermissionDto> Permissions { get; set; } = new();
}

public class LeanCreateRoleDto
{
  public string RoleName { get; set; } = null!;
  public string RoleCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public string? Remark { get; set; }
  public List<long> MenuIds { get; set; } = new();
  public List<long> PermissionIds { get; set; } = new();
}

public class LeanUpdateRoleDto
{
  public string RoleName { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; }
  public string? Remark { get; set; }
  public List<long> MenuIds { get; set; } = new();
  public List<long> PermissionIds { get; set; } = new();
}
#endregion

#region Permission
public class LeanPermissionDto
{
  public long Id { get; set; }
  public string PermissionName { get; set; } = null!;
  public string PermissionCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Path { get; set; }
  public string? Component { get; set; }
  public string? Icon { get; set; }
  public bool Status { get; set; }
  public bool Visible { get; set; }
  public string? Remark { get; set; }
  public List<LeanPermissionDto> Children { get; set; } = new();
}

public class LeanCreatePermissionDto
{
  public string PermissionName { get; set; } = null!;
  public string PermissionCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Path { get; set; }
  public string? Component { get; set; }
  public string? Icon { get; set; }
  public bool Visible { get; set; }
  public string? Remark { get; set; }
}

public class LeanUpdatePermissionDto
{
  public string PermissionName { get; set; } = null!;
  public int OrderNum { get; set; }
  public string? Path { get; set; }
  public string? Component { get; set; }
  public string? Icon { get; set; }
  public bool Status { get; set; }
  public bool Visible { get; set; }
  public string? Remark { get; set; }
}
#endregion
