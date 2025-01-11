using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 角色服务接口
/// </summary>
public interface ILeanRoleService
{
  /// <summary>
  /// 获取角色列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>角色列表</returns>
  Task<List<LeanRoleDto>> GetRoleListAsync(LeanRoleQueryDto query);

  /// <summary>
  /// 获取角色详情
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色详情</returns>
  Task<LeanRoleDto?> GetRoleAsync(long id);

  /// <summary>
  /// 创建角色
  /// </summary>
  /// <param name="dto">角色创建信息</param>
  /// <returns>角色ID</returns>
  Task<long> CreateRoleAsync(LeanRoleCreateDto dto);

  /// <summary>
  /// 更新角色
  /// </summary>
  /// <param name="dto">角色更新信息</param>
  Task UpdateRoleAsync(LeanRoleUpdateDto dto);

  /// <summary>
  /// 删除角色
  /// </summary>
  /// <param name="id">角色ID</param>
  Task DeleteRoleAsync(long id);

  /// <summary>
  /// 更新角色状态
  /// </summary>
  /// <param name="dto">状态更新信息</param>
  Task UpdateRoleStatusAsync(LeanRoleStatusUpdateDto dto);

  /// <summary>
  /// 更新角色权限
  /// </summary>
  /// <param name="dto">权限更新信息</param>
  Task UpdateRolePermissionsAsync(LeanRolePermissionUpdateDto dto);

  /// <summary>
  /// 更新角色菜单
  /// </summary>
  /// <param name="dto">菜单更新信息</param>
  Task UpdateRoleMenusAsync(LeanRoleMenuUpdateDto dto);

  /// <summary>
  /// 检查角色名称是否存在
  /// </summary>
  /// <param name="roleName">角色名称</param>
  /// <param name="excludeId">排除的角色ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckRoleNameExistAsync(string roleName, long? excludeId = null);

  /// <summary>
  /// 检查角色编码是否存在
  /// </summary>
  /// <param name="roleCode">角色编码</param>
  /// <param name="excludeId">排除的角色ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckRoleCodeExistAsync(string roleCode, long? excludeId = null);

  /// <summary>
  /// 获取角色权限ID列表
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>权限ID列表</returns>
  Task<List<long>> GetRolePermissionIdsAsync(long roleId);

  /// <summary>
  /// 获取角色菜单ID列表
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>菜单ID列表</returns>
  Task<List<long>> GetRoleMenuIdsAsync(long roleId);
}