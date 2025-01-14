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

namespace Lean.Cur.Infrastructure.Services.Logging
{
  /// <summary>
  /// 登录日志服务
  /// </summary>
  public class LoginLogService : ILoginLogService
  {
    private readonly ISqlSugarClient _db;
    private readonly ILogger<LoginLogService> _logger;

    public LoginLogService(ISqlSugarClient db, ILogger<LoginLogService> logger)
    {
      _db = db;
      _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<PagedResult<LoginLogDto>> GetPageAsync(LoginLogQueryDto queryDto)
    {
      try
      {
        var query = _db.Queryable<LeanLoginLog>()
            .WhereIF(queryDto.UserId.HasValue, x => x.UserId == queryDto.UserId)
            .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), x => x.UserName!.Contains(queryDto.UserName!))
            .WhereIF(queryDto.LoginType.HasValue, x => x.LoginType == queryDto.LoginType)
            .WhereIF(queryDto.Status.HasValue, x => x.Status == queryDto.Status)
            .WhereIF(!string.IsNullOrEmpty(queryDto.IpAddress), x => x.IpAddress!.Contains(queryDto.IpAddress!))
            .WhereIF(queryDto.StartTime.HasValue, x => x.LoginTime >= queryDto.StartTime)
            .WhereIF(queryDto.EndTime.HasValue, x => x.LoginTime <= queryDto.EndTime)
            .OrderByDescending(x => x.LoginTime);

        var total = await query.CountAsync();
        var list = await query.Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .Select<LoginLogDto>()
            .ToListAsync();

        return new PagedResult<LoginLogDto>(list, total, queryDto.PageIndex, queryDto.PageSize);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "获取登录日志分页数据失败");
        throw;
      }
    }

    /// <summary>
    /// 获取登录日志
    /// </summary>
    /// <param name="id">日志ID</param>
    /// <returns>登录日志</returns>
    public async Task<LoginLogDto> GetAsync(long id)
    {
      var log = await _db.Queryable<LeanLoginLog>()
          .Where(l => l.Id == id)
          .Select<LoginLogDto>()
          .FirstAsync() ?? throw new BusinessException("登录日志不存在");
      return log;
    }

    /// <inheritdoc/>
    public async Task<int> ClearAsync(DateTime beforeTime)
    {
      try
      {
        return await _db.Deleteable<LeanLoginLog>()
            .Where(x => x.LoginTime <= beforeTime)
            .ExecuteCommandAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "清理登录日志失败, 清理时间: {BeforeTime}", beforeTime);
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<byte[]> ExportAsync(LoginLogQueryDto queryDto)
    {
      try
      {
        var list = await _db.Queryable<LeanLoginLog>()
            .WhereIF(queryDto.UserId.HasValue, x => x.UserId == queryDto.UserId)
            .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), x => x.UserName!.Contains(queryDto.UserName!))
            .WhereIF(queryDto.LoginType.HasValue, x => x.LoginType == queryDto.LoginType)
            .WhereIF(queryDto.Status.HasValue, x => x.Status == queryDto.Status)
            .WhereIF(!string.IsNullOrEmpty(queryDto.IpAddress), x => x.IpAddress!.Contains(queryDto.IpAddress!))
            .WhereIF(queryDto.StartTime.HasValue, x => x.LoginTime >= queryDto.StartTime)
            .WhereIF(queryDto.EndTime.HasValue, x => x.LoginTime <= queryDto.EndTime)
            .OrderByDescending(x => x.LoginTime)
            .Select<LoginLogDto>()
            .ToListAsync();

        var headers = new Dictionary<string, string>
        {
          { "UserName", "用户名" },
          { "LoginTime", "登录时间" },
          { "LoginType", "登录类型" },
          { "Status", "状态" },
          { "IpAddress", "IP地址" },
          { "Location", "地理位置" },
          { "Browser", "浏览器" },
          { "Os", "操作系统" },
          { "Device", "设备" },
          { "Message", "消息" }
        };

        return await LeanExcelHelper.ExportAsync(list, headers);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "导出登录日志失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<LoginLogStatsDto> GetStatsAsync(DateTime? startTime = null, DateTime? endTime = null)
    {
      try
      {
        var query = _db.Queryable<LeanLoginLog>()
            .WhereIF(startTime.HasValue, x => x.LoginTime >= startTime)
            .WhereIF(endTime.HasValue, x => x.LoginTime <= endTime);

        var now = DateTime.Now;
        var lastDay = now.AddDays(-1);
        var lastWeek = now.AddDays(-7);
        var lastMonth = now.AddMonths(-1);

        var stats = new LoginLogStatsDto
        {
          TotalCount = await query.CountAsync(),
          SuccessCount = await query.Where(x => x.Status == 1).CountAsync(),
          FailCount = await query.Where(x => x.Status == 2).CountAsync(),
          AbnormalCount = await query.Where(x => x.LoginType == 2 && x.Status == 2).CountAsync(),
          LastDayCount = await query.Where(x => x.LoginTime >= lastDay).CountAsync(),
          LastWeekCount = await query.Where(x => x.LoginTime >= lastWeek).CountAsync(),
          LastMonthCount = await query.Where(x => x.LoginTime >= lastMonth).CountAsync()
        };

        return stats;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "获取登录日志统计数据失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<List<LoginLogTrendDto>> GetTrendAsync(int days)
    {
      try
      {
        if (days <= 0 || days > 90)
        {
          days = 7;
        }

        var endDate = DateTime.Now.Date;
        var startDate = endDate.AddDays(-days + 1);

        var query = _db.Queryable<LeanLoginLog>()
            .Where(x => x.LoginTime >= startDate && x.LoginTime < endDate.AddDays(1));

        var data = await query
            .GroupBy(x => new { Year = x.LoginTime.Year, Month = x.LoginTime.Month, Day = x.LoginTime.Day })
            .Select(x => new LoginLogTrendDto
            {
              Date = x.LoginTime,
              TotalCount = SqlFunc.AggregateCount(x.Id),
              SuccessCount = SqlFunc.AggregateSum(SqlFunc.IIF(x.Status == 1, 1, 0)),
              FailCount = SqlFunc.AggregateSum(SqlFunc.IIF(x.Status == 2, 1, 0))
            })
            .ToListAsync();

        // 补充缺失的日期
        var result = new List<LoginLogTrendDto>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
          var item = data.FirstOrDefault(x => x.Date.Date == date.Date) ?? new LoginLogTrendDto
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
        _logger.LogError(ex, "获取登录日志趋势数据失败");
        throw;
      }
    }

    /// <inheritdoc/>
    public async Task<bool> AddLoginLogAsync(long userId, HttpContext httpContext)
    {
      try
      {
        // 获取用户信息
        var user = await _db.Queryable<LeanUser>()
            .Where(u => u.Id == userId)
            .FirstAsync();

        if (user == null)
        {
          _logger.LogWarning("记录登录日志失败：用户不存在，用户ID：{UserId}", userId);
          return false;
        }

        // 获取请求信息
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        // 解析User-Agent获取浏览器和操作系统信息
        var (browser, os) = ParseUserAgent(userAgent);

        // 创建登录日志
        var log = new LeanLoginLog
        {
          UserId = userId,
          UserName = user.UserName,
          LoginType = 1, // 登录
          Status = 1, // 成功
          LoginTime = DateTime.Now,
          IpAddress = ip,
          Browser = browser,
          Os = os,
          Device = userAgent,
          Message = "登录成功"
        };

        // 保存日志
        await _db.Insertable(log).ExecuteCommandAsync();
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "记录登录日志失败，用户ID：{UserId}", userId);
        return false;
      }
    }

    /// <summary>
    /// 解析User-Agent
    /// </summary>
    private (string browser, string os) ParseUserAgent(string? userAgent)
    {
      if (string.IsNullOrEmpty(userAgent))
      {
        return ("未知", "未知");
      }

      string browser = "未知";
      string os = "未知";

      // 简单的浏览器检测
      if (userAgent.Contains("Edge"))
        browser = "Edge";
      else if (userAgent.Contains("Chrome"))
        browser = "Chrome";
      else if (userAgent.Contains("Firefox"))
        browser = "Firefox";
      else if (userAgent.Contains("Safari"))
        browser = "Safari";
      else if (userAgent.Contains("Opera"))
        browser = "Opera";
      else if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
        browser = "IE";

      // 简单的操作系统检测
      if (userAgent.Contains("Windows"))
        os = "Windows";
      else if (userAgent.Contains("Mac"))
        os = "MacOS";
      else if (userAgent.Contains("Linux"))
        os = "Linux";
      else if (userAgent.Contains("Android"))
        os = "Android";
      else if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
        os = "iOS";

      return (browser, os);
    }
  }
}