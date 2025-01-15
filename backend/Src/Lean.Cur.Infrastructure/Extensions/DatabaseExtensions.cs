using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Entities.Logging;
using Lean.Cur.Infrastructure.Database;
using Lean.Cur.Infrastructure.Interceptors;
using Microsoft.Extensions.Configuration;
using Lean.Cur.Common.Enums;
using Microsoft.Extensions.Logging;
using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Entities.Logging;
using Lean.Cur.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;

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

        // 配置SQL差异日志拦截器
        var sqlLogInterceptor = sp.GetRequiredService<SqlLogInterceptor>();
        db.Aop.DataExecuting = sqlLogInterceptor.OnExecutingChanging;

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
      logger.LogInformation("- LeanRole (角色表)");
      logger.LogInformation("- LeanRoleMenu (角色菜单关联表)");
      logger.LogInformation("- LeanUser (用户表)");
      logger.LogInformation("- LeanUserRole (用户角色关联表)");
      logger.LogInformation("- LeanMenu (菜单表)");
      logger.LogInformation("- LeanDept (部门表)");
      logger.LogInformation("- LeanPost (岗位表)");
      logger.LogInformation("- LeanNotice (通知表)");
      logger.LogInformation("- LeanSqlLog (SQL差异日志表)");
      logger.LogInformation("- LeanOperationLog (操作日志表)");
      logger.LogInformation("- LeanLoginLog (登录日志表)");
      logger.LogInformation("- LeanAuditLog (审计日志表)");

      // 获取所有需要初始化的表
      var tables = new[]
      {
            typeof(LeanUser),
            typeof(LeanUserExtend),
            typeof(LeanUserDevice),
            typeof(LeanRole),
            typeof(LeanRoleMenu),
            typeof(LeanUserRole),
            typeof(LeanMenu),
            typeof(LeanDept),
            typeof(LeanPost),
            typeof(LeanNotice),
            typeof(LeanSqlLog),
            typeof(LeanOperationLog),
            typeof(LeanLoginLog),
            typeof(LeanAuditLog)
        };

      // 使用CodeFirst初始化所有表
      foreach (var table in tables)
      {
        var tableName = db.EntityMaintenance.GetTableName(table);
        logger.LogInformation($"正在初始化表{tableName}...");

        // 如果表不存在，直接创建
        if (!db.DbMaintenance.IsAnyTable(tableName))
        {
          db.CodeFirst.InitTables(table);
          logger.LogInformation($"表{tableName}创建成功");
        }
        else
        {
          // 如果表存在，则更新表结构
          db.CodeFirst.InitTables(table);
          logger.LogInformation($"表{tableName}结构已更新");
        }
      }
    }

    /// <summary>
    /// 初始化基础数据
    /// </summary>
    private static void InitBaseData(ISqlSugarClient db, ILogger logger)
    {
      try
      {
        // 检查是否已经初始化
        logger.LogInformation("正在检查是否需要初始化基础数据...");
        if (db.Queryable<LeanUser>().Any())
        {
          logger.LogInformation("检测到已存在基础数据,跳过初始化");
          return;
        }

        // 调用种子数据初始化
        logger.LogInformation("开始初始化种子数据...");
        try
        {
          LeanDbSeed.Initialize(db);
          logger.LogInformation("种子数据初始化完成");
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "初始化种子数据时发生错误");
          throw;
        }

        logger.LogInformation("基础数据初始化完成");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "初始化基础数据失败");
        throw;
      }
    }
  }
}