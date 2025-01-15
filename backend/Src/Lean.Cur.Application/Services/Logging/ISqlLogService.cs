using Lean.Cur.Application.Dtos.Logging;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Logging;

/// <summary>
/// SQL差异日志服务接口
/// </summary>
public interface ISqlLogService
{
  /// <summary>
  /// 分页查询SQL差异日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  Task<PagedResult<SqlLogDto>> GetPageAsync(SqlLogQueryDto queryDto);

  /// <summary>
  /// 获取SQL差异日志详情
  /// </summary>
  /// <param name="id">日志ID</param>
  /// <returns>日志详情</returns>
  Task<SqlLogDto> GetAsync(long id);

  /// <summary>
  /// 清空指定日期之前的日志
  /// </summary>
  /// <param name="beforeTime">指定日期</param>
  /// <returns>清空的记录数</returns>
  Task<int> ClearAsync(DateTime beforeTime);

  /// <summary>
  /// 导出SQL差异日志
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>Excel文件字节数组</returns>
  Task<byte[]> ExportAsync(SqlLogQueryDto queryDto);

  /// <summary>
  /// 添加SQL差异日志
  /// </summary>
  /// <param name="tableName">表名</param>
  /// <param name="primaryKey">主键值</param>
  /// <param name="operationType">操作类型</param>
  /// <param name="beforeData">变更前数据</param>
  /// <param name="afterData">变更后数据</param>
  /// <param name="operatorId">操作人ID</param>
  /// <param name="operatorName">操作人名称</param>
  /// <param name="httpContext">HTTP上下文</param>
  /// <param name="remark">备注</param>
  /// <returns>添加结果</returns>
  Task<bool> AddSqlLogAsync(string tableName, string primaryKey, string operationType, string? beforeData, string? afterData, long operatorId, string operatorName, HttpContext httpContext, string? remark = null);
}