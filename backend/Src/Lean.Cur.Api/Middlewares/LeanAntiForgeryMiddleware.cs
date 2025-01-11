/**
 * @description CSRF防护中间件
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Api.Middlewares;

/// <summary>
/// CSRF防护中间件
/// </summary>
public class LeanAntiForgeryMiddleware
{
  private readonly RequestDelegate _next;
  private readonly IAntiforgery _antiforgery;
  private readonly ILogger<LeanAntiForgeryMiddleware> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanAntiForgeryMiddleware(RequestDelegate next, IAntiforgery antiforgery, ILogger<LeanAntiForgeryMiddleware> logger)
  {
    _next = next;
    _antiforgery = antiforgery;
    _logger = logger;
  }

  /// <summary>
  /// 处理请求
  /// </summary>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      // 生成防伪令牌
      var tokens = _antiforgery.GetAndStoreTokens(context);
      context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
          new CookieOptions
          {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict
          });

      // 检查是否需要验证
      if (ShouldValidate(context))
      {
        try
        {
          await _antiforgery.ValidateRequestAsync(context);
        }
        catch (AntiforgeryValidationException ex)
        {
          _logger.LogWarning(ex, "CSRF验证失败");
          context.Response.StatusCode = StatusCodes.Status400BadRequest;
          await context.Response.WriteAsJsonAsync(new { message = "CSRF验证失败" });
          return;
        }
      }

      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "CSRF中间件处理失败");
      throw;
    }
  }

  private bool ShouldValidate(HttpContext context)
  {
    // 只验证非GET、HEAD、OPTIONS和TRACE请求
    var method = context.Request.Method;
    return !HttpMethods.IsGet(method) &&
           !HttpMethods.IsHead(method) &&
           !HttpMethods.IsOptions(method) &&
           !HttpMethods.IsTrace(method);
  }
}

/// <summary>
/// CSRF中间件扩展方法
/// </summary>
public static class LeanAntiForgeryMiddlewareExtensions
{
  /// <summary>
  /// 使用CSRF防护中间件
  /// </summary>
  public static IApplicationBuilder UseLeanAntiForgeryMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<LeanAntiForgeryMiddleware>();
  }
}