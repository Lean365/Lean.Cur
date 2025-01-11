using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 角色服务实现
/// </summary>
public class LeanRoleService : ILeanRoleService
{
  private readonly ILeanBaseRepository<LeanRole> _roleRepository;
  private readonly ILeanBaseRepository<LeanRolePermission> _rolePermissionRepository;
  private readonly ILeanBaseRepository<LeanRoleMenu> _roleMenuRepository;
  private readonly ILeanBaseRepository<LeanPermission> _permissionRepository;
  private readonly ILeanBaseRepository<LeanMenu> _menuRepository;
  private readonly ILogger<LeanRoleService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRoleService(
      ILeanBaseRepository<LeanRole> roleRepository,
      ILeanBaseRepository<LeanRolePermission> rolePermissionRepository,
      ILeanBaseRepository<LeanRoleMenu> roleMenuRepository,
      ILeanBaseRepository<LeanPermission> permissionRepository,
      ILeanBaseRepository<LeanMenu> menuRepository,
      ILogger<LeanRoleService> logger)
  {
    _roleRepository = roleRepository;
    _rolePermissionRepository = rolePermissionRepository;
    _roleMenuRepository = roleMenuRepository;
    _permissionRepository = permissionRepository;
    _menuRepository = menuRepository;
    _logger = logger;
  }

