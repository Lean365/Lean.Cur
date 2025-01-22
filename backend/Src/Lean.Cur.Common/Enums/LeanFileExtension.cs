using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 文件扩展名枚举
/// </summary>
public enum LeanFileExtension
{
  /// <summary>
  /// JPG图片
  /// </summary>
  [Description(".jpg")]
  JPG = 1,

  /// <summary>
  /// JPEG图片
  /// </summary>
  [Description(".jpeg")]
  JPEG = 2,

  /// <summary>
  /// PNG图片
  /// </summary>
  [Description(".png")]
  PNG = 3,

  /// <summary>
  /// GIF图片
  /// </summary>
  [Description(".gif")]
  GIF = 4,

  /// <summary>
  /// Word文档
  /// </summary>
  [Description(".doc")]
  DOC = 5,

  /// <summary>
  /// Word文档
  /// </summary>
  [Description(".docx")]
  DOCX = 6,

  /// <summary>
  /// Excel表格
  /// </summary>
  [Description(".xls")]
  XLS = 7,

  /// <summary>
  /// Excel表格
  /// </summary>
  [Description(".xlsx")]
  XLSX = 8,

  /// <summary>
  /// PDF文档
  /// </summary>
  [Description(".pdf")]
  PDF = 9,

  /// <summary>
  /// 文本文件
  /// </summary>
  [Description(".txt")]
  TXT = 10,

  /// <summary>
  /// ZIP压缩文件
  /// </summary>
  [Description(".zip")]
  ZIP = 11,

  /// <summary>
  /// RAR压缩文件
  /// </summary>
  [Description(".rar")]
  RAR = 12,

  /// <summary>
  /// MP3音频文件
  /// </summary>
  [Description(".mp3")]
  MP3 = 13,

  /// <summary>
  /// WAV音频文件
  /// </summary>
  [Description(".wav")]
  WAV = 14,

  /// <summary>
  /// MP4视频文件
  /// </summary>
  [Description(".mp4")]
  MP4 = 15,

  /// <summary>
  /// AVI视频文件
  /// </summary>
  [Description(".avi")]
  AVI = 16
}