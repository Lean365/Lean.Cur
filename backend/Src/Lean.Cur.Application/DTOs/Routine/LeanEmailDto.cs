// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Routine;

/// <summary>
/// 邮件列表DTO
/// </summary>
public class LeanEmailDto
{
  /// <summary>
  /// 邮件ID
  /// </summary>
  [Description("邮件ID")]
  public long Id { get; set; }

  /// <summary>
  /// 发件人ID
  /// </summary>
  [Description("发件人ID")]
  public long SenderId { get; set; }

  /// <summary>
  /// 发件人名称
  /// </summary>
  [Description("发件人名称")]
  public string SenderName { get; set; } = null!;

  /// <summary>
  /// 收件人ID
  /// </summary>
  [Description("收件人ID")]
  public long ReceiverId { get; set; }

  /// <summary>
  /// 收件人名称
  /// </summary>
  [Description("收件人名称")]
  public string ReceiverName { get; set; } = null!;

  /// <summary>
  /// 邮件主题
  /// </summary>
  [Description("邮件主题")]
  public string Subject { get; set; } = null!;

  /// <summary>
  /// 是否已读
  /// </summary>
  [Description("是否已读")]
  public bool IsRead { get; set; }

  /// <summary>
  /// 是否包含附件
  /// </summary>
  [Description("是否包含附件")]
  public bool HasAttachment { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  [Description("发送时间")]
  public DateTime SendTime { get; set; }
}

/// <summary>
/// 邮件查询DTO
/// </summary>
public class LeanEmailQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 关键词（主题/内容）
  /// </summary>
  [Description("关键词")]
  public string? Keyword { get; set; }

  /// <summary>
  /// 发件人ID
  /// </summary>
  [Description("发件人ID")]
  public long? SenderId { get; set; }

  /// <summary>
  /// 收件人ID
  /// </summary>
  [Description("收件人ID")]
  public long? ReceiverId { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  [Description("开始时间")]
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [Description("结束时间")]
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  [Description("是否已读")]
  public bool? IsRead { get; set; }

  /// <summary>
  /// 是否包含附件
  /// </summary>
  [Description("是否包含附件")]
  public bool? HasAttachment { get; set; }
}

/// <summary>
/// 邮件发送DTO
/// </summary>
public class LeanEmailSendDto
{
  /// <summary>
  /// 收件人ID列表
  /// </summary>
  [Description("收件人ID列表")]
  [Required(ErrorMessage = "收件人不能为空")]
  public List<long> ReceiverIds { get; set; } = new();

  /// <summary>
  /// 抄送人ID列表
  /// </summary>
  [Description("抄送人ID列表")]
  public List<long>? CcIds { get; set; }

  /// <summary>
  /// 密送人ID列表
  /// </summary>
  [Description("密送人ID列表")]
  public List<long>? BccIds { get; set; }

  /// <summary>
  /// 邮件主题
  /// </summary>
  [Description("邮件主题")]
  [Required(ErrorMessage = "邮件主题不能为空")]
  [MaxLength(200, ErrorMessage = "邮件主题最多200个字符")]
  public string Subject { get; set; } = null!;

  /// <summary>
  /// 邮件内容
  /// </summary>
  [Description("邮件内容")]
  [Required(ErrorMessage = "邮件内容不能为空")]
  public string Content { get; set; } = null!;

  /// <summary>
  /// 附件ID列表
  /// </summary>
  [Description("附件ID列表")]
  public List<long>? AttachmentIds { get; set; }
}

/// <summary>
/// 邮件详情DTO
/// </summary>
public class LeanEmailDetailDto : LeanEmailDto
{
  /// <summary>
  /// 邮件内容
  /// </summary>
  [Description("邮件内容")]
  public string Content { get; set; } = null!;

  /// <summary>
  /// 附件列表
  /// </summary>
  [Description("附件列表")]
  public List<LeanEmailAttachmentDto> Attachments { get; set; } = new();
}