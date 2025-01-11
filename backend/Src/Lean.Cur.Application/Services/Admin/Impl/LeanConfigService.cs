using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 系统配置服务实现
/// </summary>
public class LeanConfigService : ILeanConfigService
{
  private readonly ILogger<LeanConfigService> _logger;
  private readonly ILeanBaseRepository<LeanConfig> _configRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="configRepository">系统配置仓储</param>
  public LeanConfigService(ILogger<LeanConfigService> logger, ILeanBaseRepository<LeanConfig> configRepository)
  {
    _logger = logger;
    _configRepository = configRepository;
  }

  /// <inheritdoc/>
  public async Task<List<LeanConfigDto>> GetConfigListAsync(LeanConfigQueryDto query)
  {
    var configs = await _configRepository.AsQueryable()
        .WhereIF(!string.IsNullOrEmpty(query.ConfigKey), x => x.ConfigKey.Contains(query.ConfigKey!))
        .WhereIF(!string.IsNullOrEmpty(query.ConfigGroup), x => x.ConfigGroup == query.ConfigGroup)
        .WhereIF(query.Status.HasValue, x => x.Status == query.Status)
        .WhereIF(query.StartTime.HasValue, x => x.CreateTime >= query.StartTime)
        .WhereIF(query.EndTime.HasValue, x => x.CreateTime <= query.EndTime)
        .OrderBy(x => x.OrderNum)
        .ToListAsync();

    return configs.Adapt<List<LeanConfigDto>>();
  }

  /// <inheritdoc/>
  public async Task<LeanConfigDto> GetConfigAsync(long id)
  {
    var config = await _configRepository.GetByIdAsync(id);
    if (config == null)
    {
      throw new LeanException("系统配置不存在");
    }

    return config.Adapt<LeanConfigDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateConfigAsync(LeanConfigCreateDto dto)
  {
    // 检查配置键是否存在
    var exists = await CheckConfigKeyExistAsync(dto.ConfigKey);
    if (exists)
    {
      throw new LeanException($"配置键[{dto.ConfigKey}]已存在");
    }

    var config = dto.Adapt<LeanConfig>();
    await _configRepository.InsertAsync(config);

    return config.Id;
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateConfigAsync(LeanConfigUpdateDto dto)
  {
    var config = await _configRepository.GetByIdAsync(dto.Id);
    if (config == null)
    {
      throw new LeanException("系统配置不存在");
    }

    // 检查配置键是否存在
    var exists = await CheckConfigKeyExistAsync(dto.ConfigKey, dto.Id);
    if (exists)
    {
      throw new LeanException($"配置键[{dto.ConfigKey}]已存在");
    }

    dto.Adapt(config);
    return await _configRepository.UpdateAsync(config);
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteConfigAsync(long id)
  {
    var config = await _configRepository.GetByIdAsync(id);
    if (config == null)
    {
      throw new LeanException("系统配置不存在");
    }

    // 系统内置配置不允许删除
    if (config.IsSystem == 1)
    {
      throw new LeanException("系统内置配置不允许删除");
    }

    return await _configRepository.DeleteAsync(config);
  }

  /// <inheritdoc/>
  public async Task<bool> CheckConfigKeyExistAsync(string configKey, long? excludeId = null)
  {
    return await _configRepository.AsQueryable()
        .WhereIF(excludeId.HasValue, x => x.Id != excludeId)
        .AnyAsync(x => x.ConfigKey == configKey);
  }

  /// <inheritdoc/>
  public async Task<string?> GetConfigValueByKeyAsync(string configKey)
  {
    var config = await _configRepository.AsQueryable()
        .Where(x => x.ConfigKey == configKey)
        .FirstAsync();

    return config?.ConfigValue;
  }
}