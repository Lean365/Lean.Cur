using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Infrastructure.Repositories;

public class LeanUserRepository : LeanBaseRepository<LeanUser>, ILeanUserRepository
{
  private readonly ISqlSugarClient _db;

  public LeanUserRepository(ISqlSugarClient db) : base(db)
  {
    _db = db;
  }

  public async Task<LeanUser?> GetByUsernameAsync(string username)
  {
    return await EntityDb.GetFirstAsync(u => u.UserName == username && u.Status == LeanStatus.Normal);
  }

  public async Task<List<LeanRole>> GetUserRolesAsync(long userId)
  {
    return await _db.Queryable<LeanRole>()
        .InnerJoin<LeanUserRole>((r, ur) => r.Id == ur.RoleId)
        .Where((r, ur) => ur.UserId == userId && r.Status == LeanStatus.Normal)
        .ToListAsync();
  }

  public async Task<List<LeanPermission>> GetUserPermissionsAsync(long userId)
  {
    return await _db.Queryable<LeanPermission>()
        .InnerJoin<LeanRolePermission>((p, rp) => p.Id == rp.PermissionId)
        .InnerJoin<LeanUserRole>((p, rp, ur) => rp.RoleId == ur.RoleId)
        .Where((p, rp, ur) => ur.UserId == userId && p.Status == LeanStatus.Normal)
        .ToListAsync();
  }

  public async Task AssignRolesAsync(long userId, IEnumerable<long> roleIds)
  {
    await _db.Deleteable<LeanUserRole>().Where(ur => ur.UserId == userId).ExecuteCommandAsync();
    var userRoles = roleIds.Select(roleId => new LeanUserRole
    {
      UserId = userId,
      RoleId = roleId,
      CreateTime = DateTime.Now
    });
    await _db.Insertable(userRoles.ToArray()).ExecuteCommandAsync();
  }

  public async Task<bool> HasPermissionAsync(long userId, string permissionCode)
  {
    return await _db.Queryable<LeanPermission>()
        .InnerJoin<LeanRolePermission>((p, rp) => p.Id == rp.PermissionId)
        .InnerJoin<LeanUserRole>((p, rp, ur) => rp.RoleId == ur.RoleId)
        .Where((p, rp, ur) => ur.UserId == userId && p.PermissionCode == permissionCode && p.Status == LeanStatus.Normal)
        .AnyAsync();
  }
}