using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 部门服务实现
/// </summary>
public class LeanDeptService : ILeanDeptService
{
  private readonly ILeanBaseRepository<LeanDept> _deptRepository;
  private readonly ILogger<LeanDeptService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDeptService(
      ILeanBaseRepository<LeanDept> deptRepository,
      ILogger<LeanDeptService> logger)
  {
    _deptRepository = deptRepository;
    _logger = logger;
  }

  /// <inheritdoc/>
  public async Task<List<LeanDeptDto>> GetDeptListAsync(LeanDeptQueryDto query)
  {
    try
    {
      var queryable = _deptRepository.AsQueryable();

      // 构建查询条件
      if (!string.IsNullOrEmpty(query.DeptName))
      {
        queryable = queryable.Where(x => x.DeptName.Contains(query.DeptName));
      }
      if (!string.IsNullOrEmpty(query.EnglishName))
      {
        queryable = queryable.Where(x => x.EnglishName != null && x.EnglishName.Contains(query.EnglishName));
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

      var depts = await queryable.ToListAsync();
      return depts.Adapt<List<LeanDeptDto>>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取部门列表失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<List<LeanDeptDto>> GetDeptTreeAsync(LeanDeptQueryDto query)
  {
    try
    {
      var depts = await GetDeptListAsync(query);
      return BuildDeptTree(depts);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取部门树形结构失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<LeanDeptDto?> GetDeptAsync(long id)
  {
    try
    {
      var dept = await _deptRepository.GetByIdAsync(id);
      return dept?.Adapt<LeanDeptDto>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取部门详情失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<long> CreateDeptAsync(LeanDeptCreateDto dto)
  {
    try
    {
      // 检查部门名称是否已存在
      if (await CheckDeptNameExistAsync(dto.DeptName))
      {
        throw new Exception($"部门名称 {dto.DeptName} 已存在");
      }

      // 构建祖级列表
      string ancestors = "";
      if (dto.ParentId.HasValue)
      {
        var parent = await _deptRepository.GetByIdAsync(dto.ParentId.Value);
        if (parent == null)
        {
          throw new Exception($"父部门ID {dto.ParentId.Value} 不存在");
        }
        ancestors = string.IsNullOrEmpty(parent.Ancestors)
            ? dto.ParentId.Value.ToString()
            : $"{parent.Ancestors},{dto.ParentId.Value}";
      }

      var dept = dto.Adapt<LeanDept>();
      dept.Ancestors = ancestors;

      await _deptRepository.AddAsync(dept);
      return dept.Id;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "创建部门失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateDeptAsync(LeanDeptUpdateDto dto)
  {
    try
    {
      var dept = await _deptRepository.GetByIdAsync(dto.Id);
      if (dept == null)
      {
        throw new Exception($"部门ID {dto.Id} 不存在");
      }

      // 检查部门名称是否已存在
      if (await CheckDeptNameExistAsync(dto.DeptName, dto.Id))
      {
        throw new Exception($"部门名称 {dto.DeptName} 已存在");
      }

      // 不能将部门的父部门设置为自己或其子部门
      if (dto.ParentId.HasValue)
      {
        var childrenIds = await GetDeptAndChildrenIdsAsync(dto.Id);
        if (childrenIds.Contains(dto.ParentId.Value))
        {
          throw new Exception("不能将部门的父部门设置为自己或其子部门");
        }

        // 更新祖级列表
        var parent = await _deptRepository.GetByIdAsync(dto.ParentId.Value);
        if (parent == null)
        {
          throw new Exception($"父部门ID {dto.ParentId.Value} 不存在");
        }
        dept.Ancestors = string.IsNullOrEmpty(parent.Ancestors)
            ? dto.ParentId.Value.ToString()
            : $"{parent.Ancestors},{dto.ParentId.Value}";
      }
      else
      {
        dept.Ancestors = "";
      }

      // 更新基本信息
      dept.DeptName = dto.DeptName;
      dept.EnglishName = dto.EnglishName;
      dept.ParentId = dto.ParentId;
      dept.OrderNum = dto.OrderNum;
      dept.Leader = dto.Leader;
      dept.Phone = dto.Phone;
      dept.Email = dto.Email;
      dept.Status = dto.Status;
      dept.Remark = dto.Remark;

      await _deptRepository.UpdateAsync(dept);

      // 更新子部门的祖级列表
      await UpdateChildrenAncestorsAsync(dept);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新部门失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task DeleteDeptAsync(long id)
  {
    try
    {
      var dept = await _deptRepository.GetByIdAsync(id);
      if (dept == null)
      {
        throw new Exception($"部门ID {id} 不存在");
      }

      // 检查是否存在子部门
      var hasChildren = await _deptRepository.AnyAsync(x => x.ParentId == id);
      if (hasChildren)
      {
        throw new Exception("存在子部门,不能删除");
      }

      // 检查是否存在用户
      if (dept.Users.Any())
      {
        throw new Exception("部门下存在用户,不能删除");
      }

      await _deptRepository.DeleteAsync(id);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "删除部门失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> CheckDeptNameExistAsync(string deptName, long? excludeId = null)
  {
    try
    {
      var queryable = _deptRepository.AsQueryable()
          .Where(x => x.DeptName == deptName);

      if (excludeId.HasValue)
      {
        queryable = queryable.Where(x => x.Id != excludeId.Value);
      }

      return await queryable.AnyAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查部门名称是否存在失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<List<long>> GetDeptAndChildrenIdsAsync(long deptId)
  {
    try
    {
      var ids = new List<long> { deptId };
      var children = await _deptRepository.GetListAsync(x => x.Ancestors != null && x.Ancestors.Contains(deptId.ToString()));
      ids.AddRange(children.Select(x => x.Id));
      return ids;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取部门及其所有子部门ID列表失败");
      throw;
    }
  }

  /// <summary>
  /// 构建部门树形结构
  /// </summary>
  private static List<LeanDeptDto> BuildDeptTree(List<LeanDeptDto> depts, long? parentId = null)
  {
    var tree = new List<LeanDeptDto>();
    var children = depts.Where(x => x.ParentId == parentId).ToList();
    foreach (var child in children)
    {
      child.Children = BuildDeptTree(depts, child.Id);
      tree.Add(child);
    }
    return tree;
  }

  /// <summary>
  /// 更新子部门的祖级列表
  /// </summary>
  private async Task UpdateChildrenAncestorsAsync(LeanDept dept)
  {
    try
    {
      var children = await _deptRepository.GetListAsync(x => x.ParentId == dept.Id);
      foreach (var child in children)
      {
        child.Ancestors = string.IsNullOrEmpty(dept.Ancestors)
            ? dept.Id.ToString()
            : $"{dept.Ancestors},{dept.Id}";
        await _deptRepository.UpdateAsync(child);
        await UpdateChildrenAncestorsAsync(child);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新子部门的祖级列表失败");
      throw;
    }
  }
}