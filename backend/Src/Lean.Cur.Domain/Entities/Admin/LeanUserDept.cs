using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

[SugarTable("lean_user_dept")]
public class LeanUserDept : LeanBaseEntity
{
    [SugarColumn(ColumnName = "user_id", IsNullable = false)]
    public long UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public LeanUser User { get; set; } = null!;

    [SugarColumn(ColumnName = "dept_id", IsNullable = false)]
    public long DeptId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(DeptId))]
    public LeanDepartment Department { get; set; } = null!;

    [SugarColumn(ColumnName = "is_primary", IsNullable = false)]
    public bool IsPrimary { get; set; }
}