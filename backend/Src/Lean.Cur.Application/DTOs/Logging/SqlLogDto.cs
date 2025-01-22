using System.ComponentModel;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Logging;

/// <summary>
/// SQL差异日志查询参数
/// </summary>
public class SqlLogQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 表名
  /// </summary>
  [Description("表名")]
  public string? TableName { get; set; }

  /// <summary>
  /// 主键值
  /// </summary>
  [Description("主键值")]
  public string? PrimaryKey { get; set; }

  /// <summary>
  /// 操作类型(Insert/Update/Delete)
  /// </summary>
  [Description("操作类型")]
  public string? OperationType { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  [Description("操作人ID")]
  public long? OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  [Description("操作人名称")]
  public string? OperatorName { get; set; }

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
}

/// <summary>
/// SQL差异日志数据
/// </summary>
public class SqlLogDto
{
  /// <summary>
  /// ID
  /// </summary>
  [Description("ID")]
  public long Id { get; set; }

  /// <summary>
  /// 表名
  /// </summary>
  [Description("表名")]
  public string TableName { get; set; } = string.Empty;

  /// <summary>
  /// 主键值
  /// </summary>
  [Description("主键值")]
  public string PrimaryKey { get; set; } = string.Empty;

  /// <summary>
  /// 操作类型(Insert/Update/Delete)
  /// </summary>
  [Description("操作类型")]
  public string OperationType { get; set; } = string.Empty;

  /// <summary>
  /// 变更前数据(JSON格式)
  /// </summary>
  [Description("变更前数据")]
  public string? BeforeData { get; set; }

  /// <summary>
  /// 变更后数据(JSON格式)
  /// </summary>
  [Description("变更后数据")]
  public string? AfterData { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  [Description("操作人ID")]
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  [Description("操作人名称")]
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  [Description("操作时间")]
  public DateTime OperateTime { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  [Description("IP地址")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [Description("备注")]
  public string? Remark { get; set; }
}

/// <summary>
/// SQL差异日志导出DTO
/// </summary>
public class SqlLogExportDto
{
  /// <summary>
  /// 表名
  /// </summary>
  [Description("表名")]
  public string TableName { get; set; } = string.Empty;

  /// <summary>
  /// 主键值
  /// </summary>
  [Description("主键值")]
  public string PrimaryKey { get; set; } = string.Empty;

  /// <summary>
  /// 操作类型
  /// </summary>
  [Description("操作类型")]
  public string OperationType { get; set; } = string.Empty;

  /// <summary>
  /// 变更前数据
  /// </summary>
  [Description("变更前数据")]
  public string? BeforeData { get; set; }

  /// <summary>
  /// 变更后数据
  /// </summary>
  [Description("变更后数据")]
  public string? AfterData { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  [Description("操作人名称")]
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  [Description("操作时间")]
  public string OperateTime { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  [Description("IP地址")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [Description("备注")]
  public string? Remark { get; set; }
}