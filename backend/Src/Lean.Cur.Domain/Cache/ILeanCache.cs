namespace Lean.Cur.Domain.Cache;

public interface ILeanCache
{
  T Get<T>(string key);
  void Set<T>(string key, T value, TimeSpan? expiry = null);
  void Remove(string key);
  bool Exists(string key);
  Task<T> GetAsync<T>(string key);
  Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
  Task RemoveAsync(string key);
  Task<bool> ExistsAsync(string key);
}