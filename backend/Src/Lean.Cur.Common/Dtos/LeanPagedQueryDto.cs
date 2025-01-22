namespace Lean.Cur.Common.Dtos;

/// <summary>
/// 分页查询DTO
/// </summary>
public class PagedQueryDto
{
  /// <summary>
  /// 页码（从1开始）
  /// </summary>
  public int PageIndex { get; set; } = 1;

  /// <summary>
  /// 每页记录数
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

  /// <summary>
  /// 关键字
  /// </summary>
  public string Keyword { get; set; } = string.Empty;
}