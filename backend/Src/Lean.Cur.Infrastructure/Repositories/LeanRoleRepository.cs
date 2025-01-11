using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

public class LeanRoleRepository : LeanBaseRepository<LeanRole>, ILeanRoleRepository
{
  public LeanRoleRepository(ISqlSugarClient db) : base(db)
  {
  }

  public async Task<List<LeanPermission>> GetRolePermissionsAsync(long roleId)
  {
    return await Db.Queryable<LeanPermission>()
        .InnerJoin<LeanRolePermission>((p, rp) => p.Id == rp.PermissionId)
        .Where((p, rp) => rp.RoleId == roleId && p.IsDeleted == 0)
        .Select((p, rp) => p)
        .ToListAsync();
  }

  public async Task AssignPermissionsAsync(long roleId, IEnumerable<long> permissionIds)
  {
    await Db.Deleteable<LeanRolePermission>().Where(rp => rp.RoleId == roleId).ExecuteCommandAsync();
    var rolePermissions = permissionIds.Select(permissionId => new LeanRolePermission
    {
      RoleId = roleId,
      PermissionId = permissionId,
      CreateTime = DateTime.Now
    });
    await Db.Insertable(rolePermissions.ToArray()).ExecuteCommandAsync();
  }

  public async Task<bool> HasPermissionAsync(long roleId, string permissionCode)
  {
    return await Db.Queryable<LeanPermission>()
        .InnerJoin<LeanRolePermission>((p, rp) => p.Id == rp.PermissionId)
        .Where((p, rp) => rp.RoleId == roleId && p.PermissionCode == permissionCode && p.IsDeleted == 0)
        .AnyAsync();
  }
}