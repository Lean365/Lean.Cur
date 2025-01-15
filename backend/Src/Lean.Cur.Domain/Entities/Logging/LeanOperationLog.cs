using SqlSugar;

namespace Lean.Cur.Domain.Entities.Logging
{
  /// <summary>
  /// 操作日志表
  /// </summary>
  /// <remarks>
  /// 记录用户的操作日志
  /// 
  /// 数据库映射说明：
  /// 1. 表名：lean_operation_log
  /// 2. 主键：Id (自增长)
  /// 3. 索引：
  ///    - IX_UserId (用户ID)
  ///    - IX_CreateTime (创建时间)
  ///    - IX_OperationType (操作类型)
  ///    - IX_Success (操作结果)
  /// 
  /// 业务规则：
  /// 1. 每次业务操作都记录操作日志
  /// 2. 记录操作的详细信息，包括IP、设备等
  /// 3. 支持查询和统计分析
  /// 4. 保留历史记录，不做物理删除
  /// </remarks>
  [SugarTable("lean_mon_operation_log", "操作日志表")]
  public class LeanOperationLog : LeanBaseEntity
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
    /// 模块名称
    /// </summary>
    /// <remarks>
    /// 1. 记录操作的模块名称
    /// 2. 必填字段
    /// 3. 最大长度：50个字符
    /// </remarks>
    [SugarColumn(ColumnName = "module_name", ColumnDescription = "模块名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型(新增,修改,删除等)
    /// </summary>
    /// <remarks>
    /// 1. 记录操作类型
    /// 2. 必填字段
    /// 3. 最大长度：50个字符
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
    [SugarColumn(ColumnName = "description", ColumnDescription = "操作描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string Description { get; set; } = string.Empty;

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
    /// 2. 可选字段
    /// 3. 最大长度：2000个字符
    /// </remarks>
    [SugarColumn(ColumnName = "request_params", ColumnDescription = "请求参数", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? RequestParams { get; set; }

    /// <summary>
    /// 操作结果(成功/失败)
    /// </summary>
    /// <remarks>
    /// 1. 记录操作的结果
    /// 2. 必填字段
    /// 3. true表示成功，false表示失败
    /// </remarks>
    [SugarColumn(ColumnName = "success", ColumnDescription = "操作结果", ColumnDataType = "bit", IsNullable = false)]
    public bool Success { get; set; }

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
    /// 执行时长(毫秒)
    /// </summary>
    /// <remarks>
    /// 1. 记录操作执行的时长
    /// 2. 必填字段
    /// 3. 单位：毫秒
    /// </remarks>
    [SugarColumn(ColumnName = "execution_time", ColumnDescription = "执行时长", ColumnDataType = "bigint", IsNullable = false)]
    public long ExecutionTime { get; set; }
  }
}