using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 字典类型服务实现
/// </summary>
public class LeanDictTypeService : ILeanDictTypeService
{
  private readonly ILogger<LeanDictTypeService> _logger;
  private readonly ILeanBaseRepository<LeanDictType> _dictTypeRepository;
  private readonly ILeanBaseRepository<LeanDictData> _dictDataRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="dictTypeRepository">字典类型仓储</param>
  /// <param name="dictDataRepository">字典数据仓储</param>
  public LeanDictTypeService(
      ILogger<LeanDictTypeService> logger,
      ILeanBaseRepository<LeanDictType> dictTypeRepository,
      ILeanBaseRepository<LeanDictData> dictDataRepository)
  {
    _logger = logger;
    _dictTypeRepository = dictTypeRepository;
    _dictDataRepository = dictDataRepository;
  }

  /// <inheritdoc/>
  public async Task<List<LeanDictTypeDto>> GetDictTypeListAsync(LeanDictTypeQueryDto query)
  {
    var dictTypes = await _dictTypeRepository.AsQueryable()
        .WhereIF(!string.IsNullOrEmpty(query.DictName), x => x.DictName.Contains(query.DictName!))
        .WhereIF(!string.IsNullOrEmpty(query.DictType), x => x.DictType.Contains(query.DictType!))
        .WhereIF(query.Status.HasValue, x => x.Status == query.Status)
        .WhereIF(query.StartTime.HasValue, x => x.CreateTime >= query.StartTime)
        .WhereIF(query.EndTime.HasValue, x => x.CreateTime <= query.EndTime)
        .OrderBy(x => x.OrderNum)
        .ToListAsync();

    return dictTypes.Adapt<List<LeanDictTypeDto>>();
  }

  /// <inheritdoc/>
  public async Task<LeanDictTypeDto> GetDictTypeAsync(long id)
  {
    var dictType = await _dictTypeRepository.GetByIdAsync(id);
    if (dictType == null)
    {
      throw new LeanException("字典类型不存在");
    }

    return dictType.Adapt<LeanDictTypeDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateDictTypeAsync(LeanDictTypeCreateDto dto)
  {
    // 检查字典类型是否存在
    var exists = await CheckDictTypeExistAsync(dto.DictType);
    if (exists)
    {
      throw new LeanException($"字典类型[{dto.DictType}]已存在");
    }

    var dictType = dto.Adapt<LeanDictType>();
    await _dictTypeRepository.InsertAsync(dictType);

    return dictType.Id;
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateDictTypeAsync(LeanDictTypeUpdateDto dto)
  {
    var dictType = await _dictTypeRepository.GetByIdAsync(dto.Id);
    if (dictType == null)
    {
      throw new LeanException("字典类型不存在");
    }

    // 检查字典类型是否存在
    var exists = await CheckDictTypeExistAsync(dto.DictType, dto.Id);
    if (exists)
    {
      throw new LeanException($"字典类型[{dto.DictType}]已存在");
    }

    dto.Adapt(dictType);
    return await _dictTypeRepository.UpdateAsync(dictType);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteDictTypeAsync(long id)
  {
    var dictType = await _dictTypeRepository.GetByIdAsync(id);
    if (dictType == null)
    {
      throw new LeanException("字典类型不存在");
    }

    // 系统内置字典不允许删除
    if (dictType.IsSystem == 1)
    {
      throw new LeanException("系统内置字典不允许删除");
    }

    // 检查是否存在字典数据
    var exists = await _dictDataRepository.AsQueryable()
        .AnyAsync(x => x.DictTypeId == id);
    if (exists)
    {
      throw new LeanException("存在字典数据,不允许删除");
    }

    return await _dictTypeRepository.DeleteAsync(dictType);
  }

  /// <inheritdoc/>
  public async Task<bool> CheckDictTypeExistAsync(string dictType, long? excludeId = null)
  {
    return await _dictTypeRepository.AsQueryable()
        .WhereIF(excludeId.HasValue, x => x.Id != excludeId)
        .AnyAsync(x => x.DictType == dictType);
  }
}