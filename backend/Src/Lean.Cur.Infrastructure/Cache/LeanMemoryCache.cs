using Microsoft.Extensions.Caching.Memory;

namespace Lean.Cur.Infrastructure.Cache;

public class LeanMemoryCache : ILeanCache
{
  private readonly IMemoryCache _cache;

  public LeanMemoryCache(IMemoryCache cache)
  {
    _cache = cache;
  }

  public T? Get<T>(string key)
  {
    return _cache.Get<T>(key);
  }

  public void Set<T>(string key, T value, TimeSpan? expiry = null)
  {
    var options = new MemoryCacheEntryOptions();
    if (expiry.HasValue)
    {
      options.AbsoluteExpirationRelativeToNow = expiry;
    }
    _cache.Set(key, value, options);
  }

  public void Remove(string key)
  {
    _cache.Remove(key);
  }

  public bool Exists(string key)
  {
    return _cache.TryGetValue(key, out _);
  }
}