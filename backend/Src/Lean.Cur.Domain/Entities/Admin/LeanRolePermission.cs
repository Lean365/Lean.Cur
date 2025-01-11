using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lean.Cur.Domain.Entities.Admin;

[Table("lean_role_permission")]
public class LeanRolePermission : LeanBaseEntity
{
    [Required]
    public long RoleId { get; set; }
    public virtual LeanRole Role { get; set; } = null!;

    [Required]
    public long PermissionId { get; set; }
    public virtual LeanPermission Permission { get; set; } = null!;
}