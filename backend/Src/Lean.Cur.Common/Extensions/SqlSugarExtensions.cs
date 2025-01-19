using SqlSugar;

namespace Lean.Cur.Common.Extensions;

/// <summary>
/// SqlSugar 扩展方法
/// </summary>
public static class SqlSugarExtensions
{
  /// <summary>
  /// 执行命令并返回是否有变更
  /// </summary>
  /// <typeparam name="T">实体类型</typeparam>
  /// <param name="insertable">可插入对象</param>
  /// <returns>是否有变更</returns>
  public static async Task<bool> ExecuteCommandHasChangeAsync<T>(this IInsertable<T> insertable) where T : class, new()
  {
    return await insertable.ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 执行命令并返回是否有变更
  /// </summary>
  /// <typeparam name="T">实体类型</typeparam>
  /// <param name="updateable">可更新对象</param>
  /// <returns>是否有变更</returns>
  public static async Task<bool> ExecuteCommandHasChangeAsync<T>(this IUpdateable<T> updateable) where T : class, new()
  {
    return await updateable.ExecuteCommandAsync() > 0;
  }

  /// <summary>
  /// 执行命令并返回是否有变更
  /// </summary>
  /// <typeparam name="T">实体类型</typeparam>
  /// <param name="deleteable">可删除对象</param>
  /// <returns>是否有变更</returns>
  public static async Task<bool> ExecuteCommandHasChangeAsync<T>(this IDeleteable<T> deleteable) where T : class, new()
  {
    return await deleteable.ExecuteCommandAsync() > 0;
  }
}