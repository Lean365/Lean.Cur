using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Logging;

/// <summary>
/// 操作日志控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OperationLogController : LeanBaseController
{
  private readonly IOperationLogService _operationLogService;

  public OperationLogController(IOperationLogService operationLogService)
  {
    _operationLogService = operationLogService;
  }

  /// <summary>
  /// 分页查询操作日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  [HttpGet("page")]
  public async Task<LeanApiResult<LeanPagedResult<OperationLogDto>>> GetPageAsync([FromQuery] OperationLogQueryDto queryDto)
  {
    var result = await _operationLogService.GetPageAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取操作日志详情
  /// </summary>
  /// <param name="id">日志ID</param>
  /// <returns>日志详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<OperationLogDto>> GetAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<OperationLogDto>("日志ID必须大于0");
    }
    var result = await _operationLogService.GetAsync(id);
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
    var count = await _operationLogService.ClearAsync(beforeTime);
    return Success(count, $"成功清理{count}条日志");
  }

  /// <summary>
  /// 导出操作日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>Excel文件</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] OperationLogQueryDto queryDto)
  {
    var bytes = await _operationLogService.ExportAsync(queryDto);
    return ExcelResponse(bytes, "操作日志.xlsx");
  }
}