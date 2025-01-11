namespace Lean.Cur.Application.DTOs.Position;

public class LeanPositionCreateDto
{
  public string PositionName { get; set; } = null!;
  public string PositionCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; } = true;
  public string? Remark { get; set; }
}

public class LeanPositionUpdateDto
{
  public long Id { get; set; }
  public string PositionName { get; set; } = null!;
  public string PositionCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; }
  public string? Remark { get; set; }
}

public class LeanPositionQueryDto
{
  public string? PositionName { get; set; }
  public string? PositionCode { get; set; }
  public bool? Status { get; set; }
}

public class LeanPositionInfoDto
{
  public long Id { get; set; }
  public string PositionName { get; set; } = null!;
  public string PositionCode { get; set; } = null!;
  public int OrderNum { get; set; }
  public bool Status { get; set; }
  public string? Remark { get; set; }
  public DateTime CreateTime { get; set; }
  public string? CreateBy { get; set; }
}