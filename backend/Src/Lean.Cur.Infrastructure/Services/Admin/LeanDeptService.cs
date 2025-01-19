using System.Text;
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

namespace Lean.Cur.Infrastructure.Services.Admin
{
  /// <summary>
  /// 部门管理服务实现
  /// </summary>
  public class LeanDeptService : ILeanDeptService
  {
    private readonly SqlSugarClient _db;
    private readonly LeanExcelHelper _excel;

    public LeanDeptService(SqlSugarClient db, LeanExcelHelper excel)
    {
      _db = db;
      _excel = excel;
    }

    /// <inheritdoc/>
    public async Task<PagedResult<LeanDeptDto>> GetPagedListAsync(LeanDeptQueryDto queryDto)
    {
      // 构建查询条件
      var query = _db.Queryable<LeanDept>()
          .WhereIF(!string.IsNullOrEmpty(queryDto.DeptName), d => d.DeptName.Contains(queryDto.DeptName!))
          .WhereIF(!string.IsNullOrEmpty(queryDto.DeptCode), d => d.DeptCode.Contains(queryDto.DeptCode!))
          .WhereIF(queryDto.Status.HasValue, d => d.Status == queryDto.Status)
          .WhereIF(queryDto.StartTime.HasValue, d => d.CreateTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, d => d.CreateTime <= queryDto.EndTime)
          .OrderBy(d => d.OrderNum);

      // 获取分页结果
      var result = await query.ToPagedListAsync(queryDto);

      // 获取所有部门的父级信息
      var parentIds = result.Items.Where(d => d.ParentId > 0).Select(d => d.ParentId).Distinct().ToList();
      var parents = await _db.Queryable<LeanDept>()
          .Where(d => parentIds.Contains(d.Id))
          .ToListAsync();

      // 构建部门树
      var dtos = result.Items.Adapt<List<LeanDeptDto>>();
      foreach (var dto in dtos.Where(d => d.ParentId > 0))
      {
        var parent = parents.FirstOrDefault(p => p.Id == dto.ParentId);
        if (parent != null)
        {
          dto.Children = dtos.Where(d => d.ParentId == dto.Id).ToList();
        }
      }

      return new PagedResult<LeanDeptDto>(dtos, result.Total, result.PageIndex, result.PageSize);
    }

    /// <inheritdoc/>
    public async Task<LeanDeptDto> GetByIdAsync(long id)
    {
      var dept = await _db.Queryable<LeanDept>()
          .Where(d => d.Id == id)
          .FirstAsync() ?? throw new BusinessException("部门不存在");

      var dto = dept.Adapt<LeanDeptDto>();
      if (dept.ParentId > 0)
      {
        var parent = await _db.Queryable<LeanDept>()
            .Where(d => d.Id == dept.ParentId)
            .FirstAsync();
        if (parent != null)
        {
          var children = await _db.Queryable<LeanDept>()
              .Where(d => d.ParentId == dept.Id)
              .OrderBy(d => d.OrderNum)
              .ToListAsync();
          dto.Children = children.Adapt<List<LeanDeptDto>>();
        }
      }

      return dto;
    }

    /// <inheritdoc/>
    public async Task<LeanDeptDto> CreateAsync(LeanDeptCreateDto createDto)
    {
      // 检查父级部门是否存在
      if (createDto.ParentId > 0)
      {
        var parent = await _db.Queryable<LeanDept>()
            .Where(d => d.Id == createDto.ParentId)
            .FirstAsync() ?? throw new BusinessException("父级部门不存在");
      }

      // 检查部门名称是否重复
      var existName = await _db.Queryable<LeanDept>()
          .Where(d => d.ParentId == createDto.ParentId && d.DeptName == createDto.DeptName)
          .AnyAsync();
      if (existName)
      {
        throw new BusinessException("同一父级下已存在相同名称的部门");
      }

      // 检查部门编码是否重复
      var existCode = await _db.Queryable<LeanDept>()
          .Where(d => d.DeptCode.ToLower() == createDto.DeptCode.ToLower())
          .AnyAsync();
      if (existCode)
      {
        throw new BusinessException("部门编码已存在");
      }

      // 创建部门
      var dept = createDto.Adapt<LeanDept>();
      await _db.Insertable(dept).ExecuteReturnEntityAsync();

      return dept.Adapt<LeanDeptDto>();
    }

