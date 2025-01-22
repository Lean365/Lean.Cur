using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Logging
{
  /// <summary>
  /// 操作日志查询DTO
  /// </summary>
  public class OperationLogQueryDto : LeanPagedRequest
  {
    /// <summary>
    /// 用户ID
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public string? OperationType { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    public bool? Success { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
  }

  /// <summary>
  /// 操作日志DTO
  /// </summary>
  public class OperationLogDto
  {
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string ModuleName { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public string OperationType { get; set; }

    /// <summary>
    /// 操作描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    public string RequestMethod { get; set; }

    /// <summary>
    /// 请求URL
    /// </summary>
    public string RequestUrl { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string RequestParams { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string Os { get; set; }

    /// <summary>
    /// 执行时长(毫秒)
    /// </summary>
    public long ExecutionTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 操作日志导出DTO
  /// </summary>
  public class OperationLogExportDto
  {
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string ModuleName { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public string OperationType { get; set; }

    /// <summary>
    /// 操作描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 操作结果
    /// </summary>
    public string Success { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string Os { get; set; }

    /// <summary>
    /// 执行时长(毫秒)
    /// </summary>
    public long ExecutionTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public string CreateTime { get; set; }
  }
}