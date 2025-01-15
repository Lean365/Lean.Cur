using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

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
  [SugarColumn(ColumnName = "title", ColumnDescription = "通知标题", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  public string Title { get; set; } = null!;

  /// <summary>
  /// 通知内容
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 支持富文本格式
  /// </remarks>
  [SugarColumn(ColumnName = "content", ColumnDescription = "通知内容", ColumnDataType = "nvarchar(max)", IsNullable = false)]
  public string Content { get; set; } = null!;

  /// <summary>
  /// 通知类型：1=系统通知，2=待办通知
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 默认值：1
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "type", ColumnDescription = "通知类型", ColumnDataType = "int", IsNullable = false)]
  public int Type { get; set; } = 1;

  /// <summary>
  /// 状态：true=启用，false=禁用
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 默认值：true
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "bit", IsNullable = false)]
  public bool Status { get; set; } = true;

  /// <summary>
  /// 发布时间
  /// </summary>
  /// <remarks>
  /// 1. 可选字段
  /// 2. 用于定时发布
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "publish_time", ColumnDescription = "发布时间", ColumnDataType = "datetime", IsNullable = true)]
  public DateTime? PublishTime { get; set; }

  /// <summary>
  /// 发布人
  /// </summary>
  /// <remarks>
  /// 1. 可选字段
  /// 2. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "publisher", ColumnDescription = "发布人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
  public string? Publisher { get; set; }
}