  /// <inheritdoc/>
  public async Task<List<LeanRoleDto>> GetRoleListAsync(LeanRoleQueryDto query)
  {
    try
    {
      var queryable = _roleRepository.AsQueryable();

      // 构建查询条件
      if (!string.IsNullOrEmpty(query.RoleName))
      {
        queryable = queryable.Where(x => x.RoleName.Contains(query.RoleName));
      }
      if (!string.IsNullOrEmpty(query.RoleCode))
      {
        queryable = queryable.Where(x => x.RoleCode.Contains(query.RoleCode));
      }
      if (query.Status.HasValue)
      {
        queryable = queryable.Where(x => x.Status == query.Status.Value);
      }
      if (query.StartTime.HasValue)
      {
        queryable = queryable.Where(x => x.CreateTime >= query.StartTime.Value);
      }
      if (query.EndTime.HasValue)
      {
        queryable = queryable.Where(x => x.CreateTime <= query.EndTime.Value);
      }

      // 按显示顺序和创建时间排序
      queryable = queryable.OrderBy(x => x.OrderNum).OrderByDescending(x => x.CreateTime);

      var roles = await queryable.ToListAsync();
      var roleDtos = roles.Adapt<List<LeanRoleDto>>();

      // 填充权限和菜单信息
      foreach (var roleDto in roleDtos)
      {
        roleDto.PermissionIds = await GetRolePermissionIdsAsync(roleDto.Id);
        roleDto.MenuIds = await GetRoleMenuIdsAsync(roleDto.Id);
      }

      return roleDtos;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取角色列表失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<LeanRoleDto?> GetRoleAsync(long id)
  {
    try
    {
      var role = await _roleRepository.GetByIdAsync(id);
      if (role == null)
      {
        return null;
      }

      var roleDto = role.Adapt<LeanRoleDto>();
      roleDto.PermissionIds = await GetRolePermissionIdsAsync(id);
      roleDto.MenuIds = await GetRoleMenuIdsAsync(id);

      return roleDto;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取角色详情失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<long> CreateRoleAsync(LeanRoleCreateDto dto)
  {
    try
    {
      // 检查角色名称是否已存在
      if (await CheckRoleNameExistAsync(dto.RoleName))
      {
        throw new Exception($"角色名称 {dto.RoleName} 已存在");
      }

      // 检查角色编码是否已存在
      if (await CheckRoleCodeExistAsync(dto.RoleCode))
      {
        throw new Exception($"角色编码 {dto.RoleCode} 已存在");
      }

      // 检查权限是否存在
      if (dto.PermissionIds.Any())
      {
        var permissions = await _permissionRepository.GetListAsync(x => dto.PermissionIds.Contains(x.Id));
        if (permissions.Count != dto.PermissionIds.Count)
        {
          throw new Exception("部分权限ID不存在");
        }
      }

      // 检查菜单是否存在
      if (dto.MenuIds.Any())
      {
        var menus = await _menuRepository.GetListAsync(x => dto.MenuIds.Contains(x.Id));
        if (menus.Count != dto.MenuIds.Count)
        {
          throw new Exception("部分菜单ID不存在");
        }
      }

      var role = dto.Adapt<LeanRole>();
      await _roleRepository.AddAsync(role);

      // 创建角色权限关联
      if (dto.PermissionIds.Any())
      {
        var rolePermissions = dto.PermissionIds.Select(permissionId => new LeanRolePermission
        {
          RoleId = role.Id,
          PermissionId = permissionId
        }).ToList();
        await _rolePermissionRepository.AddRangeAsync(rolePermissions);
      }

      // 创建角色菜单关联
      if (dto.MenuIds.Any())
      {
        var roleMenus = dto.MenuIds.Select(menuId => new LeanRoleMenu
        {
          RoleId = role.Id,
          MenuId = menuId
        }).ToList();
        await _roleMenuRepository.AddRangeAsync(roleMenus);
      }

      return role.Id;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "创建角色失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateRoleAsync(LeanRoleUpdateDto dto)
  {
    try
    {
      var role = await _roleRepository.GetByIdAsync(dto.Id);
      if (role == null)
      {
        throw new Exception($"角色ID {dto.Id} 不存在");
      }

      // 检查角色名称是否已存在
      if (await CheckRoleNameExistAsync(dto.RoleName, dto.Id))
      {
        throw new Exception($"角色名称 {dto.RoleName} 已存在");
      }

      // 检查角色编码是否已存在
      if (await CheckRoleCodeExistAsync(dto.RoleCode, dto.Id))
      {
        throw new Exception($"角色编码 {dto.RoleCode} 已存在");
      }

      // 检查权限是否存在
      if (dto.PermissionIds.Any())
      {
        var permissions = await _permissionRepository.GetListAsync(x => dto.PermissionIds.Contains(x.Id));
        if (permissions.Count != dto.PermissionIds.Count)
        {
          throw new Exception("部分权限ID不存在");
        }
      }

      // 检查菜单是否存在
      if (dto.MenuIds.Any())
      {
        var menus = await _menuRepository.GetListAsync(x => dto.MenuIds.Contains(x.Id));
        if (menus.Count != dto.MenuIds.Count)
        {
          throw new Exception("部分菜单ID不存在");
        }
      }

      // 更新基本信息
      role.RoleName = dto.RoleName;
      role.RoleCode = dto.RoleCode;
      role.OrderNum = dto.OrderNum;
      role.Status = dto.Status;
      role.Remark = dto.Remark;

      await _roleRepository.UpdateAsync(role);

      // 更新角色权限关联
      await UpdateRolePermissionsAsync(new LeanRolePermissionUpdateDto
      {
        Id = role.Id,
        PermissionIds = dto.PermissionIds
      });

      // 更新角色菜单关联
      await UpdateRoleMenusAsync(new LeanRoleMenuUpdateDto
      {
        Id = role.Id,
        MenuIds = dto.MenuIds
      });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新角色失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task DeleteRoleAsync(long id)
  {
    try
    {
      var role = await _roleRepository.GetByIdAsync(id);
      if (role == null)
      {
        throw new Exception($"角色ID {id} 不存在");
      }

      // 检查是否存在用户
      if (role.Users.Any())
      {
        throw new Exception("角色下存在用户,不能删除");
      }

      // 删除角色权限关联
      await _rolePermissionRepository.DeleteAsync(x => x.RoleId == id);

      // 删除角色菜单关联
      await _roleMenuRepository.DeleteAsync(x => x.RoleId == id);

      // 删除角色
      await _roleRepository.DeleteAsync(id);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "删除角色失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateRoleStatusAsync(LeanRoleStatusUpdateDto dto)
  {
    try
    {
      var role = await _roleRepository.GetByIdAsync(dto.Id);
      if (role == null)
      {
        throw new Exception($"角色ID {dto.Id} 不存在");
      }

      role.Status = dto.Status;
      await _roleRepository.UpdateAsync(role);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新角色状态失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateRolePermissionsAsync(LeanRolePermissionUpdateDto dto)
  {
    try
    {
      var role = await _roleRepository.GetByIdAsync(dto.Id);
      if (role == null)
      {
        throw new Exception($"角色ID {dto.Id} 不存在");
      }

      // 检查权限是否存在
      if (dto.PermissionIds.Any())
      {
        var permissions = await _permissionRepository.GetListAsync(x => dto.PermissionIds.Contains(x.Id));
        if (permissions.Count != dto.PermissionIds.Count)
        {
          throw new Exception("部分权限ID不存在");
        }
      }

      // 删除原有的角色权限关联
      await _rolePermissionRepository.DeleteAsync(x => x.RoleId == dto.Id);

      // 创建新的角色权限关联
      if (dto.PermissionIds.Any())
      {
        var rolePermissions = dto.PermissionIds.Select(permissionId => new LeanRolePermission
        {
          RoleId = dto.Id,
          PermissionId = permissionId
        }).ToList();
        await _rolePermissionRepository.AddRangeAsync(rolePermissions);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新角色权限失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateRoleMenusAsync(LeanRoleMenuUpdateDto dto)
  {
    try
    {
      var role = await _roleRepository.GetByIdAsync(dto.Id);
      if (role == null)
      {
        throw new Exception($"角色ID {dto.Id} 不存在");
      }

      // 检查菜单是否存在
      if (dto.MenuIds.Any())
      {
        var menus = await _menuRepository.GetListAsync(x => dto.MenuIds.Contains(x.Id));
        if (menus.Count != dto.MenuIds.Count)
        {
          throw new Exception("部分菜单ID不存在");
        }
      }

      // 删除原有的角色菜单关联
      await _roleMenuRepository.DeleteAsync(x => x.RoleId == dto.Id);

      // 创建新的角色菜单关联
      if (dto.MenuIds.Any())
      {
        var roleMenus = dto.MenuIds.Select(menuId => new LeanRoleMenu
        {
          RoleId = dto.Id,
          MenuId = menuId
        }).ToList();
        await _roleMenuRepository.AddRangeAsync(roleMenus);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新角色菜单失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> CheckRoleNameExistAsync(string roleName, long? excludeId = null)
  {
    try
    {
      var queryable = _roleRepository.AsQueryable()
          .Where(x => x.RoleName == roleName);

      if (excludeId.HasValue)
      {
        queryable = queryable.Where(x => x.Id != excludeId.Value);
      }

      return await queryable.AnyAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查角色名称是否存在失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> CheckRoleCodeExistAsync(string roleCode, long? excludeId = null)
  {
    try
    {
      var queryable = _roleRepository.AsQueryable()
          .Where(x => x.RoleCode == roleCode);

      if (excludeId.HasValue)
      {
        queryable = queryable.Where(x => x.Id != excludeId.Value);
      }

      return await queryable.AnyAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查角色编码是否存在失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<List<long>> GetRolePermissionIdsAsync(long roleId)
  {
    try
    {
      var rolePermissions = await _rolePermissionRepository.GetListAsync(x => x.RoleId == roleId);
      return rolePermissions.Select(x => x.PermissionId).ToList();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取角色权限ID列表失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<List<long>> GetRoleMenuIdsAsync(long roleId)
  {
    try
    {
      var roleMenus = await _roleMenuRepository.GetListAsync(x => x.RoleId == roleId);
      return roleMenus.Select(x => x.MenuId).ToList();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取角色菜单ID列表失败");
      throw;
    }
  }
}