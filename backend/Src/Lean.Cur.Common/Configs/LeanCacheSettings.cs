namespace Lean.Cur.Common.Configs;

/// <summary>
/// 缓存配置
/// </summary>
public class LeanCacheSettings
{
  /// <summary>
  /// 是否启用Redis
  /// </summary>
  public bool UseRedis { get; set; } = false;

  /// <summary>
  /// Redis配置
  /// </summary>
  public LeanRedisSettings? Redis { get; set; } = new();
}

/// <summary>
/// Redis配置
/// </summary>
public class LeanRedisSettings
{
  /// <summary>
  /// 连接字符串
  /// </summary>
  public string ConnectionString { get; set; } = string.Empty;

  /// <summary>
  /// 默认数据库
  /// </summary>
  public int DefaultDatabase { get; set; } = 0;

  /// <summary>
  /// 密码
  /// </summary>
  public string? Password { get; set; }

  /// <summary>
  /// SSL启用
  /// </summary>
  public bool Ssl { get; set; } = false;

  /// <summary>
  /// 连接超时（毫秒）
  /// </summary>
  public int ConnectTimeout { get; set; } = 5000;
}