using Lean.Cur.Common.Enums;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Database;

/// <summary>
/// 数据库种子数据初始化
/// </summary>
public static class LeanDbSeed
{
  /// <summary>
  /// 初始化种子数据
  /// </summary>
  public static void Initialize(SqlSugarClient db, ILogger logger, Dictionary<string, (int inserted, int updated, int deleted)> stats)
  {
    try
    {
      // 初始化默认部门
      var deptStats = InitializeDefaultDept(db);
      if (deptStats.inserted > 0 || deptStats.updated > 0)
      {
        stats["lean_dept"] = (deptStats.inserted, deptStats.updated, 0);
      }

      // 初始化默认岗位
      var postStats = InitializeDefaultPost(db);
      if (postStats.inserted > 0 || postStats.updated > 0)
      {
        stats["lean_post"] = (postStats.inserted, postStats.updated, 0);
      }

      // 初始化管理员用户
      var userStats = InitializeAdminUser(db);
      if (userStats.inserted > 0 || userStats.updated > 0)
      {
        stats["lean_user"] = (userStats.inserted, userStats.updated, 0);
      }

      // 初始化管理员角色
      var roleStats = InitializeAdminRole(db);
      if (roleStats.inserted > 0 || roleStats.updated > 0)
      {
        stats["lean_role"] = (roleStats.inserted, roleStats.updated, 0);
      }

      // 初始化默认菜单
      var menuStats = InitializeDefaultMenus(db);
      if (menuStats.inserted > 0 || menuStats.updated > 0)
      {
        stats["lean_menu"] = (menuStats.inserted, menuStats.updated, 0);
      }

      // 初始化用户角色关系
      var userRoleStats = InitializeUserRole(db, 0, 0);
      if (userRoleStats.inserted > 0 || userRoleStats.updated > 0)
      {
        stats["lean_user_role"] = (userRoleStats.inserted, userRoleStats.updated, 0);
      }

      // 初始化角色菜单关系
      var roleMenuStats = InitializeRoleMenu(db, 0);
      if (roleMenuStats.inserted > 0 || roleMenuStats.updated > 0)
      {
        stats["lean_role_menu"] = (roleMenuStats.inserted, roleMenuStats.updated, 0);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "初始化种子数据失败");
      throw;
    }
  }

