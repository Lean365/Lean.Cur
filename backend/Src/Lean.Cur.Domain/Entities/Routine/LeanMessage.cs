using System;
using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 消息实体
/// </summary>
/// <remarks>
/// 记录用户之间的即时通讯消息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_message
/// 2. 主键：id (自增长)
/// 3. 索引：
///    - IX_SenderId (发送者ID)
///    - IX_ReceiverId (接收者ID)
///    - IX_SendTime (发送时间)
///    - IX_IsRead (是否已读)
/// 
/// 业务规则：
/// 1. 消息发送后立即保存
/// 2. 支持多种消息类型(文本、图片、文件)
/// 3. 记录消息的读取状态
/// 4. 保留历史记录，不做物理删除
/// </remarks>
[SugarTable("lean_message", "即时通讯消息表")]
public class LeanMessage : LeanBaseEntity
{
  /// <summary>
  /// 消息ID
  /// </summary>
  [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
  public new long Id { get; set; }

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
  /// 消息内容
  /// </summary>
  /// <remarks>
  /// 1. 消息的具体内容
  /// 2. 必填字段
  /// 3. 最大长度：1000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "content", ColumnDescription = "消息内容", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
  [Description("消息内容")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 消息类型
  /// </summary>
  /// <remarks>
  /// 1. 消息的类型：1=文本,2=图片,3=文件
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "type", ColumnDescription = "消息类型", ColumnDataType = "int", IsNullable = false)]
  [Description("消息类型")]
  public LeanMessageType MessageType { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  /// <remarks>
  /// 1. 标记消息是否已被接收者阅读
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
  /// 1. 记录消息被阅读的时间
  /// 2. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "read_time", ColumnDescription = "阅读时间", ColumnDataType = "datetime", IsNullable = true)]
  [Description("阅读时间")]
  public DateTime? ReadTime { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  /// <remarks>
  /// 1. 记录消息发送的时间
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "send_time", ColumnDescription = "发送时间", ColumnDataType = "datetime", IsNullable = false)]
  [Description("发送时间")]
  public DateTime SendTime { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("设备类型")]
  public string DeviceType { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  [Description("设备名称")]
  public string DeviceName { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("IP地址")]
  public string IpAddress { get; set; } = string.Empty;

  /// <summary>
  /// 浏览器信息
  /// </summary>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器信息", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
  [Description("浏览器信息")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统信息
  /// </summary>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统信息", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
  [Description("操作系统信息")]
  public string? Os { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  [SugarColumn(ColumnName = "location", ColumnDescription = "登录地点", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  [Description("登录地点")]
  public string Location { get; set; } = string.Empty;

  /// <summary>
  /// 创建时间
  /// </summary>
  public new DateTime CreateTime { get; set; } = DateTime.Now;

  /// <summary>
  /// 更新时间
  /// </summary>
  public new DateTime UpdateTime { get; set; } = DateTime.Now;
}