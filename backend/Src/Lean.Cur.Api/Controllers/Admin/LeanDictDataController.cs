using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Application.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers.Admin;

/// <summary>
/// 字典数据控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/admin/[controller]")]
public class LeanDictDataController : ControllerBase
{
  private readonly ILogger<LeanDictDataController> _logger;
  private readonly ILeanDictDataService _dictDataService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="dictDataService">字典数据服务</param>
  public LeanDictDataController(ILogger<LeanDictDataController> logger, ILeanDictDataService dictDataService)
  {
    _logger = logger;
    _dictDataService = dictDataService;
  }

  /// <summary>
  /// 获取字典数据列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>字典数据列表</returns>
  [HttpGet("list")]
  public async Task<List<LeanDictDataDto>> GetDictDataListAsync([FromQuery] LeanDictDataQueryDto query)
  {
    return await _dictDataService.GetDictDataListAsync(query);
  }

  /// <summary>
  /// 获取字典数据详情
  /// </summary>
  /// <param name="id">字典数据ID</param>
  /// <returns>字典数据详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanDictDataDto> GetDictDataAsync(long id)
  {
    return await _dictDataService.GetDictDataAsync(id);
  }

  /// <summary>
  /// 创建字典数据
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>字典数据ID</returns>
  [HttpPost]
  public async Task<long> CreateDictDataAsync([FromBody] LeanDictDataCreateDto dto)
  {
    return await _dictDataService.CreateDictDataAsync(dto);
  }

  /// <summary>
  /// 更新字典数据
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<bool> UpdateDictDataAsync([FromBody] LeanDictDataUpdateDto dto)
  {
    return await _dictDataService.UpdateDictDataAsync(dto);
  }

  /// <summary>
  /// 删除字典数据
  /// </summary>
  /// <param name="id">字典数据ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteDictDataAsync(long id)
  {
    return await _dictDataService.DeleteDictDataAsync(id);
  }

  /// <summary>
  /// 根据字典类型获取字典数据列表
  /// </summary>
  /// <param name="dictType">字典类型</param>
  /// <returns>字典数据列表</returns>
  [HttpGet("type/{dictType}")]
  public async Task<List<LeanDictDataDto>> GetDictDataListByDictTypeAsync(string dictType)
  {
    return await _dictDataService.GetDictDataListByDictTypeAsync(dictType);
  }
}