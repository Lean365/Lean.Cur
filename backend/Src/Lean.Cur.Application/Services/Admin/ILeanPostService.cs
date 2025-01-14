using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 岗位服务接口
/// </summary>
public interface ILeanPostService
{
    #region 基础操作

    /// <summary>
    /// 获取分页列表
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>分页列表</returns>
    Task<PagedResult<LeanPostDto>> GetPagedListAsync(LeanPostQueryDto queryDto);

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>岗位信息</returns>
    Task<LeanPostDto> GetByIdAsync(long id);

    /// <summary>
    /// 创建岗位
    /// </summary>
    /// <param name="createDto">创建参数</param>
    /// <returns>岗位ID</returns>
    Task<long> CreateAsync(LeanPostCreateDto createDto);

    /// <summary>
    /// 更新岗位
    /// </summary>
    /// <param name="updateDto">更新参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(LeanPostUpdateDto updateDto);

    /// <summary>
    /// 删除岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 批量删除岗位
    /// </summary>
    /// <param name="ids">岗位ID集合</param>
    /// <returns>是否成功</returns>
    Task<bool> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 更新岗位状态
    /// </summary>
    /// <param name="statusDto">状态参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateStatusAsync(LeanPostStatusDto statusDto);

    #endregion

    #region 导入导出

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>导入模板</returns>
    Task<byte[]> GetImportTemplateAsync();

    /// <summary>
    /// 导入数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanPostImportResultDto> ImportAsync(IFormFile file);

    /// <summary>
    /// 导出数据
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>导出文件</returns>
    Task<byte[]> ExportAsync(LeanPostQueryDto queryDto);

    #endregion
} 