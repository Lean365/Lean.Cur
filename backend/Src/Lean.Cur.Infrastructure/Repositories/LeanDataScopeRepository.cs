using Lean.Cur.Domain.Entities.Admin;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

public class LeanDataScopeRepository : LeanBaseRepository<LeanDataScope>
{
  public LeanDataScopeRepository(ISqlSugarClient db) : base(db)
  {
  }

  public async Task<List<LeanDataScope>> GetByRoleIdAsync(long roleId)
  {
    return await EntityDb.GetListAsync(ds => ds.RoleId == roleId && ds.IsDeleted == 0);
  }

  public async Task<List<LeanDataScope>> GetByRoleIdsAsync(IEnumerable<long> roleIds)
  {
    return await EntityDb.GetListAsync(ds => roleIds.Contains(ds.RoleId) && ds.IsDeleted == 0);
  }

  public async Task<List<LeanDataScope>> GetByTableNameAsync(string tableName)
  {
    return await EntityDb.GetListAsync(ds => ds.TableName == tableName && ds.IsDeleted == 0);
  }
}