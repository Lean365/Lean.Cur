/**
 * @description 操作日志中间件
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using Microsoft.AspNetCore.Http;
using Lean.Cur.Domain.Entities.Log;
using Lean.Cur.Infrastructure.Database;

namespace Lean.Cur.Api.Middlewares;

/// <summary>
/// 操作日志中间件
/// </summary>
public class LeanOperationLogMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LeanOperationLogMiddleware> _logger;
  private readonly LeanDbContext _dbContext;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanOperationLogMiddleware(RequestDelegate next, ILogger<LeanOperationLogMiddleware> logger, LeanDbContext dbContext)
  {
    _next = next;
    _logger = logger;
    _dbContext = dbContext;
  }

  /// <summary>
  /// 处理请求
  /// </summary>
  public async Task InvokeAsync(HttpContext context)
  {
    var startTime = DateTime.Now;
    var requestBody = string.Empty;

    try
    {
      // 记录请求体
      if (context.Request.ContentLength > 0)
      {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        requestBody = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
      }

      // 执行下一个中间件
      await _next(context);

      // 记录操作日志
      var operationLog = new LeanOperationLog
      {
        RequestMethod = context.Request.Method,
        RequestPath = context.Request.Path,
        RequestBody = requestBody,
        ResponseStatus = context.Response.StatusCode,
        ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds,
        UserAgent = context.Request.Headers["User-Agent"].ToString(),
        IpAddress = context.Connection.RemoteIpAddress?.ToString(),
        UserId = GetCurrentUserId(context)
      };

      await _dbContext.OperationLogs.AddAsync(operationLog);
      await _dbContext.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "记录操作日志失败");
      throw;
    }
  }

  private long? GetCurrentUserId(HttpContext context)
  {
    // TODO: 从Token或Session中获取用户ID
    return null;
  }
}