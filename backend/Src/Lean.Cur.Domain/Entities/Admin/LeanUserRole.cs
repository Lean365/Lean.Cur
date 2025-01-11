using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

[SugarTable("lean_user_role")]
public class LeanUserRole : LeanBaseEntity
{
    [SugarColumn(ColumnName = "user_id", IsNullable = false)]
    public long UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public LeanUser User { get; set; } = null!;

    [SugarColumn(ColumnName = "role_id", IsNullable = false)]
    public long RoleId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public LeanRole Role { get; set; } = null!;
}