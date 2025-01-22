// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Routine;

/// <summary>
/// 消息管理相关的DTO定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了消息管理相关的所有DTO，包括：
/// - 基础操作相关DTO（查询、创建、更新、删除等）
/// - 消息状态相关DTO（已读、未读等）
/// - 统计分析相关DTO（未读数量、最近联系人等）
/// </remarks>

#region 基础操作

/// <summary>
/// 消息基本信息DTO
/// </summary>
/// <remarks>
/// 用于返回消息的基本信息，包含以下特点：
/// 1. 包含消息的所有基础字段
/// 2. 所有字段都是只读的，不包含验证特性
/// 3. 用于列表展示和详情展示
/// </remarks>
public class LeanMessageDto
{
  /// <summary>
  /// 消息ID
  /// </summary>
  [Description("消息ID")]
  public long Id { get; set; }

  /// <summary>
  /// 发送者ID
  /// </summary>
  [Description("发送者ID")]
  public long SenderId { get; set; }

  /// <summary>
  /// 发送者名称
  /// </summary>
  [Description("发送者名称")]
  public string SenderName { get; set; } = string.Empty;

  /// <summary>
  /// 接收者ID
  /// </summary>
  [Description("接收者ID")]
  public long ReceiverId { get; set; }

  /// <summary>
  /// 接收者名称
  /// </summary>
  [Description("接收者名称")]
  public string ReceiverName { get; set; } = string.Empty;

  /// <summary>
  /// 消息内容
  /// </summary>
  [Description("消息内容")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 消息类型(1=文本,2=图片,3=文件)
  /// </summary>
  [Description("消息类型")]
  public LeanMessageType MessageType { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  [Description("是否已读")]
  public bool IsRead { get; set; }

  /// <summary>
  /// 阅读时间
  /// </summary>
  [Description("阅读时间")]
  public DateTime? ReadTime { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  [Description("发送时间")]
  public DateTime SendTime { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  [Description("设备类型")]
  public string DeviceType { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  [Description("设备名称")]
  public string DeviceName { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  [Description("IP地址")]
  public string IpAddress { get; set; } = string.Empty;

  /// <summary>
  /// 浏览器信息
  /// </summary>
  [Description("浏览器信息")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统信息
  /// </summary>
  [Description("操作系统信息")]
  public string? Os { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  [Description("登录地点")]
  public string Location { get; set; } = string.Empty;

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 消息创建DTO
/// </summary>
/// <remarks>
/// 用于创建新消息，包含以下特点：
/// 1. 包含创建消息所需的所有必要字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanMessageCreateDto
{
  /// <summary>
  /// 发送者ID
  /// </summary>
  [Description("发送者ID")]
  public long SenderId { get; set; }

  /// <summary>
  /// 发送者名称
  /// </summary>
  [Description("发送者名称")]
  public string SenderName { get; set; } = string.Empty;

  /// <summary>
  /// 接收者ID
  /// </summary>
  [Description("接收者ID")]
  public long ReceiverId { get; set; }

  /// <summary>
  /// 消息内容
  /// </summary>
  [Description("消息内容")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 消息类型
  /// </summary>
  [Description("消息类型")]
  public LeanMessageType MessageType { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  [Description("设备类型")]
  public string DeviceType { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  [Description("设备名称")]
  public string DeviceName { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  [Description("IP地址")]
  public string IpAddress { get; set; } = string.Empty;

  /// <summary>
  /// 浏览器信息
  /// </summary>
  [Description("浏览器信息")]
  public string Browser { get; set; } = string.Empty;

  /// <summary>
  /// 操作系统
  /// </summary>
  [Description("操作系统")]
  public string Os { get; set; } = string.Empty;

  /// <summary>
  /// 登录地点
  /// </summary>
  [Description("登录地点")]
  public string Location { get; set; } = string.Empty;
}

/// <summary>
/// 消息查询DTO
/// </summary>
/// <remarks>
/// 用于查询消息列表，包含以下特点：
/// 1. 继承自LeanPagedRequest，支持分页查询
/// 2. 包含多个查询条件
/// 3. 支持按时间范围查询
/// </remarks>
public class LeanMessageQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 对话用户ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 查询与指定用户的对话记录
  /// </remarks>
  [Required(ErrorMessage = "对话用户ID不能为空")]
  [Description("对话用户ID")]
  public long TargetUserId { get; set; }

  /// <summary>
  /// 消息类型
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 按消息类型筛选
  /// </remarks>
  [Description("消息类型")]
  public LeanMessageType? Type { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 按设备类型筛选
  /// </remarks>
  [Description("设备类型")]
  public string? DeviceType { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 查询消息发送时间范围的起始时间
  /// </remarks>
  [Description("开始时间")]
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 查询消息发送时间范围的结束时间
  /// </remarks>
  [Description("结束时间")]
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 最近联系人DTO
/// </summary>
/// <remarks>
/// 用于返回最近联系人列表，包含以下特点：
/// 1. 包含联系人的基本信息
/// 2. 包含最后一条消息的概要
/// 3. 包含未读消息数量
/// </remarks>
public class LeanMessageContactDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Description("用户ID")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  [Description("用户名")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 最后一条消息
  /// </summary>
  [Description("最后一条消息")]
  public string LastMessage { get; set; } = string.Empty;

  /// <summary>
  /// 最后通讯时间
  /// </summary>
  [Description("最后通讯时间")]
  public DateTime LastTime { get; set; }

  /// <summary>
  /// 未读消息数
  /// </summary>
  [Description("未读消息数")]
  public int UnreadCount { get; set; }

  /// <summary>
  /// 最后一条消息的设备类型
  /// </summary>
  [Description("最后一条消息的设备类型")]
  public string DeviceType { get; set; } = string.Empty;

  /// <summary>
  /// 最后一条消息的登录地点
  /// </summary>
  [Description("最后一条消息的登录地点")]
  public string Location { get; set; } = string.Empty;
}

#endregion