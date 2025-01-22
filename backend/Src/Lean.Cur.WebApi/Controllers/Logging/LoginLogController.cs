using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lean.Cur.WebApi.Controllers.Logging;

/// <summary>
/// 登录日志控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoginLogController : LeanBaseController
{
  private readonly ILoginLogService _loginLogService;

  public LoginLogController(ILoginLogService loginLogService)
  {
    _loginLogService = loginLogService;
  }

  /// <summary>
  /// 分页查询登录日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  [HttpGet("page")]
  public async Task<LeanApiResult<LeanPagedResult<LoginLogDto>>> GetPageAsync([FromQuery] LoginLogQueryDto queryDto)
  {
    var result = await _loginLogService.GetPageAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取登录日志详情
  /// </summary>
  /// <param name="id">日志ID</param>
  /// <returns>日志详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LoginLogDto>> GetAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<LoginLogDto>("日志ID必须大于0");
    }
    var result = await _loginLogService.GetAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 清空指定日期之前的日志
  /// </summary>
  /// <param name="beforeTime">指定日期</param>
  /// <returns>清空的记录数</returns>
  [HttpDelete("clear")]
  public async Task<LeanApiResult<int>> ClearAsync([FromQuery] DateTime beforeTime)
  {
    if (beforeTime > DateTime.Now)
    {
      return ValidateError<int>("清理日期不能大于当前时间");
    }
    var count = await _loginLogService.ClearAsync(beforeTime);
    return Success(count, $"成功清理{count}条日志");
  }

  /// <summary>
  /// 导出登录日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>Excel文件</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LoginLogQueryDto queryDto)
  {
    var bytes = await _loginLogService.ExportAsync(queryDto);
    return ExcelResponse(bytes, "登录日志.xlsx");
  }

  /// <summary>
  /// 获取登录统计信息
  /// </summary>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>统计信息</returns>
  [HttpGet("stats")]
  public async Task<LeanApiResult<LoginLogStatsDto>> GetStatsAsync([FromQuery] DateTime? startTime = null, [FromQuery] DateTime? endTime = null)
  {
    if (startTime.HasValue && endTime.HasValue && startTime > endTime)
    {
      return ValidateError<LoginLogStatsDto>("开始时间不能大于结束时间");
    }
    var result = await _loginLogService.GetStatsAsync(startTime, endTime);
    return Success(result);
  }

  /// <summary>
  /// 获取登录趋势
  /// </summary>
  /// <param name="days">天数</param>
  /// <returns>趋势数据</returns>
  [HttpGet("trend")]
  public async Task<LeanApiResult<List<LoginLogTrendDto>>> GetTrendAsync([FromQuery] int days = 7)
  {
    if (days <= 0 || days > 90)
    {
      return ValidateError<List<LoginLogTrendDto>>("天数必须在1-90之间");
    }

    var result = await _loginLogService.GetTrendAsync(days);
    return Success(result);
  }
}