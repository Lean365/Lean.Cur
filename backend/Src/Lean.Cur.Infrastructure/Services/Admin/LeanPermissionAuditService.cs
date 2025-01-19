using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 权限审计服务实现
/// </summary>
public class LeanPermissionAuditService : ILeanPermissionAuditService
{
  private readonly SqlSugarClient _db;

  public LeanPermissionAuditService(SqlSugarClient db)
  {
    _db = db;
  }

  /// <inheritdoc/>
  public async Task<bool> LogPermissionChangeAsync(
      long roleId,
      string permission,
      PermissionAuditType auditType,
      long operatorId,
      string operatorName,
      string ipAddress,
      string? remark = null)
  {
    var audit = new LeanPermissionAudit
    {
      RoleId = roleId,
      Permission = permission,
      AuditType = auditType,
      OperatorId = operatorId,
      OperatorName = operatorName,
      IpAddress = ipAddress,
      Remark = remark
    };

    return await _db.Insertable(audit).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<PagedResult<PermissionAuditDto>> GetAuditLogsAsync(
      long? roleId = null,
      DateTime? startTime = null,
      DateTime? endTime = null,
      int pageIndex = 1,
      int pageSize = 10)
  {
    var query = _db.Queryable<LeanPermissionAudit>()
        .WhereIF(roleId.HasValue, a => a.RoleId == roleId)
        .WhereIF(startTime.HasValue, a => a.CreateTime >= startTime)
        .WhereIF(endTime.HasValue, a => a.CreateTime <= endTime)
        .OrderByDescending(a => a.CreateTime);

    var total = await query.CountAsync();
    var items = await query
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .Select(a => new PermissionAuditDto
        {
          Id = a.Id,
          RoleId = a.RoleId,
          Permission = a.Permission,
          AuditType = a.AuditType,
          OperatorId = a.OperatorId,
          OperatorName = a.OperatorName,
          IpAddress = a.IpAddress,
          Remark = a.Remark,
          CreateTime = a.CreateTime
        })
        .ToListAsync();

    return new PagedResult<PermissionAuditDto>
    {
      Total = total,
      Items = items
    };
  }
}