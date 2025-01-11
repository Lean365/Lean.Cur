/**
 * @description 数据库上下文
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using SqlSugar;
using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Infrastructure.Database;

/// <summary>
/// 数据库上下文
/// </summary>
public class LeanDbContext
{
  private readonly ISqlSugarClient _db;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDbContext(ISqlSugarClient db)
  {
    _db = db;
  }

  /// <summary>
  /// 初始化数据库
  /// </summary>
  public void InitDatabase()
  {
    // 创建数据库
    _db.DbMaintenance.CreateDatabase();

    // 创建表
    _db.CodeFirst.InitTables(typeof(LeanUser).Assembly.GetTypes()
        .Where(t => t.Namespace != null && t.Namespace.StartsWith("Lean.Cur.Domain.Entities")));

    // 检查是否已初始化
    if (_db.Queryable<LeanUser>().Any())
    {
      return;
    }

    // 创建超级管理员角色
    var adminRole = new LeanRole
    {
      RoleName = "超级管理员",
      RoleCode = "superadmin",
      OrderNum = 1,
      Status = true,
      CreateTime = DateTime.Now
    };
    _db.Insertable(adminRole).ExecuteReturnEntity();

    // 创建管理员用户
    var adminUser = new LeanUser
    {
      UserName = "admin",
      Password = "e10adc3949ba59abbe56e057f20f883e", // 123456
      NickName = "超级管理员",
      Status = true,
      CreateTime = DateTime.Now
    };
    _db.Insertable(adminUser).ExecuteReturnEntity();

    // 创建系统权限
    var permissions = new List<LeanPermission>
    {
      new() { PermissionCode = "system:user:list", PermissionName = "用户管理", OrderNum = 1 },
      new() { PermissionCode = "system:role:list", PermissionName = "角色管理", OrderNum = 2 },
      new() { PermissionCode = "system:permission:list", PermissionName = "权限管理", OrderNum = 3 },
      new() { PermissionCode = "system:dept:list", PermissionName = "部门管理", OrderNum = 4 }
    };
    _db.Insertable(permissions).ExecuteReturnEntity();

    // 分配权限给管理员角色
    var rolePermissions = permissions.Select(p => new LeanRolePermission
    {
      RoleId = adminRole.Id,
      PermissionId = p.Id,
      CreateTime = DateTime.Now
    });
    _db.Insertable(rolePermissions.ToList()).ExecuteCommand();

    // 分配角色给管理员用户
    var userRole = new LeanUserRole
    {
      UserId = adminUser.Id,
      RoleId = adminRole.Id,
      CreateTime = DateTime.Now
    };
    _db.Insertable(userRole).ExecuteCommand();
  }

  /// <summary>
  /// 获取数据库实例
  /// </summary>
  public ISqlSugarClient GetDatabase()
  {
    return _db;
  }
}