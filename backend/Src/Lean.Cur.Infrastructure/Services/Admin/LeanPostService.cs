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

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 岗位服务实现
/// </summary>
public class LeanPostService : ILeanPostService
{
    private readonly ISqlSugarClient _db;
    private readonly LeanExcelHelper _excel;

    public LeanPostService(ISqlSugarClient db, LeanExcelHelper excel)
    {
        _db = db;
        _excel = excel;
    }

    #region 基础操作

    /// <inheritdoc/>
    public async Task<PagedResult<LeanPostDto>> GetPagedListAsync(LeanPostQueryDto queryDto)
    {
        var query = _db.Queryable<LeanPost>()
            .WhereIF(!string.IsNullOrEmpty(queryDto.PostName), p => p.PostName.Contains(queryDto.PostName!))
            .WhereIF(!string.IsNullOrEmpty(queryDto.PostCode), p => p.PostCode.Contains(queryDto.PostCode!))
            .WhereIF(queryDto.Status.HasValue, p => p.Status == queryDto.Status)
            .WhereIF(queryDto.StartTime.HasValue, p => p.CreateTime >= queryDto.StartTime)
            .WhereIF(queryDto.EndTime.HasValue, p => p.CreateTime <= queryDto.EndTime)
            .OrderBy(p => p.OrderNum);

        var result = await query.ToPagedListAsync(queryDto);
        var dtos = result.Items.Adapt<List<LeanPostDto>>();
        return new PagedResult<LeanPostDto>(dtos, result.Total, result.PageIndex, result.PageSize);
    }

    /// <inheritdoc/>
    public async Task<LeanPostDto> GetByIdAsync(long id)
    {
        var post = await _db.Queryable<LeanPost>()
            .FirstAsync(p => p.Id == id) ?? throw new BusinessException("岗位不存在");

        return post.Adapt<LeanPostDto>();
    }

    /// <inheritdoc/>
    public async Task<long> CreateAsync(LeanPostCreateDto createDto)
    {
        // 检查岗位编码是否已存在
        if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostCode == createDto.PostCode))
        {
            throw new BusinessException("岗位编码已存在");
        }

        var post = createDto.Adapt<LeanPost>();
        return await _db.Insertable(post).ExecuteReturnSnowflakeIdAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(LeanPostUpdateDto updateDto)
    {
        var post = await _db.Queryable<LeanPost>()
            .FirstAsync(p => p.Id == updateDto.Id) ?? throw new BusinessException("岗位不存在");

        // 检查岗位编码是否已存在
        if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostCode == updateDto.PostCode && p.Id != updateDto.Id))
        {
            throw new BusinessException("岗位编码已存在");
        }

        updateDto.Adapt(post);
        return await _db.Updateable(post).ExecuteCommandHasChangeAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(long id)
    {
        var post = await _db.Queryable<LeanPost>()
            .FirstAsync(p => p.Id == id) ?? throw new BusinessException("岗位不存在");

        // 检查是否有用户关联
        if (await _db.Queryable<LeanUserPost>().AnyAsync(up => up.PostId == id))
        {
            throw new BusinessException("岗位已被用户使用，无法删除");
        }

        return await _db.Deleteable<LeanPost>().Where(p => p.Id == id).ExecuteCommandHasChangeAsync();
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
    public async Task<bool> UpdateStatusAsync(LeanPostStatusDto statusDto)
    {
        var post = await _db.Queryable<LeanPost>()
            .FirstAsync(p => p.Id == statusDto.Id) ?? throw new BusinessException("岗位不存在");

        post.Status = statusDto.Status;
        return await _db.Updateable(post).ExecuteCommandHasChangeAsync();
    }

    #endregion

    #region 导入导出

    /// <inheritdoc/>
    public async Task<byte[]> GetImportTemplateAsync()
    {
        var template = new List<LeanPostImportTemplateDto>
        {
            new()
            {
                PostName = "示例：开发工程师",
                PostCode = "示例：dev",
                OrderNum = "1",
                Status = LeanStatus.Normal.GetDescription(),
                Remark = "示例：负责系统开发工作"
            }
        };

        var headers = new Dictionary<string, string>
        {
            { "PostName", "岗位名称" },
            { "PostCode", "岗位编码" },
            { "OrderNum", "显示顺序" },
            { "Status", "状态" },
            { "Remark", "备注" }
        };

        return await _excel.ExportAsync(headers, template);
    }

    /// <inheritdoc/>
    public async Task<LeanPostImportResultDto> ImportAsync(IFormFile file)
    {
        var result = new LeanPostImportResultDto
        {
            TotalCount = 0,
            SuccessCount = 0,
            FailureCount = 0,
            FailureItems = new List<LeanPostImportDto>()
        };

        try
        {
            var importDtos = await _excel.ImportAsync<LeanPostImportDto>(file);
            if (importDtos == null || !importDtos.Any())
            {
                throw new BusinessException("导入的数据为空");
            }

            result.TotalCount = importDtos.Count;

            foreach (var dto in importDtos)
            {
                try
                {
                    // 检查岗位编码是否已存在
                    if (await _db.Queryable<LeanPost>().AnyAsync(p => p.PostCode == dto.PostCode))
                    {
                        throw new BusinessException($"岗位编码已存在：{dto.PostCode}");
                    }

                    var post = new LeanPost
                    {
                        PostName = dto.PostName,
                        PostCode = dto.PostCode,
                        OrderNum = dto.OrderNum,
                        Status = dto.Status,
                        Remark = dto.Remark
                    };

                    await _db.Insertable(post).ExecuteCommandAsync();
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    dto.ErrorMessage = ex.Message;
                    result.FailureItems.Add(dto);
                    result.FailureCount++;
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new BusinessException($"导入失败：{ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<byte[]> ExportAsync(LeanPostQueryDto queryDto)
    {
        var query = _db.Queryable<LeanPost>()
            .WhereIF(!string.IsNullOrEmpty(queryDto.PostName), p => p.PostName.Contains(queryDto.PostName!))
            .WhereIF(!string.IsNullOrEmpty(queryDto.PostCode), p => p.PostCode.Contains(queryDto.PostCode!))
            .WhereIF(queryDto.Status.HasValue, p => p.Status == queryDto.Status)
            .WhereIF(queryDto.StartTime.HasValue, p => p.CreateTime >= queryDto.StartTime)
            .WhereIF(queryDto.EndTime.HasValue, p => p.CreateTime <= queryDto.EndTime)
            .OrderBy(p => p.OrderNum);

        var list = await query.Select(p => new LeanPostExportDto
        {
            PostName = p.PostName,
            PostCode = p.PostCode,
            OrderNum = p.OrderNum.ToString(),
            Status = p.Status.GetDescription(),
            Remark = p.Remark,
            CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            UpdateTime = p.UpdateTime.HasValue ? p.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty
        }).ToListAsync();

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

        return await _excel.ExportAsync(headers, list);
    }

    #endregion
} 