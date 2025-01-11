using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Application.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Api.Controllers.Admin;

/// <summary>
/// 翻译控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/admin/[controller]")]
public class LeanTranslationController : ControllerBase
{
  private readonly ILogger<LeanTranslationController> _logger;
  private readonly ILeanTranslationService _translationService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="translationService">翻译服务</param>
  public LeanTranslationController(
      ILogger<LeanTranslationController> logger,
      ILeanTranslationService translationService)
  {
    _logger = logger;
    _translationService = translationService;
  }

  /// <summary>
  /// 获取翻译列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>翻译列表</returns>
  [HttpGet("list")]
  public async Task<List<LeanTranslationDto>> GetTranslationListAsync([FromQuery] LeanTranslationQueryDto query)
  {
    return await _translationService.GetTranslationListAsync(query);
  }

  /// <summary>
  /// 获取翻译详情
  /// </summary>
  /// <param name="id">翻译ID</param>
  /// <returns>翻译详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanTranslationDto> GetTranslationAsync(long id)
  {
    return await _translationService.GetTranslationAsync(id);
  }

  /// <summary>
  /// 创建翻译
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>翻译ID</returns>
  [HttpPost]
  public async Task<long> CreateTranslationAsync([FromBody] LeanTranslationCreateDto dto)
  {
    return await _translationService.CreateTranslationAsync(dto);
  }

  /// <summary>
  /// 更新翻译
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<bool> UpdateTranslationAsync([FromBody] LeanTranslationUpdateDto dto)
  {
    return await _translationService.UpdateTranslationAsync(dto);
  }

  /// <summary>
  /// 删除翻译
  /// </summary>
  /// <param name="id">翻译ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteTranslationAsync(long id)
  {
    return await _translationService.DeleteTranslationAsync(id);
  }

  /// <summary>
  /// 获取翻译值
  /// </summary>
  /// <param name="languageCode">语言代码</param>
  /// <param name="transKey">翻译键</param>
  /// <returns>翻译值</returns>
  [HttpGet("value")]
  public async Task<string?> GetTranslationValueAsync([FromQuery] string languageCode, [FromQuery] string transKey)
  {
    return await _translationService.GetTranslationValueAsync(languageCode, transKey);
  }

  /// <summary>
  /// 获取翻译字典
  /// </summary>
  /// <param name="languageCode">语言代码</param>
  /// <returns>翻译字典</returns>
  [HttpGet("dictionary")]
  public async Task<Dictionary<string, string>> GetTranslationDictionaryAsync([FromQuery] string languageCode)
  {
    return await _translationService.GetTranslationDictionaryAsync(languageCode);
  }
}