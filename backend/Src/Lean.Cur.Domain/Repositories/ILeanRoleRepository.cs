using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Repositories;

public interface ILeanRoleRepository : ILeanBaseRepository<LeanRole>
{
  Task<List<LeanPermission>> GetRolePermissionsAsync(long roleId);
  Task AssignPermissionsAsync(long roleId, IEnumerable<long> permissionIds);
  Task<bool> HasPermissionAsync(long roleId, string permissionCode);
}