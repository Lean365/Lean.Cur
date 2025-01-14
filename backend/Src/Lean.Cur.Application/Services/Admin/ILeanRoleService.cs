// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

/// <summary>
/// 角色服务接口定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了角色管理相关的所有服务接口，包括：
/// - 角色的增删改查
/// - 角色状态管理
/// - 角色权限管理
/// - 角色菜单管理
/// - 角色用户管理
/// - 角色数据导入导出
/// </remarks>

using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 角色服务接口
/// </summary>
public interface ILeanRoleService
{
    #region 基础操作

    /// <summary>
    /// 获取角色列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>角色分页列表</returns>
    Task<PagedResult<LeanRoleDto>> GetPagedListAsync(LeanRoleQueryDto queryDto);

    /// <summary>
    /// 获取角色详情
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色详情</returns>
    Task<LeanRoleDto> GetByIdAsync(long id);

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="createDto">创建参数</param>
    /// <returns>角色ID</returns>
    Task<long> CreateAsync(LeanRoleCreateDto createDto);

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="updateDto">更新参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateAsync(LeanRoleUpdateDto updateDto);

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>是否成功</returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 批量删除角色
    /// </summary>
    /// <param name="ids">角色ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 更新角色状态
    /// </summary>
    /// <param name="statusDto">状态参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateStatusAsync(LeanRoleStatusDto statusDto);

    #endregion

    #region 权限管理

    /// <summary>
    /// 获取角色权限列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>权限列表</returns>
    Task<List<string>> GetRolePermissionsAsync(long roleId);

    /// <summary>
    /// 更新角色权限
    /// </summary>
    /// <param name="permissionDto">权限参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateRolePermissionsAsync(LeanRoleMenuPermissionDto permissionDto);

    #endregion

    #region 菜单管理

    /// <summary>
    /// 获取角色菜单树
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>菜单树</returns>
    Task<List<LeanRoleMenuTreeDto>> GetRoleMenuTreeAsync(long roleId);

    /// <summary>
    /// 更新角色菜单
    /// </summary>
    /// <param name="menuDto">菜单参数</param>
    /// <returns>是否成功</returns>
    Task<bool> UpdateRoleMenusAsync(LeanRoleMenuDto menuDto);

    /// <summary>
    /// 获取角色菜单权限
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>菜单权限信息</returns>
    Task<LeanRoleMenuPermissionDto> GetRoleMenuPermissionsAsync(long roleId);

    #endregion

    #region 用户管理

    /// <summary>
    /// 获取角色用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>用户分页列表</returns>
    Task<PagedResult<LeanRoleUserListDto>> GetRoleUsersAsync(LeanRoleUserQueryDto queryDto);

    /// <summary>
    /// 分配角色用户
    /// </summary>
    /// <param name="assignDto">分配参数</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignRoleUsersAsync(LeanRoleUserAssignDto assignDto);

    /// <summary>
    /// 分配用户到角色
    /// </summary>
    /// <param name="assignDto">分配参数</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignUsersAsync(LeanRoleUserAssignDto assignDto);

    #endregion

    #region 导入导出

    /// <summary>
    /// 导入角色数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    Task<LeanRoleImportResultDto> ImportAsync(IFormFile file);

    /// <summary>
    /// 导出角色数据
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>Excel文件字节数组</returns>
    Task<byte[]> ExportAsync(LeanRoleQueryDto queryDto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>模板文件字节数组</returns>
    Task<byte[]> GetImportTemplateAsync();

    #endregion
} 