using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

public class LeanPermission : LeanBaseEntity
{
    public string PermissionName { get; set; } = null!;
    public string PermissionCode { get; set; } = null!;
    public long? ParentId { get; set; }
    public int OrderNum { get; set; }
    public string? Path { get; set; }
    public string? Component { get; set; }
    public string? Icon { get; set; }
    public bool Status { get; set; }
    public bool Visible { get; set; }
    public bool IsButton { get; set; }
    public new string? Remark { get; set; }

    public virtual List<LeanRole> Roles { get; set; } = new();
    public virtual List<LeanPermission> Children { get; set; } = new();
}