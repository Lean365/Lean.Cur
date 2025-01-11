using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lean.Cur.Domain.Entities.Admin;

[Table("lean_role_department")]
public class LeanRoleDepartment : LeanBaseEntity
{
    [Required]
    public long RoleId { get; set; }
    public virtual LeanRole Role { get; set; } = null!;

    [Required]
    public long DepartmentId { get; set; }
    public virtual LeanDepartment Department { get; set; } = null!;
}