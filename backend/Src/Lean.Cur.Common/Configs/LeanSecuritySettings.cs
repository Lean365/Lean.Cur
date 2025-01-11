/**
 * @description 安全配置类
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

namespace Lean.Cur.Common.Configs;

/// <summary>
/// 安全配置
/// </summary>
public class LeanSecuritySettings
{
  /// <summary>
  /// 限流配置
  /// </summary>
  public LeanRateLimitSettings RateLimit { get; set; } = new();

  /// <summary>
  /// SQL注入配置
  /// </summary>
  public LeanSqlInjectionSettings SqlInjection { get; set; } = new();

  /// <summary>
  /// CSRF防护配置
  /// </summary>
  public LeanAntiForgerySettings AntiForgery { get; set; } = new();

  /// <summary>
  /// 数据范围配置
  /// </summary>
  public LeanDataScopeSettings DataScope { get; set; } = new();

  /// <summary>
  /// 权限配置
  /// </summary>
  public LeanPermissionSettings Permission { get; set; } = new();
}

/// <summary>
/// 限流配置
/// </summary>
public class LeanRateLimitSettings
{
  /// <summary>
  /// 默认时间窗口（秒）
  /// </summary>
  public int DefaultSeconds { get; set; } = 1;

  /// <summary>
  /// 默认最大请求次数
  /// </summary>
  public int DefaultMaxRequests { get; set; } = 10;

  /// <summary>
  /// IP限流配置
  /// </summary>
  public LeanRateLimitRule IpRateLimit { get; set; } = new();

  /// <summary>
  /// 用户限流配置
  /// </summary>
  public LeanRateLimitRule UserRateLimit { get; set; } = new();
}

/// <summary>
/// 限流规则
/// </summary>
public class LeanRateLimitRule
{
  /// <summary>
  /// 时间窗口（秒）
  /// </summary>
  public int Seconds { get; set; } = 60;

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; set; } = 100;
}

/// <summary>
/// SQL注入配置
/// </summary>
public class LeanSqlInjectionSettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// 日志级别
  /// </summary>
  public string LogLevel { get; set; } = "Warning";

  /// <summary>
  /// 排除路径
  /// </summary>
  public string[] ExcludePaths { get; set; } = Array.Empty<string>();

  /// <summary>
  /// 自定义检测模式
  /// </summary>
  public string[] CustomPatterns { get; set; } = Array.Empty<string>();
}

/// <summary>
/// CSRF防护配置
/// </summary>
public class LeanAntiForgerySettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// 请求头名称
  /// </summary>
  public string HeaderName { get; set; } = "X-XSRF-TOKEN";

  /// <summary>
  /// Cookie名称
  /// </summary>
  public string CookieName { get; set; } = "XSRF-TOKEN";

  /// <summary>
  /// 排除路径
  /// </summary>
  public string[] ExcludePaths { get; set; } = Array.Empty<string>();
}

/// <summary>
/// 数据范围配置
/// </summary>
public class LeanDataScopeSettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// 默认数据范围类型
  /// </summary>
  public string DefaultScopeType { get; set; } = "Self";

  /// <summary>
  /// 排除路径
  /// </summary>
  public string[] ExcludePaths { get; set; } = Array.Empty<string>();
}

/// <summary>
/// 权限配置
/// </summary>
public class LeanPermissionSettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// 超级管理员角色编码
  /// </summary>
  public string SuperAdminRoleCode { get; set; } = "superadmin";

  /// <summary>
  /// 忽略路径
  /// </summary>
  public string[] IgnorePaths { get; set; } = Array.Empty<string>();

  /// <summary>
  /// 缓存过期时间（秒）
  /// </summary>
  public int CacheExpiration { get; set; } = 3600;
}