  /// <summary>
  /// 初始化默认部门
  /// </summary>
  private static (int inserted, int updated) InitializeDefaultDept(SqlSugarClient db)
  {
    var inserted = 0;
    var updated = 0;

    // 定义默认部门
    var defaultDepts = new List<LeanDept>
    {
      // 总公司
      new LeanDept
      {
        ParentId = 0,
        DeptName = "总公司",
        DeptCode = "HQ",
        OrderNum = 1,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 技术部
      new LeanDept
      {
        ParentId = 1,
        DeptName = "技术部",
        DeptCode = "TECH",
        OrderNum = 1,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 运营部
      new LeanDept
      {
        ParentId = 1,
        DeptName = "运营部",
        DeptCode = "OPS",
        OrderNum = 2,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 市场部
      new LeanDept
      {
        ParentId = 1,
        DeptName = "市场部",
        DeptCode = "MKT",
        OrderNum = 3,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 人事部
      new LeanDept
      {
        ParentId = 1,
        DeptName = "人事部",
        DeptCode = "HR",
        OrderNum = 4,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      }
    };

    foreach (var dept in defaultDepts)
    {
      var existingDept = db.Queryable<LeanDept>()
        .Where(x => x.DeptCode == dept.DeptCode)
        .First();

      if (existingDept != null)
      {
        // 检查是否需要更新
        bool needUpdate = existingDept.DeptName != dept.DeptName ||
                         existingDept.OrderNum != dept.OrderNum ||
                         existingDept.Status != dept.Status;

        if (needUpdate)
        {
          existingDept.DeptName = dept.DeptName;
          existingDept.OrderNum = dept.OrderNum;
          existingDept.Status = dept.Status;
          db.Updateable(existingDept).ExecuteCommand();
          updated++;
        }
      }
      else
      {
        db.Insertable(dept).ExecuteCommand();
        inserted++;
      }
    }

    return (inserted, updated);
  }

  /// <summary>
  /// 初始化默认岗位
  /// </summary>
  private static (int inserted, int updated) InitializeDefaultPost(SqlSugarClient db)
  {
    var inserted = 0;
    var updated = 0;

    // 定义默认岗位
    var defaultPosts = new List<LeanPost>
    {
      // 总经理
      new LeanPost
      {
        PostName = "总经理",
        PostCode = "GM",
        OrderNum = 1,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 部门经理
      new LeanPost
      {
        PostName = "部门经理",
        PostCode = "DM",
        OrderNum = 2,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 高级工程师
      new LeanPost
      {
        PostName = "高级工程师",
        PostCode = "SE",
        OrderNum = 3,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 工程师
      new LeanPost
      {
        PostName = "工程师",
        PostCode = "ENG",
        OrderNum = 4,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 产品经理
      new LeanPost
      {
        PostName = "产品经理",
        PostCode = "PM",
        OrderNum = 5,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 运营专员
      new LeanPost
      {
        PostName = "运营专员",
        PostCode = "OPE",
        OrderNum = 6,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // HR专员
      new LeanPost
      {
        PostName = "HR专员",
        PostCode = "HR",
        OrderNum = 7,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      }
    };

    foreach (var post in defaultPosts)
    {
      var existingPost = db.Queryable<LeanPost>()
        .Where(x => x.PostCode == post.PostCode)
        .First();

      if (existingPost != null)
      {
        // 检查是否需要更新
        bool needUpdate = existingPost.PostName != post.PostName ||
                         existingPost.OrderNum != post.OrderNum ||
                         existingPost.Status != post.Status;

        if (needUpdate)
        {
          existingPost.PostName = post.PostName;
          existingPost.OrderNum = post.OrderNum;
          existingPost.Status = post.Status;
          db.Updateable(existingPost).ExecuteCommand();
          updated++;
        }
      }
      else
      {
        db.Insertable(post).ExecuteCommand();
        inserted++;
      }
    }

    return (inserted, updated);
  }

  /// <summary>
  /// 初始化管理员用户
  /// </summary>
  private static (int inserted, int updated) InitializeAdminUser(SqlSugarClient db)
  {
    var inserted = 0;
    var updated = 0;

    // 获取部门ID映射
    var deptIds = db.Queryable<LeanDept>()
      .Where(x => x.Status == LeanStatus.Normal)
      .Select(x => new { x.Id, x.DeptCode })
      .ToList()
      .ToDictionary(x => x.DeptCode, x => x.Id);

    // 获取岗位ID映射
    var postIds = db.Queryable<LeanPost>()
      .Where(x => x.Status == LeanStatus.Normal)
      .Select(x => new { x.Id, x.PostCode })
      .ToList()
      .ToDictionary(x => x.PostCode, x => x.Id);

    // 定义默认用户
    var defaultUsers = new List<LeanUser>
    {
      // 超级管理员
      new LeanUser
      {
        UserName = "admin",
        NickName = "超级管理员",
        Password = "123456",
        Email = "admin@lean.com",
        Phone = "13800138000",
        DeptId = deptIds["HQ"],
        PostId = postIds["GM"],
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 技术部经理
      new LeanUser
      {
        UserName = "tech_manager",
        NickName = "技术部经理",
        Password = "123456",
        Email = "tech@lean.com",
        Phone = "13800138001",
        DeptId = deptIds["TECH"],
        PostId = postIds["DM"],
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 运营部经理
      new LeanUser
      {
        UserName = "ops_manager",
        NickName = "运营部经理",
        Password = "123456",
        Email = "ops@lean.com",
        Phone = "13800138002",
        DeptId = deptIds["OPS"],
        PostId = postIds["DM"],
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 市场部经理
      new LeanUser
      {
        UserName = "mkt_manager",
        NickName = "市场部经理",
        Password = "123456",
        Email = "mkt@lean.com",
        Phone = "13800138003",
        DeptId = deptIds["MKT"],
        PostId = postIds["DM"],
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 人事部经理
      new LeanUser
      {
        UserName = "hr_manager",
        NickName = "人事部经理",
        Password = "123456",
        Email = "hr@lean.com",
        Phone = "13800138004",
        DeptId = deptIds["HR"],
        PostId = postIds["DM"],
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      }
    };

    foreach (var user in defaultUsers)
    {
      var existingUser = db.Queryable<LeanUser>()
        .Where(x => x.UserName == user.UserName)
        .First();

      if (existingUser != null)
      {
        // 检查是否需要更新
        bool needUpdate = existingUser.NickName != user.NickName ||
                         existingUser.Email != user.Email ||
                         existingUser.Phone != user.Phone ||
                         existingUser.DeptId != user.DeptId ||
                         existingUser.PostId != user.PostId ||
                         existingUser.Status != user.Status;

        if (needUpdate)
        {
          existingUser.NickName = user.NickName;
          existingUser.Email = user.Email;
          existingUser.Phone = user.Phone;
          existingUser.DeptId = user.DeptId;
          existingUser.PostId = user.PostId;
          existingUser.Status = user.Status;
          db.Updateable(existingUser).ExecuteCommand();
          updated++;
        }
      }
      else
      {
        db.Insertable(user).ExecuteCommand();
        inserted++;
      }
    }

    return (inserted, updated);
  }

  /// <summary>
  /// 初始化管理员角色
  /// </summary>
  private static (int inserted, int updated) InitializeAdminRole(SqlSugarClient db)
  {
    var inserted = 0;
    var updated = 0;

    // 定义默认角色
    var defaultRoles = new List<LeanRole>
    {
      // 超级管理员
      new LeanRole
      {
        RoleName = "超级管理员",
        RoleCode = "ADMIN",
        OrderNum = 1,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 部门经理
      new LeanRole
      {
        RoleName = "部门经理",
        RoleCode = "MANAGER",
        OrderNum = 2,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 普通员工
      new LeanRole
      {
        RoleName = "普通员工",
        RoleCode = "EMPLOYEE",
        OrderNum = 3,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      }
    };

    foreach (var role in defaultRoles)
    {
      var existingRole = db.Queryable<LeanRole>()
        .Where(x => x.RoleCode == role.RoleCode)
        .First();

      if (existingRole != null)
      {
        // 检查是否需要更新
        bool needUpdate = existingRole.RoleName != role.RoleName ||
                         existingRole.OrderNum != role.OrderNum ||
                         existingRole.Status != role.Status;

        if (needUpdate)
        {
          existingRole.RoleName = role.RoleName;
          existingRole.OrderNum = role.OrderNum;
          existingRole.Status = role.Status;
          db.Updateable(existingRole).ExecuteCommand();
          updated++;
        }
      }
      else
      {
        db.Insertable(role).ExecuteCommand();
        inserted++;
      }
    }

    return (inserted, updated);
  }

  /// <summary>
  /// 初始化用户角色关联
  /// </summary>
  private static (int inserted, int updated) InitializeUserRole(SqlSugarClient db, long adminUserId, long adminRoleId)
  {
    var inserted = 0;
    var updated = 0;

    // 获取用户ID映射
    var userIds = db.Queryable<LeanUser>()
      .Where(x => x.Status == LeanStatus.Normal)
      .Select(x => new { x.Id, x.UserName })
      .ToList()
      .ToDictionary(x => x.UserName, x => x.Id);

    // 获取角色ID映射
    var roleIds = db.Queryable<LeanRole>()
      .Where(x => x.Status == LeanStatus.Normal)
      .Select(x => new { x.Id, x.RoleCode })
      .ToList()
      .ToDictionary(x => x.RoleCode, x => x.Id);

    // 定义用户角色关联
    var userRoles = new List<(string UserName, string RoleCode)>
    {
      // 超级管理员 -> 超级管理员角色
      ("admin", "ADMIN"),
      // 部门经理 -> 部门经理角色
      ("tech_manager", "MANAGER"),
      ("ops_manager", "MANAGER"),
      ("mkt_manager", "MANAGER"),
      ("hr_manager", "MANAGER")
    };

    foreach (var ur in userRoles)
    {
      if (!userIds.ContainsKey(ur.UserName) || !roleIds.ContainsKey(ur.RoleCode))
      {
        continue;
      }

      var userId = userIds[ur.UserName];
      var roleId = roleIds[ur.RoleCode];

      var existingUserRole = db.Queryable<LeanUserRole>()
        .Where(x => x.UserId == userId && x.RoleId == roleId)
        .First();

      if (existingUserRole == null)
      {
        var userRole = new LeanUserRole
        {
          UserId = userId,
          RoleId = roleId,
          CreateTime = DateTime.Now,
          CreateBy = 0
        };

        db.Insertable(userRole).ExecuteCommand();
        inserted++;
      }
    }

    return (inserted, updated);
  }

  /// <summary>
  /// 初始化默认菜单
  /// </summary>
  private static (int inserted, int updated) InitializeDefaultMenus(SqlSugarClient db)
  {
    var inserted = 0;
    var updated = 0;

    // 定义默认菜单
    var defaultMenus = new List<LeanMenu>
    {
      // 系统管理
      new LeanMenu
      {
        MenuName = "系统管理",
        MenuCode = "SYSTEM",
        ParentId = 0,
        Path = "/system",
        Component = "Layout",
        Icon = "system",
        OrderNum = 1,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 用户管理
      new LeanMenu
      {
        MenuName = "用户管理",
        MenuCode = "SYSTEM:USER",
        ParentId = 1, // 系统管理的ID
        Path = "user",
        Component = "/system/user/index",
        Icon = "user",
        OrderNum = 1,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 角色管理
      new LeanMenu
      {
        MenuName = "角色管理",
        MenuCode = "SYSTEM:ROLE",
        ParentId = 1,
        Path = "role",
        Component = "/system/role/index",
        Icon = "role",
        OrderNum = 2,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 菜单管理
      new LeanMenu
      {
        MenuName = "菜单管理",
        MenuCode = "SYSTEM:MENU",
        ParentId = 1,
        Path = "menu",
        Component = "/system/menu/index",
        Icon = "menu",
        OrderNum = 3,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 部门管理
      new LeanMenu
      {
        MenuName = "部门管理",
        MenuCode = "SYSTEM:DEPT",
        ParentId = 1,
        Path = "dept",
        Component = "/system/dept/index",
        Icon = "dept",
        OrderNum = 4,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 岗位管理
      new LeanMenu
      {
        MenuName = "岗位管理",
        MenuCode = "SYSTEM:POST",
        ParentId = 1,
        Path = "post",
        Component = "/system/post/index",
        Icon = "post",
        OrderNum = 5,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      },
      // 通知公告
      new LeanMenu
      {
        MenuName = "通知公告",
        MenuCode = "SYSTEM:NOTICE",
        ParentId = 1,
        Path = "notice",
        Component = "/system/notice/index",
        Icon = "notice",
        OrderNum = 6,
        Status = LeanStatus.Normal,
        CreateTime = DateTime.Now,
        CreateBy = 0
      }
    };

    foreach (var menu in defaultMenus)
    {
      var existingMenu = db.Queryable<LeanMenu>()
        .Where(x => x.MenuCode == menu.MenuCode)
        .First();

      if (existingMenu != null)
      {
        // 检查是否需要更新
        bool needUpdate = existingMenu.MenuName != menu.MenuName ||
                        existingMenu.Path != menu.Path ||
                        existingMenu.Component != menu.Component ||
                        existingMenu.Icon != menu.Icon ||
                        existingMenu.OrderNum != menu.OrderNum ||
                        existingMenu.Status != menu.Status;

        if (needUpdate)
        {
          existingMenu.MenuName = menu.MenuName;
          existingMenu.Path = menu.Path;
          existingMenu.Component = menu.Component;
          existingMenu.Icon = menu.Icon;
          existingMenu.OrderNum = menu.OrderNum;
          existingMenu.Status = menu.Status;
          db.Updateable(existingMenu).ExecuteCommand();
          updated++;
        }
      }
      else
      {
        db.Insertable(menu).ExecuteCommand();
        inserted++;
      }
    }

    return (inserted, updated);
  }

  /// <summary>
  /// 初始化角色菜单关系
  /// </summary>
  private static (int inserted, int updated) InitializeRoleMenu(SqlSugarClient db, long adminRoleId)
  {
    var inserted = 0;
    var updated = 0;

    // 获取角色ID映射
    var roleIds = db.Queryable<LeanRole>()
      .Where(x => x.Status == LeanStatus.Normal)
      .Select(x => new { x.Id, x.RoleCode })
      .ToList()
      .ToDictionary(x => x.RoleCode, x => x.Id);

    // 获取菜单ID映射
    var menuIds = db.Queryable<LeanMenu>()
      .Where(x => x.Status == LeanStatus.Normal)
      .Select(x => new { x.Id, x.MenuCode })
      .ToList()
      .ToDictionary(x => x.MenuCode, x => x.Id);

    // 定义角色菜单权限
    var roleMenus = new Dictionary<string, List<string>>
    {
      // 超级管理员拥有所有菜单权限
      {
        "ADMIN", new List<string>
        {
          "SYSTEM",
          "SYSTEM:USER",
          "SYSTEM:ROLE",
          "SYSTEM:MENU",
          "SYSTEM:DEPT",
          "SYSTEM:POST",
          "SYSTEM:NOTICE"
        }
      },
      // 部门经理拥有部分菜单权限
      {
        "MANAGER", new List<string>
        {
          "SYSTEM",
          "SYSTEM:USER",
          "SYSTEM:NOTICE"
        }
      }
    };

    foreach (var rm in roleMenus)
    {
      if (!roleIds.ContainsKey(rm.Key))
      {
        continue;
      }

      var roleId = roleIds[rm.Key];

      foreach (var menuCode in rm.Value)
      {
        if (!menuIds.ContainsKey(menuCode))
        {
          continue;
        }

        var menuId = menuIds[menuCode];

        var existingRoleMenu = db.Queryable<LeanRoleMenu>()
          .Where(x => x.RoleId == roleId && x.MenuId == menuId)
          .First();

        if (existingRoleMenu == null)
        {
          var roleMenu = new LeanRoleMenu
          {
            RoleId = roleId,
            MenuId = menuId,
            CreateTime = DateTime.Now,
            CreateBy = 0
          };

          db.Insertable(roleMenu).ExecuteCommand();
          inserted++;
        }
      }
    }

    return (inserted, updated);
  }
}