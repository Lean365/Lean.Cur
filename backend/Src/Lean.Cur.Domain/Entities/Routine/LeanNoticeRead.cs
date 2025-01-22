using System.ComponentModel;
using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 通知公告阅读记录
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：用于记录用户对通知公告的阅读状态
/// </remarks>
public class LeanNoticeRead : LeanBaseEntity
{
  /// <summary>
  /// 通知公告ID
  /// </summary>
  [Description("通知公告ID")]
  public long NoticeId { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  [Description("用户ID")]
  public long UserId { get; set; }

  /// <summary>
  /// 阅读时间
  /// </summary>
  [Description("阅读时间")]
  public DateTime ReadTime { get; set; }

  /// <summary>
  /// 导航属性：通知公告
  /// </summary>
  public virtual LeanNotice Notice { get; set; } = null!;

  /// <summary>
  /// 导航属性：用户
  /// </summary>
  public virtual LeanUser User { get; set; } = null!;
}