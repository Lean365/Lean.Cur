using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Application.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Api.Controllers.Admin;

/// <summary>
/// 语言控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/admin/[controller]")]
public class LeanLanguageController : ControllerBase
{
  private readonly ILogger<LeanLanguageController> _logger;
  private readonly ILeanLanguageService _languageService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="languageService">语言服务</param>
  public LeanLanguageController(
      ILogger<LeanLanguageController> logger,
      ILeanLanguageService languageService)
  {
    _logger = logger;
    _languageService = languageService;
  }

  /// <summary>
  /// 获取语言列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>语言列表</returns>
  [HttpGet("list")]
  public async Task<List<LeanLanguageDto>> GetLanguageListAsync([FromQuery] LeanLanguageQueryDto query)
  {
    return await _languageService.GetLanguageListAsync(query);
  }

  /// <summary>
  /// 获取语言详情
  /// </summary>
  /// <param name="id">语言ID</param>
  /// <returns>语言详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanLanguageDto> GetLanguageAsync(long id)
  {
    return await _languageService.GetLanguageAsync(id);
  }

  /// <summary>
  /// 创建语言
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>语言ID</returns>
  [HttpPost]
  public async Task<long> CreateLanguageAsync([FromBody] LeanLanguageCreateDto dto)
  {
    return await _languageService.CreateLanguageAsync(dto);
  }

  /// <summary>
  /// 更新语言
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<bool> UpdateLanguageAsync([FromBody] LeanLanguageUpdateDto dto)
  {
    return await _languageService.UpdateLanguageAsync(dto);
  }

  /// <summary>
  /// 删除语言
  /// </summary>
  /// <param name="id">语言ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteLanguageAsync(long id)
  {
    return await _languageService.DeleteLanguageAsync(id);
  }
}