namespace Lean.Cur.Infrastructure.Cache;

public interface ILeanCache
{
  T? Get<T>(string key);
  void Set<T>(string key, T value, TimeSpan? expiry = null);
  void Remove(string key);
  bool Exists(string key);
}