// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// 消息服务接口
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本接口定义了消息相关的所有服务方法，包括：
/// 1. 基础操作（查询、发送、删除等）
/// 2. 聊天记录管理（历史记录、最近联系人等）
/// 3. 消息状态管理（已读、未读等）
/// </remarks>
public interface ILeanMessageService
{
  #region 基础操作

  /// <summary>
  /// 获取消息分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  Task<LeanPagedResult<LeanMessageDto>> GetPagedListAsync(LeanMessageQueryDto queryDto);

  /// <summary>
  /// 获取消息列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>消息列表</returns>
  Task<List<LeanMessageDto>> GetListAsync(LeanMessageQueryDto queryDto);

  /// <summary>
  /// 获取消息详情
  /// </summary>
  /// <param name="id">消息ID</param>
  /// <returns>消息详情</returns>
  Task<LeanMessageDto> GetByIdAsync(long id);

  /// <summary>
  /// 发送消息
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>消息ID</returns>
  /// <remarks>
  /// 1. 消息发送后立即保存
  /// 2. 支持多种消息类型(文本、图片、文件)
  /// 3. 记录发送时的设备信息
  /// </remarks>
  Task<long> SendMessageAsync(LeanMessageCreateDto createDto);

  /// <summary>
  /// 删除消息
  /// </summary>
  /// <param name="id">消息ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 批量删除消息
  /// </summary>
  /// <param name="ids">消息ID列表</param>
  /// <returns>是否成功</returns>
  Task<bool> BatchDeleteAsync(List<long> ids);

  #endregion

  #region 聊天记录管理

  /// <summary>
  /// 获取与指定用户的聊天记录
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  /// <remarks>
  /// 查询条件中必须包含:
  /// 1. 当前用户ID
  /// 2. 目标用户ID
  /// 3. 分页参数
  /// </remarks>
  Task<LeanPagedResult<LeanMessageDto>> GetChatHistoryAsync(LeanMessageQueryDto queryDto);

  /// <summary>
  /// 获取最近联系人列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="count">获取数量</param>
  /// <returns>最近联系人列表</returns>
  /// <remarks>
  /// 返回结果包含:
  /// 1. 联系人基本信息
  /// 2. 最后一条消息
  /// 3. 未读消息数量
  /// </remarks>
  Task<List<LeanMessageContactDto>> GetRecentContactsAsync(long userId, int count = 20);

  #endregion

  #region 消息状态管理

  /// <summary>
  /// 获取未读消息列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>未读消息列表</returns>
  Task<List<LeanMessageDto>> GetUnreadMessagesAsync(long userId);

  /// <summary>
  /// 获取未读消息数量
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>未读消息数量</returns>
  Task<int> GetUnreadCountAsync(long userId);

  /// <summary>
  /// 标记消息为已读
  /// </summary>
  /// <param name="messageId">消息ID</param>
  /// <returns>是否成功</returns>
  Task<bool> MarkAsReadAsync(long messageId);

  /// <summary>
  /// 批量标记消息为已读
  /// </summary>
  /// <param name="messageIds">消息ID列表</param>
  /// <returns>是否成功</returns>
  Task<bool> BatchMarkAsReadAsync(List<long> messageIds);

  /// <summary>
  /// 标记与指定用户的所有消息为已读
  /// </summary>
  /// <param name="userId">当前用户ID</param>
  /// <param name="targetUserId">目标用户ID</param>
  /// <returns>是否成功</returns>
  Task<bool> MarkAllAsReadAsync(long userId, long targetUserId);

  #endregion
}