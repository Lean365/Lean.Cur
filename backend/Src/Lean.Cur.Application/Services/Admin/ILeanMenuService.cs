using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 菜单服务接口
/// </summary>
public interface ILeanMenuService
{
  /// <summary>
  /// 获取菜单列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>菜单列表</returns>
  Task<List<LeanMenuDto>> GetMenuListAsync(LeanMenuQueryDto query);

  /// <summary>
  /// 获取菜单树
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>菜单树</returns>
  Task<List<LeanMenuDto>> GetMenuTreeAsync(LeanMenuQueryDto query);

  /// <summary>
  /// 获取菜单详情
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns>菜单详情</returns>
  Task<LeanMenuDto> GetMenuAsync(long id);

  /// <summary>
  /// 创建菜单
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>菜单ID</returns>
  Task<long> CreateMenuAsync(LeanMenuCreateDto dto);

  /// <summary>
  /// 更新菜单
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns></returns>
  Task UpdateMenuAsync(LeanMenuUpdateDto dto);

  /// <summary>
  /// 删除菜单
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns></returns>
  Task DeleteMenuAsync(long id);

  /// <summary>
  /// 检查菜单名称是否存在
  /// </summary>
  /// <param name="menuName">菜单名称</param>
  /// <param name="excludeId">排除的菜单ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckMenuNameExistAsync(string menuName, long? excludeId = null);

  /// <summary>
  /// 获取菜单及其子菜单ID列表
  /// </summary>
  /// <param name="menuId">菜单ID</param>
  /// <returns>菜单ID列表</returns>
  Task<List<long>> GetMenuAndChildrenIdsAsync(long menuId);

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>菜单树</returns>
  Task<List<LeanMenuDto>> GetUserMenuTreeAsync(long userId);
}