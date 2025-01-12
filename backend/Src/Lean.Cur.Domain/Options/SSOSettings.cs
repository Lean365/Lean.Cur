namespace Lean.Cur.Domain.Options
{
  /// <summary>
  /// SSO配置
  /// </summary>
  public class SSOSettings
  {
    /// <summary>
    /// 是否启用SSO
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Token有效期（分钟）
    /// </summary>
    public int TokenValidityMinutes { get; set; } = 30;

    /// <summary>
    /// 刷新Token有效期（天）
    /// </summary>
    public int RefreshTokenValidityDays { get; set; } = 7;

    /// <summary>
    /// 是否启用单点登录
    /// </summary>
    public bool SingleSignOn { get; set; } = true;

    /// <summary>
    /// 最大并发会话数
    /// </summary>
    public int MaxConcurrentSessions { get; set; } = 3;

    /// <summary>
    /// 会话超时时间
    /// </summary>
    /// <remarks>
    /// 1. 用户在此时间内无任何操作则自动退出
    /// 2. 退出时会删除相关Token
    /// 3. 默认20分钟
    /// 4. 可通过配置文件修改
    /// </remarks>
    public TimeSpan SessionTimeout { get; set; } = TimeSpan.FromMinutes(20);

    /// <summary>
    /// 设备追踪配置
    /// </summary>
    public DeviceTrackingOptions DeviceTracking { get; set; }

    /// <summary>
    /// 安全策略配置
    /// </summary>
    public SecurityPolicyOptions SecurityPolicies { get; set; }
  }

  /// <summary>
  /// 设备追踪配置
  /// </summary>
  public class DeviceTrackingOptions
  {
    /// <summary>
    /// 是否启用设备追踪
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 是否追踪设备信息
    /// </summary>
    public bool TrackDeviceInfo { get; set; } = true;

    /// <summary>
    /// 是否追踪位置信息
    /// </summary>
    public bool TrackLocation { get; set; } = true;
  }

  /// <summary>
  /// 安全策略配置
  /// </summary>
  public class SecurityPolicyOptions
  {
    /// <summary>
    /// 是否要求安全连接
    /// </summary>
    public bool RequireSecureConnection { get; set; } = true;

    /// <summary>
    /// 是否验证IP地址
    /// </summary>
    public bool ValidateIPAddress { get; set; } = true;

    /// <summary>
    /// 是否验证User-Agent
    /// </summary>
    public bool ValidateUserAgent { get; set; } = true;

    /// <summary>
    /// 是否阻止并发登录
    /// </summary>
    public bool PreventConcurrentLogin { get; set; } = true;
  }
}