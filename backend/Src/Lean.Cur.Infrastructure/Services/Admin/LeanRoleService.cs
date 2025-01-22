using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Entities.Admin;
using Mapster;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System.Linq;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 角色管理服务实现
/// </summary>
public class LeanRoleService : ILeanRoleService
{
  private readonly SqlSugarClient _db;
  private readonly LeanExcelHelper _excel;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">数据库访问对象</param>
  /// <param name="excel">Excel 操作对象</param>
  public LeanRoleService(SqlSugarClient db, LeanExcelHelper excel)
  {
    _db = db;
    _excel = excel;
  }

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanRoleDto>> GetPagedListAsync(LeanRoleQueryDto queryDto)
  {
    var query = _db.Queryable<LeanRole>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.RoleName), r => r.RoleName.Contains(queryDto.RoleName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.RoleCode), r => r.RoleCode.Contains(queryDto.RoleCode!))
        .WhereIF(queryDto.Status.HasValue, r => r.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, r => r.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, r => r.CreateTime <= queryDto.EndTime)
        .OrderBy(r => r.OrderNum);

    var result = await query.ToPagedListAsync(queryDto);
    var dtos = result.Items.Adapt<List<LeanRoleDto>>();
    return new LeanPagedResult<LeanRoleDto>(dtos, result.Total, result.PageIndex, result.PageSize);
  }

  /// <inheritdoc/>
  public async Task<LeanRoleDto> GetByIdAsync(long id)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == id) ?? throw new BusinessException("角色不存在");

    return role.Adapt<LeanRoleDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanRoleCreateDto createDto)
  {
    // 检查角色编码是否已存在
    if (await _db.Queryable<LeanRole>().AnyAsync(r => r.RoleCode == createDto.RoleCode))
    {
      throw new BusinessException("角色编码已存在");
    }

    var role = createDto.Adapt<LeanRole>();
    return await _db.Insertable(role).ExecuteReturnSnowflakeIdAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanRoleUpdateDto updateDto)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == updateDto.Id) ?? throw new BusinessException("角色不存在");

    // 检查角色编码是否已存在
    if (await _db.Queryable<LeanRole>().AnyAsync(r => r.RoleCode == updateDto.RoleCode && r.Id != updateDto.Id))
    {
      throw new BusinessException("角色编码已存在");
    }

    updateDto.Adapt(role);
    return await _db.Updateable(role).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == id) ?? throw new BusinessException("角色不存在");

    // 检查是否有用户关联
    if (await _db.Queryable<LeanUserRole>().AnyAsync(ur => ur.RoleId == id))
    {
      throw new BusinessException("角色已被用户使用，无法删除");
    }

    return await _db.Deleteable<LeanRole>().Where(r => r.Id == id).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchDeleteAsync(List<long> ids)
  {
    foreach (var id in ids)
    {
      await DeleteAsync(id);
    }
    return true;
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateStatusAsync(LeanRoleStatusDto statusDto)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == statusDto.Id) ?? throw new BusinessException("角色不存在");

    role.Status = statusDto.Status;
    return await _db.Updateable(role).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<byte[]> GetImportTemplateAsync()
  {
    var headers = new Dictionary<string, string>
        {
            { "RoleName", "角色名称" },
            { "RoleCode", "角色编码" },
            { "OrderNum", "显示顺序" },
            { "Status", "状态" },
            { "Remark", "备注" }
        };

    return await _excel.GenerateTemplateAsync(headers);
  }

  /// <inheritdoc/>
  public async Task<LeanRoleImportResultDto> ImportAsync(IFormFile file)
  {
    var importResult = _excel.Import<LeanRoleImportDto>(file.OpenReadStream(), file.FileName);
    if (importResult == null || !importResult.Any())
    {
      throw new BusinessException("导入数据为空");
    }

    var result = new LeanRoleImportResultDto
    {
      TotalCount = importResult.Count(),
      SuccessCount = 0,
      FailureCount = 0,
      FailureItems = new List<LeanRoleImportDto>()
    };

    foreach (var item in importResult)
    {
      try
      {
        // 检查角色编码是否已存在
        if (await _db.Queryable<LeanRole>().AnyAsync(r => r.RoleCode == item.RoleCode))
        {
          throw new BusinessException($"角色编码已存在：{item.RoleCode}");
        }

        var role = new LeanRole
        {
          RoleName = item.RoleName,
          RoleCode = item.RoleCode,
          OrderNum = Convert.ToInt32(item.OrderNum),
          Status = item.Status,
          Remark = item.Remark
        };

        await _db.Insertable(role).ExecuteCommandAsync();
        result.SuccessCount++;
      }
      catch (Exception ex)
      {
        item.ErrorMessage = ex.Message;
        result.FailureItems.Add(item);
        result.FailureCount++;
      }
    }

    return result;
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(LeanRoleQueryDto queryDto)
  {
    var query = _db.Queryable<LeanRole>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.RoleName), r => r.RoleName!.Contains(queryDto.RoleName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.RoleCode), r => r.RoleCode!.Contains(queryDto.RoleCode!))
        .WhereIF(queryDto.Status.HasValue, r => r.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, r => r.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, r => r.CreateTime <= queryDto.EndTime)
        .Where(r => r.IsDeleted == 0)
        .OrderBy(r => r.OrderNum);

    var list = await query.Select(r => new LeanRoleExportDto
    {
      RoleName = r.RoleName,
      RoleCode = r.RoleCode,
      OrderNum = r.OrderNum,
      Status = r.Status.ToString(),
      Remark = r.Remark,
      CreateTime = r.CreateTime,
      UpdateTime = r.UpdateTime
    }).ToListAsync();

    var headers = new Dictionary<string, string>
        {
            { "RoleName", "角色名称" },
            { "RoleCode", "角色编码" },
            { "OrderNum", "显示顺序" },
            { "Status", "状态" },
            { "Remark", "备注" },
            { "CreateTime", "创建时间" },
            { "UpdateTime", "更新时间" }
        };

    return await _excel.ExportAsync(headers, list);
  }

  /// <inheritdoc/>
  public async Task<List<string>> GetRolePermissionsAsync(long roleId)
  {
    // 获取角色直接拥有的权限
    var directPermissions = await _db.Queryable<LeanRoleMenu>()
        .Where(rm => rm.RoleId == roleId)
        .Select(rm => rm.Permission)
        .Where(p => !string.IsNullOrEmpty(p))
        .ToListAsync();

    // 获取继承的权限
    var inheritedPermissions = await GetInheritedPermissionsAsync(roleId);

    // 合并权限并去重
    var allPermissions = directPermissions.Concat(inheritedPermissions)
        .Where(p => p != null)
        .Select(p => p!)
        .Distinct()
        .ToList();

    return allPermissions;
  }

  /// <summary>
  /// 获取继承的权限
  /// </summary>
  private async Task<List<string>> GetInheritedPermissionsAsync(long roleId)
  {
    var inheritedPermissions = new List<string>();

    // 获取角色信息
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == roleId) ?? throw new BusinessException("角色不存在");

    // 如果没有继承设置，直接返回空列表
    if (string.IsNullOrEmpty(role.InheritedRoleIds) || role.InheritanceType == null)
    {
      return inheritedPermissions;
    }

    // 获取继承的角色ID列表
    var inheritedRoleIds = role.InheritedRoleIds.Split(',').Select(long.Parse).ToList();

    foreach (var inheritedRoleId in inheritedRoleIds)
    {
      if (role.InheritanceType == 1) // 完全继承
      {
        var parentPermissions = await _db.Queryable<LeanRoleMenu>()
            .Where(rm => rm.RoleId == inheritedRoleId)
            .Select(rm => rm.Permission)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToListAsync();

        inheritedPermissions.AddRange(parentPermissions);
      }
      else if (role.InheritanceType == 2 && !string.IsNullOrEmpty(role.InheritedPermissions)) // 部分继承
      {
        var permissionList = role.InheritedPermissions.Split(',').ToList();
        var parentPermissions = await _db.Queryable<LeanRoleMenu>()
            .Where(rm => rm.RoleId == inheritedRoleId)
            .Where(rm => permissionList.Contains(rm.Permission!))
            .Select(rm => rm.Permission)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToListAsync();

        inheritedPermissions.AddRange(parentPermissions);
      }
    }

    return inheritedPermissions.Where(p => p != null).Select(p => p!).ToList();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateRolePermissionsAsync(LeanRoleMenuPermissionDto permissionDto)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == permissionDto.RoleId) ?? throw new BusinessException("角色不存在");

    // 检查菜单是否存在
    var menus = await _db.Queryable<LeanMenu>()
        .Where(m => permissionDto.MenuIds.Contains(m.Id))
        .ToListAsync();

    if (menus.Count != permissionDto.MenuIds.Count)
    {
      throw new BusinessException("部分菜单不存在");
    }

    // 检查菜单状态
    if (menus.Any(m => m.Status != LeanStatus.Normal))
    {
      throw new BusinessException("部分菜单已被禁用");
    }

    // 更新角色菜单权限
    await _db.Ado.BeginTranAsync();
    try
    {
      // 删除原有角色菜单
      await _db.Deleteable<LeanRoleMenu>().Where(rm => rm.RoleId == permissionDto.RoleId).ExecuteCommandAsync();

      // 添加新的角色菜单
      var roleMenus = permissionDto.MenuIds.Select(menuId =>
      {
        var permission = permissionDto.Permissions.FirstOrDefault(p => p.MenuId == menuId);
        return new LeanRoleMenu
        {
          RoleId = permissionDto.RoleId,
          MenuId = menuId,
          Permission = permission?.Permission
        };
      }).ToList();

      await _db.Insertable(roleMenus).ExecuteCommandAsync();
      await _db.Ado.CommitTranAsync();
      return true;
    }
    catch (Exception)
    {
      await _db.Ado.RollbackTranAsync();
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<List<LeanRoleMenuTreeDto>> GetRoleMenuTreeAsync(long roleId)
  {
    // 获取所有菜单
    var menus = await _db.Queryable<LeanMenu>()
        .Where(m => m.Status == LeanStatus.Normal)
        .OrderBy(m => m.OrderNum)
        .ToListAsync();

    // 获取角色菜单
    var roleMenus = await _db.Queryable<LeanRoleMenu>()
        .Where(rm => rm.RoleId == roleId)
        .Select(rm => rm.MenuId)
        .ToListAsync();

    // 构建菜单树
    return BuildMenuTree(menus, roleMenus, 0);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateRoleMenusAsync(LeanRoleMenuDto menuDto)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == menuDto.RoleId) ?? throw new BusinessException("角色不存在");

    // 检查菜单是否存在
    var menus = await _db.Queryable<LeanMenu>()
        .Where(m => menuDto.MenuIds.Contains(m.Id))
        .ToListAsync();

    if (menus.Count != menuDto.MenuIds.Count)
    {
      throw new BusinessException("部分菜单不存在");
    }

    // 检查菜单状态
    if (menus.Any(m => m.Status != LeanStatus.Normal))
    {
      throw new BusinessException("部分菜单已被禁用");
    }

    // 更新角色菜单
    await _db.Ado.BeginTranAsync();
    try
    {
      // 删除原有角色菜单
      await _db.Deleteable<LeanRoleMenu>().Where(rm => rm.RoleId == menuDto.RoleId).ExecuteCommandAsync();

      // 添加新的角色菜单
      var roleMenus = menuDto.MenuIds.Select(menuId => new LeanRoleMenu
      {
        RoleId = menuDto.RoleId,
        MenuId = menuId
      }).ToList();

      await _db.Insertable(roleMenus).ExecuteCommandAsync();
      await _db.Ado.CommitTranAsync();
      return true;
    }
    catch (Exception)
    {
      await _db.Ado.RollbackTranAsync();
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<LeanRoleMenuPermissionDto> GetRoleMenuPermissionsAsync(long roleId)
  {
    // 获取角色信息
    var role = await _db.Queryable<LeanRole>()
        .Where(r => r.Id == roleId)
        .FirstAsync() ?? throw new BusinessException("角色不存在");

    // 获取角色菜单关系
    var roleMenus = await _db.Queryable<LeanRoleMenu>()
        .Where(rm => rm.RoleId == roleId)
        .ToListAsync();

    // 获取所有正常状态的菜单,按排序号排序
    var menus = await _db.Queryable<LeanMenu>()
        .Where(m => m.Status == LeanStatus.Normal)
        .OrderBy(m => m.OrderNum)
        .ToListAsync();

    // 构建菜单权限列表
    var permissions = new List<MenuPermission>();
    foreach (var menu in menus)
    {
      if (!string.IsNullOrEmpty(menu.Permission))
      {
        permissions.Add(new MenuPermission
        {
          MenuId = menu.Id,
          Permission = menu.Permission
        });
      }
    }

    // 返回结果
    return new LeanRoleMenuPermissionDto
    {
      RoleId = roleId,
      MenuIds = roleMenus.Select(rm => rm.MenuId).ToList(),
      Permissions = permissions
    };
  }

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanRoleUserListDto>> GetRoleUsersAsync(LeanRoleUserQueryDto queryDto)
  {
    var query = _db.Queryable<LeanUser, LeanUserRole>((u, ur) => new JoinQueryInfos(
            JoinType.Inner, u.Id == ur.UserId
        ))
        .Where((u, ur) => ur.RoleId == queryDto.RoleId)
        .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), (u, ur) => u.UserName.Contains(queryDto.UserName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.NickName), (u, ur) => u.NickName.Contains(queryDto.NickName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.Email), (u, ur) => u.Email.Contains(queryDto.Email!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.Phone), (u, ur) => u.Phone.Contains(queryDto.Phone!))
        .WhereIF(queryDto.Status.HasValue, (u, ur) => u.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, (u, ur) => u.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, (u, ur) => u.CreateTime <= queryDto.EndTime);

    var result = await query.ToPagedListAsync(queryDto);
    var dtos = result.Items.Adapt<List<LeanRoleUserListDto>>();
    return new LeanPagedResult<LeanRoleUserListDto>(dtos, result.Total, result.PageIndex, result.PageSize);
  }

  /// <inheritdoc/>
  public async Task<bool> AssignRoleUsersAsync(LeanRoleUserAssignDto assignDto)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == assignDto.RoleId) ?? throw new BusinessException("角色不存在");

    if (role.Status != LeanStatus.Normal)
    {
      throw new BusinessException($"角色[{role.RoleName}]已被禁用，不能分配用户");
    }

    var users = await _db.Queryable<LeanUser>()
        .Where(u => assignDto.UserIds.Contains(u.Id))
        .ToListAsync();

    if (users.Count != assignDto.UserIds.Count)
    {
      throw new BusinessException("部分用户不存在");
    }

    // 检查用户状态
    var disabledUsers = users.Where(u => u.Status != LeanStatus.Normal).ToList();
    if (disabledUsers.Any())
    {
      throw new BusinessException($"用户[{string.Join(",", disabledUsers.Select(u => u.UserName))}]已被禁用，不能分配角色");
    }

    // 更新用户角色
    await _db.Ado.BeginTranAsync();
    try
    {
      // 删除原有用户角色
      await _db.Deleteable<LeanUserRole>().Where(ur => ur.RoleId == assignDto.RoleId).ExecuteCommandAsync();

      // 添加新的用户角色
      var userRoles = assignDto.UserIds.Select(userId => new LeanUserRole
      {
        UserId = userId,
        RoleId = assignDto.RoleId
      }).ToList();

      await _db.Insertable(userRoles).ExecuteCommandAsync();
      await _db.Ado.CommitTranAsync();
      return true;
    }
    catch (Exception)
    {
      await _db.Ado.RollbackTranAsync();
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> AssignUsersAsync(LeanRoleUserAssignDto assignDto)
  {
    if (assignDto.RoleId <= 0)
    {
      throw new LeanUserFriendlyException("角色ID必须大于0");
    }
    if (assignDto.UserIds == null || !assignDto.UserIds.Any())
    {
      throw new LeanUserFriendlyException("请选择要分配的用户");
    }

    // 检查角色是否存在
    var role = await _db.Queryable<LeanRole>()
        .Where(r => r.Id == assignDto.RoleId && r.IsDeleted == 0)
        .FirstAsync();
    if (role == null)
    {
      throw new LeanUserFriendlyException("角色不存在");
    }

    // 检查用户是否存在
    var users = await _db.Queryable<LeanUser>()
        .Where(u => assignDto.UserIds.Contains(u.Id) && u.IsDeleted == 0)
        .ToListAsync();
    if (users.Count != assignDto.UserIds.Count)
    {
      throw new LeanUserFriendlyException("部分用户不存在");
    }

    // 分配或取消分配用户
    if (assignDto.IsAssign == 1)
    {
      // 分配用户
      var existUserIds = await _db.Queryable<LeanUserRole>()
          .Where(ur => ur.RoleId == assignDto.RoleId && assignDto.UserIds.Contains(ur.UserId) && ur.IsDeleted == 0)
          .Select(ur => ur.UserId)
          .ToListAsync();

      var newUserIds = assignDto.UserIds.Except(existUserIds).ToList();
      if (!newUserIds.Any())
      {
        return true;
      }

      var userRoles = newUserIds.Select(userId => new LeanUserRole
      {
        RoleId = assignDto.RoleId,
        UserId = userId
      }).ToList();

      var result = await _db.Insertable(userRoles).ExecuteCommandAsync();
      return result > 0;
    }
    else
    {
      // 取消分配
      var result = await _db.Updateable<LeanUserRole>()
          .SetColumns(ur => new LeanUserRole { IsDeleted = 1 })
          .Where(ur => ur.RoleId == assignDto.RoleId && assignDto.UserIds.Contains(ur.UserId) && ur.IsDeleted == 0)
          .ExecuteCommandAsync();
      return result > 0;
    }
  }

  /// <summary>
  /// 更新角色继承关系
  /// </summary>
  public async Task<bool> UpdateRoleInheritanceAsync(LeanRoleInheritanceUpdateDto updateDto)
  {
    var role = await _db.Queryable<LeanRole>()
        .FirstAsync(r => r.Id == updateDto.RoleId) ?? throw new BusinessException("角色不存在");

    // 检查是否存在循环继承
    if (updateDto.InheritedRoleIds.Contains(role.Id))
    {
      throw new BusinessException("不能继承自身");
    }

    // 检查继承的角色是否都存在
    var inheritedRoles = await _db.Queryable<LeanRole>()
        .Where(r => updateDto.InheritedRoleIds.Contains(r.Id))
        .ToListAsync();

    if (inheritedRoles.Count != updateDto.InheritedRoleIds.Count)
    {
      throw new BusinessException("存在无效的继承角色ID");
    }

    // 更新继承关系
    role.InheritedRoleIds = string.Join(",", updateDto.InheritedRoleIds);
    role.InheritanceType = updateDto.InheritanceType;
    role.InheritedPermissions = updateDto.InheritedPermissions != null ?
        string.Join(",", updateDto.InheritedPermissions) : null;

    return await _db.Updateable(role).ExecuteCommandHasChangeAsync();
  }

  #region 私有方法

  /// <summary>
  /// 构建菜单树
  /// </summary>
  /// <param name="allMenus">所有菜单</param>
  /// <param name="roleMenuIds">角色菜单ID列表</param>
  /// <param name="parentId">父级ID</param>
  /// <returns>菜单树</returns>
  private List<LeanRoleMenuTreeDto> BuildMenuTree(List<LeanMenu> allMenus, List<long> roleMenuIds, long parentId = 0)
  {
    return allMenus
        .Where(m => m.ParentId == parentId)
        .Select(m => new LeanRoleMenuTreeDto
        {
          Id = m.Id,
          ParentId = m.ParentId,
          Name = m.MenuName,
          Code = m.MenuCode,
          Type = m.MenuType,
          OrderNum = m.OrderNum,
          Icon = m.Icon,
          Path = m.Path,
          Component = m.Component,
          Permission = m.Permission,
          IsExternal = m.External,
          IsCache = m.Cache,
          IsVisible = m.Visible,
          IsChecked = roleMenuIds.Contains(m.Id) ? 1 : 0,
          Children = BuildMenuTree(allMenus, roleMenuIds, m.Id)
        })
        .ToList();
  }

  #endregion 私有方法
}