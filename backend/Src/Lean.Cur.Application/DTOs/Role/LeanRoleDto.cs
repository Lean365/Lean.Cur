namespace Lean.Cur.Application.DTOs.Role;

public class LeanRoleCreateDto
{
  public string RoleName { get; set; } = null!;
  public string RoleCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; } = true;
  public string? Remark { get; set; }
  public List<long> MenuIds { get; set; } = new();
  public List<long> PermissionIds { get; set; } = new();
}

public class LeanRoleUpdateDto
{
  public long Id { get; set; }
  public string RoleName { get; set; } = null!;
  public string RoleCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; }
  public string? Remark { get; set; }
}

public class LeanRoleQueryDto
{
  public string? RoleName { get; set; }
  public string? RoleCode { get; set; }
  public bool? Status { get; set; }
}

public class LeanRoleInfoDto
{
  public long Id { get; set; }
  public string RoleName { get; set; } = null!;
  public string RoleCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; }
  public string? Remark { get; set; }
  public List<long> MenuIds { get; set; } = new();
  public List<long> PermissionIds { get; set; } = new();
  public DateTime CreateTime { get; set; }
  public string? CreateBy { get; set; }
}