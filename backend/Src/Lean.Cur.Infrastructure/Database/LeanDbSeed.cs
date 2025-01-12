using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Security;
using Lean.Cur.Domain.Entities.Admin;
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
    // 初始化管理员用户
    InitializeAdminUser(db);

    // 在这里添加其他种子数据的初始化
  }

  /// <summary>
  /// 初始化管理员用户
  /// </summary>
  private static void InitializeAdminUser(ISqlSugarClient db)
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
        Remark = "系统默认创建的管理员账号"
      };

      db.Insertable(admin).ExecuteCommand();
    }
  }
}