    /// <inheritdoc/>
    public async Task<LeanDeptDto> UpdateAsync(LeanDeptUpdateDto updateDto)
    {
      // 获取部门信息
      var dept = await _db.Queryable<LeanDept>()
          .Where(d => d.Id == updateDto.Id)
          .FirstAsync() ?? throw new BusinessException("部门不存在");

      // 检查父级部门是否存在
      if (updateDto.ParentId > 0)
      {
        var parent = await _db.Queryable<LeanDept>()
            .Where(d => d.Id == updateDto.ParentId)
            .FirstAsync() ?? throw new BusinessException("父级部门不存在");

        // 检查是否将父级设置为自己或自己的子部门
        if (updateDto.ParentId == dept.Id)
        {
          throw new BusinessException("不能将自己设置为父级部门");
        }

        var children = await GetChildrenAsync(dept.Id);
        if (children.Any(c => c.Id == updateDto.ParentId))
        {
          throw new BusinessException("不能将子部门设置为父级部门");
        }
      }

      // 检查部门名称是否重复
      var existName = await _db.Queryable<LeanDept>()
          .Where(d => d.ParentId == updateDto.ParentId && d.DeptName == updateDto.DeptName && d.Id != updateDto.Id)
          .AnyAsync();
      if (existName)
      {
        throw new BusinessException("同一父级下已存在相同名称的部门");
      }

      // 更新部门
      dept = updateDto.Adapt(dept);
      await _db.Updateable(dept).ExecuteCommandAsync();

      return dept.Adapt<LeanDeptDto>();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(long id)
    {
      // 获取部门信息
      var dept = await _db.Queryable<LeanDept>()
          .Where(d => d.Id == id)
          .FirstAsync() ?? throw new BusinessException("部门不存在");

      // 检查是否有子部门
      var hasChildren = await _db.Queryable<LeanDept>()
          .Where(d => d.ParentId == id)
          .AnyAsync();
      if (hasChildren)
      {
        throw new BusinessException("存在子部门，无法删除");
      }

      // 检查是否有用户
      var hasUsers = await _db.Queryable<LeanUser>()
          .Where(u => u.DeptId != null && u.DeptId.Value == id)
          .AnyAsync();
      if (hasUsers)
      {
        throw new BusinessException("部门下存在用户，无法删除");
      }

      // 删除部门
      return await _db.Deleteable<LeanDept>()
          .Where(d => d.Id == id)
          .ExecuteCommandHasChangeAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> BatchDeleteAsync(List<long> ids)
    {
      if (!ids.Any())
      {
        return false;
      }

      // 检查是否有子部门
      var hasChildren = await _db.Queryable<LeanDept>()
          .Where(d => ids.Contains(d.ParentId))
          .AnyAsync();
      if (hasChildren)
      {
        throw new BusinessException("选中的部门中存在子部门，无法删除");
      }

      // 检查是否有用户
      var hasUsers = await _db.Queryable<LeanUser>()
          .Where(u => u.DeptId != null && ids.Contains(u.DeptId.Value))
          .AnyAsync();
      if (hasUsers)
      {
        throw new BusinessException("选中的部门中存在用户，无法删除");
      }

      // 删除部门
      return await _db.Deleteable<LeanDept>()
          .Where(d => ids.Contains(d.Id))
          .ExecuteCommandHasChangeAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateStatusAsync(LeanDeptStatusDto statusDto)
    {
      var dept = await _db.Queryable<LeanDept>()
          .FirstAsync(d => d.Id == statusDto.Id) ?? throw new BusinessException("部门不存在");

      dept.Status = statusDto.Status;
      return await _db.Updateable(dept).ExecuteCommandHasChangeAsync();
    }

    /// <inheritdoc/>
    public async Task<byte[]> GetImportTemplateAsync()
    {
      var headers = new Dictionary<string, string>
            {
                { "DeptName", "部门名称" },
                { "DeptCode", "部门编码" },
                { "ParentId", "父级部门ID" },
                { "OrderNum", "显示顺序" },
                { "Leader", "负责人" },
                { "Phone", "联系电话" },
                { "Email", "邮箱" },
                { "Status", "状态" },
                { "Remark", "备注" }
            };

      return await _excel.GenerateTemplateAsync(headers);
    }

    /// <inheritdoc/>
    public async Task<LeanDeptImportResultDto> ImportAsync(IFormFile file)
    {
      var importResult = _excel.Import<LeanDeptImportDto>(file.OpenReadStream(), file.FileName);
      if (importResult == null || !importResult.Any())
      {
        throw new BusinessException("导入数据为空");
      }

      var result = new LeanDeptImportResultDto
      {
        TotalCount = importResult.Count,
        SuccessCount = 0,
        FailureCount = 0,
        FailureItems = new List<LeanDeptImportDto>()
      };

      foreach (var item in importResult)
      {
        try
        {
          // 验证父级部门
          var parentDept = await _db.Queryable<LeanDept>()
              .FirstAsync(d => d.DeptName == item.ParentDeptName && d.IsDeleted == 0);
          if (parentDept == null)
          {
            throw new BusinessException($"父级部门不存在：{item.ParentDeptName}");
          }

          // 检查部门名称是否已存在
          if (await _db.Queryable<LeanDept>().AnyAsync(d => d.DeptName == item.DeptName && d.ParentId == parentDept.Id && d.IsDeleted == 0))
          {
            throw new BusinessException($"同级部门下已存在相同名称的部门：{item.DeptName}");
          }

          // 检查部门编码是否已存在
          if (await _db.Queryable<LeanDept>().AnyAsync(d => d.DeptCode == item.DeptCode && d.IsDeleted == 0))
          {
            throw new BusinessException($"部门编码已存在：{item.DeptCode}");
          }

          var dept = new LeanDept
          {
            DeptName = item.DeptName,
            DeptCode = item.DeptCode,
            ParentId = parentDept.Id,
            Leader = item.Leader,
            Phone = item.Phone,
            Email = item.Email,
            OrderNum = item.OrderNum,
            Status = item.Status
          };

          await _db.Insertable(dept).ExecuteCommandAsync();
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
    public async Task<byte[]> ExportAsync(LeanDeptQueryDto queryDto)
    {
      var list = await _db.Queryable<LeanDept>()
          .WhereIF(!string.IsNullOrEmpty(queryDto.DeptName), d => d.DeptName.Contains(queryDto.DeptName))
          .WhereIF(!string.IsNullOrEmpty(queryDto.DeptCode), d => d.DeptCode.Contains(queryDto.DeptCode))
          .WhereIF(queryDto.Status.HasValue, d => d.Status == queryDto.Status)
          .WhereIF(queryDto.StartTime.HasValue, d => d.CreateTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, d => d.CreateTime <= queryDto.EndTime)
          .OrderBy(d => d.OrderNum)
          .Select<LeanDeptExportDto>()
          .ToListAsync();

      var headers = new Dictionary<string, string>
        {
            { "DeptName", "部门名称" },
            { "DeptCode", "部门编码" },
            { "ParentDeptName", "上级部门" },
            { "OrderNum", "显示顺序" },
            { "Status", "状态" },
            { "Remark", "备注" },
            { "CreateTime", "创建时间" },
            { "UpdateTime", "更新时间" }
        };

      return await _excel.ExportAsync<LeanDeptExportDto>(headers, list);
    }

    /// <inheritdoc/>
    private async Task<List<LeanDept>> GetChildrenAsync(long deptId)
    {
      var children = new List<LeanDept>();
      var dept = await _db.Queryable<LeanDept>()
          .FirstAsync(d => d.Id == deptId);

      if (dept != null)
      {
        children.Add(dept);
        var subDepts = await _db.Queryable<LeanDept>()
            .Where(d => d.ParentId == deptId)
            .ToListAsync();

        foreach (var subDept in subDepts)
        {
          var subChildren = await GetChildrenAsync(subDept.Id);
          children.AddRange(subChildren);
        }
      }

      return children;
    }

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>部门列表</returns>
    public async Task<List<LeanDeptDto>> GetListAsync(LeanDeptQueryDto queryDto)
    {
      var query = _db.Queryable<LeanDept>();

      // 部门名称
      if (!string.IsNullOrEmpty(queryDto.DeptName))
      {
        query = query.Where(d => d.DeptName.Contains(queryDto.DeptName));
      }

      // 部门编码
      if (!string.IsNullOrEmpty(queryDto.DeptCode))
      {
        query = query.Where(d => d.DeptCode.Contains(queryDto.DeptCode));
      }

      // 状态
      if (queryDto.Status.HasValue)
      {
        query = query.Where(d => d.Status == queryDto.Status.Value);
      }

      // 创建时间范围
      if (queryDto.StartTime.HasValue)
      {
        query = query.Where(d => d.CreateTime >= queryDto.StartTime.Value);
      }
      if (queryDto.EndTime.HasValue)
      {
        query = query.Where(d => d.CreateTime <= queryDto.EndTime.Value);
      }

      // 排序
      query = query.OrderBy(d => d.OrderNum);

      // 查询并转换为DTO
      var list = await query.Select<LeanDeptDto>().ToListAsync();
      return list;
    }
  }
}