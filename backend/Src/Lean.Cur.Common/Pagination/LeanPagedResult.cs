using System.Text.Json.Serialization;

namespace Lean.Cur.Common.Pagination;

/// <summary>
/// 分页结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class LeanPagedResult<T>
{
    /// <summary>
    /// 数据列表
    /// </summary>
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// 总记录数
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// 当前页码
    /// </summary>
    [JsonPropertyName("pageIndex")]
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页记录数
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    [JsonPropertyName("pageCount")]
    public int PageCount => Total > 0 ? (int)Math.Ceiling(Total / (double)PageSize) : 0;

    /// <summary>
    /// 是否有上一页
    /// </summary>
    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious => PageIndex > 1;

    /// <summary>
    /// 是否有下一页
    /// </summary>
    [JsonPropertyName("hasNext")]
    public bool HasNext => PageIndex < PageCount;

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanPagedResult()
    { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="items">数据列表</param>
    /// <param name="total">总记录数</param>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageSize">每页记录数</param>
    public LeanPagedResult(List<T> items, int total, int pageIndex, int pageSize)
    {
        Items = items;
        Total = total;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}