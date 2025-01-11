namespace Lean.Cur.Application.DTOs.Department;

public class LeanDepartmentCreateDto
{
  public string DeptName { get; set; } = null!;
  public string DeptCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Leader { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public bool Status { get; set; } = true;
}

public class LeanDepartmentUpdateDto
{
  public long Id { get; set; }
  public string DeptName { get; set; } = null!;
  public string DeptCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Leader { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public bool Status { get; set; }
}

public class LeanDepartmentQueryDto
{
  public string? DeptName { get; set; }
  public string? DeptCode { get; set; }
  public bool? Status { get; set; }
}

public class LeanDepartmentInfoDto
{
  public long Id { get; set; }
  public string DeptName { get; set; } = null!;
  public string DeptCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Leader { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public bool Status { get; set; }
  public List<LeanDepartmentInfoDto> Children { get; set; } = new();
  public DateTime CreateTime { get; set; }
  public string? CreateBy { get; set; }
}