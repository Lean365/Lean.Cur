using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Logging;

/// <summary>
/// 登录日志服务接口
/// </summary>
public interface ILoginLogService
{
  /// <summary>
  /// 分页查询登录日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  Task<PagedResult<LoginLogDto>> GetPageAsync(LoginLogQueryDto queryDto);

  /// <summary>
  /// 获取登录日志详情
  /// </summary>
  /// <param name="id">日志ID</param>
  /// <returns>日志详情</returns>
  Task<LoginLogDto> GetAsync(long id);

  /// <summary>
  /// 清空指定日期之前的日志
  /// </summary>
  /// <param name="beforeTime">指定日期</param>
  /// <returns>清空的记录数</returns>
  Task<int> ClearAsync(DateTime beforeTime);

  /// <summary>
  /// 导出登录日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>Excel文件字节数组</returns>
  Task<byte[]> ExportAsync(LoginLogQueryDto queryDto);

  /// <summary>
  /// 获取登录统计信息
  /// </summary>
  /// <param name="startTime">开始时间</param>
  /// <param name="endTime">结束时间</param>
  /// <returns>统计信息</returns>
  Task<LoginLogStatsDto> GetStatsAsync(DateTime? startTime = null, DateTime? endTime = null);

  /// <summary>
  /// 获取登录趋势
  /// </summary>
  /// <param name="days">天数</param>
  /// <returns>趋势数据</returns>
  Task<List<LoginLogTrendDto>> GetTrendAsync(int days);

  /// <summary>
  /// 添加登录日志
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="httpContext">HTTP上下文</param>
  /// <returns>添加结果</returns>
  Task<bool> AddLoginLogAsync(long userId, HttpContext httpContext);
}