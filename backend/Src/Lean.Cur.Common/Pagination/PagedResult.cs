namespace Lean.Cur.Common.Pagination;

/// <summary>
/// 分页结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// 页码（从1开始）
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总记录数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 数据列表
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// 构造函数
    /// </summary>
    public PagedResult() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="items">数据列表</param>
    /// <param name="total">总记录数</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页大小</param>
    public PagedResult(List<T> items, int total, int pageIndex, int pageSize)
    {
        Items = items;
        Total = total;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
} 