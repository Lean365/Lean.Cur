using StackExchange.Redis;
using Lean.Cur.Domain.Cache;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Infrastructure.Cache;

/// <summary>
/// Redis缓存实现
/// </summary>
public class LeanRedisCache : ILeanCache
{
  private readonly IConnectionMultiplexer _redis;
  private readonly IDatabase _db;
  private readonly ILogger<LeanRedisCache> _logger;

  public LeanRedisCache(IConnectionMultiplexer redis, ILogger<LeanRedisCache> logger)
  {
    _redis = redis;
    _db = redis.GetDatabase();
    _logger = logger;
  }

  public T? Get<T>(string key)
  {
    try
    {
      _logger.LogInformation("正在从Redis获取键 {Key} 的值", key);
      var value = _db.StringGet(key);
      if (!value.HasValue)
      {
        _logger.LogInformation("Redis中未找到键 {Key} 的值", key);
        return default;
      }
      var result = System.Text.Json.JsonSerializer.Deserialize<T>(value!);
      _logger.LogInformation("成功从Redis获取键 {Key} 的值", key);
      return result;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "从Redis获取键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public async Task<T?> GetAsync<T>(string key)
  {
    try
    {
      _logger.LogInformation("正在异步从Redis获取键 {Key} 的值", key);
      var value = await _db.StringGetAsync(key);
      if (!value.HasValue)
      {
        _logger.LogInformation("Redis中未找到键 {Key} 的值", key);
        return default;
      }
      var result = System.Text.Json.JsonSerializer.Deserialize<T>(value!);
      _logger.LogInformation("成功从Redis获取键 {Key} 的值", key);
      return result;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步从Redis获取键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public void Set<T>(string key, T value, TimeSpan? expiry = null)
  {
    try
    {
      _logger.LogInformation("正在向Redis设置键 {Key} 的值，过期时间: {Expiry}", key, expiry);
      var jsonValue = System.Text.Json.JsonSerializer.Serialize(value);
      _db.StringSet(key, jsonValue, expiry);
      _logger.LogInformation("成功向Redis设置键 {Key} 的值", key);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "向Redis设置键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
  {
    try
    {
      _logger.LogInformation("正在异步向Redis设置键 {Key} 的值，过期时间: {Expiry}", key, expiry);
      var jsonValue = System.Text.Json.JsonSerializer.Serialize(value);
      await _db.StringSetAsync(key, jsonValue, expiry);
      _logger.LogInformation("成功向Redis设置键 {Key} 的值", key);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步向Redis设置键 {Key} 的值时发生错误", key);
      throw;
    }
  }

  public void Remove(string key)
  {
    try
    {
      _logger.LogInformation("正在从Redis删除键 {Key}", key);
      var deleted = _db.KeyDelete(key);
      _logger.LogInformation("从Redis删除键 {Key} {Result}", key, deleted ? "成功" : "失败");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "从Redis删除键 {Key} 时发生错误", key);
      throw;
    }
  }

  public async Task RemoveAsync(string key)
  {
    try
    {
      _logger.LogInformation("正在异步从Redis删除键 {Key}", key);
      var deleted = await _db.KeyDeleteAsync(key);
      _logger.LogInformation("从Redis删除键 {Key} {Result}", key, deleted ? "成功" : "失败");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步从Redis删除键 {Key} 时发生错误", key);
      throw;
    }
  }

  public bool Exists(string key)
  {
    try
    {
      _logger.LogInformation("正在检查Redis键 {Key} 是否存在", key);
      var exists = _db.KeyExists(key);
      _logger.LogInformation("Redis键 {Key} {Result}", key, exists ? "存在" : "不存在");
      return exists;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查Redis键 {Key} 是否存在时发生错误", key);
      throw;
    }
  }

  public async Task<bool> ExistsAsync(string key)
  {
    try
    {
      _logger.LogInformation("正在异步检查Redis键 {Key} 是否存在", key);
      var exists = await _db.KeyExistsAsync(key);
      _logger.LogInformation("Redis键 {Key} {Result}", key, exists ? "存在" : "不存在");
      return exists;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "异步检查Redis键 {Key} 是否存在时发生错误", key);
      throw;
    }
  }
}