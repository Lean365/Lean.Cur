// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 消息设备关联实体
/// </summary>
/// <remarks>
/// 记录消息的设备相关信息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_message_device
/// 2. 主键：id
/// 3. 索引：
///    - IX_MessageId (消息ID)
///    - IX_SenderId (发送者ID)
///    - IX_DeviceType (设备类型)
/// 
/// 业务规则：
/// 1. 每条消息记录发送时的设备信息
/// 2. 支持设备类型统计分析
/// 3. 支持按设备类型查询消息
/// </remarks>
[SugarTable("lean_message_device", "消息设备关联表")]
public class LeanMessageDevice : LeanBaseEntity
{
  /// <summary>
  /// 消息ID
  /// </summary>
  /// <remarks>
  /// 1. 关联消息表的主键
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "message_id", ColumnDescription = "消息ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("消息ID")]
  public long MessageId { get; set; }

  /// <summary>
  /// 发送者ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "sender_id", ColumnDescription = "发送者ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("发送者ID")]
  public long SenderId { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 1. 记录发送消息的设备类型(PC、Mobile、Tablet、Other)
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("设备类型")]
  public string DeviceType { get; set; } = null!;

  /// <summary>
  /// 设备名称
  /// </summary>
  /// <remarks>
  /// 1. 记录发送消息的设备名称
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  [Description("设备名称")]
  public string DeviceName { get; set; } = null!;

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 1. 记录发送消息时的IP地址
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("IP地址")]
  public string IpAddress { get; set; } = null!;

  /// <summary>
  /// 浏览器信息
  /// </summary>
  /// <remarks>
  /// 1. 记录发送消息时的浏览器信息
  /// 2. 可选字段
  /// 3. 最大长度：200个字符
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器信息", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
  [Description("浏览器信息")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统信息
  /// </summary>
  /// <remarks>
  /// 1. 记录发送消息时的操作系统信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统信息", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  [Description("操作系统信息")]
  public string? Os { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  /// <remarks>
  /// 1. 记录发送消息时的地理位置
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// 4. 格式：国家-省份-城市
  /// </remarks>
  [SugarColumn(ColumnName = "location", ColumnDescription = "登录地点", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  [Description("登录地点")]
  public string Location { get; set; } = null!;

  /// <summary>
  /// 导航属性：消息信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(MessageId))]
  public virtual LeanMessage Message { get; set; } = null!;

  /// <summary>
  /// 导航属性：发送者信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(SenderId))]
  public virtual LeanUser Sender { get; set; } = null!;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMessageDevice()
  {
    DeviceType = string.Empty;
    DeviceName = string.Empty;
    IpAddress = string.Empty;
    Location = string.Empty;
  }
}