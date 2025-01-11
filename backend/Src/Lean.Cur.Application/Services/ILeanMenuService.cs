using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services;

public interface ILeanMenuService
{
  Task<List<LeanPermission>> GetUserMenusAsync(long userId);
  Task<List<LeanPermission>> GetMenuTreeAsync();
  Task<bool> ValidatePermissionAsync(long userId, string permissionCode);
}