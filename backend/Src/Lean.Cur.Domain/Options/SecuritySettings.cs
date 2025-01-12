namespace Lean.Cur.Domain.Options
{
  /// <summary>
  /// 安全配置
  /// </summary>
  public class SecuritySettings
  {
    /// <summary>
    /// 登录限制配置
    /// </summary>
    public LoginLimitsOptions LoginLimits { get; set; }
  }

  /// <summary>
  /// 登录限制配置
  /// </summary>
  public class LoginLimitsOptions
  {
    /// <summary>
    /// 每小时最大尝试次数
    /// </summary>
    public int HourlyMaxAttempts { get; set; } = 3;

    /// <summary>
    /// 每天最大尝试次数
    /// </summary>
    public int DailyMaxAttempts { get; set; } = 9;

    /// <summary>
    /// 最大在线设备数
    /// </summary>
    public int MaxOnlineDevices { get; set; } = 3;

    /// <summary>
    /// 锁定时长
    /// </summary>
    /// <remarks>
    /// 1. 默认为永久锁定（TimeSpan.MaxValue）
    /// 2. 锁定后需要管理员手动解锁
    /// 3. 可以通过配置修改为临时锁定
    /// 4. 临时锁定时长到期后自动解锁
    /// </remarks>
    public TimeSpan LockoutDuration { get; set; } = TimeSpan.MaxValue;
  }
}