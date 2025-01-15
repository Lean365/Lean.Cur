using Lean.Cur.Application.Services.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System.Security.Claims;

namespace Lean.Cur.Infrastructure.Interceptors;

/// <summary>
/// SQL差异日志拦截器
/// </summary>
public class SqlLogInterceptor
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IServiceProvider _serviceProvider;
  private readonly ILogger<SqlLogInterceptor> _logger;

  public SqlLogInterceptor(
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider serviceProvider,
    ILogger<SqlLogInterceptor> logger)
  {
    _httpContextAccessor = httpContextAccessor;
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  /// <summary>
  /// 数据更新前
  /// </summary>
  public void OnExecutingChanging(object oldValue, DataFilterModel entityInfo)
  {
    // 如果是日志表或者是初始化种子数据，跳过记录
    if (entityInfo.EntityName.StartsWith("lean_mon_") ||
        (entityInfo.EntityValue is LeanBaseEntity baseEntity && baseEntity.CreateBy == 0))
    {
      return;
    }

    string tableName = "Unknown";
    string operationType = "Unknown";
    string? primaryKeyValue = null;
    string? userName = null;
    long userId = 0;

    try
    {
      // 获取用户信息
      var httpContext = _httpContextAccessor.HttpContext;

      // 如果没有HttpContext，使用系统默认值
      if (httpContext == null)
      {
        userId = 0; // 系统用户ID
        userName = "System"; // 系统用户名
      }
      else
      {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userNameClaim = httpContext.User.FindFirst(ClaimTypes.Name);

        if (userIdClaim == null || userNameClaim == null)
        {
          userId = 0;
          userName = "System";
        }
        else if (!long.TryParse(userIdClaim.Value, out userId))
        {
          userId = 0;
          userName = "System";
        }
        else
        {
          userName = userNameClaim.Value;
        }
      }

      // 获取表名和主键值
      tableName = entityInfo.EntityName;
      var primaryKeyProperty = entityInfo.EntityValue.GetType().GetProperties()
        .FirstOrDefault(p => p.GetCustomAttributes(typeof(SugarColumn), false)
          .Any(a => ((SugarColumn)a).IsPrimaryKey));

      if (primaryKeyProperty == null)
      {
        _logger.LogError("记录SQL差异日志失败: 无法获取主键属性, 表名={TableName}, 操作类型={OperationType}, 用户={UserName}",
          tableName,
          entityInfo.OperationType,
          userName);
        return;
      }

      var primaryKeyName = primaryKeyProperty.Name;
      primaryKeyValue = primaryKeyProperty.GetValue(entityInfo.EntityValue)?.ToString();
      if (string.IsNullOrEmpty(primaryKeyValue))
      {
        _logger.LogError("记录SQL差异日志失败: 无法获取主键值, 表名={TableName}, 操作类型={OperationType}, 用户={UserName}",
          tableName,
          entityInfo.OperationType,
          userName);
        return;
      }

      // 获取操作类型
      operationType = entityInfo.OperationType switch
      {
        DataFilterType.InsertByObject => "Insert",
        DataFilterType.UpdateByObject => "Update",
        DataFilterType.DeleteByObject => "Delete",
        _ => "Unknown"
      };

      // 获取变更前数据
      string? beforeData = null;
      if (operationType != "Insert")
      {
        try
        {
          beforeData = JsonConvert.SerializeObject(oldValue);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "记录SQL差异日志失败: 序列化变更前数据失败, 表名={TableName}, 主键={PrimaryKey}, 操作类型={OperationType}, 用户={UserName}",
            tableName,
            primaryKeyValue,
            operationType,
            userName);
          return;
        }
      }

      // 获取变更后数据
      string? afterData = null;
      if (operationType != "Delete")
      {
        try
        {
          afterData = JsonConvert.SerializeObject(entityInfo.EntityValue);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "记录SQL差异日志失败: 序列化变更后数据失败, 表名={TableName}, 主键={PrimaryKey}, 操作类型={OperationType}, 用户={UserName}",
            tableName,
            primaryKeyValue,
            operationType,
            userName);
          return;
        }
      }

      // 创建新的作用域来解析作用域服务
      using var scope = _serviceProvider.CreateScope();
      var sqlLogService = scope.ServiceProvider.GetRequiredService<ISqlLogService>();

      // 记录SQL差异日志
      sqlLogService.AddSqlLogAsync(
        tableName,
        primaryKeyValue,
        operationType,
        beforeData,
        afterData,
        userId,
        userName,
        httpContext ?? new DefaultHttpContext(),
        "系统自动记录").GetAwaiter().GetResult();

      _logger.LogInformation(
        "记录SQL差异日志成功: 表名={TableName}, 主键={PrimaryKey}, 操作类型={OperationType}, 操作人={UserName}",
        tableName,
        primaryKeyValue,
        operationType,
        userName);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex,
        "记录SQL差异日志失败: 表名={TableName}, 主键={PrimaryKey}, 操作类型={OperationType}, 用户={UserName}",
        tableName,
        primaryKeyValue,
        operationType,
        userName);
    }
  }
}