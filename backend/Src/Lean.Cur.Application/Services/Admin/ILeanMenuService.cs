using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 菜单服务接口
/// </summary>
public interface ILeanMenuService
{
    #region 基础操作

    /// <summary>
    /// 获取菜单分页列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>菜单分页列表</returns>
    Task<LeanPagedResult<LeanMenuDto>> GetPagedListAsync(LeanMenuQueryDto queryDto);

    /// <summary>
    /// 获取菜单树形列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>菜单树形列表</returns>
    Task<List<LeanMenuDto>> GetTreeListAsync(LeanMenuQueryDto queryDto);

    /// <summary>
    /// 获取菜单详情
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>菜单详情</returns>
    Task<LeanMenuDto> GetByIdAsync(long id);

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="createDto">创建参数</param>
    /// <returns>菜单ID</returns>
    Task<long> CreateAsync(LeanMenuCreateDto createDto);

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="updateDto">更新参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(LeanMenuUpdateDto updateDto);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    /// <param name="ids">菜单ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 更新菜单状态
    /// </summary>
    /// <param name="statusDto">状态参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateStatusAsync(LeanMenuStatusDto statusDto);

    #endregion 基础操作

    #region 导入导出

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>导入模板</returns>
    Task<byte[]> GetImportTemplateAsync();

    /// <summary>
    /// 导入菜单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanMenuImportResultDto> ImportAsync(IFormFile file);

    /// <summary>
    /// 导出菜单
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>导出文件</returns>
    Task<byte[]> ExportAsync(LeanMenuQueryDto queryDto);

    #endregion 导入导出
} 