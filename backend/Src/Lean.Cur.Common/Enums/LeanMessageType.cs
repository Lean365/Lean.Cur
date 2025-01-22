using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 消息类型枚举
/// </summary>
/// <remarks>
/// 用于区分不同类型的即时通讯消息
/// </remarks>
public enum LeanMessageType
{
  /// <summary>
  /// 文本消息
  /// </summary>
  [Description("文本消息")]
  Text = 1,

  /// <summary>
  /// 图片消息
  /// </summary>
  [Description("图片消息")]
  Image = 2,

  /// <summary>
  /// 文件消息
  /// </summary>
  [Description("文件消息")]
  File = 3
}