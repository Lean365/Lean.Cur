using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Repositories;

public interface ILeanUserRepository : ILeanBaseRepository<LeanUser>
{
  Task<LeanUser?> GetByUsernameAsync(string username);
  Task<List<LeanRole>> GetUserRolesAsync(long userId);
  Task<List<LeanPermission>> GetUserPermissionsAsync(long userId);
  Task AssignRolesAsync(long userId, IEnumerable<long> roleIds);
  Task<bool> HasPermissionAsync(long userId, string permissionCode);
}