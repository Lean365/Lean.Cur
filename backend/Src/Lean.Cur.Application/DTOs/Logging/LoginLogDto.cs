using System.ComponentModel;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Logging;

/// <summary>
/// 登录日志查询参数
/// </summary>
public class LoginLogQueryDto : PagedRequest
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Description("用户ID")]
  public long? UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  [Description("用户名")]
  public string? UserName { get; set; }

  /// <summary>
  /// 登录类型（1-登录，2-登出，3-刷新令牌，4-会话过期）
  /// </summary>
  [Description("登录类型")]
  public int? LoginType { get; set; }

  /// <summary>
  /// 状态（1-成功，2-失败）
  /// </summary>
  [Description("状态")]
  public int? Status { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  [Description("IP地址")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  [Description("浏览器")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  [Description("操作系统")]
  public string? Os { get; set; }

  /// <summary>
  /// 地理位置
  /// </summary>
  [Description("地理位置")]
  public string? Location { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  [Description("开始时间")]
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [Description("结束时间")]
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 排序字段
  /// </summary>
  [Description("排序字段")]
  public string? SortField { get; set; }

  /// <summary>
  /// 排序方式（asc/desc）
  /// </summary>
  [Description("排序方式")]
  public string? SortOrder { get; set; }
}

/// <summary>
/// 登录日志统计数据
/// </summary>
public class LoginLogStatsDto
{
  /// <summary>
  /// 总登录次数
  /// </summary>
  [DisplayName("总登录次数")]
  public int TotalCount { get; set; }

  /// <summary>
  /// 成功登录次数
  /// </summary>
  [DisplayName("成功登录次数")]
  public int SuccessCount { get; set; }

  /// <summary>
  /// 失败登录次数
  /// </summary>
  [DisplayName("失败登录次数")]
  public int FailCount { get; set; }

  /// <summary>
  /// 异常退出次数
  /// </summary>
  [DisplayName("异常退出次数")]
  public int AbnormalCount { get; set; }

  /// <summary>
  /// 最近一天登录次数
  /// </summary>
  [DisplayName("最近一天登录次数")]
  public int LastDayCount { get; set; }

  /// <summary>
  /// 最近一周登录次数
  /// </summary>
  [DisplayName("最近一周登录次数")]
  public int LastWeekCount { get; set; }

  /// <summary>
  /// 最近一月登录次数
  /// </summary>
  [DisplayName("最近一月登录次数")]
  public int LastMonthCount { get; set; }
}

/// <summary>
/// 登录日志趋势数据
/// </summary>
public class LoginLogTrendDto
{
  /// <summary>
  /// 日期
  /// </summary>
  [DisplayName("日期")]
  public DateTime Date { get; set; }

  /// <summary>
  /// 登录总数
  /// </summary>
  [DisplayName("登录总数")]
  public int TotalCount { get; set; }

  /// <summary>
  /// 成功登录数
  /// </summary>
  [DisplayName("成功登录数")]
  public int SuccessCount { get; set; }

  /// <summary>
  /// 失败登录数
  /// </summary>
  [DisplayName("失败登录数")]
  public int FailCount { get; set; }
}

/// <summary>
/// 登录日志数据
/// </summary>
public class LoginLogDto
{
  /// <summary>
  /// ID
  /// </summary>
  [Description("ID")]
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  [Description("用户ID")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  [Description("用户名")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 登录类型（1-登录，2-登出，3-刷新令牌，4-会话过期）
  /// </summary>
  [Description("登录类型")]
  public int LoginType { get; set; }

  /// <summary>
  /// 状态（1-成功，2-失败）
  /// </summary>
  [Description("状态")]
  public int Status { get; set; }

  /// <summary>
  /// 登录时间
  /// </summary>
  [Description("登录时间")]
  public DateTime LoginTime { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  [Description("IP地址")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 地理位置
  /// </summary>
  [Description("地理位置")]
  public string? Location { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  [Description("浏览器")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  [Description("操作系统")]
  public string? Os { get; set; }

  /// <summary>
  /// 设备信息
  /// </summary>
  [Description("设备信息")]
  public string? Device { get; set; }

  /// <summary>
  /// 消息
  /// </summary>
  [Description("消息")]
  public string? Message { get; set; }
}