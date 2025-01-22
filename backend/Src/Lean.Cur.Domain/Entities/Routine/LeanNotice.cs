using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 通知公告表
/// </summary>
/// <remarks>
/// 记录系统的通知和公告信息
///
/// 数据库映射说明：
/// 1. 表名：lean_notice
/// 2. 主键：Id (自增长)
/// 3. 索引：
///    - IX_Title (通知标题)
///    - IX_Type (通知类型)
///    - IX_Status (状态)
///    - IX_PublishTime (发布时间)
///
/// 业务规则：
/// 1. 通知标题和内容为必填项
/// 2. 通知类型分为系统通知和待办通知
/// 3. 状态分为启用和禁用
/// 4. 支持按时间发布和撤回
/// 5. 支持附件上传和管理
/// </remarks>
[SugarTable("lean_notice", "通知公告表")]
public class LeanNotice : LeanBaseEntity
{
  /// <summary>
  /// 通知标题
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 最大长度：100个字符
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "notice_title", ColumnDescription = "通知标题", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  public string NoticeTitle { get; set; } = null!;

  /// <summary>
  /// 通知内容
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 支持富文本格式
  /// </remarks>
  [SugarColumn(ColumnName = "notice_content", ColumnDescription = "通知内容", ColumnDataType = "nvarchar(max)", IsNullable = false)]
  public string NoticeContent { get; set; } = null!;

  /// <summary>
  /// 通知类型
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 默认为系统通知
  /// </remarks>
  [SugarColumn(ColumnName = "notice_type", ColumnDescription = "通知类型", ColumnDataType = "int", DefaultValue = "1", IsNullable = false)]
  public LeanNoticeType NoticeType { get; set; } = LeanNoticeType.System;

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 默认为正常状态
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", DefaultValue = "1", IsNullable = false)]
  public LeanStatus Status { get; set; } = LeanStatus.Normal;

  /// <summary>
  /// 发布时间
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 默认为当前时间
  /// </remarks>
  [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", DefaultValue = "GETDATE()", IsNullable = false)]
  public DateTime PublishTime { get; set; } = DateTime.Now;

  /// <summary>
  /// 是否已读
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 默认为未读
  /// </remarks>
  [SugarColumn(ColumnName = "is_read", ColumnDescription = "是否已读", ColumnDataType = "bit", DefaultValue = "0", IsNullable = false)]
  public bool IsRead { get; set; }

  /// <summary>
  /// 附件文件名
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 最大长度：200个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_name", ColumnDescription = "附件文件名", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
  public string? FileName { get; set; }

  /// <summary>
  /// 附件路径
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_path", ColumnDescription = "附件路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  public string? FilePath { get; set; }

  /// <summary>
  /// 附件大小(字节)
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// </remarks>
  [SugarColumn(ColumnName = "file_size", ColumnDescription = "附件大小", ColumnDataType = "bigint", IsNullable = true)]
  public long? FileSize { get; set; }

  /// <summary>
  /// 附件类型
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "file_type", ColumnDescription = "附件类型", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
  public string? FileType { get; set; }

  /// <summary>
  /// 上传时间
  /// </summary>
  [SugarColumn(ColumnName = "upload_time", ColumnDescription = "上传时间", ColumnDataType = "datetime", IsNullable = true)]
  public DateTime? UploadTime { get; set; }

  /// <summary>
  /// 发布人
  /// </summary>
  /// <remarks>
  /// 1. 记录发布人姓名
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "publisher", ColumnDescription = "发布人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
  [Description("发布人")]
  public string? Publisher { get; set; }

  /// <summary>
  /// 导航属性：阅读记录
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanNoticeRead.NoticeId))]
  public List<LeanNoticeRead> ReadRecords { get; set; } = new();

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanNotice()
  {
    NoticeType = LeanNoticeType.System;
    Status = LeanStatus.Normal;
    PublishTime = DateTime.Now;
    IsRead = false;
  }
}