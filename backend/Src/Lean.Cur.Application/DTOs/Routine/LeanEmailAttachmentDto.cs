// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Dtos.Routine;

/// <summary>
/// 邮件附件DTO
/// </summary>
public class LeanEmailAttachmentDto
{
  /// <summary>
  /// 附件ID
  /// </summary>
  [Description("附件ID")]
  public long Id { get; set; }

  /// <summary>
  /// 邮件ID
  /// </summary>
  [Description("邮件ID")]
  public long EmailId { get; set; }

  /// <summary>
  /// 文件名
  /// </summary>
  [Description("文件名")]
  public string FileName { get; set; } = null!;

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  [Description("文件大小")]
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  [Description("文件类型")]
  public string FileType { get; set; } = null!;

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [Description("文件扩展名")]
  public string FileExtension { get; set; } = null!;

  /// <summary>
  /// 上传时间
  /// </summary>
  [Description("上传时间")]
  public DateTime UploadTime { get; set; }
}

/// <summary>
/// 邮件附件上传DTO
/// </summary>
public class LeanEmailAttachmentUploadDto
{
  /// <summary>
  /// 文件
  /// </summary>
  [Description("文件")]
  [Required(ErrorMessage = "请选择要上传的文件")]
  public IFormFile File { get; set; } = null!;

  /// <summary>
  /// 临时邮件ID（如果是草稿箱）
  /// </summary>
  [Description("临时邮件ID")]
  public long? TempEmailId { get; set; }
}

/// <summary>
/// 邮件附件下载DTO
/// </summary>
public class LeanEmailAttachmentDownloadDto
{
  /// <summary>
  /// 文件名
  /// </summary>
  [Description("文件名")]
  public string FileName { get; set; } = null!;

  /// <summary>
  /// 文件类型
  /// </summary>
  [Description("文件类型")]
  public string FileType { get; set; } = null!;

  /// <summary>
  /// 文件路径
  /// </summary>
  [Description("文件路径")]
  public string FilePath { get; set; } = null!;
}