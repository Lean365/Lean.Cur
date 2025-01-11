using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 字典数据服务实现
/// </summary>
public class LeanDictDataService : ILeanDictDataService
{
  private readonly ILogger<LeanDictDataService> _logger;
  private readonly ILeanBaseRepository<LeanDictData> _dictDataRepository;
  private readonly ILeanBaseRepository<LeanDictType> _dictTypeRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="dictDataRepository">字典数据仓储</param>
  /// <param name="dictTypeRepository">字典类型仓储</param>
  public LeanDictDataService(
      ILogger<LeanDictDataService> logger,
      ILeanBaseRepository<LeanDictData> dictDataRepository,
      ILeanBaseRepository<LeanDictType> dictTypeRepository)
  {
    _logger = logger;
    _dictDataRepository = dictDataRepository;
    _dictTypeRepository = dictTypeRepository;
  }

  /// <inheritdoc/>
  public async Task<List<LeanDictDataDto>> GetDictDataListAsync(LeanDictDataQueryDto query)
  {
    var dictDatas = await _dictDataRepository.AsQueryable()
        .WhereIF(query.DictTypeId.HasValue, x => x.DictTypeId == query.DictTypeId)
        .WhereIF(!string.IsNullOrEmpty(query.DictLabel), x => x.DictLabel.Contains(query.DictLabel!))
        .WhereIF(!string.IsNullOrEmpty(query.DictValue), x => x.DictValue.Contains(query.DictValue!))
        .WhereIF(query.Status.HasValue, x => x.Status == query.Status)
        .WhereIF(query.StartTime.HasValue, x => x.CreateTime >= query.StartTime)
        .WhereIF(query.EndTime.HasValue, x => x.CreateTime <= query.EndTime)
        .OrderBy(x => x.OrderNum)
        .Includes(x => x.DictType)
        .ToListAsync();

    var dtos = dictDatas.Adapt<List<LeanDictDataDto>>();
    foreach (var dto in dtos)
    {
      if (dictDatas.FirstOrDefault(x => x.Id == dto.Id)?.DictType != null)
      {
        dto.DictTypeName = dictDatas.First(x => x.Id == dto.Id).DictType!.DictName;
        dto.DictType = dictDatas.First(x => x.Id == dto.Id).DictType!.DictType;
      }
    }

    return dtos;
  }

  /// <inheritdoc/>
  public async Task<LeanDictDataDto> GetDictDataAsync(long id)
  {
    var dictData = await _dictDataRepository.AsQueryable()
        .Where(x => x.Id == id)
        .Includes(x => x.DictType)
        .FirstAsync();

    if (dictData == null)
    {
      throw new LeanException("字典数据不存在");
    }

    var dto = dictData.Adapt<LeanDictDataDto>();
    if (dictData.DictType != null)
    {
      dto.DictTypeName = dictData.DictType.DictName;
      dto.DictType = dictData.DictType.DictType;
    }

    return dto;
  }

  /// <inheritdoc/>
  public async Task<long> CreateDictDataAsync(LeanDictDataCreateDto dto)
  {
    // 检查字典类型是否存在
    var dictType = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
    if (dictType == null)
    {
      throw new LeanException("字典类型不存在");
    }

    var dictData = dto.Adapt<LeanDictData>();
    await _dictDataRepository.InsertAsync(dictData);

    return dictData.Id;
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateDictDataAsync(LeanDictDataUpdateDto dto)
  {
    var dictData = await _dictDataRepository.GetByIdAsync(dto.Id);
    if (dictData == null)
    {
      throw new LeanException("字典数据不存在");
    }

    // 检查字典类型是否存在
    var dictType = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
    if (dictType == null)
    {
      throw new LeanException("字典类型不存在");
    }

    dto.Adapt(dictData);
    return await _dictDataRepository.UpdateAsync(dictData);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteDictDataAsync(long id)
  {
    var dictData = await _dictDataRepository.GetByIdAsync(id);
    if (dictData == null)
    {
      throw new LeanException("字典数据不存在");
    }

    return await _dictDataRepository.DeleteAsync(dictData);
  }

  /// <inheritdoc/>
  public async Task<List<LeanDictDataDto>> GetDictDataListByDictTypeAsync(string dictType)
  {
    var dictDatas = await _dictDataRepository.AsQueryable()
        .Where(x => x.DictType!.DictType == dictType)
        .Where(x => x.Status == LeanStatus.Enable)
        .OrderBy(x => x.OrderNum)
        .Includes(x => x.DictType)
        .ToListAsync();

    var dtos = dictDatas.Adapt<List<LeanDictDataDto>>();
    foreach (var dto in dtos)
    {
      if (dictDatas.FirstOrDefault(x => x.Id == dto.Id)?.DictType != null)
      {
        dto.DictTypeName = dictDatas.First(x => x.Id == dto.Id).DictType!.DictName;
        dto.DictType = dictDatas.First(x => x.Id == dto.Id).DictType!.DictType;
      }
    }

    return dtos;
  }
}