namespace Lean.Cur.Common.Auth.Jwt;

/// <summary>
/// JWT配置选项
/// </summary>
public class LeanJwtOptions
{
  /// <summary>
  /// 密钥
  /// </summary>
  public string SecretKey { get; set; } = string.Empty;

  /// <summary>
  /// 发行者
  /// </summary>
  public string Issuer { get; set; } = string.Empty;

  /// <summary>
  /// 接收者
  /// </summary>
  public string Audience { get; set; } = string.Empty;

  /// <summary>
  /// 访问令牌过期时间（分钟）
  /// </summary>
  public int AccessTokenExpiryMinutes { get; set; } = 120;

  /// <summary>
  /// 刷新令牌过期时间（天）
  /// </summary>
  public int RefreshTokenExpiryDays { get; set; } = 7;
}