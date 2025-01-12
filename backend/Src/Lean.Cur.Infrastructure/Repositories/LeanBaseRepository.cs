using System.Linq.Expressions;
using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Repositories;
using Lean.Cur.Infrastructure.Database;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

/// <summary>
/// 基类仓储
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public class LeanLeanRepository<TEntity> : ILeanRepository<TEntity> where TEntity : LeanBaseEntity, new()
{
    protected readonly LeanDbContext DbContext;
    protected readonly ISugarQueryable<TEntity> Query;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    public LeanLeanRepository(LeanDbContext dbContext)
    {
        DbContext = dbContext;
        Query = dbContext.Db.Queryable<TEntity>();
    }

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>实体</returns>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await DbContext.Db.Queryable<TEntity>().InSingleAsync(id);
    }

    /// <summary>
    /// 获取所有实体
    /// </summary>
    /// <returns>实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync()
    {
        return await Query.ToListAsync();
    }

    /// <summary>
    /// 根据条件获取实体列表
    /// </summary>
    /// <param name="predicate">条件表达式</param>
    /// <returns>实体列表</returns>
    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>实体</returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await DbContext.Db.Insertable(entity).ExecuteCommandAsync();
        return entity;
    }

    /// <summary>
    /// 批量创建实体
    /// </summary>
    /// <param name="entities">实体列表</param>
    /// <returns>实体列表</returns>
    public virtual async Task<List<TEntity>> CreateRangeAsync(List<TEntity> entities)
    {
        await DbContext.Db.Insertable(entities).ExecuteCommandAsync();
        return entities;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>实体</returns>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdateTime = DateTime.Now;
        await DbContext.Db.Updateable(entity).ExecuteCommandAsync();
        return entity;
    }

    /// <summary>
    /// 根据条件更新实体
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="predicate">条件表达式</param>
    /// <returns>实体</returns>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
    {
        entity.UpdateTime = DateTime.Now;
        await DbContext.Db.Updateable(entity).Where(predicate).ExecuteCommandAsync();
        return entity;
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>是否成功</returns>
    public virtual async Task DeleteAsync(long id)
    {
        await DbContext.Db.Deleteable<TEntity>().In(id).ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量删除实体
    /// </summary>
    /// <param name="ids">实体ID列表</param>
    /// <returns>是否成功</returns>
    public virtual async Task DeleteRangeAsync(List<long> ids)
    {
        await DbContext.Db.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
    }

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>是否存在</returns>
    public virtual async Task<bool> ExistsAsync(long id)
    {
        return await DbContext.Db.Queryable<TEntity>().InSingleAsync(id) != null;
    }

    /// <summary>
    /// 根据条件判断是否存在
    /// </summary>
    /// <param name="predicate">条件表达式</param>
    /// <returns>是否存在</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.AnyAsync(predicate);
    }

    /// <summary>
    /// 获取查询对象
    /// </summary>
    /// <returns>查询对象</returns>
    public virtual ISugarQueryable<TEntity> AsQueryable()
    {
        return Query;
    }

    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="predicate">条件表达式</param>
    /// <returns>数量</returns>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.CountAsync(predicate);
    }
} 