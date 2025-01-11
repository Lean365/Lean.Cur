using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

[SugarTable("lean_dept")]
public class LeanDepartment : LeanBaseEntity
{
    [SugarColumn(ColumnName = "dept_name", Length = 30, IsNullable = false)]
    public string DeptName { get; set; } = null!;

    [SugarColumn(ColumnName = "parent_id", IsNullable = true)]
    public long? ParentId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(ParentId))]
    public LeanDepartment? Parent { get; set; }

    [SugarColumn(ColumnName = "ancestors", Length = 50, IsNullable = true)]
    public string? Ancestors { get; set; }

    [SugarColumn(ColumnName = "order_num", IsNullable = false)]
    public int OrderNum { get; set; }

    [SugarColumn(ColumnName = "leader", Length = 20, IsNullable = true)]
    public string? Leader { get; set; }

    [SugarColumn(ColumnName = "phone", Length = 11, IsNullable = true)]
    public string? Phone { get; set; }

    [SugarColumn(ColumnName = "email", Length = 50, IsNullable = true)]
    public string? Email { get; set; }

    [SugarColumn(ColumnName = "status", IsNullable = false)]
    public bool Status { get; set; } = true;

    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    public List<LeanDepartment> Children { get; set; } = new();

    [Navigate(NavigateType.OneToMany, nameof(LeanUser.DeptId))]
    public List<LeanUser> Users { get; set; } = new();
}