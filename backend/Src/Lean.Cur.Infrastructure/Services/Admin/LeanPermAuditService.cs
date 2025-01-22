using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using SqlSugar;
using Mapster;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 权限审计服务实现
/// </summary>
public class LeanPermAuditService : ILeanPermAuditService
{
  private readonly SqlSugarClient _db;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">数据库客户端</param>
  public LeanPermAuditService(SqlSugarClient db)
  {
    _db = db;
  }

  /// <inheritdoc/>
  public async Task<bool> LogPermissionChangeAsync(LeanPermAuditDto auditDto)
  {
    if (auditDto == null)
    {
      return false;
    }

    // 创建审计记录
    var audit = new LeanPermAudit
    {
      AuditType = auditDto.AuditType!.Value,
      RoleId = auditDto.RoleId!.Value,
      Permission = auditDto.Permission!,
      OperatorId = auditDto.OperatorId!.Value,
      OperatorName = auditDto.OperatorName!,
      IpAddress = auditDto.IpAddress!,
      Remark = auditDto.Remark
    };

    return await _db.Insertable(audit).ExecuteCommandAsync() > 0;
  }

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanPermAuditDto>> GetPermissionAuditsAsync(LeanPermAuditDto queryDto)
  {
    // 构建查询
    var query = _db.Queryable<LeanPermAudit>()
        .WhereIF(queryDto.RoleId > 0, a => a.RoleId == queryDto.RoleId)
        .WhereIF(queryDto.AuditType.HasValue, a => a.AuditType == queryDto.AuditType)
        .WhereIF(!string.IsNullOrEmpty(queryDto.Permission), a => a.Permission.Contains(queryDto.Permission))
        .WhereIF(queryDto.StartTime.HasValue, a => a.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, a => a.CreateTime <= queryDto.EndTime)
        .OrderByDescending(a => a.CreateTime);

    // 获取总记录数
    var total = await query.CountAsync();

    // 执行分页查询
    var items = await query
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .ToListAsync();

    // 转换为DTO
    var dtos = items.Adapt<List<LeanPermAuditDto>>();

    // 返回分页结果
    return new LeanPagedResult<LeanPermAuditDto>(dtos, total, queryDto.PageIndex, queryDto.PageSize);
  }
}