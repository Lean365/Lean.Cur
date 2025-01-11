using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Application.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers.Admin;

/// <summary>
/// 字典类型控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/admin/[controller]")]
public class LeanDictTypeController : ControllerBase
{
  private readonly ILogger<LeanDictTypeController> _logger;
  private readonly ILeanDictTypeService _dictTypeService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="dictTypeService">字典类型服务</param>
  public LeanDictTypeController(ILogger<LeanDictTypeController> logger, ILeanDictTypeService dictTypeService)
  {
    _logger = logger;
    _dictTypeService = dictTypeService;
  }

  /// <summary>
  /// 获取字典类型列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>字典类型列表</returns>
  [HttpGet("list")]
  public async Task<List<LeanDictTypeDto>> GetDictTypeListAsync([FromQuery] LeanDictTypeQueryDto query)
  {
    return await _dictTypeService.GetDictTypeListAsync(query);
  }

  /// <summary>
  /// 获取字典类型详情
  /// </summary>
  /// <param name="id">字典类型ID</param>
  /// <returns>字典类型详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanDictTypeDto> GetDictTypeAsync(long id)
  {
    return await _dictTypeService.GetDictTypeAsync(id);
  }

  /// <summary>
  /// 创建字典类型
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>字典类型ID</returns>
  [HttpPost]
  public async Task<long> CreateDictTypeAsync([FromBody] LeanDictTypeCreateDto dto)
  {
    return await _dictTypeService.CreateDictTypeAsync(dto);
  }

  /// <summary>
  /// 更新字典类型
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<bool> UpdateDictTypeAsync([FromBody] LeanDictTypeUpdateDto dto)
  {
    return await _dictTypeService.UpdateDictTypeAsync(dto);
  }

  /// <summary>
  /// 删除字典类型
  /// </summary>
  /// <param name="id">字典类型ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteDictTypeAsync(long id)
  {
    return await _dictTypeService.DeleteDictTypeAsync(id);
  }
}