// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Cache;
using Lean.Cur.Domain.Entities.Admin;
using Mapster;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 岗位服务实现
/// </summary>
public class LeanPostService : ILeanPostService
{
  private readonly SqlSugarClient _db;
  private readonly ILeanCache _cache;
  private readonly LeanExcelHelper _excel;

  public LeanPostService(SqlSugarClient db, ILeanCache cache, LeanExcelHelper excel)
  {
    _db = db;
    _cache = cache;
    _excel = excel;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<PagedResult<LeanPostDto>> GetPagedListAsync(LeanPostQueryDto queryDto)
  {
    var query = _db.Queryable<LeanPost>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.PostName), p => p.PostName!.Contains(queryDto.PostName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.PostCode), p => p.PostCode!.Contains(queryDto.PostCode!))
        .WhereIF(queryDto.Status.HasValue, p => p.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, p => p.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, p => p.CreateTime <= queryDto.EndTime)
        .Where(p => p.IsDeleted == 0);

    var total = await query.CountAsync();
    var items = await query
        .OrderBy(p => p.OrderNum)
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .Select(p => new LeanPostDto
        {
          Id = p.Id,
          PostName = p.PostName ?? string.Empty,
          PostCode = p.PostCode ?? string.Empty,
          OrderNum = p.OrderNum,
          Status = p.Status,
          Remark = p.Remark,
          CreateTime = p.CreateTime,
          CreateBy = p.CreateBy == null ? string.Empty : p.CreateBy.ToString(),
          UpdateTime = p.UpdateTime,
          UpdateBy = p.UpdateBy == null ? string.Empty : p.UpdateBy.ToString()
        })
        .ToListAsync();

    return new PagedResult<LeanPostDto>
    {
      Total = total,
      Items = items
    };
  }

  /// <inheritdoc/>
  public async Task<LeanPostDto> GetByIdAsync(long id)
  {
    var post = await _db.Queryable<LeanPost>()
        .FirstAsync(p => p.Id == id && p.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("岗位不存在");

    return post.Adapt<LeanPostDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanPostCreateDto createDto)
  {
    // 检查岗位编码是否已存在
    if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostCode == createDto.PostCode && p.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("岗位编码已存在");
    }

    // 检查岗位名称是否已存在
    if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostName == createDto.PostName && p.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("岗位名称已存在");
    }

    var post = createDto.Adapt<LeanPost>();
    return await _db.Insertable(post).ExecuteReturnIdentityAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanPostUpdateDto updateDto)
  {
    var post = await _db.Queryable<LeanPost>()
        .FirstAsync(p => p.Id == updateDto.Id && p.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("岗位不存在");

    // 检查岗位名称是否已存在
    if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostName == updateDto.PostName && p.Id != updateDto.Id && p.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("岗位名称已存在");
    }

    post.PostName = updateDto.PostName;
    post.OrderNum = updateDto.OrderNum;
    post.Status = updateDto.Status;
    post.Remark = updateDto.Remark;

    return await _db.Updateable(post).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    var post = await _db.Queryable<LeanPost>()
        .FirstAsync(p => p.Id == id && p.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("岗位不存在");

    // 检查是否有用户关联此岗位
    if (await _db.Queryable<LeanUser>().AnyAsync(u => u.PostId == id && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("岗位下存在用户，无法删除");
    }

    return await _db.Deleteable(post).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchDeleteAsync(List<long> ids)
  {
    if (!ids.Any())
    {
      throw new LeanUserFriendlyException("请选择要删除的岗位");
    }

    // 检查是否有用户关联这些岗位
    if (await _db.Queryable<LeanUser>().AnyAsync(u => ids.Contains(u.PostId!.Value) && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("选中的岗位下存在用户，无法删除");
    }

    return await _db.Deleteable<LeanPost>().Where(p => ids.Contains(p.Id)).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateStatusAsync(LeanPostStatusDto statusDto)
  {
    var post = await _db.Queryable<LeanPost>()
        .FirstAsync(p => p.Id == statusDto.Id && p.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("岗位不存在");

    post.Status = statusDto.Status;
    return await _db.Updateable(post).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<List<LeanOptionModel>> GetOptionsAsync()
  {
    var posts = await _db.Queryable<LeanPost>()
        .Where(p => p.Status == LeanStatus.Normal && p.IsDeleted == 0)
        .OrderBy(p => p.OrderNum)
        .Select(p => new LeanOptionModel
        {
          Label = p.PostName ?? string.Empty,
          Value = p.Id.ToString()
        })
        .ToListAsync();

    return posts;
  }

  #endregion

  #region 导入导出

  /// <inheritdoc/>
  public async Task<byte[]> GetImportTemplateAsync()
  {
    var template = new List<LeanPostTempleteDto>
    {
        new()
        {
            PostName = "技术总监",
            PostCode = "tech_director",
            OrderNum = 1,
            Remark = "负责技术团队管理"
        }
    };

    return _excel.Export(template);
  }

  /// <inheritdoc/>
  public async Task<LeanPostImportResultDto> ImportAsync(IFormFile file)
  {
    var importItems = _excel.Import<LeanPostImportDto>(file);
    if (!importItems.Any())
    {
      throw new LeanUserFriendlyException("导入数据为空");
    }

    var result = new LeanPostImportResultDto
    {
      TotalCount = importItems.Count
    };

    foreach (var item in importItems)
    {
      try
      {
        // 检查岗位编码是否已存在
        if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostCode == item.PostCode && p.IsDeleted == 0))
        {
          item.ErrorMessage = "岗位编码已存在";
          result.FailureItems.Add(item);
          continue;
        }

        // 检查岗位名称是否已存在
        if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostName == item.PostName && p.IsDeleted == 0))
        {
          item.ErrorMessage = "岗位名称已存在";
          result.FailureItems.Add(item);
          continue;
        }

        var post = item.Adapt<LeanPost>();
        post.Status = LeanStatus.Normal;
        await _db.Insertable(post).ExecuteCommandAsync();
        result.SuccessCount++;
      }
      catch (Exception ex)
      {
        item.ErrorMessage = ex.Message;
        result.FailureItems.Add(item);
      }
    }

    result.FailureCount = result.FailureItems.Count;
    return result;
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(LeanPostQueryDto queryDto)
  {
    var query = _db.Queryable<LeanPost>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.PostName), p => p.PostName!.Contains(queryDto.PostName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.PostCode), p => p.PostCode!.Contains(queryDto.PostCode!))
        .WhereIF(queryDto.Status.HasValue, p => p.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, p => p.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, p => p.CreateTime <= queryDto.EndTime)
        .Where(p => p.IsDeleted == 0);

    var posts = await query.OrderBy(p => p.OrderNum).ToListAsync();

    var exportData = posts.Select(p => new LeanPostExportDto
    {
      PostName = p.PostName,
      PostCode = p.PostCode,
      OrderNum = p.OrderNum.ToString(),
      Status = p.Status.ToString(),
      Remark = p.Remark,
      CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
      UpdateTime = p.UpdateTime?.ToString("yyyy-MM-dd HH:mm:ss")
    }).ToList();

    var headers = new Dictionary<string, string>
    {
        { "PostName", "岗位名称" },
        { "PostCode", "岗位编码" },
        { "OrderNum", "显示顺序" },
        { "Status", "状态" },
        { "Remark", "备注" },
        { "CreateTime", "创建时间" },
        { "UpdateTime", "更新时间" }
    };

    return await _excel.ExportAsync(headers, exportData);
  }

  #endregion
}