using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services;

public interface ILeanPermissionService
{
  Task<List<LeanPermission>> GetPermissionListAsync(string? keyword);
  Task<LeanPermission?> GetPermissionAsync(long id);
  Task<LeanPermission> CreatePermissionAsync(LeanPermission permission);
  Task<LeanPermission> UpdatePermissionAsync(LeanPermission permission);
  Task DeletePermissionAsync(long id);
  Task<List<LeanPermission>> GetPermissionTreeAsync();
  Task<List<LeanPermission>> GetUserMenusAsync(long userId);
  Task<List<LeanPermission>> GetUserButtonsAsync(long userId);
  Task<bool> ValidatePermissionAsync(long userId, string permissionCode);
  Task<List<string>> GetUserPermissionCodesAsync(long userId);
  Task AssignUserRolesAsync(long userId, IEnumerable<long> roleIds);
  Task AssignRolePermissionsAsync(long roleId, IEnumerable<long> permissionIds);
  Task<bool> CheckPermissionCodeAsync(string code);
}