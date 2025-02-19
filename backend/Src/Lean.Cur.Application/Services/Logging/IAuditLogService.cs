using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Logging;

/// <summary>
/// 审计日志服务接口
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// 分页查询审计日志
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>分页结果</returns>
    Task<LeanPagedResult<AuditLogDto>> GetPageAsync(AuditLogQueryDto queryDto);

    /// <summary>
    /// 获取审计日志详情
    /// </summary>
    /// <param name="id">日志ID</param>
    /// <returns>日志详情</returns>
    Task<AuditLogDto> GetAsync(long id);

    /// <summary>
    /// 清空指定日期之前的日志
    /// </summary>
    /// <param name="beforeTime">指定日期</param>
    /// <returns>清空的记录数</returns>
    Task<int> ClearAsync(DateTime beforeTime);

    /// <summary>
    /// 导出审计日志
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>Excel文件字节数组</returns>
    Task<byte[]> ExportAsync(AuditLogQueryDto queryDto);

    /// <summary>
    /// 获取审计统计信息
    /// </summary>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>统计信息</returns>
    Task<AuditLogStatsDto> GetStatsAsync(DateTime? startTime = null, DateTime? endTime = null);

    /// <summary>
    /// 获取审计趋势
    /// </summary>
    /// <param name="days">天数</param>
    /// <returns>趋势数据</returns>
    Task<List<AuditLogTrendDto>> GetTrendAsync(int days);

    /// <summary>
    /// 添加审计日志
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="operationType">操作类型</param>
    /// <param name="operationDesc">操作描述</param>
    /// <param name="httpContext">HTTP上下文</param>
    /// <param name="executionTime">执行时长(毫秒)</param>
    /// <param name="status">状态(0-失败 1-成功)</param>
    /// <param name="errorMessage">错误信息</param>
    /// <returns>添加结果</returns>
    Task<bool> AddAuditLogAsync(long userId, string operationType, string operationDesc, HttpContext httpContext, long executionTime, int status, string? errorMessage = null);
} 