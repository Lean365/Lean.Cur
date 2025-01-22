using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.Dtos.Common;

/// <summary>
/// 文件信息DTO
/// </summary>
public class LeanFileDto
{
  /// <summary>
  /// 文件ID
  /// </summary>
  [Description("文件ID")]
  public long Id { get; set; }

  /// <summary>
  /// 原始文件名
  /// </summary>
  [Description("原始文件名")]
  public string OriginalName { get; set; }

  /// <summary>
  /// 文件大小(字节)
  /// </summary>
  [Description("文件大小(字节)")]
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  [Description("文件类型")]
  public LeanFileType FileType { get; set; }

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [Description("文件扩展名")]
  public LeanFileExtension FileExtension { get; set; }

  /// <summary>
  /// 业务类型
  /// </summary>
  [Description("业务类型")]
  public LeanBusinessType BusinessType { get; set; }

  /// <summary>
  /// 业务ID
  /// </summary>
  [Description("业务ID")]
  public long? BusinessId { get; set; }

  /// <summary>
  /// 是否临时文件
  /// </summary>
  [Description("是否临时文件")]
  public bool IsTemp { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [Description("创建时间")]
  public DateTime CreateTime { get; set; }
}