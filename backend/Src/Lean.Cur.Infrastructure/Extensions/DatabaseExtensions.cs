using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Lean.Cur.Common.Enums;
using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Entities.Logging;
using Lean.Cur.Infrastructure.Database;

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
      var connectionString = configuration.GetConnectionString("Default");

      // 配置 SqlSugar
      services.AddSingleton<SqlSugarClient>(sp =>
      {
        var db = new SqlSugarClient(new ConnectionConfig()
        {
          ConnectionString = connectionString,
          DbType = DbType.SqlServer,
          IsAutoCloseConnection = true,
          InitKeyType = InitKeyType.Attribute,
          MoreSettings = new ConnMoreSettings()
          {
            IsWithNoLockQuery = true,  // 查询默认设置为 NoLock
            SqlServerCodeFirstNvarchar = false // 使用实体类中指定的列类型
          }
        });

        // 配置 SQL 执行 AOP
        db.Aop.OnLogExecuting = (sql, parameters) =>
        {
          var logService = sp.GetService<ILogger<SqlSugarClient>>();
          // 将SQL执行日志以Debug级别记录（通常写入文件）
          logService?.LogDebug($"SQL: {sql}");
        };

        // 配置错误日志
        db.Aop.OnError = (ex) =>
        {
          var logService = sp.GetService<ILogger<SqlSugarClient>>();
          logService?.LogError(ex, "SQL执行错误");
        };

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
      var configuration = serviceProvider.GetRequiredService<IConfiguration>();
      var db = serviceProvider.GetRequiredService<SqlSugarClient>();

      try
      {
        // 检查是否需要初始化数据库
        var shouldInit = configuration.GetValue<bool>("DatabaseSettings:InitDb");
        if (!shouldInit)
        {
          logger.LogInformation("数据库初始化已禁用");
          return;
        }

        // 创建数据库
        db.DbMaintenance.CreateDatabase();
        logger.LogInformation("数据库创建/验证成功");

        // 连接数据库
        db.Open();
        logger.LogInformation("数据库连接成功");

        // 初始化表结构
        InitTables(db, logger);
        logger.LogInformation("数据表初始化完成");

        // 初始化基础数据（无论表是否重建，都执行种子数据初始化）
        InitBaseData(db, configuration, logger);
        logger.LogInformation("基础数据初始化完成");
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "数据库初始化失败");
        throw;
      }
    }

    /// <summary>
    /// 配置全局过滤器
    /// </summary>
    private static void ConfigureGlobalFilter(SqlSugarClient db)
    {
      // 配置软删除过滤器
      db.QueryFilter.AddTableFilter<LeanBaseEntity>(it => it.IsDeleted == 0);
    }

    /// <summary>
    /// 初始化表结构
    /// </summary>
    private static void InitTables(SqlSugarClient db, ILogger logger)
    {
      logger.LogInformation("准备初始化数据表...");

      // 定义C#类型到SQL类型的映射
      var typeMap = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
      {
        // 字符串类型
        { "String", new[] { "nvarchar", "varchar", "char", "nchar", "text", "ntext" } },
        { "Char", new[] { "char", "nchar" } },
        
        // 整数类型
        { "Byte", new[] { "tinyint" } },
        { "SByte", new[] { "smallint" } },
        { "Int16", new[] { "smallint" } },
        { "UInt16", new[] { "int" } },
        { "Int32", new[] { "int" } },
        { "UInt32", new[] { "bigint" } },
        { "Int64", new[] { "bigint" } },
        { "UInt64", new[] { "decimal" } },

        // 浮点数类型
        { "Single", new[] { "real" } },
        { "Double", new[] { "float" } },
        { "Decimal", new[] { "decimal", "numeric", "money", "smallmoney" } },

        // 日期时间类型
        { "DateTime", new[] { "datetime", "datetime2", "smalldatetime", "date" } },
        { "DateTimeOffset", new[] { "datetimeoffset" } },
        { "TimeSpan", new[] { "time" } },

        // 布尔类型
        { "Boolean", new[] { "bit" } },

        // 二进制类型
        { "Byte[]", new[] { "binary", "varbinary", "image" } },

        // 其他类型
        { "Guid", new[] { "uniqueidentifier" } },
        { "Object", new[] { "sql_variant" } },
        { "TimeOnly", new[] { "time" } },
        { "DateOnly", new[] { "date" } },
        { "XmlDocument", new[] { "xml" } }
      };

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
        logger.LogInformation($"正在检查表{tableName}...");

        // 如果表存在，检查表结构是否有变化
        if (db.DbMaintenance.IsAnyTable(tableName))
        {
          // 获取当前表的所有列信息
          var tableColumns = db.DbMaintenance.GetColumnInfosByTableName(tableName);

          // 获取实体的所有属性信息
          var entityInfo = db.EntityMaintenance.GetEntityInfo(table);

          // 获取数据库列（被标记为 SugarColumn 的属性）
          var entityColumns = entityInfo.Columns
              .Where(c => !c.IsIgnore)
              .ToList();

          // 获取导航属性
          var navigateProperties = entityInfo.Columns
              .Where(c => c.PropertyInfo.GetCustomAttributes(true).Any(a => a.GetType().Name == "NavigateAttribute"))
              .ToList();

          bool needRebuild = false;

          // 检查导航属性相关的外键列
          foreach (var navProperty in navigateProperties)
          {
            var navigateAttr = navProperty.PropertyInfo
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType().Name == "NavigateAttribute") as dynamic;

            if (navigateAttr != null)
            {
              // 获取外键列名
              string foreignKeyName = navigateAttr.Name?.ToString() ?? string.Empty;
              if (!string.IsNullOrEmpty(foreignKeyName))
              {
                // 检查外键列是否存在
                var foreignKeyColumn = tableColumns.FirstOrDefault(x =>
                    x.DbColumnName.Equals(foreignKeyName, StringComparison.OrdinalIgnoreCase));

                if (foreignKeyColumn == null)
                {
                  needRebuild = true;
                  logger.LogInformation($"表{tableName}缺少导航属性{navProperty.PropertyName}的外键列: {foreignKeyName}");
                  break;
                }
              }
            }
          }

          // 如果不需要因为导航属性重建，则检查普通列
          if (!needRebuild)
          {
            // 检查是否有新增或修改的列
            foreach (var entityColumn in entityColumns)
            {
              var dbColumn = tableColumns.FirstOrDefault(x =>
                  x.DbColumnName.Equals(entityColumn.DbColumnName, StringComparison.OrdinalIgnoreCase));

              // 如果列不存在
              if (dbColumn == null)
              {
                needRebuild = true;
                logger.LogInformation($"表{tableName}新增列: {entityColumn.DbColumnName}");
                break;
              }
              else
              {
                bool columnChanged = false;

                // 检查列名是否变化
                if (!dbColumn.DbColumnName.Equals(entityColumn.DbColumnName, StringComparison.OrdinalIgnoreCase))
                {
                  columnChanged = true;
                  logger.LogInformation($"表{tableName}列名变更: {dbColumn.DbColumnName} -> {entityColumn.DbColumnName}");
                }

                // 检查列描述是否变化
                if (dbColumn.ColumnDescription != entityColumn.ColumnDescription)
                {
                  columnChanged = true;
                  logger.LogInformation($"表{tableName}列{entityColumn.DbColumnName}描述: {dbColumn.ColumnDescription} -> {entityColumn.ColumnDescription}");
                }

                // 检查数据类型是否变化
                if (!string.IsNullOrEmpty(entityColumn.DataType))
                {
                  var specifiedType = entityColumn.DataType.Split('(')[0]; // 移除长度信息
                  if (!dbColumn.DataType.Equals(specifiedType, StringComparison.OrdinalIgnoreCase))
                  {
                    columnChanged = true;
                    logger.LogInformation($"表{tableName}列{entityColumn.DbColumnName}类型: {dbColumn.DataType} -> {specifiedType}");
                  }
                }

                // 检查长度是否变化
                if (entityColumn.Length > 0 && dbColumn.Length != entityColumn.Length)
                {
                  columnChanged = true;
                  logger.LogInformation($"表{tableName}列{entityColumn.DbColumnName}长度: {dbColumn.Length} -> {entityColumn.Length}");
                }

                // 检查可空性是否变化
                if (dbColumn.IsNullable != entityColumn.IsNullable)
                {
                  columnChanged = true;
                  logger.LogInformation($"表{tableName}列{entityColumn.DbColumnName}可空性: {dbColumn.IsNullable} -> {entityColumn.IsNullable}");
                }

                // 检查小数位是否变化
                if (dbColumn.DecimalDigits != entityColumn.DecimalDigits &&
                    !dbColumn.DataType.Equals("datetime", StringComparison.OrdinalIgnoreCase) &&
                    !dbColumn.DataType.StartsWith("datetime2", StringComparison.OrdinalIgnoreCase))
                {
                  columnChanged = true;
                  logger.LogInformation($"表{tableName}列{entityColumn.DbColumnName}小数位: {dbColumn.DecimalDigits} -> {entityColumn.DecimalDigits}");
                }

                // 检查主键是否变化
                if (dbColumn.IsPrimarykey != entityColumn.IsPrimarykey)
                {
                  columnChanged = true;
                  logger.LogInformation($"表{tableName}列{entityColumn.DbColumnName}主键: {dbColumn.IsPrimarykey} -> {entityColumn.IsPrimarykey}");
                }

                if (columnChanged)
                {
                  needRebuild = true;
                  break;
                }
              }
            }
          }

          // 如果还不需要重建，检查是否有需要删除的列
          if (!needRebuild)
          {
            foreach (var dbColumn in tableColumns)
            {
              // 尝试通过列名或属性名找到对应的实体列
              var entityColumn = entityColumns.FirstOrDefault(x =>
                  x.DbColumnName.Equals(dbColumn.DbColumnName, StringComparison.OrdinalIgnoreCase) ||
                  x.PropertyName.Equals(dbColumn.DbColumnName, StringComparison.OrdinalIgnoreCase));

              if (entityColumn == null)
              {
                // 再次检查是否是导航属性的外键列
                var isNavigationColumn = navigateProperties.Any(nav =>
                {
                  var attr = nav.PropertyInfo.GetCustomAttributes(true)
                      .FirstOrDefault(a => a.GetType().Name == "NavigateAttribute") as dynamic;
                  var foreignKeyName = attr?.Name?.ToString() ?? string.Empty;
                  return !string.IsNullOrEmpty(foreignKeyName) &&
                         foreignKeyName.Equals(dbColumn.DbColumnName, StringComparison.OrdinalIgnoreCase);
                });

                if (!isNavigationColumn)
                {
                  needRebuild = true;
                  logger.LogInformation($"表{tableName}删除列: {dbColumn.DbColumnName}");
                  break;
                }
              }
            }
          }

          // 只有在需要重建时才重建表
          if (needRebuild)
          {
            logger.LogInformation($"开始重建表{tableName}...");
            // 将表删除和创建的SQL以Debug级别记录到文件
            logger.LogDebug($"删除表: DROP TABLE [{tableName}]");
            db.DbMaintenance.DropTable(tableName);
            db.CodeFirst.InitTables(table);
            logger.LogInformation($"表{tableName}重建完成");
          }
          else
          {
            logger.LogInformation($"表{tableName}结构未变化，无需重建");
          }
        }
        else
        {
          // 表不存在，直接创建
          logger.LogInformation($"创建新表{tableName}...");
          db.CodeFirst.InitTables(table);
          logger.LogInformation($"表{tableName}创建成功");
        }
      }
    }

    /// <summary>
    /// 初始化基础数据
    /// </summary>
    private static void InitBaseData(SqlSugarClient db, IConfiguration configuration, ILogger logger)
    {
      try
      {
        // 开启事务
        db.BeginTran();

        // 记录每个表的更新统计
        var stats = new Dictionary<string, (int inserted, int updated, int deleted)>();

        // 初始化种子数据，并统计更新情况
        LeanDbSeed.Initialize(db, logger, stats);

        // 提交事务
        db.CommitTran();

        // 输出统计信息
        foreach (var stat in stats)
        {
          if (stat.Value.inserted > 0 || stat.Value.updated > 0 || stat.Value.deleted > 0)
          {
            logger.LogInformation($"表{stat.Key}数据更新: 新增{stat.Value.inserted}条，修改{stat.Value.updated}条，删除{stat.Value.deleted}条");
          }
        }
      }
      catch (Exception ex)
      {
        // 回滚事务
        db.RollbackTran();
        throw new Exception("初始化基础数据失败", ex);
      }
    }
  }
}