using SqlSugar;

namespace Lean.Cur.Domain.Entities.Logging;

/// <summary>
/// 登录日志表
/// </summary>
/// <remarks>
/// 记录用户的登录、登出等操作日志
/// 
/// 数据库映射说明：
/// 1. 表名：lean_login_log
/// 2. 主键：Id (自增长)
/// 3. 索引：
///    - IX_UserId (用户ID)
///    - IX_LoginTime (登录时间)
///    - IX_LoginType (登录类型)
///    - IX_Status (状态)
/// 
/// 业务规则：
/// 1. 每次登录、登出操作都记录日志
/// 2. 记录操作的详细信息，包括IP、设备等
/// 3. 支持查询和统计分析
/// 4. 保留历史记录，不做物理删除
/// </remarks>
[SugarTable("lean_login_log", "登录日志表")]
public class LeanLoginLog : LeanBaseEntity
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
  /// 登录类型（1-登录，2-登出，3-刷新令牌，4-会话过期）
  /// </summary>
  /// <remarks>
  /// 1. 记录操作类型
  /// 2. 必填字段
  /// 3. 用于统计分析
  /// </remarks>
  [SugarColumn(ColumnName = "login_type", ColumnDescription = "登录类型", ColumnDataType = "int", IsNullable = false)]
  public int LoginType { get; set; }

  /// <summary>
  /// 状态（1-成功，2-失败）
  /// </summary>
  /// <remarks>
  /// 1. 记录操作结果
  /// 2. 必填字段
  /// 3. 用于统计分析
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  public int Status { get; set; }

  /// <summary>
  /// 登录时间
  /// </summary>
  /// <remarks>
  /// 1. 记录操作发生的时间
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "login_time", ColumnDescription = "登录时间", ColumnDataType = "datetime", IsNullable = false)]
  public DateTime LoginTime { get; set; }

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
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的操作系统信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
  public string? Os { get; set; }

  /// <summary>
  /// 设备信息
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的设备信息
  /// 2. 可选字段
  /// 3. 最大长度：200个字符
  /// </remarks>
  [SugarColumn(ColumnName = "device", ColumnDescription = "设备信息", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
  public string? Device { get; set; }

  /// <summary>
  /// 消息
  /// </summary>
  /// <remarks>
  /// 1. 记录操作的详细信息
  /// 2. 可选字段
  /// 3. 最大长度：500个字符
  /// 4. 包含成功/失败原因等
  /// </remarks>
  [SugarColumn(ColumnName = "message", ColumnDescription = "消息", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public string? Message { get; set; }
}