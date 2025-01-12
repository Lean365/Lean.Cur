namespace Lean.Cur.Domain.Cache;

/// <summary>
/// 缓存接口
/// </summary>
public interface ILeanCache
{
  /// <summary>
  /// 获取缓存
  /// </summary>
  T? Get<T>(string key);

  /// <summary>
  /// 异步获取缓存
  /// </summary>
  Task<T?> GetAsync<T>(string key);

  /// <summary>
  /// 设置缓存
  /// </summary>
  void Set<T>(string key, T value, TimeSpan? expiry = null);

  /// <summary>
  /// 异步设置缓存
  /// </summary>
  Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

  /// <summary>
  /// 移除缓存
  /// </summary>
  void Remove(string key);

  /// <summary>
  /// 异步移除缓存
  /// </summary>
  Task RemoveAsync(string key);
}