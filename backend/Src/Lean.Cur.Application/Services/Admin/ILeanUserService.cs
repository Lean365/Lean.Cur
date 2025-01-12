// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

/// <summary>
/// 用户服务接口定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-01
/// 修改时间：2024-01-01
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了用户管理相关的所有服务接口，包括：
/// - 用户的增删改查
/// - 用户状态管理
/// - 用户密码管理
/// - 用户数据导入导出
/// </remarks>

using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 用户服务接口
/// </summary>
public interface ILeanUserService
{
  /// <summary>
  /// 获取用户列表（分页）
  /// </summary>
  Task<PagedResult<LeanUserDto>> GetPagedListAsync(LeanUserQueryDto queryDto);

  /// <summary>
  /// 获取用户详情
  /// </summary>
  Task<LeanUserDto> GetByIdAsync(long id);

  /// <summary>
  /// 创建用户
  /// </summary>
  Task<long> CreateAsync(LeanUserCreateDto createDto);

  /// <summary>
  /// 更新用户
  /// </summary>
  Task<bool> UpdateAsync(LeanUserUpdateDto updateDto);

  /// <summary>
  /// 删除用户
  /// </summary>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 批量删除用户
  /// </summary>
  Task<bool> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 更新用户状态
  /// </summary>
  Task<bool> UpdateStatusAsync(LeanUserStatusDto statusDto);

  /// <summary>
  /// 重置用户密码
  /// </summary>
  Task<bool> ResetPasswordAsync(LeanUserResetPasswordDto resetDto);

  /// <summary>
  /// 修改用户密码
  /// </summary>
  Task<bool> UpdatePasswordAsync(long userId, LeanUserUpdatePasswordDto updateDto);

  /// <summary>
  /// 导入用户数据
  /// </summary>
  Task<(int total, int success, List<string> errors)> ImportAsync(IFormFile file);

  /// <summary>
  /// 导出用户数据
  /// </summary>
  Task<byte[]> ExportAsync(LeanUserQueryDto queryDto);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  Task<byte[]> GetImportTemplateAsync();

  /// <summary>
  /// 获取用户角色代码
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>角色代码</returns>
  Task<string> GetUserRoleCodeAsync(long userId);

  /// <summary>
  /// 获取用户权限列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>权限列表</returns>
  Task<List<string>> GetUserPermissionsAsync(long userId);
}