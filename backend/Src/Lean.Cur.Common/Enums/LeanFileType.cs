using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 文件类型枚举
/// </summary>
public enum LeanFileType
{
  /// <summary>
  /// 图片
  /// </summary>
  [Description("图片")]
  Image = 1,

  /// <summary>
  /// Word文档
  /// </summary>
  [Description("Word文档")]
  Word = 2,

  /// <summary>
  /// Excel表格
  /// </summary>
  [Description("Excel表格")]
  Excel = 3,

  /// <summary>
  /// PDF文档
  /// </summary>
  [Description("PDF文档")]
  PDF = 4,

  /// <summary>
  /// 文本文件
  /// </summary>
  [Description("文本文件")]
  Text = 5,

  /// <summary>
  /// 压缩包
  /// </summary>
  [Description("压缩包")]
  Archive = 6,

  /// <summary>
  /// 音频文件
  /// </summary>
  [Description("音频文件")]
  Audio = 7,

  /// <summary>
  /// 视频文件
  /// </summary>
  [Description("视频文件")]
  Video = 8,

  /// <summary>
  /// 其他类型
  /// </summary>
  [Description("其他类型")]
  Other = 9
}