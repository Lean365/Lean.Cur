using SqlSugar;

namespace Lean.Cur.Domain.Entities.Logging;

/// <summary>
/// SQL差异日志表
/// </summary>
/// <remarks>
/// 记录数据库表数据变更前后的差异
/// 
/// 数据库映射说明：
/// 1. 表名：lean_sql_log
/// 2. 主键：Id (自增长)
/// 3. 索引：
///    - IX_TableName (表名)
///    - IX_OperateTime (操作时间)
///    - IX_OperationType (操作类型)
///    - IX_OperatorId (操作人ID)
/// 
/// 业务规则：
/// 1. 每次数据变更都记录变更前后的数据
/// 2. 支持查询和统计分析
/// 3. 保留历史记录，不做物理删除
/// </remarks>
[SugarTable("lean_mon_diff_sql_log", "SQL差异日志表")]
public class LeanSqlLog : LeanBaseEntity
{
  /// <summary>
  /// 表名
  /// </summary>
  /// <remarks>
  /// 1. 记录变更的表名
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// 4. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "table_name", ColumnDescription = "表名", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  public string TableName { get; set; } = string.Empty;

  /// <summary>
  /// 主键值
  /// </summary>
  /// <remarks>
  /// 1. 记录变更数据的主键值
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "primary_key", ColumnDescription = "主键值", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  public string PrimaryKey { get; set; } = string.Empty;

  /// <summary>
  /// 操作类型(Insert/Update/Delete)
  /// </summary>
  /// <remarks>
  /// 1. 记录数据变更的类型
  /// 2. 必填字段
  /// 3. 最大长度：20个字符
  /// 4. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "operation_type", ColumnDescription = "操作类型", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
  public string OperationType { get; set; } = string.Empty;

  /// <summary>
  /// 变更前数据(JSON格式)
  /// </summary>
  /// <remarks>
  /// 1. 记录变更前的数据
  /// 2. 可选字段
  /// 3. 最大长度：4000个字符
  /// 4. 使用JSON格式存储
  /// </remarks>
  [SugarColumn(ColumnName = "before_data", ColumnDescription = "变更前数据", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
  public string? BeforeData { get; set; }

  /// <summary>
  /// 变更后数据(JSON格式)
  /// </summary>
  /// <remarks>
  /// 1. 记录变更后的数据
  /// 2. 可选字段
  /// 3. 最大长度：4000个字符
  /// 4. 使用JSON格式存储
  /// </remarks>
  [SugarColumn(ColumnName = "after_data", ColumnDescription = "变更后数据", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
  public string? AfterData { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "operator_id", ColumnDescription = "操作人ID", ColumnDataType = "bigint", IsNullable = false)]
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  /// <remarks>
  /// 1. 记录操作时的用户名
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "operator_name", ColumnDescription = "操作人名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  /// <remarks>
  /// 1. 记录数据变更的时间
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "operate_time", ColumnDescription = "操作时间", ColumnDataType = "datetime", IsNullable = false)]
  public DateTime OperateTime { get; set; }

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
  /// 备注
  /// </summary>
  /// <remarks>
  /// 1. 记录额外的说明信息
  /// 2. 可选字段
  /// 3. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public string? Remark { get; set; }
}