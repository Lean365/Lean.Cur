using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

public class LeanUserRepository : LeanBaseRepository<LeanUser>, ILeanUserRepository
{
  public LeanUserRepository(ISqlSugarClient db) : base(db)
  {
  }

  public async Task<LeanUser?> GetByUsernameAsync(string username)
  {
    return await EntityDb.GetFirstAsync(u => u.UserName == username && !u.IsDeleted);
  }

  public async Task<List<LeanRole>> GetUserRolesAsync(long userId)
  {
    return await Db.Queryable<LeanRole>()
        .InnerJoin<LeanUserRole>((r, ur) => r.Id == ur.RoleId)
        .Where((r, ur) => ur.UserId == userId && !r.IsDeleted)
        .Select((r, ur) => r)
        .ToListAsync();
  }

  public async Task<List<LeanPermission>> GetUserPermissionsAsync(long userId)
  {
    return await Db.Queryable<LeanPermission>()
        .InnerJoin<LeanRolePermission>((p, rp) => p.Id == rp.PermissionId)
        .InnerJoin<LeanRole>((p, rp, r) => rp.RoleId == r.Id)
        .InnerJoin<LeanUserRole>((p, rp, r, ur) => r.Id == ur.RoleId)
        .Where((p, rp, r, ur) => ur.UserId == userId && !p.IsDeleted)
        .Select((p, rp, r, ur) => p)
        .Distinct()
        .ToListAsync();
  }

  public async Task AssignRolesAsync(long userId, IEnumerable<long> roleIds)
  {
    await Db.Deleteable<LeanUserRole>().Where(ur => ur.UserId == userId).ExecuteCommandAsync();
    var userRoles = roleIds.Select(roleId => new LeanUserRole
    {
      UserId = userId,
      RoleId = roleId,
      CreateTime = DateTime.Now
    });
    await Db.Insertable(userRoles.ToArray()).ExecuteCommandAsync();
  }

  public async Task<bool> HasPermissionAsync(long userId, string permissionCode)
  {
    return await Db.Queryable<LeanPermission>()
        .InnerJoin<LeanRolePermission>((p, rp) => p.Id == rp.PermissionId)
        .InnerJoin<LeanRole>((p, rp, r) => rp.RoleId == r.Id)
        .InnerJoin<LeanUserRole>((p, rp, r, ur) => r.Id == ur.RoleId)
        .Where((p, rp, r, ur) => ur.UserId == userId && p.PermissionCode == permissionCode && !p.IsDeleted)
        .AnyAsync();
  }
}