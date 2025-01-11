using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services;

public interface ILeanRoleService
{
  Task<List<LeanRole>> GetRoleListAsync(string? keyword);
  Task<LeanRole?> GetRoleAsync(long id);
  Task<LeanRole> CreateRoleAsync(LeanRole role);
  Task<LeanRole> UpdateRoleAsync(LeanRole role);
  Task DeleteRoleAsync(long id);
  Task<bool> IsRoleCodeExistsAsync(string roleCode, long? excludeId = null);
  Task<List<LeanPermission>> GetRolePermissionsAsync(long roleId);
  Task AssignPermissionsAsync(long roleId, IEnumerable<long> permissionIds);
}