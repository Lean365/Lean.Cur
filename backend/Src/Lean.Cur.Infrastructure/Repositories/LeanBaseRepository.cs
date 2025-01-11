using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Repositories;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

/// <summary>
/// 基础仓储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class LeanBaseRepository<TEntity> : ILeanBaseRepository<TEntity> where TEntity : LeanBaseEntity, new()
{
  protected readonly ISqlSugarClient Db;
  protected readonly ISimpleClient<TEntity> EntityDb;

  protected LeanBaseRepository(ISqlSugarClient db)
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
  /// 创建实体
  /// </summary>
  public virtual async Task<TEntity> CreateAsync(TEntity entity)
  {
    entity.CreateTime = DateTime.Now;
    await EntityDb.InsertAsync(entity);
    return entity;
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
  /// 检查实体是否存在
  /// </summary>
  public virtual async Task<bool> ExistsAsync(long id)
  {
    return await EntityDb.IsAnyAsync(e => e.Id == id && e.IsDeleted == 0);
  }
}