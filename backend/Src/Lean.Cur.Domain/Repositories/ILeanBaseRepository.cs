using Lean.Cur.Domain.Entities;

namespace Lean.Cur.Domain.Repositories;

/// <summary>
/// 基础仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface ILeanBaseRepository<TEntity> where TEntity : LeanBaseEntity
{
  /// <summary>
  /// 根据ID获取实体
  /// </summary>
  Task<TEntity?> GetByIdAsync(long id);

  /// <summary>
  /// 获取所有实体列表
  /// </summary>
  Task<List<TEntity>> GetListAsync();

  /// <summary>
  /// 创建实体
  /// </summary>
  Task<TEntity> CreateAsync(TEntity entity);

  /// <summary>
  /// 更新实体
  /// </summary>
  Task<TEntity> UpdateAsync(TEntity entity);

  /// <summary>
  /// 删除实体
  /// </summary>
  Task DeleteAsync(long id);

  /// <summary>
  /// 检查实体是否存在
  /// </summary>
  Task<bool> ExistsAsync(long id);
}