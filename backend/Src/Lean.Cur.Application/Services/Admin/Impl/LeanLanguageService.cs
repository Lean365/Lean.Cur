using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 语言服务实现
/// </summary>
public class LeanLanguageService : ILeanLanguageService
{
  private readonly ILogger<LeanLanguageService> _logger;
  private readonly ILeanBaseRepository<LeanLanguage> _languageRepository;
  private readonly ILeanBaseRepository<LeanTranslation> _translationRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="languageRepository">语言仓储</param>
  /// <param name="translationRepository">翻译仓储</param>
  public LeanLanguageService(
      ILogger<LeanLanguageService> logger,
      ILeanBaseRepository<LeanLanguage> languageRepository,
      ILeanBaseRepository<LeanTranslation> translationRepository)
  {
    _logger = logger;
    _languageRepository = languageRepository;
    _translationRepository = translationRepository;
  }

  /// <inheritdoc/>
  public async Task<List<LeanLanguageDto>> GetLanguageListAsync(LeanLanguageQueryDto query)
  {
    var languages = await _languageRepository.AsQueryable()
        .WhereIF(!string.IsNullOrEmpty(query.LanguageName), x => x.LanguageName.Contains(query.LanguageName!))
        .WhereIF(!string.IsNullOrEmpty(query.LanguageCode), x => x.LanguageCode.Contains(query.LanguageCode!))
        .WhereIF(query.Status.HasValue, x => x.Status == query.Status)
        .WhereIF(query.StartTime.HasValue, x => x.CreateTime >= query.StartTime)
        .WhereIF(query.EndTime.HasValue, x => x.CreateTime <= query.EndTime)
        .OrderBy(x => x.OrderNum)
        .ToListAsync();

    return languages.Adapt<List<LeanLanguageDto>>();
  }

  /// <inheritdoc/>
  public async Task<LeanLanguageDto> GetLanguageAsync(long id)
  {
    var language = await _languageRepository.GetByIdAsync(id);
    if (language == null)
    {
      throw new LeanException("语言不存在");
    }

    return language.Adapt<LeanLanguageDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateLanguageAsync(LeanLanguageCreateDto dto)
  {
    // 检查语言代码是否存在
    var exists = await CheckLanguageCodeExistAsync(dto.LanguageCode);
    if (exists)
    {
      throw new LeanException($"语言代码[{dto.LanguageCode}]已存在");
    }

    // 如果设置为默认语言,则将其他语言设置为非默认
    if (dto.IsDefault == 1)
    {
      await _languageRepository.UpdateAsync(x => new LeanLanguage { IsDefault = 0 }, x => x.IsDefault == 1);
    }

    var language = dto.Adapt<LeanLanguage>();
    await _languageRepository.InsertAsync(language);

    return language.Id;
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateLanguageAsync(LeanLanguageUpdateDto dto)
  {
    var language = await _languageRepository.GetByIdAsync(dto.Id);
    if (language == null)
    {
      throw new LeanException("语言不存在");
    }

    // 检查语言代码是否存在
    var exists = await CheckLanguageCodeExistAsync(dto.LanguageCode, dto.Id);
    if (exists)
    {
      throw new LeanException($"语言代码[{dto.LanguageCode}]已存在");
    }

    // 如果设置为默认语言,则将其他语言设置为非默认
    if (dto.IsDefault == 1 && language.IsDefault == 0)
    {
      await _languageRepository.UpdateAsync(x => new LeanLanguage { IsDefault = 0 }, x => x.IsDefault == 1);
    }

    dto.Adapt(language);
    return await _languageRepository.UpdateAsync(language);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteLanguageAsync(long id)
  {
    var language = await _languageRepository.GetByIdAsync(id);
    if (language == null)
    {
      throw new LeanException("语言不存在");
    }

    // 默认语言不允许删除
    if (language.IsDefault == 1)
    {
      throw new LeanException("默认语言不允许删除");
    }

    // 检查是否存在翻译
    var exists = await _translationRepository.AsQueryable()
        .AnyAsync(x => x.LanguageId == id);
    if (exists)
    {
      throw new LeanException("存在翻译数据,不允许删除");
    }

    return await _languageRepository.DeleteAsync(language);
  }

  /// <inheritdoc/>
  public async Task<bool> CheckLanguageCodeExistAsync(string languageCode, long? excludeId = null)
  {
    return await _languageRepository.AsQueryable()
        .WhereIF(excludeId.HasValue, x => x.Id != excludeId)
        .AnyAsync(x => x.LanguageCode == languageCode);
  }
}