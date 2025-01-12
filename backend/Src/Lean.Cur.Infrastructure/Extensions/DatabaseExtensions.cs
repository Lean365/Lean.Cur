using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Entities.Logging;
using Lean.Cur.Domain.Entities.Authorization;
using Lean.Cur.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Lean.Cur.Common.Enums;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Infrastructure.Extensions
{
  /// <summary>
  /// 数据库扩展方法
  /// </summary>
  public static class DatabaseExtensions
  {
    /// <summary>
    /// 添加数据库服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns></returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      // 注册 AOP 提供者
      services.AddSingleton<LeanAopProvider>();

      // 注册 SqlSugar 客户端为单例
      services.AddSingleton<ISqlSugarClient>(sp =>
      {
        var logger = sp.GetRequiredService<ILogger<SqlSugarClient>>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        logger.LogInformation("正在配置数据库连接: {ConnectionString}", connectionString);

        var db = new SqlSugarClient(new ConnectionConfig()
        {
          ConnectionString = connectionString,
          DbType = DbType.SqlServer,
          IsAutoCloseConnection = true,
          InitKeyType = InitKeyType.Attribute,
          ConfigureExternalServices = new ConfigureExternalServices
          {
            EntityService = (property, column) =>
            {
              if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
              {
                column.IsNullable = true;
              }
            }
          },
          // 启用SQL执行日志
          MoreSettings = new ConnMoreSettings
          {
            IsAutoRemoveDataCache = true,
            IsWithNoLockQuery = true
          }
        });

        // 配置 AOP
        var aop = sp.GetRequiredService<LeanAopProvider>();
        aop.ConfigureAop(db);

        // 配置全局过滤器
        ConfigureGlobalFilter(db);

        logger.LogInformation("数据库连接配置完成");
        return db;
      });

      return services;
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
      var logger = serviceProvider.GetRequiredService<ILogger<SqlSugarClient>>();
      var db = serviceProvider.GetRequiredService<ISqlSugarClient>();

      try
      {
        logger.LogInformation("开始初始化数据库...");

        // 创建数据库
        logger.LogInformation("正在创建数据库...");
        db.DbMaintenance.CreateDatabase();
        logger.LogInformation("数据库创建成功");

        // 初始化表结构
        logger.LogInformation("开始初始化数据表结构...");
        InitTables(db, logger);
        logger.LogInformation("数据表结构初始化完成");

        // 初始化基础数据
        logger.LogInformation("开始初始化基础数据...");
        InitBaseData(db, logger);
        logger.LogInformation("基础数据初始化完成");

        logger.LogInformation("数据库初始化完成");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "数据库初始化失败: {Message}", ex.Message);
        throw new Exception($"数据库初始化失败: {ex.Message}", ex);
      }
    }

    /// <summary>
    /// 配置全局过滤器
    /// </summary>
    private static void ConfigureGlobalFilter(ISqlSugarClient db)
    {
      // 配置软删除过滤器
      db.QueryFilter.AddTableFilter<LeanBaseEntity>(it => it.IsDeleted == 0);
    }

    /// <summary>
    /// 初始化表结构
    /// </summary>
    private static void InitTables(ISqlSugarClient db, ILogger logger)
    {
      logger.LogInformation("准备初始化以下数据表:");
      logger.LogInformation("- LeanUser (用户表)");
      logger.LogInformation("- LeanUserExtend (用户扩展信息表)");
      logger.LogInformation("- LeanUserDevice (用户设备信息表)");
      logger.LogInformation("- LeanRole (角色表)");
      logger.LogInformation("- LeanPermission (权限表)");
      logger.LogInformation("- LeanUserRole (用户角色关联表)");
      logger.LogInformation("- LeanRolePermission (角色权限关联表)");

      db.CodeFirst.InitTables(
        typeof(LeanUser),
        typeof(LeanUserExtend),
        typeof(LeanUserDevice),
        typeof(LeanRole),
        typeof(LeanPermission),
        typeof(LeanUserRole),
        typeof(LeanRolePermission)
      );
    }

    /// <summary>
    /// 初始化基础数据
    /// </summary>
    private static void InitBaseData(ISqlSugarClient db, ILogger logger)
    {
      // 检查是否已经初始化
      if (db.Queryable<LeanUser>().Any())
      {
        logger.LogInformation("检测到已存在基础数据,跳过初始化");
        return;
      }

      logger.LogInformation("开始创建管理员用户...");
      // 创建管理员用户
      var admin = new LeanUser
      {
        UserName = "admin",
        Password = "123456", // TODO: 需要加密
        NickName = "管理员",
        EnglishName = "Administrator",
        Email = "admin@lean.com",
        Phone = "13800138000",
        Status = (int)LeanStatus.Normal,
        UserType = (int)UserType.Admin,
        CreateTime = DateTime.Now,
        CreateBy = 0
      };
      db.Insertable(admin).ExecuteCommand();
      logger.LogInformation("管理员用户创建成功: {UserName}", admin.UserName);

      logger.LogInformation("开始创建管理员用户扩展信息...");
      // 创建管理员用户扩展信息
      var adminExtend = new LeanUserExtend
      {
        UserId = admin.Id,
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
      db.Insertable(adminExtend).ExecuteCommand();
      logger.LogInformation("管理员用户扩展信息创建成功");

      logger.LogInformation("开始创建管理员角色...");
      // 创建管理员角色
      var adminRole = new LeanRole
      {
        RoleCode = "admin",
        RoleName = "管理员",
        RoleType = 1,
        DataScope = 1,
        RoleSort = 1,
        Status = 1,
        CreateTime = DateTime.Now,
        CreateBy = 0
      };
      db.Insertable(adminRole).ExecuteCommand();
      logger.LogInformation("管理员角色创建成功: {RoleName}", adminRole.RoleName);

      logger.LogInformation("开始创建用户角色关联...");
      // 创建用户角色关联
      var userRole = new LeanUserRole
      {
        UserId = admin.Id,
        RoleId = adminRole.Id,
        CreateTime = DateTime.Now,
        CreateBy = 0
      };
      db.Insertable(userRole).ExecuteCommand();
      logger.LogInformation("用户角色关联创建成功");
    }
  }
}