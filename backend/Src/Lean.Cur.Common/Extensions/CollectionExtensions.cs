using System.Collections;
using Lean.Cur.Common.Pagination;
using SqlSugar;

namespace Lean.Cur.Common.Extensions;

/// <summary>
/// 集合扩展方法
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// 判断集合是否为空
    /// </summary>
    /// <param name="collection">集合</param>
    /// <returns>是否为空</returns>
    public static bool IsNullOrEmpty(this ICollection collection)
    {
        return collection == null || collection.Count == 0;
    }

    /// <summary>
    /// 判断集合是否为空
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="collection">集合</param>
    /// <returns>是否为空</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    /// <summary>
    /// 判断集合是否不为空
    /// </summary>
    /// <param name="collection">集合</param>
    /// <returns>是否不为空</returns>
    public static bool IsNotNullOrEmpty(this ICollection collection)
    {
        return !IsNullOrEmpty(collection);
    }

    /// <summary>
    /// 判断集合是否不为空
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="collection">集合</param>
    /// <returns>是否不为空</returns>
    public static bool IsNotNullOrEmpty<T>(this ICollection<T> collection)
    {
        return !IsNullOrEmpty(collection);
    }

    /// <summary>
    /// 遍历集合
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="collection">集合</param>
    /// <param name="action">操作</param>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        if (collection == null || action == null) return;
        foreach (var item in collection)
        {
            action(item);
        }
    }

    /// <summary>
    /// 遍历集合
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="collection">集合</param>
    /// <param name="action">操作</param>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
    {
        if (collection == null || action == null) return;
        var index = 0;
        foreach (var item in collection)
        {
            action(item, index++);
        }
    }

    /// <summary>
    /// 转换为分页列表
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="query">查询对象</param>
    /// <param name="request">分页请求</param>
    /// <returns>分页结果</returns>
    public static async Task<PagedResult<T>> ToPagedListAsync<T>(this ISugarQueryable<T> query, PagedRequest request)
    {
        var total = await query.CountAsync();
        var items = await query.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

        return new PagedResult<T>(items, total, request.PageIndex, request.PageSize);
    }
} 