using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Entities.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lean.Cur.Infrastructure.Services.Logging
{
  /// <summary>
  /// 审计日志服务
  /// </summary>
  public class AuditLogService : IAuditLogService
  {
    private readonly SqlSugarClient _db;
    private readonly ILogger<AuditLogService> _logger;
    private readonly LeanExcelHelper _excel;

    public AuditLogService(SqlSugarClient db, ILogger<AuditLogService> logger, LeanExcelHelper excel)
    {
      _db = db;
      _logger = logger;
      _excel = excel;
    }

    /// <inheritdoc/>
    public async Task<PagedResult<AuditLogDto>> GetPageAsync(AuditLogQueryDto queryDto)
    {
      try
      {
        var query = _db.Queryable<LeanAuditLog>()
            .WhereIF(queryDto.UserId.HasValue, x => x.UserId == queryDto.UserId)
            .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), x => x.UserName!.Contains(queryDto.UserName!))
            .WhereIF(!string.IsNullOrEmpty(queryDto.OperationType), x => x.OperationType!.Contains(queryDto.OperationType!))
            .WhereIF(queryDto.Status.HasValue, x => x.Status == queryDto.Status)
            .WhereIF(!string.IsNullOrEmpty(queryDto.IpAddress), x => x.IpAddress!.Contains(queryDto.IpAddress!))
            .WhereIF(queryDto.StartTime.HasValue, x => x.CreateTime >= queryDto.StartTime)
            .WhereIF(queryDto.EndTime.HasValue, x => x.CreateTime <= queryDto.EndTime)
            .OrderByDescending(x => x.CreateTime);

        var total = await query.CountAsync();
        var list = await query.Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .Select<AuditLogDto>()
            .ToListAsync();

        return new PagedResult<AuditLogDto>(list, total, queryDto.PageIndex, queryDto.PageSize);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "获取审计日志分页数据失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<AuditLogDto> GetAsync(long id)
    {
      var log = await _db.Queryable<LeanAuditLog>()
          .Where(l => l.Id == id)
          .Select<AuditLogDto>()
          .FirstAsync() ?? throw new BusinessException("审计日志不存在");
      return log;
    }

    /// <inheritdoc/>
    public async Task<int> ClearAsync(DateTime beforeTime)
    {
      try
      {
        return await _db.Deleteable<LeanAuditLog>()
            .Where(x => x.CreateTime <= beforeTime)
            .ExecuteCommandAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "清理审计日志失败, 清理时间: {BeforeTime}", beforeTime);
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<byte[]> ExportAsync(AuditLogQueryDto queryDto)
    {
      try
      {
        var list = await _db.Queryable<LeanAuditLog>()
            .WhereIF(queryDto.UserId.HasValue, x => x.UserId == queryDto.UserId)
            .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), x => x.UserName!.Contains(queryDto.UserName!))
            .WhereIF(!string.IsNullOrEmpty(queryDto.OperationType), x => x.OperationType!.Contains(queryDto.OperationType!))
            .WhereIF(queryDto.Status.HasValue, x => x.Status == queryDto.Status)
            .WhereIF(!string.IsNullOrEmpty(queryDto.IpAddress), x => x.IpAddress!.Contains(queryDto.IpAddress!))
            .WhereIF(queryDto.StartTime.HasValue, x => x.CreateTime >= queryDto.StartTime)
            .WhereIF(queryDto.EndTime.HasValue, x => x.CreateTime <= queryDto.EndTime)
            .OrderByDescending(x => x.CreateTime)
            .Select<AuditLogDto>()
            .ToListAsync();

        var headers = new Dictionary<string, string>
            {
                { "UserName", "用户名" },
                { "OperationType", "操作类型" },
                { "OperationDesc", "操作描述" },
                { "RequestMethod", "请求方法" },
                { "RequestUrl", "请求地址" },
                { "RequestParams", "请求参数" },
                { "ExecutionTime", "执行时长" },
                { "Status", "状态" },
                { "ErrorMessage", "错误信息" },
                { "IpAddress", "IP地址" },
                { "Location", "地理位置" },
                { "Browser", "浏览器" },
                { "Os", "操作系统" },
                { "CreateTime", "操作时间" }
            };

        return await _excel.ExportAsync<AuditLogDto>(headers, list);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "导出审计日志失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<AuditLogStatsDto> GetStatsAsync(DateTime? startTime = null, DateTime? endTime = null)
    {
      try
      {
        var query = _db.Queryable<LeanAuditLog>()
            .WhereIF(startTime.HasValue, x => x.CreateTime >= startTime)
            .WhereIF(endTime.HasValue, x => x.CreateTime <= endTime);

        var now = DateTime.Now;
        var lastDay = now.AddDays(-1);
        var lastWeek = now.AddDays(-7);
        var lastMonth = now.AddMonths(-1);

        var stats = new AuditLogStatsDto
        {
          TotalCount = await query.CountAsync(),
          SuccessCount = await query.Where(x => x.Status == 1).CountAsync(),
          FailCount = await query.Where(x => x.Status == 0).CountAsync(),
          LastDayCount = await query.Where(x => x.CreateTime >= lastDay).CountAsync(),
          LastWeekCount = await query.Where(x => x.CreateTime >= lastWeek).CountAsync(),
          LastMonthCount = await query.Where(x => x.CreateTime >= lastMonth).CountAsync()
        };

        return stats;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "获取审计日志统计数据失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<List<AuditLogTrendDto>> GetTrendAsync(int days)
    {
      try
      {
        if (days <= 0 || days > 90)
        {
          days = 7;
        }

        var endDate = DateTime.Now.Date;
        var startDate = endDate.AddDays(-days + 1);

        var query = _db.Queryable<LeanAuditLog>()
            .Where(x => x.CreateTime >= startDate && x.CreateTime < endDate.AddDays(1));

        var data = await query
            .GroupBy(x => new { Year = x.CreateTime.Year, Month = x.CreateTime.Month, Day = x.CreateTime.Day })
            .Select(x => new AuditLogTrendDto
            {
              Date = x.CreateTime,
              TotalCount = SqlFunc.AggregateCount(x.Id),
              SuccessCount = SqlFunc.AggregateSum(SqlFunc.IIF(x.Status == 1, 1, 0)),
              FailCount = SqlFunc.AggregateSum(SqlFunc.IIF(x.Status == 0, 1, 0))
            })
            .ToListAsync();

        // 补充缺失的日期
        var result = new List<AuditLogTrendDto>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
          var item = data.FirstOrDefault(x => x.Date.Date == date.Date) ?? new AuditLogTrendDto
          {
            Date = date,
            TotalCount = 0,
            SuccessCount = 0,
            FailCount = 0
          };
          result.Add(item);
        }

        return result;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "获取审计日志趋势数据失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<bool> AddAuditLogAsync(long userId, string operationType, string operationDesc, HttpContext httpContext, long executionTime, int status, string? errorMessage = null)
    {
      try
      {
        // 获取用户信息
        var user = await _db.Queryable<LeanUser>()
            .Where(u => u.Id == userId)
            .FirstAsync();

        if (user == null)
        {
          _logger.LogWarning("记录审计日志失败：用户不存在，用户ID：{UserId}", userId);
          return false;
        }

        // 获取请求信息
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        // 解析User-Agent获取浏览器和操作系统信息
        var (browser, os) = ParseUserAgent(userAgent);

        // 创建审计日志
        var log = new LeanAuditLog
        {
          UserId = userId,
          UserName = user.UserName,
          OperationType = operationType,
          OperationDesc = operationDesc,
          RequestMethod = httpContext.Request.Method,
          RequestUrl = httpContext.Request.Path,
          RequestParams = httpContext.Request.QueryString.ToString(),
          ExecutionTime = executionTime,
          Status = status,
          ErrorMessage = errorMessage,
          IpAddress = ip,
          Browser = browser,
          Os = os
        };

        // 保存日志
        await _db.Insertable(log).ExecuteCommandAsync();
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "添加审计日志失败");
        return false;
      }
    }

    /// <summary>
    /// 解析User-Agent获取浏览器和操作系统信息
    /// </summary>
    /// <param name="userAgent">User-Agent字符串</param>
    /// <returns>浏览器和操作系统信息</returns>
    private (string Browser, string Os) ParseUserAgent(string userAgent)
    {
      // 这里可以使用第三方库如UAParser来解析User-Agent
      // 为了简单起见,这里只做简单的判断
      var browser = "Unknown";
      var os = "Unknown";

      userAgent = userAgent.ToLower();

      if (userAgent.Contains("msie") || userAgent.Contains("trident"))
      {
        browser = "IE";
      }
      else if (userAgent.Contains("edge"))
      {
        browser = "Edge";
      }
      else if (userAgent.Contains("chrome"))
      {
        browser = "Chrome";
      }
      else if (userAgent.Contains("firefox"))
      {
        browser = "Firefox";
      }
      else if (userAgent.Contains("safari"))
      {
        browser = "Safari";
      }

      if (userAgent.Contains("windows"))
      {
        os = "Windows";
      }
      else if (userAgent.Contains("mac"))
      {
        os = "MacOS";
      }
      else if (userAgent.Contains("linux"))
      {
        os = "Linux";
      }
      else if (userAgent.Contains("android"))
      {
        os = "Android";
      }
      else if (userAgent.Contains("iphone") || userAgent.Contains("ipad"))
      {
        os = "iOS";
      }

      return (browser, os);
    }
  }
}