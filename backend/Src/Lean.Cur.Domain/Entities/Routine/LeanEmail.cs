// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;
using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 邮件实体
/// </summary>
/// <remarks>
/// 记录系统邮件的发送和接收信息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_email
/// 2. 主键：id (自增长)
/// 3. 索引：
///    - IX_SenderId (发送者ID)
///    - IX_ReceiverId (接收者ID)
///    - IX_SendTime (发送时间)
///    - IX_Status (状态)
/// 
/// 业务规则：
/// 1. 邮件发送后立即保存
/// 2. 支持多种邮件类型(普通、紧急、密送等)
/// 3. 记录邮件的读取状态
/// 4. 支持附件管理
/// 5. 保留历史记录，不做物理删除
/// </remarks>
[SugarTable("lean_email", "邮件表")]
public class LeanEmail : LeanBaseEntity
{
  /// <summary>
  /// 发送者ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "sender_id", ColumnDescription = "发送者ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("发送者ID")]
  public long SenderId { get; set; }

  /// <summary>
  /// 发送者名称
  /// </summary>
  /// <remarks>
  /// 1. 记录发送时的用户名称
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "sender_name", ColumnDescription = "发送者名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("发送者名称")]
  public string SenderName { get; set; } = null!;

  /// <summary>
  /// 接收者ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "receiver_id", ColumnDescription = "接收者ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("接收者ID")]
  public long ReceiverId { get; set; }

  /// <summary>
  /// 接收者名称
  /// </summary>
  /// <remarks>
  /// 1. 记录发送时的接收者名称
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "receiver_name", ColumnDescription = "接收者名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("接收者名称")]
  public string ReceiverName { get; set; } = null!;

  /// <summary>
  /// 抄送人ID列表
  /// </summary>
  /// <remarks>
  /// 1. 记录抄送人ID列表，以逗号分隔
  /// 2. 可选字段
  /// 3. 最大长度：1000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "cc_ids", ColumnDescription = "抄送人ID列表", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
  [Description("抄送人ID列表")]
  public string? CcIds { get; set; }

  /// <summary>
  /// 抄送人名称列表
  /// </summary>
  /// <remarks>
  /// 1. 记录抄送人名称列表，以逗号分隔
  /// 2. 可选字段
  /// 3. 最大长度：1000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "cc_names", ColumnDescription = "抄送人名称列表", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
  [Description("抄送人名称列表")]
  public string? CcNames { get; set; }

  /// <summary>
  /// 密送人ID列表
  /// </summary>
  /// <remarks>
  /// 1. 记录密送人ID列表，以逗号分隔
  /// 2. 可选字段
  /// 3. 最大长度：1000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "bcc_ids", ColumnDescription = "密送人ID列表", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
  [Description("密送人ID列表")]
  public string? BccIds { get; set; }

  /// <summary>
  /// 密送人名称列表
  /// </summary>
  /// <remarks>
  /// 1. 记录密送人名称列表，以逗号分隔
  /// 2. 可选字段
  /// 3. 最大长度：1000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "bcc_names", ColumnDescription = "密送人名称列表", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
  [Description("密送人名称列表")]
  public string? BccNames { get; set; }

  /// <summary>
  /// 邮件主题
  /// </summary>
  /// <remarks>
  /// 1. 邮件的标题
  /// 2. 必填字段
  /// 3. 最大长度：200个字符
  /// </remarks>
  [SugarColumn(ColumnName = "subject", ColumnDescription = "邮件主题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
  [Description("邮件主题")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// 邮件内容
  /// </summary>
  /// <remarks>
  /// 1. 邮件的具体内容
  /// 2. 必填字段
  /// 3. 支持HTML格式
  /// </remarks>
  [SugarColumn(ColumnName = "content", ColumnDescription = "邮件内容", ColumnDataType = "nvarchar(max)", IsNullable = false)]
  [Description("邮件内容")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 邮件类型
  /// </summary>
  /// <remarks>
  /// 1. 邮件的类型：1=普通,2=紧急,3=密送
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "type", ColumnDescription = "邮件类型", ColumnDataType = "int", IsNullable = false)]
  [Description("邮件类型")]
  public LeanEmailType EmailType { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  /// <remarks>
  /// 1. 标记邮件是否已被接收者阅读
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "is_read", ColumnDescription = "是否已读", ColumnDataType = "bit", IsNullable = false)]
  [Description("是否已读")]
  public bool IsRead { get; set; }

  /// <summary>
  /// 阅读时间
  /// </summary>
  /// <remarks>
  /// 1. 记录邮件被阅读的时间
  /// 2. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "read_time", ColumnDescription = "阅读时间", ColumnDataType = "datetime", IsNullable = true)]
  [Description("阅读时间")]
  public DateTime? ReadTime { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  /// <remarks>
  /// 1. 记录邮件发送的时间
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "send_time", ColumnDescription = "发送时间", ColumnDataType = "datetime", IsNullable = false)]
  [Description("发送时间")]
  public DateTime SendTime { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 1. 邮件状态：1=草稿,2=已发送,3=已撤回,4=已删除
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  [Description("状态")]
  public LeanEmailStatus Status { get; set; }

  /// <summary>
  /// 是否有附件
  /// </summary>
  [SugarColumn(ColumnName = "has_attachment", ColumnDescription = "是否有附件", ColumnDataType = "bit", IsNullable = false)]
  [Description("是否有附件")]
  public bool HasAttachment { get; set; }

  /// <summary>
  /// 导航属性：发送者信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(SenderId))]
  public virtual LeanUser Sender { get; set; } = null!;

  /// <summary>
  /// 导航属性：接收者信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(ReceiverId))]
  public virtual LeanUser Receiver { get; set; } = null!;

  /// <summary>
  /// 导航属性：附件列表
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanEmailAttachment.EmailId))]
  public virtual List<LeanEmailAttachment>? Attachments { get; set; }
}