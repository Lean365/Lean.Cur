using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Repositories;

public interface ILeanPermissionRepository : ILeanBaseRepository<LeanPermission>
{
  Task<List<LeanPermission>> GetMenusAsync();
  Task<List<LeanPermission>> GetButtonsAsync();
  Task<List<LeanPermission>> GetChildrenAsync(long parentId);
  Task<List<LeanPermission>> GetByCodesAsync(IEnumerable<string> codes);
}