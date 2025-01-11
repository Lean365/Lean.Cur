using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

[SugarTable("lean_user_position")]
public class LeanUserPosition : LeanBaseEntity
{
    [SugarColumn(ColumnName = "user_id", IsNullable = false)]
    public long UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public LeanUser User { get; set; } = null!;

    [SugarColumn(ColumnName = "position_id", IsNullable = false)]
    public long PositionId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(PositionId))]
    public LeanPosition Position { get; set; } = null!;

    [SugarColumn(ColumnName = "is_primary", IsNullable = false)]
    public bool IsPrimary { get; set; }
}