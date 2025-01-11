using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Application.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers.Admin;

/// <summary>
/// 系统配置控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/admin/[controller]")]
public class LeanConfigController : ControllerBase
{
  private readonly ILogger<LeanConfigController> _logger;
  private readonly ILeanConfigService _configService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="configService">系统配置服务</param>
  public LeanConfigController(ILogger<LeanConfigController> logger, ILeanConfigService configService)
  {
    _logger = logger;
    _configService = configService;
  }

  /// <summary>
  /// 获取系统配置列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>系统配置列表</returns>
  [HttpGet("list")]
  public async Task<List<LeanConfigDto>> GetConfigListAsync([FromQuery] LeanConfigQueryDto query)
  {
    return await _configService.GetConfigListAsync(query);
  }

  /// <summary>
  /// 获取系统配置详情
  /// </summary>
  /// <param name="id">系统配置ID</param>
  /// <returns>系统配置详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanConfigDto> GetConfigAsync(long id)
  {
    return await _configService.GetConfigAsync(id);
  }

  /// <summary>
  /// 创建系统配置
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>系统配置ID</returns>
  [HttpPost]
  public async Task<long> CreateConfigAsync([FromBody] LeanConfigCreateDto dto)
  {
    return await _configService.CreateConfigAsync(dto);
  }

  /// <summary>
  /// 更新系统配置
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<bool> UpdateConfigAsync([FromBody] LeanConfigUpdateDto dto)
  {
    return await _configService.UpdateConfigAsync(dto);
  }

  /// <summary>
  /// 删除系统配置
  /// </summary>
  /// <param name="id">系统配置ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteConfigAsync(long id)
  {
    return await _configService.DeleteConfigAsync(id);
  }

  /// <summary>
  /// 根据配置键获取配置值
  /// </summary>
  /// <param name="configKey">配置键</param>
  /// <returns>配置值</returns>
  [HttpGet("value/{configKey}")]
  public async Task<string?> GetConfigValueByKeyAsync(string configKey)
  {
    return await _configService.GetConfigValueByKeyAsync(configKey);
  }
}