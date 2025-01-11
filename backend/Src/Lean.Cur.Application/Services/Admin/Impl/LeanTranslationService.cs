using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 翻译服务实现
/// </summary>
public class LeanTranslationService : ILeanTranslationService
{
  private readonly ILogger<LeanTranslationService> _logger;
  private readonly ILeanBaseRepository<LeanTranslation> _translationRepository;
  private readonly ILeanBaseRepository<LeanLanguage> _languageRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="translationRepository">翻译仓储</param>
  /// <param name="languageRepository">语言仓储</param>
  public LeanTranslationService(
      ILogger<LeanTranslationService> logger,
      ILeanBaseRepository<LeanTranslation> translationRepository,
      ILeanBaseRepository<LeanLanguage> languageRepository)
  {
    _logger = logger;
    _translationRepository = translationRepository;
    _languageRepository = languageRepository;
  }

  /// <inheritdoc/>
  public async Task<List<LeanTranslationDto>> GetTranslationListAsync(LeanTranslationQueryDto query)
  {
    var translations = await _translationRepository.AsQueryable()
        .WhereIF(query.LanguageId.HasValue, x => x.LanguageId == query.LanguageId)
        .WhereIF(!string.IsNullOrEmpty(query.TransKey), x => x.TransKey.Contains(query.TransKey!))
        .WhereIF(!string.IsNullOrEmpty(query.TransValue), x => x.TransValue.Contains(query.TransValue!))
        .WhereIF(!string.IsNullOrEmpty(query.Module), x => x.Module.Contains(query.Module!))
        .WhereIF(query.Status.HasValue, x => x.Status == query.Status)
        .WhereIF(query.StartTime.HasValue, x => x.CreateTime >= query.StartTime)
        .WhereIF(query.EndTime.HasValue, x => x.CreateTime <= query.EndTime)
        .Includes(x => x.Language)
        .ToListAsync();

    var dtos = translations.Adapt<List<LeanTranslationDto>>();
    foreach (var dto in dtos)
    {
      if (translations.FirstOrDefault(x => x.Id == dto.Id)?.Language != null)
      {
        dto.LanguageName = translations.First(x => x.Id == dto.Id).Language!.LanguageName;
        dto.LanguageCode = translations.First(x => x.Id == dto.Id).Language!.LanguageCode;
      }
    }

    return dtos;
  }

  /// <inheritdoc/>
  public async Task<LeanTranslationDto> GetTranslationAsync(long id)
  {
    var translation = await _translationRepository.AsQueryable()
        .Where(x => x.Id == id)
        .Includes(x => x.Language)
        .FirstAsync();

    if (translation == null)
    {
      throw new LeanException("翻译不存在");
    }

    var dto = translation.Adapt<LeanTranslationDto>();
    if (translation.Language != null)
    {
      dto.LanguageName = translation.Language.LanguageName;
      dto.LanguageCode = translation.Language.LanguageCode;
    }

    return dto;
  }

  /// <inheritdoc/>
  public async Task<long> CreateTranslationAsync(LeanTranslationCreateDto dto)
  {
    // 检查语言是否存在
    var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
    if (language == null)
    {
      throw new LeanException("语言不存在");
    }

    var translation = dto.Adapt<LeanTranslation>();
    await _translationRepository.InsertAsync(translation);

    return translation.Id;
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateTranslationAsync(LeanTranslationUpdateDto dto)
  {
    var translation = await _translationRepository.GetByIdAsync(dto.Id);
    if (translation == null)
    {
      throw new LeanException("翻译不存在");
    }

    // 检查语言是否存在
    var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
    if (language == null)
    {
      throw new LeanException("语言不存在");
    }

    dto.Adapt(translation);
    return await _translationRepository.UpdateAsync(translation);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteTranslationAsync(long id)
  {
    var translation = await _translationRepository.GetByIdAsync(id);
    if (translation == null)
    {
      throw new LeanException("翻译不存在");
    }

    return await _translationRepository.DeleteAsync(translation);
  }

  /// <inheritdoc/>
  public async Task<string?> GetTranslationValueAsync(string languageCode, string transKey)
  {
    var translation = await _translationRepository.AsQueryable()
        .Where(x => x.Language!.LanguageCode == languageCode)
        .Where(x => x.TransKey == transKey)
        .Where(x => x.Status == LeanStatus.Enable)
        .Includes(x => x.Language)
        .FirstAsync();

    return translation?.TransValue;
  }

  /// <inheritdoc/>
  public async Task<Dictionary<string, string>> GetTranslationDictionaryAsync(string languageCode)
  {
    var translations = await _translationRepository.AsQueryable()
        .Where(x => x.Language!.LanguageCode == languageCode)
        .Where(x => x.Status == LeanStatus.Enable)
        .Includes(x => x.Language)
        .ToListAsync();

    return translations.ToDictionary(x => x.TransKey, x => x.TransValue);
  }
}