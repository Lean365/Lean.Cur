using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

[SugarTable("lean_role_menu")]
public class LeanRoleMenu : LeanBaseEntity
{
    [SugarColumn(ColumnName = "role_id", IsNullable = false)]
    public long RoleId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public LeanRole Role { get; set; } = null!;

    [SugarColumn(ColumnName = "menu_id", IsNullable = false)]
    public long MenuId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(MenuId))]
    public LeanMenu Menu { get; set; } = null!;
}