using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 存储路径枚举
/// </summary>
public enum LeanStoragePath
{
  /// <summary>
  /// 邮件附件
  /// </summary>
  [Description("邮件附件")]
  EmailAttachment = 1,

  /// <summary>
  /// 用户头像
  /// </summary>
  [Description("用户头像")]
  UserAvatar = 2,

  /// <summary>
  /// 系统文档
  /// </summary>
  [Description("系统文档")]
  SystemDocument = 3,

  /// <summary>
  /// 临时文件
  /// </summary>
  [Description("临时文件")]
  Temporary = 4,

  /// <summary>
  /// 导入文件
  /// </summary>
  [Description("导入文件")]
  Import = 5,

  /// <summary>
  /// 导出文件
  /// </summary>
  [Description("导出文件")]
  Export = 6
}