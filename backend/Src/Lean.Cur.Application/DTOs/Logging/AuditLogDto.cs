using System.ComponentModel;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Logging;

/// <summary>
/// 审计日志查询参数
/// </summary>
public class AuditLogQueryDto : LeanPagedRequest
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
    /// 操作类型
    /// </summary>
    [Description("操作类型")]
    public string? OperationType { get; set; }

    /// <summary>
    /// 状态（0-失败，1-成功）
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
/// 审计日志统计数据
/// </summary>
public class AuditLogStatsDto
{
    /// <summary>
    /// 总操作次数
    /// </summary>
    [DisplayName("总操作次数")]
    public int TotalCount { get; set; }

    /// <summary>
    /// 成功操作次数
    /// </summary>
    [DisplayName("成功操作次数")]
    public int SuccessCount { get; set; }

    /// <summary>
    /// 失败操作次数
    /// </summary>
    [DisplayName("失败操作次数")]
    public int FailCount { get; set; }

    /// <summary>
    /// 最近一天操作次数
    /// </summary>
    [DisplayName("最近一天操作次数")]
    public int LastDayCount { get; set; }

    /// <summary>
    /// 最近一周操作次数
    /// </summary>
    [DisplayName("最近一周操作次数")]
    public int LastWeekCount { get; set; }

    /// <summary>
    /// 最近一月操作次数
    /// </summary>
    [DisplayName("最近一月操作次数")]
    public int LastMonthCount { get; set; }

    /// <summary>
    /// 平均执行时长(毫秒)
    /// </summary>
    [DisplayName("平均执行时长")]
    public long AvgExecutionTime { get; set; }
}

/// <summary>
/// 审计日志趋势数据
/// </summary>
public class AuditLogTrendDto
{
    /// <summary>
    /// 日期
    /// </summary>
    [DisplayName("日期")]
    public DateTime Date { get; set; }

    /// <summary>
    /// 操作总数
    /// </summary>
    [DisplayName("操作总数")]
    public int TotalCount { get; set; }

    /// <summary>
    /// 成功操作数
    /// </summary>
    [DisplayName("成功操作数")]
    public int SuccessCount { get; set; }

    /// <summary>
    /// 失败操作数
    /// </summary>
    [DisplayName("失败操作数")]
    public int FailCount { get; set; }

    /// <summary>
    /// 平均执行时长(毫秒)
    /// </summary>
    [DisplayName("平均执行时长")]
    public long AvgExecutionTime { get; set; }
}

/// <summary>
/// 审计日志数据
/// </summary>
public class AuditLogDto
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
    /// 操作类型
    /// </summary>
    [Description("操作类型")]
    public string OperationType { get; set; } = string.Empty;

    /// <summary>
    /// 操作描述
    /// </summary>
    [Description("操作描述")]
    public string OperationDesc { get; set; } = string.Empty;

    /// <summary>
    /// 请求方法
    /// </summary>
    [Description("请求方法")]
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求URL
    /// </summary>
    [Description("请求URL")]
    public string RequestUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数
    /// </summary>
    [Description("请求参数")]
    public string RequestParams { get; set; } = string.Empty;

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
    /// 执行时长(毫秒)
    /// </summary>
    [Description("执行时长")]
    public long ExecutionTime { get; set; }

    /// <summary>
    /// 状态（0-失败，1-成功）
    /// </summary>
    [Description("状态")]
    public int Status { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [Description("错误信息")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    public DateTime CreateTime { get; set; }
} 