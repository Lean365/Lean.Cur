using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 部门服务接口
/// </summary>
public interface ILeanDeptService
{
  /// <summary>
  /// 获取部门列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>部门列表</returns>
  Task<List<LeanDeptDto>> GetDeptListAsync(LeanDeptQueryDto query);

  /// <summary>
  /// 获取部门树形结构
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>部门树形结构</returns>
  Task<List<LeanDeptDto>> GetDeptTreeAsync(LeanDeptQueryDto query);

  /// <summary>
  /// 获取部门详情
  /// </summary>
  /// <param name="id">部门ID</param>
  /// <returns>部门详情</returns>
  Task<LeanDeptDto?> GetDeptAsync(long id);

  /// <summary>
  /// 创建部门
  /// </summary>
  /// <param name="dto">部门创建信息</param>
  /// <returns>部门ID</returns>
  Task<long> CreateDeptAsync(LeanDeptCreateDto dto);

  /// <summary>
  /// 更新部门
  /// </summary>
  /// <param name="dto">部门更新信息</param>
  Task UpdateDeptAsync(LeanDeptUpdateDto dto);

  /// <summary>
  /// 删除部门
  /// </summary>
  /// <param name="id">部门ID</param>
  Task DeleteDeptAsync(long id);

  /// <summary>
  /// 检查部门名称是否存在
  /// </summary>
  /// <param name="deptName">部门名称</param>
  /// <param name="excludeId">排除的部门ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckDeptNameExistAsync(string deptName, long? excludeId = null);

  /// <summary>
  /// 获取部门及其所有子部门ID列表
  /// </summary>
  /// <param name="deptId">部门ID</param>
  /// <returns>部门ID列表</returns>
  Task<List<long>> GetDeptAndChildrenIdsAsync(long deptId);
}