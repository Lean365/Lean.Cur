using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 菜单服务实现
/// </summary>
public class LeanMenuService : ILeanMenuService
{
  private readonly ILogger<LeanMenuService> _logger;
  private readonly ILeanBaseRepository<LeanMenu> _menuRepository;
  private readonly ILeanBaseRepository<LeanRoleMenu> _roleMenuRepository;
  private readonly ILeanBaseRepository<LeanUserRole> _userRoleRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMenuService(
      ILogger<LeanMenuService> logger,
      ILeanBaseRepository<LeanMenu> menuRepository,
      ILeanBaseRepository<LeanRoleMenu> roleMenuRepository,
      ILeanBaseRepository<LeanUserRole> userRoleRepository)
  {
    _logger = logger;
    _menuRepository = menuRepository;
    _roleMenuRepository = roleMenuRepository;
    _userRoleRepository = userRoleRepository;
  }

  /// <summary>
  /// 获取菜单列表
  /// </summary>
  public async Task<List<LeanMenuDto>> GetMenuListAsync(LeanMenuQueryDto query)
  {
    var menus = await _menuRepository.GetListAsync(x =>
        (string.IsNullOrEmpty(query.MenuName) || x.MenuName.Contains(query.MenuName)) &&
        (string.IsNullOrEmpty(query.MenuType) || x.MenuType == query.MenuType) &&
        (!query.Visible.HasValue || x.Visible == query.Visible.Value) &&
        (!query.Status.HasValue || x.Status == query.Status.Value));

    return menus.OrderBy(x => x.OrderNum).Adapt<List<LeanMenuDto>>();
  }

  /// <summary>
  /// 获取菜单树
  /// </summary>
  public async Task<List<LeanMenuDto>> GetMenuTreeAsync(LeanMenuQueryDto query)
  {
    var menus = await GetMenuListAsync(query);
    return BuildMenuTree(menus);
  }

  /// <summary>
  /// 获取菜单详情
  /// </summary>
  public async Task<LeanMenuDto> GetMenuAsync(long id)
  {
    var menu = await _menuRepository.GetAsync(id);
    return menu.Adapt<LeanMenuDto>();
  }

  /// <summary>
  /// 创建菜单
  /// </summary>
  public async Task<long> CreateMenuAsync(LeanMenuCreateDto dto)
  {
    // 检查菜单名称是否存在
    if (await CheckMenuNameExistAsync(dto.MenuName))
    {
      throw new Exception($"菜单名称 {dto.MenuName} 已存在");
    }

    // 检查父菜单是否存在
    if (dto.ParentId.HasValue)
    {
      var parentMenu = await _menuRepository.GetAsync(dto.ParentId.Value);
      if (parentMenu == null)
      {
        throw new Exception($"父菜单 {dto.ParentId.Value} 不存在");
      }
    }

    var menu = dto.Adapt<LeanMenu>();
    await _menuRepository.InsertAsync(menu);

    return menu.Id;
  }

  /// <summary>
  /// 更新菜单
  /// </summary>
  public async Task UpdateMenuAsync(LeanMenuUpdateDto dto)
  {
    var menu = await _menuRepository.GetAsync(dto.Id);
    if (menu == null)
    {
      throw new Exception($"菜单 {dto.Id} 不存在");
    }

    // 检查菜单名称是否存在
    if (await CheckMenuNameExistAsync(dto.MenuName, dto.Id))
    {
      throw new Exception($"菜单名称 {dto.MenuName} 已存在");
    }

    // 检查父菜单是否存在且不能是自己或其子菜单
    if (dto.ParentId.HasValue)
    {
      if (dto.ParentId.Value == dto.Id)
      {
        throw new Exception("父菜单不能是自己");
      }

      var childrenIds = await GetMenuAndChildrenIdsAsync(dto.Id);
      if (childrenIds.Contains(dto.ParentId.Value))
      {
        throw new Exception("父菜单不能是其子菜单");
      }

      var parentMenu = await _menuRepository.GetAsync(dto.ParentId.Value);
      if (parentMenu == null)
      {
        throw new Exception($"父菜单 {dto.ParentId.Value} 不存在");
      }
    }

    dto.Adapt(menu);
    await _menuRepository.UpdateAsync(menu);
  }

  /// <summary>
  /// 删除菜单
  /// </summary>
  public async Task DeleteMenuAsync(long id)
  {
    var menu = await _menuRepository.GetAsync(id);
    if (menu == null)
    {
      throw new Exception($"菜单 {id} 不存在");
    }

    // 检查是否有子菜单
    var childrenCount = await _menuRepository.CountAsync(x => x.ParentId == id);
    if (childrenCount > 0)
    {
      throw new Exception("请先删除子菜单");
    }

    // 删除菜单角色关联
    await _roleMenuRepository.DeleteAsync(x => x.MenuId == id);

    // 删除菜单
    await _menuRepository.DeleteAsync(menu);
  }

  /// <summary>
  /// 检查菜单名称是否存在
  /// </summary>
  public async Task<bool> CheckMenuNameExistAsync(string menuName, long? excludeId = null)
  {
    return await _menuRepository.AnyAsync(x =>
        x.MenuName == menuName &&
        (!excludeId.HasValue || x.Id != excludeId.Value));
  }

  /// <summary>
  /// 获取菜单及其子菜单ID列表
  /// </summary>
  public async Task<List<long>> GetMenuAndChildrenIdsAsync(long menuId)
  {
    var menus = await _menuRepository.GetListAsync();
    var result = new List<long> { menuId };
    GetChildrenMenuIds(menus, menuId, result);
    return result;
  }

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  public async Task<List<LeanMenuDto>> GetUserMenuTreeAsync(long userId)
  {
    // 获取用户角色ID列表
    var roleIds = await _userRoleRepository
        .GetListAsync(x => x.UserId == userId)
        .ContinueWith(t => t.Result.Select(x => x.RoleId).ToList());

    // 获取角色菜单ID列表
    var menuIds = await _roleMenuRepository
        .GetListAsync(x => roleIds.Contains(x.RoleId))
        .ContinueWith(t => t.Result.Select(x => x.MenuId).Distinct().ToList());

    // 获取菜单列表
    var menus = await _menuRepository
        .GetListAsync(x => menuIds.Contains(x.Id) && x.Status == 1);

    // 转换为DTO并构建树形结构
    var menuDtos = menus
        .OrderBy(x => x.OrderNum)
        .Adapt<List<LeanMenuDto>>();

    return BuildMenuTree(menuDtos);
  }

  /// <summary>
  /// 构建菜单树
  /// </summary>
  private List<LeanMenuDto> BuildMenuTree(List<LeanMenuDto> menus, long? parentId = null)
  {
    return menus
        .Where(x => x.ParentId == parentId)
        .Select(x =>
        {
          x.Children = BuildMenuTree(menus, x.Id);
          return x;
        })
        .ToList();
  }

  /// <summary>
  /// 获取子菜单ID列表
  /// </summary>
  private void GetChildrenMenuIds(List<LeanMenu> menus, long menuId, List<long> menuIds)
  {
    var children = menus.Where(x => x.ParentId == menuId);
    foreach (var child in children)
    {
      menuIds.Add(child.Id);
      GetChildrenMenuIds(menus, child.Id, menuIds);
    }
  }
}