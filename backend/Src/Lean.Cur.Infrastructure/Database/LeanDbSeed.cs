using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Security;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Database;

/// <summary>
/// 数据库种子数据
/// </summary>
public static class LeanDbSeed
{
  /// <summary>
  /// 初始化种子数据
  /// </summary>
  public static void Initialize(ISqlSugarClient db)
  {
    try
    {
      // 初始化管理员用户
      var adminUser = InitializeAdminUser(db);
      if (adminUser != null)
      {
        // 初始化管理员角色
        var adminRole = InitializeAdminRole(db);

        // 初始化用户角色关联
        if (adminRole != null)
        {
          InitializeUserRole(db, adminUser.Id, adminRole.Id);
        }

        // 初始化用户扩展信息
        InitializeUserExtend(db, adminUser.Id);
      }
    }
    catch (Exception ex)
    {
      throw new Exception("初始化种子数据失败", ex);
    }
  }

  /// <summary>
  /// 初始化管理员用户
  /// </summary>
  private static LeanUser? InitializeAdminUser(ISqlSugarClient db)
  {
    try
    {
      if (!db.Queryable<LeanUser>().Any())
      {
        var salt = LeanPassword.GenerateSalt();
        var admin = new LeanUser
        {
          UserName = "admin",
          NickName = "系统管理员",
          EnglishName = "Administrator",
          Gender = Gender.Unknown,
          Email = "admin@lean.cur",
          Phone = "13800138000",
          Status = LeanStatus.Normal,
          UserType = UserType.Admin,
          PasswordSalt = salt,
          Password = LeanPassword.HashPassword("123456", salt),  // 默认密码：123456
          Remark = "系统默认创建的管理员账号",
          CreateTime = DateTime.Now,
          CreateBy = 0
        };

        var result = db.Insertable(admin).ExecuteReturnEntity();
        return result;
      }
      return null;
    }
    catch (Exception ex)
    {
      throw new Exception("初始化管理员用户失败", ex);
    }
  }

  /// <summary>
  /// 初始化管理员角色
  /// </summary>
  private static LeanRole? InitializeAdminRole(ISqlSugarClient db)
  {
    try
    {
      if (!db.Queryable<LeanRole>().Any())
      {
        var adminRole = new LeanRole
        {
          RoleName = "超级管理员",
          RoleCode = "admin",
          RoleType = (LeanRoleType)1,
          DataScope = (LeanDataScope)1,
          OrderNum = 1,
          Status = LeanStatus.Normal,
          Remark = "系统超级管理员",
          CreateTime = DateTime.Now,
          CreateBy = 0
        };

        var result = db.Insertable(adminRole).ExecuteReturnEntity();
        return result;
      }
      return null;
    }
    catch (Exception ex)
    {
      throw new Exception("初始化管理员角色失败", ex);
    }
  }

  /// <summary>
  /// 初始化用户角色关联
  /// </summary>
  private static void InitializeUserRole(ISqlSugarClient db, long userId, long roleId)
  {
    try
    {
      if (!db.Queryable<LeanUserRole>().Any(x => x.UserId == userId && x.RoleId == roleId))
      {
        var userRole = new LeanUserRole
        {
          UserId = userId,
          RoleId = roleId,
          CreateTime = DateTime.Now,
          CreateBy = 0
        };

        db.Insertable(userRole).ExecuteCommand();
      }
    }
    catch (Exception ex)
    {
      throw new Exception("初始化用户角色关联失败", ex);
    }
  }

  /// <summary>
  /// 初始化用户扩展信息
  /// </summary>
  private static void InitializeUserExtend(ISqlSugarClient db, long userId)
  {
    try
    {
      if (!db.Queryable<LeanUserExtend>().Any(x => x.UserId == userId))
      {
        var userExtend = new LeanUserExtend
        {
          UserId = userId,
          FirstLoginTime = DateTime.Now,
          LastLoginTime = DateTime.Now,
          LoginCount = 0,
          ErrorCount = 0,
          TotalErrorCount = 0,
          OnlineStatus = 0,
          LastHeartbeatTime = DateTime.Now,
          LastActiveTime = DateTime.Now,
          CreateTime = DateTime.Now,
          CreateBy = 0
        };

        db.Insertable(userExtend).ExecuteCommand();
      }
    }
    catch (Exception ex)
    {
      throw new Exception("初始化用户扩展信息失败", ex);
    }
  }
}