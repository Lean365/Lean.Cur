using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 部门管理服务接口
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本接口定义了部门管理相关的所有服务方法，包括：
/// 1. 基础操作（查询、创建、更新、删除等）
/// 2. 导入导出功能
/// </remarks>
public interface ILeanDeptService
{
    /// <summary>
    /// 获取部门分页列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>分页结果</returns>
    Task<PagedResult<LeanDeptDto>> GetPagedListAsync(LeanDeptQueryDto queryDto);

    /// <summary>
    /// 获取部门详情
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>部门详情</returns>
    Task<LeanDeptDto> GetByIdAsync(long id);

    /// <summary>
    /// 创建部门
    /// </summary>
    /// <param name="createDto">创建参数</param>
    /// <returns>创建结果</returns>
    /// <remarks>
    /// 1. 部门名称在同一父级下唯一
    /// 2. 部门编码全局唯一，不区分大小写
    /// </remarks>
    Task<LeanDeptDto> CreateAsync(LeanDeptCreateDto createDto);

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="updateDto">更新参数</param>
    /// <returns>更新结果</returns>
    /// <remarks>
    /// 1. 部门名称在同一父级下唯一
    /// 2. 不允许修改部门编码
    /// 3. 不允许将父级设置为自己或自己的子部门
    /// </remarks>
    Task<LeanDeptDto> UpdateAsync(LeanDeptUpdateDto updateDto);

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 1. 不允许删除有子部门的部门
    /// 2. 不允许删除有用户的部门
    /// </remarks>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 批量删除部门
    /// </summary>
    /// <param name="ids">部门ID集合</param>
    /// <returns>删除结果</returns>
    /// <remarks>
    /// 1. 不允许删除有子部门的部门
    /// 2. 不允许删除有用户的部门
    /// </remarks>
    Task<bool> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 更新部门状态
    /// </summary>
    /// <param name="statusDto">状态参数</param>
    /// <returns>更新结果</returns>
    /// <remarks>
    /// 1. 不允许停用有子部门的部门
    /// 2. 不允许停用有用户的部门
    /// </remarks>
    Task<bool> UpdateStatusAsync(LeanDeptStatusDto statusDto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>导入模板字节数组</returns>
    Task<byte[]> GetImportTemplateAsync();

    /// <summary>
    /// 导入部门数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    /// <remarks>
    /// 1. 上级部门必须存在
    /// 2. 部门名称在同一父级下唯一
    /// 3. 部门编码全局唯一，不区分大小写
    /// </remarks>
    Task<LeanDeptImportResultDto> ImportAsync(IFormFile file);

    /// <summary>
    /// 导出部门数据
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>导出文件字节数组</returns>
    Task<byte[]> ExportAsync(LeanDeptQueryDto queryDto);

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>部门列表</returns>
    Task<List<LeanDeptDto>> GetListAsync(LeanDeptQueryDto queryDto);
} 