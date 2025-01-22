namespace Lean.Cur.Common.Pagination;

/// <summary>
/// 分页请求
/// </summary>
public class LeanPagedRequest
{
  /// <summary>
  /// 页码（从1开始）
  /// </summary>
  public int PageIndex { get; set; } = 1;

  /// <summary>
  /// 每页大小
  /// </summary>
  public int PageSize { get; set; } = 10;

  /// <summary>
  /// 排序字段
  /// </summary>
  public string OrderBy { get; set; } = string.Empty;

  /// <summary>
  /// 是否降序
  /// </summary>
  public bool IsDesc { get; set; }
}