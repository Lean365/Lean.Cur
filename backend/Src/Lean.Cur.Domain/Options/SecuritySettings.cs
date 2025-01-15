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

  /// <summary>
  /// 密码策略配置
  /// </summary>
  public class PasswordPolicyOptions
  {
    /// <summary>
    /// 最小长度
    /// </summary>
    public int MinLength { get; set; } = 8;

    /// <summary>
    /// 是否要求大写字母
    /// </summary>
    public bool RequireUppercase { get; set; } = true;

    /// <summary>
    /// 是否要求小写字母
    /// </summary>
    public bool RequireLowercase { get; set; } = true;

    /// <summary>
    /// 是否要求数字
    /// </summary>
    public bool RequireDigit { get; set; } = true;

    /// <summary>
    /// 是否要求特殊字符
    /// </summary>
    public bool RequireSpecialChar { get; set; } = true;

    /// <summary>
    /// 密码历史记录数
    /// </summary>
    public int PasswordHistory { get; set; } = 3;

    /// <summary>
    /// 密码有效期(天)
    /// </summary>
    public int PasswordExpirationDays { get; set; } = 90;
  }
}