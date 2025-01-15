using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Logging
{
    /// <summary>
    /// 审计日志控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditLogController : LeanBaseController
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        /// <summary>
        /// 分页查询审计日志
        /// </summary>
        /// <param name="queryDto">查询参数</param>
        /// <returns>分页结果</returns>
        [HttpGet("page")]
        public async Task<LeanApiResponse<PagedResult<AuditLogDto>>> GetPageAsync([FromQuery] AuditLogQueryDto queryDto)
        {
            if (!ModelState.IsValid)
            {
                return ValidateError<PagedResult<AuditLogDto>>("请求参数验证失败");
            }

            var result = await _auditLogService.GetPageAsync(queryDto);
            return Success(result);
        }

        /// <summary>
        /// 获取审计日志详情
        /// </summary>
        /// <param name="id">日志ID</param>
        /// <returns>日志详情</returns>
        [HttpGet("{id}")]
        public async Task<LeanApiResponse<AuditLogDto>> GetAsync(long id)
        {
            if (id <= 0)
            {
                return ValidateError<AuditLogDto>("日志ID必须大于0");
            }

            var result = await _auditLogService.GetAsync(id);
            return Success(result);
        }

        /// <summary>
        /// 清空指定日期之前的日志
        /// </summary>
        /// <param name="beforeTime">指定日期</param>
        /// <returns>清空的记录数</returns>
        [HttpDelete("clear")]
        public async Task<LeanApiResponse<int>> ClearAsync(DateTime beforeTime)
        {
            if (beforeTime >= DateTime.Now)
            {
                return ValidateError<int>("清理时间必须小于当前时间");
            }

            var result = await _auditLogService.ClearAsync(beforeTime);
            return Success(result);
        }

        /// <summary>
        /// 导出审计日志
        /// </summary>
        /// <param name="queryDto">查询参数</param>
        /// <returns>Excel文件</returns>
        [HttpGet("export")]
        public async Task<IActionResult> ExportAsync([FromQuery] AuditLogQueryDto queryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("请求参数验证失败");
            }

            var result = await _auditLogService.ExportAsync(queryDto);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"审计日志_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        /// <summary>
        /// 获取审计统计信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>统计信息</returns>
        [HttpGet("stats")]
        public async Task<LeanApiResponse<AuditLogStatsDto>> GetStatsAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            if (startTime.HasValue && endTime.HasValue && startTime > endTime)
            {
                return ValidateError<AuditLogStatsDto>("开始时间不能大于结束时间");
            }

            var result = await _auditLogService.GetStatsAsync(startTime, endTime);
            return Success(result);
        }

        /// <summary>
        /// 获取审计趋势
        /// </summary>
        /// <param name="days">天数</param>
        /// <returns>趋势数据</returns>
        [HttpGet("trend")]
        public async Task<LeanApiResponse<List<AuditLogTrendDto>>> GetTrendAsync(int days = 7)
        {
            if (days <= 0 || days > 90)
            {
                return ValidateError<List<AuditLogTrendDto>>("天数必须在1-90之间");
            }

            var result = await _auditLogService.GetTrendAsync(days);
            return Success(result);
        }
    }
} 