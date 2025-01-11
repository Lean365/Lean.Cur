using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 翻译服务接口
/// </summary>
public interface ILeanTranslationService
{
  /// <summary>
  /// 获取翻译列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>翻译列表</returns>
  Task<List<LeanTranslationDto>> GetTranslationListAsync(LeanTranslationQueryDto query);

  /// <summary>
  /// 获取翻译详情
  /// </summary>
  /// <param name="id">翻译ID</param>
  /// <returns>翻译详情</returns>
  Task<LeanTranslationDto> GetTranslationAsync(long id);

  /// <summary>
  /// 创建翻译
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>翻译ID</returns>
  Task<long> CreateTranslationAsync(LeanTranslationCreateDto dto);

  /// <summary>
  /// 更新翻译
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateTranslationAsync(LeanTranslationUpdateDto dto);

  /// <summary>
  /// 删除翻译
  /// </summary>
  /// <param name="id">翻译ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteTranslationAsync(long id);

  /// <summary>
  /// 根据语言代码和翻译键获取翻译值
  /// </summary>
  /// <param name="languageCode">语言代码</param>
  /// <param name="transKey">翻译键</param>
  /// <returns>翻译值</returns>
  Task<string?> GetTranslationValueAsync(string languageCode, string transKey);

  /// <summary>
  /// 根据语言代码获取翻译字典
  /// </summary>
  /// <param name="languageCode">语言代码</param>
  /// <returns>翻译字典</returns>
  Task<Dictionary<string, string>> GetTranslationDictionaryAsync(string languageCode);
}