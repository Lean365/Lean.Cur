// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Routine;

/// <summary>
/// 在线用户管理相关的DTO定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了在线用户管理相关的所有DTO，包括：
/// - 基础操作相关DTO（查询、详情等）
/// - 统计分析相关DTO（在线时长、活跃度等）
/// </remarks>

#region 基础操作

/// <summary>
/// 在线用户基本信息DTO
/// </summary>
/// <remarks>
/// 用于返回在线用户的基本信息，包含以下特点：
/// 1. 包含在线用户的所有基础字段
/// 2. 所有字段都是只读的，不包含验证特性
/// 3. 用于列表展示和详情展示
/// </remarks>
public class LeanOnlineUserDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 关联用户表的主键
  /// </remarks>
  [Required(ErrorMessage = "用户ID不能为空")]
  [Description("用户ID")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 显示用户的登录名
  /// </remarks>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
  [Description("用户名")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 连接ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. SignalR连接的唯一标识
  /// </remarks>
  [Required(ErrorMessage = "连接ID不能为空")]
  [StringLength(100, ErrorMessage = "连接ID长度不能超过100个字符")]
  [Description("连接ID")]
  public string ConnectionId { get; set; } = string.Empty;

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 用户的登录IP地址
  /// </remarks>
  [Required(ErrorMessage = "IP地址不能为空")]
  [StringLength(50, ErrorMessage = "IP地址长度不能超过50个字符")]
  [Description("IP地址")]
  public string IpAddress { get; set; } = string.Empty;

  /// <summary>
  /// 浏览器
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 记录用户使用的浏览器信息
  /// </remarks>
  [StringLength(100, ErrorMessage = "浏览器信息长度不能超过100个字符")]
  [Description("浏览器")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 记录用户使用的操作系统信息
  /// </remarks>
  [StringLength(100, ErrorMessage = "操作系统信息长度不能超过100个字符")]
  [Description("操作系统")]
  public string? Os { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 格式：国家-省份-城市
  /// </remarks>
  [Required(ErrorMessage = "登录地点不能为空")]
  [StringLength(100, ErrorMessage = "登录地点长度不能超过100个字符")]
  [Description("登录地点")]
  public string Location { get; set; } = string.Empty;

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. PC、Mobile、Tablet、Other
  /// </remarks>
  [Required(ErrorMessage = "设备类型不能为空")]
  [StringLength(50, ErrorMessage = "设备类型长度不能超过50个字符")]
  [Description("设备类型")]
  public string DeviceType { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 记录具体的设备型号
  /// </remarks>
  [Required(ErrorMessage = "设备名称不能为空")]
  [StringLength(100, ErrorMessage = "设备名称长度不能超过100个字符")]
  [Description("设备名称")]
  public string DeviceName { get; set; } = string.Empty;

  /// <summary>
  /// 最后活动时间
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 记录用户最后一次活动的时间
  /// </remarks>
  [Required(ErrorMessage = "最后活动时间不能为空")]
  [Description("最后活动时间")]
  public DateTime LastActiveTime { get; set; }

  /// <summary>
  /// PC端当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 每天零点重置为0
  /// </remarks>
  [Required(ErrorMessage = "PC端当天在线时长不能为空")]
  [Description("PC端当天在线时长")]
  public int PcTodayOnlineTime { get; set; }

  /// <summary>
  /// PC端累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 持续累加,不会重置
  /// </remarks>
  [Required(ErrorMessage = "PC端累计在线时长不能为空")]
  [Description("PC端累计在线时长")]
  public int PcTotalOnlineTime { get; set; }

  /// <summary>
  /// 移动端当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 每天零点重置为0
  /// </remarks>
  [Required(ErrorMessage = "移动端当天在线时长不能为空")]
  [Description("移动端当天在线时长")]
  public int MobileTodayOnlineTime { get; set; }

  /// <summary>
  /// 移动端累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 持续累加,不会重置
  /// </remarks>
  [Required(ErrorMessage = "移动端累计在线时长不能为空")]
  [Description("移动端累计在线时长")]
  public int MobileTotalOnlineTime { get; set; }

  /// <summary>
  /// 平板端当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 每天零点重置为0
  /// </remarks>
  [Required(ErrorMessage = "平板端当天在线时长不能为空")]
  [Description("平板端当天在线时长")]
  public int TabletTodayOnlineTime { get; set; }

  /// <summary>
  /// 平板端累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 持续累加,不会重置
  /// </remarks>
  [Required(ErrorMessage = "平板端累计在线时长不能为空")]
  [Description("平板端累计在线时长")]
  public int TabletTotalOnlineTime { get; set; }

  /// <summary>
  /// 其他设备当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 每天零点重置为0
  /// </remarks>
  [Required(ErrorMessage = "其他设备当天在线时长不能为空")]
  [Description("其他设备当天在线时长")]
  public int OtherTodayOnlineTime { get; set; }

  /// <summary>
  /// 其他设备累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 持续累加,不会重置
  /// </remarks>
  [Required(ErrorMessage = "其他设备累计在线时长不能为空")]
  [Description("其他设备累计在线时长")]
  public int OtherTotalOnlineTime { get; set; }

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
/// 在线用户查询DTO
/// </summary>
/// <remarks>
/// 用于查询在线用户列表，包含以下特点：
/// 1. 继承自LeanPagedRequest，支持分页查询
/// 2. 包含多个查询条件
/// 3. 支持按时间范围查询
/// </remarks>
public class LeanOnlineUserQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 支持模糊查询
  /// </remarks>
  [StringLength(50, ErrorMessage = "用户名长度不能超过50个字符")]
  [Description("用户名")]
  public string? UserName { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 支持模糊查询
  /// </remarks>
  [StringLength(50, ErrorMessage = "IP地址长度不能超过50个字符")]
  [Description("IP地址")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. PC、Mobile、Tablet、Other
  /// </remarks>
  [StringLength(50, ErrorMessage = "设备类型长度不能超过50个字符")]
  [Description("设备类型")]
  public string? DeviceType { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 查询在线时间范围的起始时间
  /// </remarks>
  [Description("开始时间")]
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  /// <remarks>
  /// 1. 选填字段
  /// 2. 查询在线时间范围的结束时间
  /// </remarks>
  [Description("结束时间")]
  public DateTime? EndTime { get; set; }
}

#endregion