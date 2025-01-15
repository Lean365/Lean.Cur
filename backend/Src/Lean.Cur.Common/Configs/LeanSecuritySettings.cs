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
  public LeanRateLimitSettings? RateLimit { get; set; }

  /// <summary>
  /// SQL注入配置
  /// </summary>
  public LeanSqlInjectionSettings? SqlInjection { get; set; }

  /// <summary>
  /// CSRF防护配置
  /// </summary>
  public LeanAntiForgerySettings? AntiForgery { get; set; }

  /// <summary>
  /// 数据范围配置
  /// </summary>
  public LeanDataScopeSettings? DataScope { get; set; }

  /// <summary>
  /// 权限配置
  /// </summary>
  public LeanPermissionSettings? Permission { get; set; }

  /// <summary>
  /// 安全审计配置
  /// </summary>
  public LeanAuditSettings? Audit { get; set; }

  /// <summary>
  /// 文件上传安全配置
  /// </summary>
  public LeanFileUploadSettings? FileUpload { get; set; }
}

/// <summary>
/// 限流配置
/// </summary>
public class LeanRateLimitSettings
{
  /// <summary>
  /// 默认时间窗口（秒）
  /// </summary>
  public int DefaultSeconds { get; set; }

  /// <summary>
  /// 默认最大请求次数
  /// </summary>
  public int DefaultMaxRequests { get; set; }

  /// <summary>
  /// IP限流配置
  /// </summary>
  public LeanRateLimitRule? IpRateLimit { get; set; }

  /// <summary>
  /// 用户限流配置
  /// </summary>
  public LeanRateLimitRule? UserRateLimit { get; set; }
}

/// <summary>
/// 限流规则
/// </summary>
public class LeanRateLimitRule
{
  /// <summary>
  /// 时间窗口（秒）
  /// </summary>
  public int Seconds { get; set; }

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; set; }
}

/// <summary>
/// SQL注入配置
/// </summary>
public class LeanSqlInjectionSettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; }

  /// <summary>
  /// 日志级别
  /// </summary>
  public string LogLevel { get; set; } = "Warning";

  /// <summary>
  /// 排除路径
  /// </summary>
  public List<string> ExcludePaths { get; set; } = new();

  /// <summary>
  /// 自定义检测模式
  /// </summary>
  public List<string> CustomPatterns { get; set; } = new();
}

/// <summary>
/// CSRF防护配置
/// </summary>
public class LeanAntiForgerySettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; }

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
  public List<string> ExcludePaths { get; set; } = new();
}

/// <summary>
/// 数据范围配置
/// </summary>
public class LeanDataScopeSettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; }

  /// <summary>
  /// 默认数据范围类型
  /// </summary>
  public string DefaultScopeType { get; set; } = "Self";

  /// <summary>
  /// 排除路径
  /// </summary>
  public List<string> ExcludePaths { get; set; } = new();
}

/// <summary>
/// 权限配置
/// </summary>
public class LeanPermissionSettings
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; }

  /// <summary>
  /// 超级管理员角色编码
  /// </summary>
  public string SuperAdminRoleCode { get; set; } = "superadmin";

  /// <summary>
  /// 忽略路径
  /// </summary>
  public List<string> IgnorePaths { get; set; } = new();

  /// <summary>
  /// 缓存过期时间（秒）
  /// </summary>
  public int CacheExpiration { get; set; }
}

/// <summary>
/// 安全审计配置
/// </summary>
public class LeanAuditSettings
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 是否记录操作日志
    /// </summary>
    public bool LogOperations { get; set; } = true;

    /// <summary>
    /// 是否记录敏感数据访问
    /// </summary>
    public bool LogSensitiveAccess { get; set; } = true;

    /// <summary>
    /// 是否记录系统异常
    /// </summary>
    public bool LogExceptions { get; set; } = true;

    /// <summary>
    /// 日志保留天数
    /// </summary>
    public int LogRetentionDays { get; set; } = 180;

    /// <summary>
    /// 敏感操作类型
    /// </summary>
    public List<string> SensitiveOperations { get; set; } = new()
    {
        "DELETE",
        "UPDATE",
        "EXPORT",
        "UPLOAD"
    };

    /// <summary>
    /// 敏感数据类型
    /// </summary>
    public List<string> SensitiveDataTypes { get; set; } = new()
    {
        "Password",
        "PhoneNumber", 
        "IdCard",
        "BankCard"
    };
}

/// <summary>
/// 文件上传安全配置
/// </summary>
public class LeanFileUploadSettings
{
    /// <summary>
    /// 允许的文件类型
    /// </summary>
    public List<string> AllowedFileTypes { get; set; } = new()
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx"
    };

    /// <summary>
    /// 最大文件大小(MB)
    /// </summary>
    public int MaxFileSizeMB { get; set; } = 10;

    /// <summary>
    /// 是否扫描病毒
    /// </summary>
    public bool ScanVirus { get; set; } = true;

    /// <summary>
    /// 是否验证文件签名
    /// </summary>
    public bool ValidateSignature { get; set; } = true;

    /// <summary>
    /// 上传目录白名单
    /// </summary>
    public List<string> AllowedUploadPaths { get; set; } = new()
    {
        "uploads/images",
        "uploads/documents",
        "uploads/temp"
    };

    /// <summary>
    /// 是否重命名文件
    /// </summary>
    public bool RenameFiles { get; set; } = true;

    /// <summary>
    /// 重命名规则
    /// </summary>
    public string RenamePattern { get; set; } = "{yyyy}{MM}{dd}{HH}{mm}{ss}{rand:6}";
}