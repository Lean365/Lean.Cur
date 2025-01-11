using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

public class LeanPermissionRepository : LeanBaseRepository<LeanPermission>, ILeanPermissionRepository
{
  public LeanPermissionRepository(ISqlSugarClient db) : base(db)
  {
  }

  public async Task<List<LeanPermission>> GetMenusAsync()
  {
    return await EntityDb.GetListAsync(p => !p.IsButton && !p.IsDeleted);
  }

  public async Task<List<LeanPermission>> GetButtonsAsync()
  {
    return await EntityDb.GetListAsync(p => p.IsButton && !p.IsDeleted);
  }

  public async Task<List<LeanPermission>> GetChildrenAsync(long parentId)
  {
    return await EntityDb.GetListAsync(p => p.ParentId == parentId && !p.IsDeleted);
  }

  public async Task<List<LeanPermission>> GetByCodesAsync(IEnumerable<string> codes)
  {
    return await EntityDb.GetListAsync(p => codes.Contains(p.PermissionCode) && !p.IsDeleted);
  }
}