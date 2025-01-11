/**
 * @description 接口限流中间件
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Lean.Cur.Api.Middlewares;

/// <summary>
/// 限流特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LeanRateLimitAttribute : Attribute
{
  /// <summary>
  /// 时间窗口（秒）
  /// </summary>
  public int Seconds { get; }

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRateLimitAttribute(int seconds = 1, int maxRequests = 10)
  {
    Seconds = seconds;
    MaxRequests = maxRequests;
  }
}

/// <summary>
/// 接口限流中间件
/// </summary>
public class LeanRateLimitMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LeanRateLimitMiddleware> _logger;
  private readonly IMemoryCache _cache;
  private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRateLimitMiddleware(RequestDelegate next, ILogger<LeanRateLimitMiddleware> logger, IMemoryCache cache)
  {
    _next = next;
    _logger = logger;
    _cache = cache;
    _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
  }

  /// <summary>
  /// 处理请求
  /// </summary>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      // 获取限流特性
      var endpoint = context.GetEndpoint();
      var rateLimit = endpoint?.Metadata.GetMetadata<LeanRateLimitAttribute>();

      if (rateLimit == null)
      {
        await _next(context);
        return;
      }

      // 获取客户端标识（IP + 路径 + 用户ID）
      var clientId = GetClientId(context);
      var cacheKey = $"ratelimit_{clientId}";

      // 使用信号量确保并发安全
      var semaphore = _semaphores.GetOrAdd(clientId, _ => new SemaphoreSlim(1, 1));
      await semaphore.WaitAsync();

      try
      {
        // 获取请求计数器
        var counter = _cache.Get<RequestCounter>(cacheKey);
        var now = DateTime.UtcNow;

        if (counter == null)
        {
          counter = new RequestCounter
          {
            Count = 0,
            LastReset = now
          };
        }

        // 检查是否需要重置计数器
        if ((now - counter.LastReset).TotalSeconds >= rateLimit.Seconds)
        {
          counter.Count = 0;
          counter.LastReset = now;
        }

        // 检查是否超过限制
        if (counter.Count >= rateLimit.MaxRequests)
        {
          _logger.LogWarning($"请求频率超限: {clientId}");
          context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
          await context.Response.WriteAsJsonAsync(new { message = "请求频率超限，请稍后再试" });
          return;
        }

        // 更新计数器
        counter.Count++;
        _cache.Set(cacheKey, counter, TimeSpan.FromSeconds(rateLimit.Seconds));

        await _next(context);
      }
      finally
      {
        semaphore.Release();
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "限流中间件处理失败");
      throw;
    }
  }

  private string GetClientId(HttpContext context)
  {
    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    var path = context.Request.Path.Value ?? "/";
    var userId = context.User?.Identity?.Name ?? "anonymous";
    return $"{ip}_{path}_{userId}";
  }

  private class RequestCounter
  {
    public int Count { get; set; }
    public DateTime LastReset { get; set; }
  }
}

/// <summary>
/// 限流中间件扩展方法
/// </summary>
public static class LeanRateLimitMiddlewareExtensions
{
  /// <summary>
  /// 使用接口限流中间件
  /// </summary>
  public static IApplicationBuilder UseLeanRateLimit(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<LeanRateLimitMiddleware>();
  }
}