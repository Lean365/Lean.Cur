using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.Dtos.Admin;

/// <summary>
/// 权限审计DTO
/// </summary>
public class PermissionAuditDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 权限标识
  /// </summary>
  public string Permission { get; set; } = string.Empty;

  /// <summary>
  /// 操作类型
  /// </summary>
  public PermissionAuditType AuditType { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  public string IpAddress { get; set; } = string.Empty;

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }
}