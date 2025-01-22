namespace Lean.Cur.Common.Enums;

/// <summary>
/// 邮件状态
/// </summary>
public enum LeanEmailStatus
{
  /// <summary>
  /// 草稿
  /// </summary>
  Draft = 0,

  /// <summary>
  /// 待发送
  /// </summary>
  Pending = 1,

  /// <summary>
  /// 发送中
  /// </summary>
  Sending = 2,

  /// <summary>
  /// 已发送
  /// </summary>
  Sent = 3,

  /// <summary>
  /// 发送失败
  /// </summary>
  Failed = 4,

  /// <summary>
  /// 已删除
  /// </summary>
  Deleted = 5
}