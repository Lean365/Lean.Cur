using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Repositories;
using SqlSugar;
using System.Linq.Expressions;

namespace Lean.Cur.Infrastructure.Repositories;

/// <summary>
/// 仓储基类
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class LeanRepository<TEntity> : ILeanRepository<TEntity> where TEntity : LeanBaseEntity, new()
{
  protected readonly SqlSugarClient Db;
  protected readonly ISimpleClient<TEntity> EntityDb;

  /// <summary>
  /// 构造函数
  /// </summary>
  protected LeanRepository(SqlSugarClient db)
  {
    Db = db;
    EntityDb = db.GetSimpleClient<TEntity>();
  }

  /// <summary>
  /// 根据ID获取实体
  /// </summary>
  public virtual async Task<TEntity?> GetByIdAsync(long id)
  {
    return await EntityDb.GetByIdAsync(id);
  }

  /// <summary>
  /// 获取所有实体列表
  /// </summary>
  public virtual async Task<List<TEntity>> GetListAsync()
  {
    return await EntityDb.AsQueryable()
        .Where(e => e.IsDeleted == 0)
        .ToListAsync();
  }

  /// <summary>
  /// 根据条件获取实体列表
  /// </summary>
  public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await EntityDb.AsQueryable()
        .Where(predicate)
        .Where(e => e.IsDeleted == 0)
        .ToListAsync();
  }

  /// <summary>
  /// 创建实体
  /// </summary>
  public virtual async Task<TEntity> CreateAsync(TEntity entity)
  {
    entity.CreateTime = DateTime.Now;
    await EntityDb.InsertAsync(entity);
    return entity;
  }

  /// <summary>
  /// 批量创建实体
  /// </summary>
  public virtual async Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities)
  {
    var now = DateTime.Now;
    foreach (var entity in entities)
    {
      entity.CreateTime = now;
    }
    await EntityDb.InsertRangeAsync(entities);
    return entities;
  }

  /// <summary>
  /// 更新实体
  /// </summary>
  public virtual async Task<TEntity> UpdateAsync(TEntity entity)
  {
    entity.UpdateTime = DateTime.Now;
    await EntityDb.UpdateAsync(entity);
    return entity;
  }

  /// <summary>
  /// 更新实体
  /// </summary>
  public virtual async Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
  {
    entity.UpdateTime = DateTime.Now;
    await EntityDb.AsUpdateable(entity)
        .Where(predicate)
        .ExecuteCommandAsync();
    return entity;
  }

  /// <summary>
  /// 删除实体
  /// </summary>
  public virtual async Task DeleteAsync(long id)
  {
    var entity = await GetByIdAsync(id);
    if (entity != null)
    {
      entity.IsDeleted = 1;
      entity.UpdateTime = DateTime.Now;
      await EntityDb.UpdateAsync(entity);
    }
  }

  /// <summary>
  /// 批量删除实体
  /// </summary>
  public virtual async Task DeleteRangeAsync(List<long> ids)
  {
    var now = DateTime.Now;
    await EntityDb.AsUpdateable()
        .SetColumns(it => new TEntity { IsDeleted = 1, UpdateTime = now })
        .Where(it => ids.Contains(it.Id))
        .ExecuteCommandAsync();
  }

  /// <summary>
  /// 检查实体是否存在
  /// </summary>
  public virtual async Task<bool> ExistsAsync(long id)
  {
    return await EntityDb.IsAnyAsync(e => e.Id == id && e.IsDeleted == 0);
  }

  /// <summary>
  /// 根据条件检查实体是否存在
  /// </summary>
  public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await EntityDb.AsQueryable()
        .Where(predicate)
        .Where(e => e.IsDeleted == 0)
        .AnyAsync();
  }

  /// <summary>
  /// 获取查询对象
  /// </summary>
  public virtual ISugarQueryable<TEntity> AsQueryable()
  {
    return EntityDb.AsQueryable()
        .Where(e => e.IsDeleted == 0);
  }

  /// <summary>
  /// 获取总数
  /// </summary>
  public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await EntityDb.AsQueryable()
        .Where(predicate)
        .Where(e => e.IsDeleted == 0)
        .CountAsync();
  }
}