using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 系统配置服务接口
/// </summary>
public interface ILeanConfigService
{
  /// <summary>
  /// 获取系统配置列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>系统配置列表</returns>
  Task<List<LeanConfigDto>> GetConfigListAsync(LeanConfigQueryDto query);

  /// <summary>
  /// 获取系统配置详情
  /// </summary>
  /// <param name="id">系统配置ID</param>
  /// <returns>系统配置详情</returns>
  Task<LeanConfigDto> GetConfigAsync(long id);

  /// <summary>
  /// 创建系统配置
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>系统配置ID</returns>
  Task<long> CreateConfigAsync(LeanConfigCreateDto dto);

  /// <summary>
  /// 更新系统配置
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateConfigAsync(LeanConfigUpdateDto dto);

  /// <summary>
  /// 删除系统配置
  /// </summary>
  /// <param name="id">系统配置ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteConfigAsync(long id);

  /// <summary>
  /// 检查配置键是否存在
  /// </summary>
  /// <param name="configKey">配置键</param>
  /// <param name="excludeId">排除的ID</param>
  /// <returns>是否存在</returns>
  Task<bool> CheckConfigKeyExistAsync(string configKey, long? excludeId = null);

  /// <summary>
  /// 根据配置键获取配置值
  /// </summary>
  /// <param name="configKey">配置键</param>
  /// <returns>配置值</returns>
  Task<string?> GetConfigValueByKeyAsync(string configKey);
}
