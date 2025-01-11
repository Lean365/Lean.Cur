using Microsoft.Extensions.Caching.Memory;
using Lean.Cur.Domain.Cache;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Infrastructure.Cache;

/// <summary>
/// 内存缓存实现
/// </summary>
public class LeanMemoryCache : ILeanCache
{
  private readonly IMemoryCache _cache;
  private readonly ILogger<LeanMemoryCache> _logger;

  public LeanMemoryCache(IMemoryCache cache, ILogger<LeanMemoryCache> logger)
  {
    _cache = cache;
    _logger = logger;
  }

  public T? Get<T>(string key)
  {
    try
    {
      _logger.LogInformation("正在从内存缓存获取键 {Key} 的值", key);
      var value = _cache.Get<T>(key);
      if (value == null)
      {
        _logger.LogInformation("内存缓存中未找到键 {Key} 的值", key);
      }
      else
      {
        _logger.LogInformation("成功从内存缓存获取键 {Key} 的值", key);
      }
      return value;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "从内存缓存获取键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public async Task<T?> GetAsync<T>(string key)
  {
    try
    {
      _logger.LogInformation("正在异步从内存缓存获取键 {Key} 的值", key);
      return await Task.FromResult(Get<T>(key));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步从内存缓存获取键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public void Set<T>(string key, T value, TimeSpan? expiry = null)
  {
    try
    {
      _logger.LogInformation("正在向内存缓存设置键 {Key} 的值，过期时间: {Expiry}", key, expiry);
      var options = new MemoryCacheEntryOptions();
      if (expiry.HasValue)
      {
        options.AbsoluteExpirationRelativeToNow = expiry;
        _logger.LogInformation("为键 {Key} 设置过期时间: {Expiry}", key, expiry);
      }
      _cache.Set(key, value, options);
      _logger.LogInformation("成功向内存缓存设置键 {Key} 的值", key);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "向内存缓存设置键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
  {
    try
    {
      _logger.LogInformation("正在异步向内存缓存设置键 {Key} 的值", key);
      await Task.Run(() => Set(key, value, expiry));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步向内存缓存设置键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public void Remove(string key)
  {
    try
    {
      _logger.LogInformation("正在从内存缓存删除键 {Key}", key);
      _cache.Remove(key);
      _logger.LogInformation("成功从内存缓存删除键 {Key}", key);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "从内存缓存删除键 {Key} 时发生错误", key);
      throw;
    }
  }

  public async Task RemoveAsync(string key)
  {
    try
    {
      _logger.LogInformation("正在异步从内存缓存删除键 {Key}", key);
      await Task.Run(() => Remove(key));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步从内存缓存删除键 {Key} 时发生错误", key);
      throw;
    }
  }

  public bool Exists(string key)
  {
    try
    {
      _logger.LogInformation("正在检查内存缓存键 {Key} 是否存在", key);
      var exists = _cache.TryGetValue(key, out _);
      _logger.LogInformation("内存缓存键 {Key} {Result}", key, exists ? "存在" : "不存在");
      return exists;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查内存缓存键 {Key} 是否存在时发生错误", key);
      throw;
    }
  }

  public async Task<bool> ExistsAsync(string key)
  {
    try
    {
      _logger.LogInformation("正在异步检查内存缓存键 {Key} 是否存在", key);
      return await Task.FromResult(Exists(key));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步检查内存缓存键 {Key} 是否存在时发生错误", key);
      throw;
    }
  }
}