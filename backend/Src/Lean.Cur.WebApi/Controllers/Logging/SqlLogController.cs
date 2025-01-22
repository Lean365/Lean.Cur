using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Logging;

/// <summary>
/// SQL差异日志控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SqlLogController : LeanBaseController
{
  private readonly ISqlLogService _sqlLogService;

  public SqlLogController(ISqlLogService sqlLogService)
  {
    _sqlLogService = sqlLogService;
  }

  /// <summary>
  /// 分页查询SQL差异日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  [HttpGet("page")]
  public async Task<LeanApiResult<LeanPagedResult<SqlLogDto>>> GetPageAsync([FromQuery] SqlLogQueryDto queryDto)
  {
    var result = await _sqlLogService.GetPageAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取SQL差异日志详情
  /// </summary>
  /// <param name="id">日志ID</param>
  /// <returns>日志详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<SqlLogDto>> GetAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<SqlLogDto>("日志ID必须大于0");
    }
    var result = await _sqlLogService.GetAsync(id);
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
    var count = await _sqlLogService.ClearAsync(beforeTime);
    return Success(count, $"成功清理{count}条日志");
  }

  /// <summary>
  /// 导出SQL差异日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>Excel文件</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] SqlLogQueryDto queryDto)
  {
    var bytes = await _sqlLogService.ExportAsync(queryDto);
    return ExcelResponse(bytes, "SQL差异日志.xlsx");
  }
}