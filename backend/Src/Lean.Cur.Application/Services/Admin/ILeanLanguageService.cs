using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 语言服务接口
/// </summary>
public interface ILeanLanguageService
{
  /// <summary>
  /// 获取语言列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>语言列表</returns>
  Task<List<LeanLanguageDto>> GetLanguageListAsync(LeanLanguageQueryDto query);

  /// <summary>
  /// 获取语言详情
  /// </summary>
  /// <param name="id">语言ID</param>
  /// <returns>语言详情</returns>
  Task<LeanLanguageDto> GetLanguageAsync(long id);

  /// <summary>
  /// 创建语言
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>语言ID</returns>
  Task<long> CreateLanguageAsync(LeanLanguageCreateDto dto);

  /// <summary>
  /// 更新语言
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateLanguageAsync(LeanLanguageUpdateDto dto);

  /// <summary>
  /// 删除语言
  /// </summary>
  /// <param name="id">语言ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteLanguageAsync(long id);

  /// <summary>
  /// 检查语言代码是否存在
  /// </summary>
  /// <param name="languageCode">语言代码</param>
  /// <param name="excludeId">排除的ID</param>
  /// <returns>是否存在</returns>
  Task<bool> CheckLanguageCodeExistAsync(string languageCode, long? excludeId = null);
}