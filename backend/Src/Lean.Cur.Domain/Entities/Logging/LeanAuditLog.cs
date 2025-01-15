using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Logging;

/// <summary>
/// 审计日志表
/// </summary>
/// <remarks>
/// 记录用户的操作审计日志
/// 
/// 数据库映射说明：
/// 1. 表名：lean_audit_log
/// 2. 主键：Id (自增长)
/// 3. 索引：
///    - IX_UserId (用户ID)
///    - IX_CreateTime (创建时间)
///    - IX_OperationType (操作类型)
///    - IX_Status (状态)
/// 
/// 业务规则：
/// 1. 每次业务操作都记录审计日志
/// 2. 记录操作的详细信息，包括IP、设备等
/// 3. 支持查询和统计分析
/// 4. 保留历史记录，不做物理删除
/// </remarks>
[SugarTable("lean_mon_audit_log", "审计日志表")]
public class LeanAuditLog : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 1. 记录操作时的用户名
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 操作类型
  /// </summary>
  /// <remarks>
  /// 1. 记录操作类型，如：新增、修改、删除等
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// 4. 用于统计分析
  /// </remarks>
  [SugarColumn(ColumnName = "operation_type", ColumnDescription = "操作类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string OperationType { get; set; } = string.Empty;

  /// <summary>
  /// 操作描述
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的详细描述
  /// 2. 必填字段
  /// 3. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "operation_desc", ColumnDescription = "操作描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
  public string OperationDesc { get; set; } = string.Empty;

  /// <summary>
  /// 请求方法
  /// </summary>
  /// <remarks>
  /// 1. 记录HTTP请求方法
  /// 2. 必填字段
  /// 3. 最大长度：10个字符
  /// </remarks>
  [SugarColumn(ColumnName = "request_method", ColumnDescription = "请求方法", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
  public string RequestMethod { get; set; } = string.Empty;

  /// <summary>
  /// 请求URL
  /// </summary>
  /// <remarks>
  /// 1. 记录请求的URL地址
  /// 2. 必填字段
  /// 3. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "request_url", ColumnDescription = "请求URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
  public string RequestUrl { get; set; } = string.Empty;

  /// <summary>
  /// 请求参数
  /// </summary>
  /// <remarks>
  /// 1. 记录请求的参数信息
  /// 2. 必填字段
  /// 3. 最大长度：2000个字符
  /// 4. JSON格式存储
  /// </remarks>
  [SugarColumn(ColumnName = "request_params", ColumnDescription = "请求参数", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
  public string RequestParams { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的IP地址
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 地理位置
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的地理位置
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 格式：省份-城市-区县
  /// </remarks>
  [SugarColumn(ColumnName = "location", ColumnDescription = "地理位置", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
  public string? Location { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的浏览器信息
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的操作系统信息
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
  public string? Os { get; set; }

  /// <summary>
  /// 执行时长(毫秒)
  /// </summary>
  /// <remarks>
  /// 1. 记录操作执行的时长
  /// 2. 必填字段
  /// 3. 用于性能分析
  /// </remarks>
  [SugarColumn(ColumnName = "execution_time", ColumnDescription = "执行时长", ColumnDataType = "bigint", IsNullable = false)]
  public long ExecutionTime { get; set; }

  /// <summary>
  /// 状态（0-失败，1-成功）
  /// </summary>
  /// <remarks>
  /// 1. 记录操作结果
  /// 2. 必填字段
  /// 3. 用于统计分析
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  public int Status { get; set; }

  /// <summary>
  /// 错误信息
  /// </summary>
  /// <remarks>
  /// 1. 记录操作失败时的错误信息
  /// 2. 可选字段
  /// 3. 最大长度：2000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "error_message", ColumnDescription = "错误信息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
  public string? ErrorMessage { get; set; }
}