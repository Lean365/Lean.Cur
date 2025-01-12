using System.Linq.Expressions;
using Lean.Cur.Domain.Entities;
using SqlSugar;

namespace Lean.Cur.Domain.Repositories;

/// <summary>
/// 通用仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface ILeanRepository<TEntity> where TEntity : LeanBaseEntity
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
  /// 根据条件获取实体列表
  /// </summary>
  Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

  /// <summary>
  /// 创建实体
  /// </summary>
  Task<TEntity> CreateAsync(TEntity entity);

  /// <summary>
  /// 批量创建实体
  /// </summary>
  Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities);

  /// <summary>
  /// 更新实体
  /// </summary>
  Task<TEntity> UpdateAsync(TEntity entity);

  /// <summary>
  /// 更新实体
  /// </summary>
  Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate);

  /// <summary>
  /// 删除实体
  /// </summary>
  Task DeleteAsync(long id);

  /// <summary>
  /// 批量删除实体
  /// </summary>
  Task DeleteRangeAsync(List<long> ids);

  /// <summary>
  /// 检查实体是否存在
  /// </summary>
  Task<bool> ExistsAsync(long id);

  /// <summary>
  /// 根据条件检查实体是否存在
  /// </summary>
  Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

  /// <summary>
  /// 获取查询对象
  /// </summary>
  ISugarQueryable<TEntity> AsQueryable();

  /// <summary>
  /// 获取总数
  /// </summary>
  Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
}