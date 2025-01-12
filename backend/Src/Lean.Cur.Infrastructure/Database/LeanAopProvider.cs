using SqlSugar;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Lean.Cur.Infrastructure.Database
{
  /// <summary>
  /// SqlSugar AOP提供者
  /// </summary>
  public class LeanAopProvider
  {
    private readonly ILogger<LeanAopProvider> _logger;

    public LeanAopProvider(ILogger<LeanAopProvider> logger)
    {
      _logger = logger;
    }

    /// <summary>
    /// 配置AOP
    /// </summary>
    public void ConfigureAop(ISqlSugarClient db)
    {
      // SQL执行前
      db.Aop.OnLogExecuting = (sql, parameters) =>
      {
        // 对SQL语句进行脱敏
        var maskedSql = MaskSensitiveInfo(sql);

        string paramStr = "无参数";
        if (parameters != null && parameters.Length > 0)
        {
          string[] paramArray = new string[parameters.Length];
          for (int i = 0; i < parameters.Length; i++)
          {
            var paramName = parameters[i].ParameterName;
            var paramValue = MaskSensitiveParamValue(paramName, parameters[i].Value?.ToString());
            paramArray[i] = $"{paramName}={paramValue}";
          }
          paramStr = string.Join(", ", paramArray);
        }
        _logger.LogInformation("\n【SQL语句】: {0} \n【参数】: {1}", maskedSql, paramStr);
      };

      // SQL执行后
      db.Aop.OnLogExecuted = (sql, parameters) =>
      {
        _logger.LogDebug("SQL执行耗时: {0}ms", db.Ado.SqlExecutionTime);
      };

      // 异常处理
      db.Aop.OnError = (exp) =>
      {
        var maskedMessage = MaskSensitiveInfo(exp.Message);
        _logger.LogError(exp, "SQL执行出错: {0}", maskedMessage);
      };

      // 数据执行前
      db.Aop.DataExecuting = (oldValue, entityInfo) =>
      {
        if (entityInfo.PropertyName == "UpdateTime")
        {
          entityInfo.SetValue(DateTime.Now);
        }
      };

      // 差异日志
      db.Aop.OnDiffLogEvent = diffLog =>
      {
        var maskedDiffLog = MaskSensitiveInfo(diffLog?.ToString());
        _logger.LogInformation("数据变更: {0}", maskedDiffLog);
      };
    }

    /// <summary>
    /// 对敏感信息进行脱敏
    /// </summary>
    private string MaskSensitiveInfo(string input)
    {
      if (string.IsNullOrEmpty(input)) return input;

      // 密码脱敏
      input = Regex.Replace(input, @"Password=([^;]*)", "Password=******", RegexOptions.IgnoreCase);
      input = Regex.Replace(input, @"pwd=([^;]*)", "pwd=******", RegexOptions.IgnoreCase);

      // 数据库连接信息脱敏
      input = Regex.Replace(input, @"Server=([^;]*)", "Server=******", RegexOptions.IgnoreCase);
      input = Regex.Replace(input, @"Database=([^;]*)", "Database=******", RegexOptions.IgnoreCase);
      input = Regex.Replace(input, @"User Id=([^;]*)", "User Id=******", RegexOptions.IgnoreCase);
      input = Regex.Replace(input, @"Uid=([^;]*)", "Uid=******", RegexOptions.IgnoreCase);

      // 手机号脱敏（保留前8位，后3位替换为*）
      input = Regex.Replace(input, @"1\d{9}(\d{2})", "1$1***");

      // 邮箱脱敏（用户名部分最后3位替换为*）
      input = Regex.Replace(input, @"([a-zA-Z0-9_-]+)([a-zA-Z0-9_-]{3})@", "$1***@");

      // 用户名脱敏（最后3位替换为*）
      input = Regex.Replace(input, @"UserName=([a-zA-Z0-9_-]+)([a-zA-Z0-9_-]{3})[^;]*", "UserName=$1***");

      return input;
    }

    /// <summary>
    /// 对敏感参数值进行脱敏
    /// </summary>
    private string MaskSensitiveParamValue(string paramName, string paramValue)
    {
      if (string.IsNullOrEmpty(paramValue)) return "null";

      var lowerParamName = paramName.ToLower();

      // 完全脱敏的参数
      if (lowerParamName.Contains("password") ||
          lowerParamName.Contains("pwd") ||
          lowerParamName.Contains("secret") ||
          lowerParamName.Contains("token") ||
          lowerParamName.Contains("key"))
      {
        return "******";
      }

      // 部分脱敏的参数
      if (lowerParamName.Contains("phone") || lowerParamName.Contains("mobile"))
      {
        // 手机号保留前8位，后3位替换为*
        return Regex.Replace(paramValue, @"1\d{9}(\d{2})", "1$1***");
      }

      if (lowerParamName.Contains("email"))
      {
        // 邮箱用户名部分最后3位替换为*
        return Regex.Replace(paramValue, @"([a-zA-Z0-9_-]+)([a-zA-Z0-9_-]{3})@", "$1***@");
      }

      if (lowerParamName.Contains("username"))
      {
        // 用户名最后3位替换为*
        if (paramValue.Length > 3)
        {
          return paramValue.Substring(0, paramValue.Length - 3) + "***";
        }
      }

      return paramValue;
    }
  }
}