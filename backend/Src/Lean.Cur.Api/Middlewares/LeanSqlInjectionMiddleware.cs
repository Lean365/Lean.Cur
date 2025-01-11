/**
 * @description SQL注入防护中间件
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Api.Middlewares;

/// <summary>
/// SQL注入防护中间件
/// </summary>
public class LeanSqlInjectionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LeanSqlInjectionMiddleware> _logger;
  private readonly Regex _sqlInjectionRegex;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanSqlInjectionMiddleware(RequestDelegate next, ILogger<LeanSqlInjectionMiddleware> logger)
  {
    _next = next;
    _logger = logger;

    // SQL注入检测正则表达式
    _sqlInjectionRegex = new Regex(
        @"(\b(select|insert|update|delete|drop|truncate|exec|declare|union|create|alter)\b)|(-{2})|(/\*.*\*/)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
  }

  /// <summary>
  /// 处理请求
  /// </summary>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      // 检查查询字符串
      var queryString = context.Request.QueryString.Value;
      if (!string.IsNullOrEmpty(queryString) && ContainsSqlInjection(queryString))
      {
        _logger.LogWarning($"检测到查询字符串中的SQL注入尝试: {queryString}");
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { message = "检测到潜在的SQL注入攻击" });
        return;
      }

      // 检查请求体
      if (context.Request.HasJsonContentType())
      {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        if (ContainsSqlInjection(body))
        {
          _logger.LogWarning($"检测到请求体中的SQL注入尝试: {body}");
          context.Response.StatusCode = StatusCodes.Status400BadRequest;
          await context.Response.WriteAsJsonAsync(new { message = "检测到潜在的SQL注入攻击" });
          return;
        }
      }

      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "SQL注入检测失败");
      throw;
    }
  }

  private bool ContainsSqlInjection(string input)
  {
    if (string.IsNullOrEmpty(input))
    {
      return false;
    }

    // 检查SQL注入特征
    if (_sqlInjectionRegex.IsMatch(input))
    {
      return true;
    }

    // 检查常见的SQL注入字符
    var suspiciousChars = new[] { "';", "--;", "/*", "*/", "@@", "@", "char", "nchar", "varchar", "nvarchar", "alter", "begin", "cast", "create", "cursor", "declare", "delete", "drop", "end", "exec", "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects", "syscolumns", "table", "update" };
    return suspiciousChars.Any(c => input.Contains(c, StringComparison.OrdinalIgnoreCase));
  }
}