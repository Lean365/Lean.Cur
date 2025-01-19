using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 权限审计服务接口
/// </summary>
public interface ILeanPermissionAuditService
{
  /// <summary>
  /// 记录权限变更
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <param name="permission">权限标识</param>
  /// <param name="auditType">操作类型</param>
  /// <param name="operatorId">操作人ID</param>
  /// <param name="operatorName">操作人名称</param>
  /// <param name="ipAddress">IP地址</param>
  /// <param name="remark">备注</param>
  /// <returns>是否成功</returns>
  Task<bool> LogPermissionChangeAsync(
      long roleId,
      string permission,
      PermissionAuditType auditType,
      long operatorId,
      string operatorName,
      string ipAddress,
      string? remark = null);

  /// <summary>
  /// 获取权限变更记录
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <param name="pageIndex">页码</param>
  /// <param name="pageSize">每页大小</param>
  /// <returns>分页结果</returns>
  Task<PagedResult<PermissionAuditDto>> GetAuditLogsAsync(
      long? roleId = null,
      DateTime? startTime = null,
      DateTime? endTime = null,
      int pageIndex = 1,
      int pageSize = 10);
}