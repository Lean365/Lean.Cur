using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Cache;
using Lean.Cur.Domain.Entities.Admin;
using Mapster;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System.Text;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 菜单服务实现
/// </summary>
public class LeanMenuService : ILeanMenuService
{
  private readonly ISqlSugarClient _db;
  private readonly ILeanCache _cache;
  private readonly LeanExcelHelper _excel;

  public LeanMenuService(ISqlSugarClient db, ILeanCache cache, LeanExcelHelper excel)
  {
    _db = db;
    _cache = cache;
    _excel = excel;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<PagedResult<LeanMenuDto>> GetPagedListAsync(LeanMenuQueryDto queryDto)
  {
    var query = _db.Queryable<LeanMenu>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.MenuName), m => m.MenuName!.Contains(queryDto.MenuName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.MenuCode), m => m.MenuCode!.Contains(queryDto.MenuCode!))
        .WhereIF(queryDto.MenuType.HasValue, m => m.MenuType == queryDto.MenuType)
        .WhereIF(queryDto.Status.HasValue, m => m.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, m => m.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, m => m.CreateTime <= queryDto.EndTime)
        .Where(m => m.IsDeleted == 0);

    var total = await query.CountAsync();
    var items = await query
        .OrderBy(m => m.OrderNum)
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .Select(m => new LeanMenuDto
        {
          Id = m.Id,
          ParentId = m.ParentId,
          MenuName = m.MenuName,
          MenuCode = m.MenuCode,
          MenuType = m.MenuType,
          Path = m.Path,
          Component = m.Component,
          Permission = m.Permission,
          Icon = m.Icon,
          OrderNum = m.OrderNum,
          Status = m.Status,
          Visible = m.Visible,
          Cache = m.Cache,
          External = m.External,
          Remark = m.Remark,
          CreateTime = m.CreateTime,
          UpdateTime = m.UpdateTime
        })
        .ToListAsync();

