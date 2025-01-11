/**
 * @description 全局异常处理中间件
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Models;

namespace Lean.Cur.Api.Middlewares;

/// <summary>
/// 全局异常处理中间件
/// </summary>
public class LeanGlobalExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LeanGlobalExceptionMiddleware> _logger;
  private readonly IWebHostEnvironment _env;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanGlobalExceptionMiddleware(
      RequestDelegate next,
      ILogger<LeanGlobalExceptionMiddleware> logger,
      IWebHostEnvironment env)
  {
    _next = next;
    _logger = logger;
    _env = env;
  }

  /// <summary>
  /// 处理请求
  /// </summary>
  public async Task InvokeAsync(HttpContext context)
  {
    var startTime = DateTime.Now;
    var requestId = Guid.NewGuid().ToString("N");
    var requestPath = context.Request.Path;
    var requestMethod = context.Request.Method;
    var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    var userAgent = context.Request.Headers["User-Agent"].ToString();

    try
    {
      context.Request.EnableBuffering();
      var requestBody = await GetRequestBodyAsync(context.Request);

      await _next(context);

      // 记录正常请求日志
      var duration = (long)(DateTime.Now - startTime).TotalMilliseconds;
      var response = new LeanApiResponse
      {
        Code = LeanErrorCode.Success,
        Message = "请求成功",
        BusinessType = GetBusinessType(context),
        Path = requestPath,
        Method = requestMethod,
        Params = requestBody,
        Duration = duration,
        RequestId = requestId,
        ClientIp = clientIp,
        UserAgent = userAgent,
        OperatorId = GetCurrentUserId(context),
        OperatorName = GetCurrentUserName(context)
      };

      _logger.LogInformation("请求处理成功: {@Response}", response);
    }
    catch (Exception ex)
    {
      var duration = (long)(DateTime.Now - startTime).TotalMilliseconds;
      await HandleExceptionAsync(context, ex, duration, requestId, requestPath, requestMethod, clientIp, userAgent);
    }
  }

  private async Task HandleExceptionAsync(
      HttpContext context,
      Exception ex,
      long duration,
      string requestId,
      string requestPath,
      string requestMethod,
      string clientIp,
      string userAgent)
  {
    _logger.LogError(ex, "请求处理失败: {RequestId}", requestId);

    var response = new LeanApiResponse
    {
      Code = GetErrorCode(ex),
      Message = GetErrorMessage(ex),
      BusinessType = GetBusinessType(context),
      Path = requestPath,
      Method = requestMethod,
      Duration = duration,
      RequestId = requestId,
      ClientIp = clientIp,
      UserAgent = userAgent,
      OperatorId = GetCurrentUserId(context),
      OperatorName = GetCurrentUserName(context)
    };

    // 在开发环境下返回详细错误信息
    if (_env.IsDevelopment())
    {
      response.StackTrace = ex.StackTrace;
    }

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = GetHttpStatusCode(response.Code);

    var jsonOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = true
    };

    await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
  }

  private static async Task<string> GetRequestBodyAsync(HttpRequest request)
  {
    if (!request.Body.CanRead || request.ContentLength == null || request.ContentLength == 0)
    {
      return string.Empty;
    }

    try
    {
      using var reader = new StreamReader(request.Body, leaveOpen: true);
      var body = await reader.ReadToEndAsync();
      request.Body.Position = 0;
      return body;
    }
    catch
    {
      return string.Empty;
    }
  }

  private static LeanErrorCode GetErrorCode(Exception ex)
  {
    return ex switch
    {
      UnauthorizedAccessException => LeanErrorCode.Unauthorized,
      ArgumentException => LeanErrorCode.BadRequest,
      InvalidOperationException => LeanErrorCode.BadRequest,
      KeyNotFoundException => LeanErrorCode.NotFound,
      NotImplementedException => LeanErrorCode.ServerError,
      TimeoutException => LeanErrorCode.RequestTimeout,
      _ => LeanErrorCode.ServerError
    };
  }

  private static string GetErrorMessage(Exception ex)
  {
    return ex switch
    {
      UnauthorizedAccessException => "未经授权的访问",
      ArgumentException => "请求参数错误",
      InvalidOperationException => "无效的操作",
      KeyNotFoundException => "请求的资源不存在",
      NotImplementedException => "请求的功能尚未实现",
      TimeoutException => "请求超时",
      _ => "服务器内部错误"
    };
  }

  private static int GetHttpStatusCode(LeanErrorCode errorCode)
  {
    return errorCode switch
    {
      LeanErrorCode.Success => (int)HttpStatusCode.OK,
      LeanErrorCode.BadRequest => (int)HttpStatusCode.BadRequest,
      LeanErrorCode.Unauthorized => (int)HttpStatusCode.Unauthorized,
      LeanErrorCode.Forbidden => (int)HttpStatusCode.Forbidden,
      LeanErrorCode.NotFound => (int)HttpStatusCode.NotFound,
      LeanErrorCode.MethodNotAllowed => (int)HttpStatusCode.MethodNotAllowed,
      LeanErrorCode.RequestTimeout => (int)HttpStatusCode.RequestTimeout,
      LeanErrorCode.TooManyRequests => (int)HttpStatusCode.TooManyRequests,
      LeanErrorCode.ServerError => (int)HttpStatusCode.InternalServerError,
      LeanErrorCode.ServiceUnavailable => (int)HttpStatusCode.ServiceUnavailable,
      LeanErrorCode.GatewayTimeout => (int)HttpStatusCode.GatewayTimeout,
      _ => (int)HttpStatusCode.InternalServerError
    };
  }

  private static LeanBusinessType GetBusinessType(HttpContext context)
  {
    var endpoint = context.GetEndpoint();
    if (endpoint == null)
    {
      return LeanBusinessType.Other;
    }

    var method = context.Request.Method.ToUpper();
    var path = context.Request.Path.Value?.ToLower() ?? string.Empty;

    // 根据HTTP方法和路径判断业务类型
    return (method, path) switch
    {
      ("GET", _) when path.EndsWith("/export") => LeanBusinessType.Export,
      ("POST", _) when path.EndsWith("/import") => LeanBusinessType.Import,
      ("POST", _) when path.Contains("/login") => LeanBusinessType.Login,
      ("POST", _) when path.Contains("/logout") => LeanBusinessType.Logout,
      ("POST", _) when path.Contains("/register") => LeanBusinessType.Register,
      ("POST", _) when path.Contains("/reset-password") => LeanBusinessType.ResetPassword,
      ("POST", _) when path.Contains("/change-password") => LeanBusinessType.ChangePassword,
      ("GET", _) => LeanBusinessType.Query,
      ("POST", _) => LeanBusinessType.Insert,
      ("PUT", _) => LeanBusinessType.Update,
      ("DELETE", _) => LeanBusinessType.Delete,
      _ => LeanBusinessType.Other
    };
  }

  private static long? GetCurrentUserId(HttpContext context)
  {
    var userIdClaim = context.User.FindFirst("sub");
    return userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId) ? userId : null;
  }

  private static string? GetCurrentUserName(HttpContext context)
  {
    return context.User.Identity?.Name;
  }
}