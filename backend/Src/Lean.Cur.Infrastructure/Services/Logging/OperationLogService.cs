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

namespace Lean.Cur.Infrastructure.Services.Logging;

/// <summary>
/// 操作日志服务
/// </summary>
public class OperationLogService : IOperationLogService
{
  private readonly SqlSugarClient _db;
  private readonly ILogger<OperationLogService> _logger;
  private readonly LeanExcelHelper _excel;

  public OperationLogService(SqlSugarClient db, ILogger<OperationLogService> logger, LeanExcelHelper excel)
  {
    _db = db;
    _logger = logger;
    _excel = excel;
  }

  /// <inheritdoc/>
  public async Task<PagedResult<OperationLogDto>> GetPageAsync(OperationLogQueryDto queryDto)
  {
    try
    {
      var query = _db.Queryable<LeanOperationLog>()
          .WhereIF(queryDto.UserId.HasValue, x => x.UserId == queryDto.UserId)
          .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), x => x.UserName!.Contains(queryDto.UserName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.ModuleName), x => x.ModuleName!.Contains(queryDto.ModuleName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.OperationType), x => x.OperationType!.Contains(queryDto.OperationType!))
          .WhereIF(queryDto.Success.HasValue, x => x.Success == queryDto.Success)
          .WhereIF(!string.IsNullOrEmpty(queryDto.IpAddress), x => x.IpAddress!.Contains(queryDto.IpAddress!))
          .WhereIF(queryDto.StartTime.HasValue, x => x.CreateTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, x => x.CreateTime <= queryDto.EndTime);

      var total = await query.CountAsync();

      var list = await query.OrderByDescending(x => x.CreateTime)
          .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
          .Take(queryDto.PageSize)
          .Select<OperationLogDto>()
          .ToListAsync();

      return new PagedResult<OperationLogDto>(list, total, queryDto.PageIndex, queryDto.PageSize);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "分页查询操作日志失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<OperationLogDto> GetAsync(long id)
  {
    var log = await _db.Queryable<LeanOperationLog>()
        .Where(l => l.Id == id)
        .Select<OperationLogDto>()
        .FirstAsync() ?? throw new BusinessException("操作日志不存在");
    return log;
  }

  /// <inheritdoc/>
  public async Task<int> ClearAsync(DateTime beforeTime)
  {
    try
    {
      return await _db.Deleteable<LeanOperationLog>()
          .Where(x => x.CreateTime <= beforeTime)
          .ExecuteCommandAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "清理操作日志失败, 清理时间: {BeforeTime}", beforeTime);
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(OperationLogQueryDto queryDto)
  {
    try
    {
      var list = await _db.Queryable<LeanOperationLog>()
          .WhereIF(queryDto.UserId.HasValue, x => x.UserId == queryDto.UserId)
          .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), x => x.UserName!.Contains(queryDto.UserName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.ModuleName), x => x.ModuleName!.Contains(queryDto.ModuleName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.OperationType), x => x.OperationType!.Contains(queryDto.OperationType!))
          .WhereIF(queryDto.Success.HasValue, x => x.Success == queryDto.Success)
          .WhereIF(!string.IsNullOrEmpty(queryDto.IpAddress), x => x.IpAddress!.Contains(queryDto.IpAddress!))
          .WhereIF(queryDto.StartTime.HasValue, x => x.CreateTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, x => x.CreateTime <= queryDto.EndTime)
          .OrderByDescending(x => x.CreateTime)
          .Select<OperationLogExportDto>()
          .ToListAsync();

      var headers = new Dictionary<string, string>
            {
                { "UserName", "用户名" },
                { "ModuleName", "模块名称" },
                { "OperationType", "操作类型" },
                { "Description", "操作描述" },
                { "Success", "操作结果" },
                { "IpAddress", "IP地址" },
                { "Browser", "浏览器" },
                { "Os", "操作系统" },
                { "ExecutionTime", "执行时长(毫秒)" },
                { "CreateTime", "操作时间" }
            };

      return await _excel.ExportAsync<OperationLogExportDto>(headers, list);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "导出操作日志失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> AddOperationLogAsync(long userId, string moduleName, string operationType, string description, HttpContext httpContext)
  {
    try
    {
      // 获取用户信息
      var user = await _db.Queryable<LeanUser>()
          .Where(u => u.Id == userId)
          .FirstAsync();

      if (user == null)
      {
        _logger.LogWarning("记录操作日志失败：用户不存在，用户ID：{UserId}", userId);
        return false;
      }

      // 获取请求信息
      var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
      var ip = httpContext.Connection.RemoteIpAddress?.ToString();

      // 解析User-Agent获取浏览器和操作系统信息
      var (browser, os) = ParseUserAgent(userAgent);

      // 创建操作日志
      var log = new LeanOperationLog
      {
        UserId = userId,
        UserName = user.UserName,
        ModuleName = moduleName,
        OperationType = operationType,
        Description = description,
        RequestMethod = httpContext.Request.Method,
        RequestUrl = httpContext.Request.Path,
        RequestParams = await GetRequestParams(httpContext),
        Success = true, // 默认为成功，失败时需要单独处理
        IpAddress = ip,
        Browser = browser,
        Os = os,
        ExecutionTime = 0 // 执行时长需要在操作完成时设置
      };

      await _db.Insertable(log).ExecuteCommandAsync();
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "添加操作日志失败");
      return false;
    }
  }

  /// <summary>
  /// 解析User-Agent获取浏览器和操作系统信息
  /// </summary>
  private (string browser, string os) ParseUserAgent(string userAgent)
  {
    // 这里可以使用第三方库如UAParser来解析，这里简单处理
    var browser = "Unknown";
    var os = "Unknown";

    userAgent = userAgent.ToLower();

    // 解析浏览器
    if (userAgent.Contains("edge"))
      browser = "Edge";
    else if (userAgent.Contains("chrome"))
      browser = "Chrome";
    else if (userAgent.Contains("firefox"))
      browser = "Firefox";
    else if (userAgent.Contains("safari"))
      browser = "Safari";
    else if (userAgent.Contains("opera"))
      browser = "Opera";
    else if (userAgent.Contains("msie") || userAgent.Contains("trident"))
      browser = "IE";

    // 解析操作系统
    if (userAgent.Contains("windows"))
      os = "Windows";
    else if (userAgent.Contains("mac"))
      os = "MacOS";
    else if (userAgent.Contains("linux"))
      os = "Linux";
    else if (userAgent.Contains("android"))
      os = "Android";
    else if (userAgent.Contains("iphone") || userAgent.Contains("ipad"))
      os = "iOS";

    return (browser, os);
  }

  /// <summary>
  /// 获取请求参数
  /// </summary>
  private async Task<string> GetRequestParams(HttpContext httpContext)
  {
    try
    {
      var request = httpContext.Request;

      // 如果是GET请求，返回查询字符串
      if (request.Method == "GET")
        return request.QueryString.ToString();

      // 如果是POST/PUT请求，尝试读取请求体
      if (request.Method == "POST" || request.Method == "PUT")
      {
        if (request.ContentLength > 0)
        {
          request.EnableBuffering();
          using var reader = new StreamReader(request.Body, leaveOpen: true);
          var body = await reader.ReadToEndAsync();
          request.Body.Position = 0; // 重置位置，允许后续中间件读取
          return body;
        }
      }

      return string.Empty;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取请求参数失败");
      return string.Empty;
    }
  }
}