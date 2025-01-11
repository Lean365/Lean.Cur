namespace Lean.Cur.Application.DTOs.Menu;

public class LeanMenuCreateDto
{
  public string MenuName { get; set; } = null!;
  public string MenuCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Path { get; set; }
  public string? Component { get; set; }
  public string? Query { get; set; }
  public bool IsFrame { get; set; }
  public bool IsCache { get; set; }
  public string MenuType { get; set; } = null!;
  public bool Visible { get; set; } = true;
  public bool Status { get; set; } = true;
  public string? Perms { get; set; }
  public string? Icon { get; set; }
  public string? Remark { get; set; }
}

public class LeanMenuUpdateDto
{
  public long Id { get; set; }
  public string MenuName { get; set; } = null!;
  public string MenuCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Path { get; set; }
  public string? Component { get; set; }
  public string? Query { get; set; }
  public bool IsFrame { get; set; }
  public bool IsCache { get; set; }
  public string MenuType { get; set; } = null!;
  public bool Visible { get; set; }
  public bool Status { get; set; }
  public string? Perms { get; set; }
  public string? Icon { get; set; }
  public string? Remark { get; set; }
}

public class LeanMenuQueryDto
{
  public string? MenuName { get; set; }
  public string? MenuCode { get; set; }
  public bool? Status { get; set; }
}

public class LeanMenuInfoDto
{
  public long Id { get; set; }
  public string MenuName { get; set; } = null!;
  public string MenuCode { get; set; } = null!;
  public long? ParentId { get; set; }
  public int OrderNum { get; set; }
  public string? Path { get; set; }
  public string? Component { get; set; }
  public string? Query { get; set; }
  public bool IsFrame { get; set; }
  public bool IsCache { get; set; }
  public string MenuType { get; set; } = null!;
  public bool Visible { get; set; }
  public bool Status { get; set; }
  public string? Perms { get; set; }
  public string? Icon { get; set; }
  public string? Remark { get; set; }
  public List<LeanMenuInfoDto> Children { get; set; } = new();
  public DateTime CreateTime { get; set; }
  public string? CreateBy { get; set; }
}