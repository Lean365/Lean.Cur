namespace Lean.Cur.Common.Enums;

/// <summary>
/// 邮件类型
/// </summary>
public enum LeanEmailType
{
  /// <summary>
  /// 系统邮件
  /// </summary>
  System = 0,

  /// <summary>
  /// 用户邮件
  /// </summary>
  User = 1,

  /// <summary>
  /// 通知邮件
  /// </summary>
  Notification = 2,

  /// <summary>
  /// 验证码邮件
  /// </summary>
  Verification = 3,

  /// <summary>
  /// 营销邮件
  /// </summary>
  Marketing = 4
}