namespace Lean.Cur.Common.Excel;

/// <summary>
/// Excel导入结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class LeanExcelResult<T> where T : class
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 数据列表
    /// </summary>
    public List<T>? Data { get; set; }

    /// <summary>
    /// 错误行信息
    /// </summary>
    public List<LeanExcelErrorRow>? ErrorRows { get; set; }
}

/// <summary>
/// Excel错误行信息
/// </summary>
public class LeanExcelErrorRow
{
    /// <summary>
    /// 行号
    /// </summary>
    public int RowIndex { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
} 