using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Entities.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Logging;

/// <summary>
/// SQL差异日志服务
/// </summary>
public class SqlLogService : ISqlLogService
{
  private readonly SqlSugarClient _db;
  private readonly ILogger<SqlLogService> _logger;
  private readonly LeanExcelHelper _excel;

  public SqlLogService(SqlSugarClient db, ILogger<SqlLogService> logger, LeanExcelHelper excel)
  {
    _db = db;
    _logger = logger;
    _excel = excel;
  }

  /// <inheritdoc/>
  public async Task<PagedResult<SqlLogDto>> GetPageAsync(SqlLogQueryDto queryDto)
  {
    try
    {
      var query = _db.Queryable<LeanSqlLog>()
          .WhereIF(!string.IsNullOrEmpty(queryDto.TableName), x => x.TableName!.Contains(queryDto.TableName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.PrimaryKey), x => x.PrimaryKey!.Contains(queryDto.PrimaryKey!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.OperationType), x => x.OperationType!.Contains(queryDto.OperationType!))
          .WhereIF(queryDto.OperatorId.HasValue, x => x.OperatorId == queryDto.OperatorId)
          .WhereIF(!string.IsNullOrEmpty(queryDto.OperatorName), x => x.OperatorName!.Contains(queryDto.OperatorName!))
          .WhereIF(queryDto.StartTime.HasValue, x => x.OperateTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, x => x.OperateTime <= queryDto.EndTime);

      var total = await query.CountAsync();

      var list = await query.OrderByDescending(x => x.OperateTime)
          .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
          .Take(queryDto.PageSize)
          .Select<SqlLogDto>()
          .ToListAsync();

      return new PagedResult<SqlLogDto>(list, total, queryDto.PageIndex, queryDto.PageSize);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "分页查询SQL差异日志失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<SqlLogDto> GetAsync(long id)
  {
    var log = await _db.Queryable<LeanSqlLog>()
        .Where(l => l.Id == id)
        .Select<SqlLogDto>()
        .FirstAsync() ?? throw new BusinessException("SQL差异日志不存在");
    return log;
  }

  /// <inheritdoc/>
  public async Task<int> ClearAsync(DateTime beforeTime)
  {
    try
    {
      return await _db.Deleteable<LeanSqlLog>()
          .Where(x => x.OperateTime <= beforeTime)
          .ExecuteCommandAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "清理SQL差异日志失败, 清理时间: {BeforeTime}", beforeTime);
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(SqlLogQueryDto queryDto)
  {
    try
    {
      var list = await _db.Queryable<LeanSqlLog>()
          .WhereIF(!string.IsNullOrEmpty(queryDto.TableName), x => x.TableName!.Contains(queryDto.TableName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.PrimaryKey), x => x.PrimaryKey!.Contains(queryDto.PrimaryKey!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.OperationType), x => x.OperationType!.Contains(queryDto.OperationType!))
          .WhereIF(queryDto.OperatorId.HasValue, x => x.OperatorId == queryDto.OperatorId)
          .WhereIF(!string.IsNullOrEmpty(queryDto.OperatorName), x => x.OperatorName!.Contains(queryDto.OperatorName!))
          .WhereIF(queryDto.StartTime.HasValue, x => x.OperateTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, x => x.OperateTime <= queryDto.EndTime)
          .OrderByDescending(x => x.OperateTime)
          .Select<SqlLogExportDto>()
          .ToListAsync();

      var headers = new Dictionary<string, string>
            {
                { "TableName", "表名" },
                { "PrimaryKey", "主键值" },
                { "OperationType", "操作类型" },
                { "BeforeData", "变更前数据" },
                { "AfterData", "变更后数据" },
                { "OperatorName", "操作人" },
                { "OperateTime", "操作时间" },
                { "IpAddress", "IP地址" },
                { "Remark", "备注" }
            };

      return await _excel.ExportAsync<SqlLogExportDto>(headers, list);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "导出SQL差异日志失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> AddSqlLogAsync(string tableName, string primaryKey, string operationType, string? beforeData, string? afterData, long operatorId, string operatorName, HttpContext httpContext, string? remark = null)
  {
    try
    {
      // 获取请求信息
      var ip = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";

      // 创建SQL差异日志
      var log = new LeanSqlLog
      {
        TableName = tableName,
        PrimaryKey = primaryKey,
        OperationType = operationType,
        BeforeData = beforeData,
        AfterData = afterData,
        OperatorId = operatorId,
        OperatorName = operatorName,
        OperateTime = DateTime.Now,
        IpAddress = ip,
        Remark = remark
      };

      await _db.Insertable(log).ExecuteCommandAsync();
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "添加SQL差异日志失败");
      return false;
    }
  }
}