namespace Lean.Cur.Domain.Options;

/// <summary>
/// SignalR配置选项
/// </summary>
public class SignalROptions
{
  /// <summary>
  /// 是否启用详细错误信息
  /// </summary>
  public bool EnableDetailedErrors { get; set; }

  /// <summary>
  /// 保持连接时间间隔(秒)
  /// </summary>
  public int KeepAliveInterval { get; set; }

  /// <summary>
  /// 客户端超时时间(秒)
  /// </summary>
  public int ClientTimeoutInterval { get; set; }

  /// <summary>
  /// 最大接收消息大小(字节)
  /// </summary>
  public int MaximumReceiveMessageSize { get; set; }

  /// <summary>
  /// Hub配置
  /// </summary>
  public HubOptions Hubs { get; set; } = null!;

  /// <summary>
  /// CORS配置
  /// </summary>
  public SignalRCorsOptions Cors { get; set; } = null!;
}

/// <summary>
/// Hub配置选项
/// </summary>
public class HubOptions
{
  /// <summary>
  /// 消息Hub路径
  /// </summary>
  public string Message { get; set; } = null!;

  /// <summary>
  /// 通知Hub路径
  /// </summary>
  public string Notice { get; set; } = null!;

  /// <summary>
  /// 在线用户Hub路径
  /// </summary>
  public string Online { get; set; } = null!;
}

/// <summary>
/// SignalR CORS配置选项
/// </summary>
public class SignalRCorsOptions
{
  /// <summary>
  /// 允许的来源
  /// </summary>
  public List<string> Origins { get; set; } = null!;

  /// <summary>
  /// 允许的请求头
  /// </summary>
  public List<string> Headers { get; set; } = null!;
}