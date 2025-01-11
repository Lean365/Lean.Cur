namespace Lean.Cur.Application.DTOs;

public class LeanNoticeDto
{
    public long Id { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 通知内容
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// 通知类型：1=系统通知，2=待办通知
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 状态：true=启用，false=禁用
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人
    /// </summary>
    public string Publisher { get; set; }

    public DateTime CreateTime { get; set; }
    public long CreateBy { get; set; }
    public DateTime? UpdateTime { get; set; }
    public long? UpdateBy { get; set; }
    public string? Remark { get; set; }
} 