    return new PagedResult<LeanMenuDto>
    {
      Total = total,
      Items = items
    };
  }

  /// <inheritdoc/>
  public async Task<List<LeanMenuDto>> GetTreeListAsync(LeanMenuQueryDto queryDto)
  {
    var query = _db.Queryable<LeanMenu>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.MenuName), m => m.MenuName.Contains(queryDto.MenuName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.MenuCode), m => m.MenuCode.Contains(queryDto.MenuCode!))
        .WhereIF(queryDto.MenuType.HasValue, m => m.MenuType == queryDto.MenuType)
        .WhereIF(queryDto.Status.HasValue, m => m.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, m => m.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, m => m.CreateTime <= queryDto.EndTime)
        .OrderBy(m => m.OrderNum);

    var menus = await query.ToListAsync();
    var dtos = menus.Adapt<List<LeanMenuDto>>();
    var tree = BuildTree(dtos, 0);
    return tree;
  }

  /// <inheritdoc/>
  public async Task<LeanMenuDto> GetByIdAsync(long id)
  {
    var menu = await _db.Queryable<LeanMenu>()
        .Where(m => m.Id == id)
        .FirstAsync() ?? throw new BusinessException("菜单不存在");

    var dto = menu.Adapt<LeanMenuDto>();
    if (menu.ParentId > 0)
    {
      var parentMenu = await _db.Queryable<LeanMenu>()
          .Where(m => m.Id == menu.ParentId)
          .FirstAsync();
      if (parentMenu != null)
      {
        var children = await _db.Queryable<LeanMenu>()
            .Where(m => m.ParentId == menu.Id)
            .OrderBy(m => m.OrderNum)
            .ToListAsync();
        dto.Children = children.Adapt<List<LeanMenuDto>>();
      }
    }

    return dto;
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanMenuCreateDto createDto)
  {
    // 验证父级菜单
    var parent = await _db.Queryable<LeanMenu>().FirstAsync(m => m.Id == createDto.ParentId);
    if (parent == null)
    {
      throw new BusinessException("父级菜单不存在");
    }

    // 按钮不能作为父级
    if (parent.MenuType == LeanMenuType.Button)
    {
      throw new BusinessException("按钮不能作为父级菜单");
    }

    // 验证必填字段
    switch (createDto.MenuType)
    {
      case LeanMenuType.Menu when string.IsNullOrEmpty(createDto.Path):
        throw new BusinessException("路由地址不能为空");
      case LeanMenuType.Menu when string.IsNullOrEmpty(createDto.Component):
        throw new BusinessException("组件路径不能为空");
      case LeanMenuType.Button when string.IsNullOrEmpty(createDto.Permission):
        throw new BusinessException("权限标识不能为空");
    }

    // 检查菜单名称是否已存在
    if (await _db.Queryable<LeanMenu>().AnyAsync(m => m.MenuName == createDto.MenuName && m.ParentId == createDto.ParentId && m.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("同级菜单下已存在相同名称的菜单");
    }

    // 检查菜单编码是否已存在
    if (await _db.Queryable<LeanMenu>().AnyAsync(m => m.MenuCode == createDto.MenuCode && m.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("菜单编码已存在");
    }

    var menu = createDto.Adapt<LeanMenu>();
    return await _db.Insertable(menu).ExecuteReturnIdentityAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanMenuUpdateDto updateDto)
  {
    var menu = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.Id == updateDto.Id && m.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("菜单不存在");

    // 验证父级菜单
    var parentMenu = await _db.Queryable<LeanMenu>().FirstAsync(m => m.Id == updateDto.ParentId);
    if (parentMenu == null)
    {
      throw new BusinessException("父级菜单不存在");
    }

    // 按钮不能作为父级
    if (parentMenu.MenuType == LeanMenuType.Button)
    {
      throw new BusinessException("按钮不能作为父级菜单");
    }

    // 验证必填字段
    switch (updateDto.MenuType)
    {
      case LeanMenuType.Menu when string.IsNullOrEmpty(updateDto.Path):
        throw new BusinessException("路由地址不能为空");
      case LeanMenuType.Menu when string.IsNullOrEmpty(updateDto.Component):
        throw new BusinessException("组件路径不能为空");
      case LeanMenuType.Button when string.IsNullOrEmpty(updateDto.Permission):
        throw new BusinessException("权限标识不能为空");
    }

    // 检查菜单名称是否已存在
    if (await _db.Queryable<LeanMenu>().AnyAsync(m => m.MenuName == updateDto.MenuName && m.ParentId == updateDto.ParentId && m.Id != updateDto.Id && m.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("同级菜单下已存在相同名称的菜单");
    }

    // 检查菜单编码是否已存在
    if (await _db.Queryable<LeanMenu>().AnyAsync(m => m.MenuCode == updateDto.MenuCode && m.Id != updateDto.Id && m.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("菜单编码已存在");
    }

    // 检查父级菜单是否存在
    if (updateDto.ParentId > 0)
    {
      // 检查是否将菜单移动到自己或自己的子菜单下
      if (await IsChildrenAsync(updateDto.Id, updateDto.ParentId))
      {
        throw new LeanUserFriendlyException("不能将菜单移动到自己或自己的子菜单下");
      }
    }

    updateDto.Adapt(menu);
    return await _db.Updateable(menu).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    var menu = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.Id == id && m.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("菜单不存在");

    // 检查是否有子菜单
    if (await _db.Queryable<LeanMenu>().AnyAsync(m => m.ParentId == id && m.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("该菜单下有子菜单，不能删除");
    }

    return await _db.Deleteable(menu).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchDeleteAsync(List<long> ids)
  {
    if (ids == null || !ids.Any())
    {
      throw new LeanUserFriendlyException("请选择要删除的菜单");
    }

    var menus = await _db.Queryable<LeanMenu>()
        .Where(m => ids.Contains(m.Id) && m.IsDeleted == 0)
        .ToListAsync();

    // 检查是否有子菜单
    foreach (var menu in menus)
    {
      if (await _db.Queryable<LeanMenu>().AnyAsync(m => m.ParentId == menu.Id && m.IsDeleted == 0))
      {
        throw new LeanUserFriendlyException($"菜单[{menu.MenuName}]下有子菜单，不能删除");
      }
    }

    return await _db.Deleteable(menus).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateStatusAsync(LeanMenuStatusDto statusDto)
  {
    var menu = await _db.Queryable<LeanMenu>()
        .FirstAsync(m => m.Id == statusDto.Id && m.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("菜单不存在");

    menu.Status = statusDto.Status;
    return await _db.Updateable(menu).ExecuteCommandHasChangeAsync();
  }

  #endregion 基础操作

  #region 导入导出

  /// <inheritdoc/>
  public async Task<byte[]> GetImportTemplateAsync()
  {
    var headers = new Dictionary<string, string>
        {
            { "MenuName", "菜单名称" },
            { "MenuCode", "菜单编码" },
            { "MenuType", "菜单类型" },
            { "ParentId", "父级菜单ID" },
            { "Status", "状态" },
            { "OrderNum", "显示顺序" },
            { "Path", "路由地址" },
            { "Component", "组件路径" },
            { "Permission", "权限标识" },
            { "Icon", "菜单图标" },
            { "Visible", "显示状态" },
            { "Cache", "是否缓存" },
            { "External", "是否外链" },
            { "Remark", "备注" }
        };

    return await _excel.GenerateTemplateAsync(headers);
  }

  /// <inheritdoc/>
  public async Task<LeanMenuImportResultDto> ImportAsync(IFormFile file)
  {
    var result = new LeanMenuImportResultDto();
    var importResult = _excel.Import<LeanMenuImportDto>(file.OpenReadStream(), file.FileName);
    if (importResult == null || !importResult.Any())
    {
      throw new BusinessException("导入数据为空");
    }

    result.TotalCount = importResult.Count();

    foreach (var importDto in importResult)
    {
      try
      {
        // 验证父级菜单
        var parentMenu = await _db.Queryable<LeanMenu>()
            .FirstAsync(m => m.MenuName == importDto.ParentMenuName && m.IsDeleted == 0);
        if (parentMenu == null)
        {
          importDto.ErrorMessage = $"父级菜单[{importDto.ParentMenuName}]不存在";
          result.FailureItems.Add(importDto);
          continue;
        }

        // 按钮不能作为父级
        if (parentMenu.MenuType == LeanMenuType.Button)
        {
          importDto.ErrorMessage = $"按钮[{importDto.ParentMenuName}]不能作为父级菜单";
          result.FailureItems.Add(importDto);
          continue;
        }

        // 验证菜单名称是否已存在
        var existMenu = await _db.Queryable<LeanMenu>()
            .FirstAsync(m => m.MenuName == importDto.MenuName && m.ParentId == parentMenu.Id && m.IsDeleted == 0);
        if (existMenu != null)
        {
          importDto.ErrorMessage = $"菜单[{importDto.MenuName}]在父级菜单[{importDto.ParentMenuName}]下已存在";
          result.FailureItems.Add(importDto);
          continue;
        }

        // 验证菜单编码是否已存在
        existMenu = await _db.Queryable<LeanMenu>()
            .FirstAsync(m => m.MenuCode == importDto.MenuCode && m.IsDeleted == 0);
        if (existMenu != null)
        {
          importDto.ErrorMessage = $"菜单编码[{importDto.MenuCode}]已存在";
          result.FailureItems.Add(importDto);
          continue;
        }

        // 创建菜单
        var menu = new LeanMenu
        {
          MenuName = importDto.MenuName,
          MenuCode = importDto.MenuCode,
          MenuType = importDto.MenuType,
          ParentId = parentMenu.Id,
          Path = importDto.Path,
          Component = importDto.Component,
          Permission = importDto.Permission,
          Icon = importDto.Icon,
          OrderNum = importDto.OrderNum,
          Status = LeanStatus.Normal,
          Visible = importDto.Visible,
          Cache = importDto.Cache,
          External = importDto.External,
          Remark = importDto.Remark
        };

        await _db.Insertable(menu).ExecuteCommandAsync();
        result.SuccessCount++;
      }
      catch (Exception ex)
      {
        importDto.ErrorMessage = ex.Message;
        result.FailureItems.Add(importDto);
      }
    }

    result.FailureCount = result.FailureItems.Count;
    return result;
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(LeanMenuQueryDto queryDto)
  {
    var list = await _db.Queryable<LeanMenu>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.MenuName), m => m.MenuName.Contains(queryDto.MenuName))
        .WhereIF(!string.IsNullOrEmpty(queryDto.MenuCode), m => m.MenuCode.Contains(queryDto.MenuCode))
        .WhereIF(queryDto.Status.HasValue, m => m.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, m => m.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, m => m.CreateTime <= queryDto.EndTime)
        .OrderBy(m => m.OrderNum)
        .Select<LeanMenuExportDto>()
        .ToListAsync();

    var headers = new Dictionary<string, string>
    {
      { "MenuName", "菜单名称" },
      { "MenuCode", "菜单编码" },
      { "MenuType", "菜单类型" },
      { "ParentMenuName", "上级菜单" },
      { "OrderNum", "显示顺序" },
      { "Path", "路由地址" },
      { "Component", "组件路径" },
      { "Permission", "权限标识" },
      { "Icon", "菜单图标" },
      { "Visible", "显示状态" },
      { "Cache", "是否缓存" },
      { "External", "是否外链" },
      { "Status", "状态" },
      { "Remark", "备注" },
      { "CreateTime", "创建时间" },
      { "UpdateTime", "更新时间" }
    };

    return await _excel.ExportAsync<LeanMenuExportDto>(headers, list);
  }

  #endregion 导入导出

  #region 私有方法

  /// <summary>
  /// 构建菜单树
  /// </summary>
  /// <param name="menus">菜单列表</param>
  /// <param name="parentId">父级ID</param>
  /// <returns>菜单树</returns>
  private List<LeanMenuDto> BuildTree(List<LeanMenuDto> menus, long parentId = 0)
  {
    return menus
        .Where(m => m.ParentId == parentId)
        .Select(m =>
        {
          m.Children = BuildTree(menus, m.Id);
          return m;
        })
        .ToList();
  }

  /// <summary>
  /// 判断是否是子菜单
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <param name="parentId">父级ID</param>
  /// <returns>是否是子菜单</returns>
  private async Task<bool> IsChildrenAsync(long id, long parentId)
  {
    if (id == parentId)
    {
      return true;
    }

    var children = await _db.Queryable<LeanMenu>()
        .Where(m => m.ParentId == id)
        .Select(m => m.Id)
        .ToListAsync();

    foreach (var childId in children)
    {
      if (await IsChildrenAsync(childId, parentId))
      {
        return true;
      }
    }

    return false;
  }

  #endregion 私有方法
}