using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Logging;

/// <summary>
/// 操作日志服务接口
/// </summary>
public interface IOperationLogService
{
  /// <summary>
  /// 分页查询操作日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  Task<PagedResult<OperationLogDto>> GetPageAsync(OperationLogQueryDto queryDto);

  /// <summary>
  /// 获取操作日志详情
  /// </summary>
  /// <param name="id">日志ID</param>
  /// <returns>日志详情</returns>
  Task<OperationLogDto> GetAsync(long id);

  /// <summary>
  /// 清空指定日期之前的日志
  /// </summary>
  /// <param name="beforeTime">指定日期</param>
  /// <returns>清空的记录数</returns>
  Task<int> ClearAsync(DateTime beforeTime);

  /// <summary>
  /// 导出操作日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>Excel文件字节数组</returns>
  Task<byte[]> ExportAsync(OperationLogQueryDto queryDto);

  /// <summary>
  /// 添加操作日志
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="moduleName">模块名称</param>
  /// <param name="operationType">操作类型</param>
  /// <param name="description">操作描述</param>
  /// <param name="httpContext">HTTP上下文</param>
  /// <returns>添加结果</returns>
  Task<bool> AddOperationLogAsync(long userId, string moduleName, string operationType, string description, HttpContext httpContext);
}