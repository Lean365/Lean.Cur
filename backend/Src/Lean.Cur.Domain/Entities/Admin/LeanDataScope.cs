using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Entities;

public class LeanDataScope : LeanBaseEntity
{
  public long RoleId { get; set; }
  public virtual LeanRole Role { get; set; } = null!;

  public string TableName { get; set; } = null!;
  public string PermissionType { get; set; } = null!; // All, Department, Custom
  public string? CustomScope { get; set; }
}