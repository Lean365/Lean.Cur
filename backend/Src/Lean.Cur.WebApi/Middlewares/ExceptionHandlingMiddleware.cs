using System.Net;
using System.Text.Json;
using Lean.Cur.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.WebApi.Middlewares;

/// <summary>
/// 异常处理中间件
/// </summary>
public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="next">请求委托</param>
  /// <param name="logger">日志接口</param>
  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  /// <summary>
  /// 调用
  /// </summary>
  /// <param name="context">HTTP上下文</param>
  /// <returns>任务</returns>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";
    var response = new { message = exception.Message };

    switch (exception)
    {
      case LeanException:
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        break;
      default:
        _logger.LogError(exception, "An unhandled exception has occurred.");
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        response = new { message = "An error occurred while processing your request." };
        break;
    }

    var json = JsonSerializer.Serialize(response);
    await context.Response.WriteAsync(json);
  }
}