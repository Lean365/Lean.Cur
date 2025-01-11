using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 字典类型服务接口
/// </summary>
public interface ILeanDictTypeService
{
  /// <summary>
  /// 获取字典类型列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>字典类型列表</returns>
  Task<List<LeanDictTypeDto>> GetDictTypeListAsync(LeanDictTypeQueryDto query);

  /// <summary>
  /// 获取字典类型详情
  /// </summary>
  /// <param name="id">字典类型ID</param>
  /// <returns>字典类型详情</returns>
  Task<LeanDictTypeDto> GetDictTypeAsync(long id);

  /// <summary>
  /// 创建字典类型
  /// </summary>
  /// <param name="dto">创建参数</param>
  /// <returns>字典类型ID</returns>
  Task<long> CreateDictTypeAsync(LeanDictTypeCreateDto dto);

  /// <summary>
  /// 更新字典类型
  /// </summary>
  /// <param name="dto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateDictTypeAsync(LeanDictTypeUpdateDto dto);

  /// <summary>
  /// 删除字典类型
  /// </summary>
  /// <param name="id">字典类型ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteDictTypeAsync(long id);

  /// <summary>
  /// 检查字典类型是否存在
  /// </summary>
  /// <param name="dictType">字典类型</param>
  /// <param name="excludeId">排除的ID</param>
  /// <returns>是否存在</returns>
  Task<bool> CheckDictTypeExistAsync(string dictType, long? excludeId = null);
}