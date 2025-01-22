using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 权限审计服务接口
/// </summary>
public interface ILeanPermAuditService
{
  /// <summary>
  /// 记录权限变更
  /// </summary>
  /// <param name="auditDto">审计信息</param>
  /// <returns>是否成功</returns>
  Task<bool> LogPermissionChangeAsync(LeanPermAuditDto auditDto);

  /// <summary>
  /// 获取权限变更记录
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  Task<LeanPagedResult<LeanPermAuditDto>> GetPermissionAuditsAsync(LeanPermAuditDto queryDto);
}