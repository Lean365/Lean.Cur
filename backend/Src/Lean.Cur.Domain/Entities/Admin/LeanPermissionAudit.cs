using Lean.Cur.Common.Enums;
using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 权限变更审计日志
/// </summary>
/// <remarks>
/// 记录角色权限变更的审计日志
/// 
/// 数据库映射说明：
/// 1. 表名：lean_mon_permission_audit
/// 2. 主键：id (自增长)
/// 3. 索引：
///    - IX_RoleId (角色ID)
///    - IX_CreateTime (创建时间)
///    - IX_AuditType (操作类型)
/// 
/// 业务规则：
/// 1. 每次权限变更都记录审计日志
/// 2. 记录操作的详细信息，包括操作人、IP等
/// 3. 支持按角色和时间范围查询
/// 4. 保留历史记录，不做物理删除
/// </remarks>
[SugarTable("lean_mon_permission_audit", "权限变更审计日志表")]
public class LeanPermissionAudit : LeanBaseEntity
{
  /// <summary>
  /// 操作类型
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 记录权限变更的具体操作类型
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "audit_type", ColumnDescription = "操作类型", ColumnDataType = "int", IsNullable = false)]
  [Description("操作类型")]
  public PermissionAuditType AuditType { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 关联角色表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("角色ID")]
  public long RoleId { get; set; }

  /// <summary>
  /// 权限标识
  /// </summary>
  /// <remarks>
  /// 1. 记录变更的具体权限标识
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "permission", ColumnDescription = "权限标识", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  [Description("权限标识")]
  public string Permission { get; set; } = string.Empty;

  /// <summary>
  /// 操作人ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "operator_id", ColumnDescription = "操作人ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("操作人ID")]
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  /// <remarks>
  /// 1. 记录操作时的用户名称
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "operator_name", ColumnDescription = "操作人名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("操作人名称")]
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 1. 记录操作时的IP地址
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("IP地址")]
  public string IpAddress { get; set; } = string.Empty;

  /// <summary>
  /// 备注
  /// </summary>
  /// <remarks>
  /// 1. 可选的补充说明
  /// 2. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  [Description("备注")]
  public string? Remark { get; set; }
}