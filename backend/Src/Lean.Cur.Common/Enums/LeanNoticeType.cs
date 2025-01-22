using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 通知类型枚举
/// </summary>
/// <remarks>
/// 用于区分不同类型的通知公告
/// </remarks>
public enum LeanNoticeType
{
  /// <summary>
  /// 系统通知
  /// </summary>
  [Description("系统通知")]
  System = 1,

  /// <summary>
  /// 待办通知
  /// </summary>
  [Description("待办通知")]
  Todo = 2
}