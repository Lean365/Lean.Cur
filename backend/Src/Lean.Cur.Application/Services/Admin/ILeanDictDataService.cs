using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 字典数据服务接口
/// </summary>
public interface ILeanDictDataService
{
  /// <summary>
  /// 获取字典数据列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>字典数据列表</returns>
  Task<List<LeanDictDataDto>> GetDictDataListAsync(LeanDictDataQueryDto query);

  /// <summary>
  /// 获取字典数据详情
  /// </summary>
  /// <param name="id">字典数据ID</param>
  /// <returns>字典数据详情</returns>
  Task<LeanDictDataDto> GetDictDataAsync(long id);

  /// <summary>
  /// 创建字典数据
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>字典数据ID</returns>
  Task<long> CreateDictDataAsync(LeanDictDataCreateDto dto);

  /// <summary>
  /// 更新字典数据
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateDictDataAsync(LeanDictDataUpdateDto dto);

  /// <summary>
  /// 删除字典数据
  /// </summary>
  /// <param name="id">字典数据ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteDictDataAsync(long id);

  /// <summary>
  /// 根据字典类型获取字典数据列表
  /// </summary>
  /// <param name="dictType">字典类型</param>
  /// <returns>字典数据列表</returns>
  Task<List<LeanDictDataDto>> GetDictDataListByDictTypeAsync(string dictType);
}