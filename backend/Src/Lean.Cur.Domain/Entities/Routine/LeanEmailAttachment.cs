// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 邮件附件实体
/// </summary>
/// <remarks>
/// 记录邮件的附件信息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_email_attachment
/// 2. 主键：id (自增长)
/// 3. 索引：
///    - IX_EmailId (邮件ID)
///    - IX_FileName (文件名)
/// 
/// 业务规则：
/// 1. 附件上传后立即保存
/// 2. 支持多种文件类型
/// 3. 记录文件大小和类型
/// 4. 支持文件预览和下载
/// </remarks>
[SugarTable("lean_email_attachment", "邮件附件表")]
public class LeanEmailAttachment : LeanBaseEntity
{
  /// <summary>
  /// 邮件ID
  /// </summary>
  /// <remarks>
  /// 1. 关联邮件表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "email_id", ColumnDescription = "邮件ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("邮件ID")]
  public long EmailId { get; set; }

  /// <summary>
  /// 文件名
  /// </summary>
  /// <remarks>
  /// 1. 附件的原始文件名
  /// 2. 必填字段
  /// 3. 最大长度：200个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
  [Description("文件名")]
  public string FileName { get; set; } = null!;

  /// <summary>
  /// 文件路径
  /// </summary>
  /// <remarks>
  /// 1. 附件的存储路径
  /// 2. 必填字段
  /// 3. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_path", ColumnDescription = "文件路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
  [Description("文件路径")]
  public string FilePath { get; set; } = null!;

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  /// <remarks>
  /// 1. 记录文件的大小
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "file_size", ColumnDescription = "文件大小", ColumnDataType = "bigint", IsNullable = false)]
  [Description("文件大小")]
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  /// <remarks>
  /// 1. 记录文件的MIME类型
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_type", ColumnDescription = "文件类型", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  [Description("文件类型")]
  public string FileType { get; set; } = null!;

  /// <summary>
  /// 文件扩展名
  /// </summary>
  /// <remarks>
  /// 1. 记录文件的扩展名
  /// 2. 必填字段
  /// 3. 最大长度：20个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_extension", ColumnDescription = "文件扩展名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
  [Description("文件扩展名")]
  public string FileExtension { get; set; } = null!;

  /// <summary>
  /// 上传时间
  /// </summary>
  /// <remarks>
  /// 1. 记录文件的上传时间
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "upload_time", ColumnDescription = "上传时间", ColumnDataType = "datetime", IsNullable = false)]
  [Description("上传时间")]
  public DateTime UploadTime { get; set; }

  /// <summary>
  /// 导航属性：邮件信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(EmailId))]
  public virtual LeanEmail Email { get; set; } = null!;
}