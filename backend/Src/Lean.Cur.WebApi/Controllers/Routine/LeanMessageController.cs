// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Routine;

/// <summary>
/// 消息控制器
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：提供消息管理相关的API接口
/// </remarks>
[ApiController]
[Route("api/routine/message")]
[Authorize]
public class LeanMessageController : LeanBaseController
{
  private readonly ILeanMessageService _messageService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="messageService">消息服务</param>
  public LeanMessageController(ILeanMessageService messageService)
  {
    _messageService = messageService;
  }

  /// <summary>
  /// 获取消息列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>消息列表</returns>
  [HttpGet("list")]
  [ProducesResponseType(typeof(LeanApiResult<List<LeanMessageDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<List<LeanMessageDto>>> GetListAsync([FromQuery] LeanMessageQueryDto queryDto)
  {
    var result = await _messageService.GetListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取消息分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  [HttpGet("page")]
  [ProducesResponseType(typeof(LeanApiResult<LeanPagedResult<LeanMessageDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanPagedResult<LeanMessageDto>>> GetPagedListAsync([FromQuery] LeanMessageQueryDto queryDto)
  {
    var result = await _messageService.GetPagedListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取消息详情
  /// </summary>
  /// <param name="id">消息ID</param>
  /// <returns>消息详情</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(LeanApiResult<LeanMessageDto>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanMessageDto>> GetByIdAsync(long id)
  {
    var result = await _messageService.GetByIdAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>消息ID</returns>
  [HttpPost]
  [ProducesResponseType(typeof(LeanApiResult<long>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<long>> SendMessageAsync([FromBody] LeanMessageCreateDto createDto)
  {
    var result = await _messageService.SendMessageAsync(createDto);
    return Success(result);
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  /// <param name="id">消息ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> DeleteAsync(long id)
  {
    var result = await _messageService.DeleteAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 批量删除消息
  /// </summary>
  /// <param name="ids">消息ID列表</param>
  /// <returns>是否成功</returns>
  [HttpDelete("batch")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _messageService.BatchDeleteAsync(ids);
    return Success(result);
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  /// <param name="id">消息ID</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}/read")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> MarkAsReadAsync(long id)
  {
    var result = await _messageService.MarkAsReadAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 批量标记消息已读
  /// </summary>
  /// <param name="ids">消息ID列表</param>
  /// <returns>是否成功</returns>
  [HttpPut("batch-read")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> BatchMarkAsReadAsync([FromBody] List<long> ids)
  {
    var result = await _messageService.BatchMarkAsReadAsync(ids);
    return Success(result);
  }

  /// <summary>
  /// 获取未读消息数量
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>未读消息数量</returns>
  [HttpGet("unread/count/{userId}")]
  [ProducesResponseType(typeof(LeanApiResult<int>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<int>> GetUnreadCountAsync(long userId)
  {
    var result = await _messageService.GetUnreadCountAsync(userId);
    return Success(result);
  }

  /// <summary>
  /// 获取聊天历史记录
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>聊天历史记录</returns>
  [HttpGet("chat-history")]
  [ProducesResponseType(typeof(LeanApiResult<LeanPagedResult<LeanMessageDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanPagedResult<LeanMessageDto>>> GetChatHistoryAsync([FromQuery] LeanMessageQueryDto queryDto)
  {
    var result = await _messageService.GetChatHistoryAsync(queryDto);
    return Success(result);
  }

  #region 聊天记录管理

  /// <summary>
  /// 获取与指定用户的聊天记录
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  [HttpGet("chat-history/paged")]
  [ProducesResponseType(typeof(LeanApiResult<LeanPagedResult<LeanMessageDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanPagedResult<LeanMessageDto>>> GetChatHistory([FromQuery] LeanMessageQueryDto queryDto)
  {
    var result = await _messageService.GetChatHistoryAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取最近联系人列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="count">获取数量</param>
  /// <returns>最近联系人列表</returns>
  [HttpGet("recent-contacts/{userId}")]
  [ProducesResponseType(typeof(LeanApiResult<List<LeanMessageContactDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<List<LeanMessageContactDto>>> GetRecentContacts(
      [FromRoute] long userId,
      [FromQuery] int count = 20)
  {
    var result = await _messageService.GetRecentContactsAsync(userId, count);
    return Success(result);
  }

  #endregion

  #region 消息状态管理

  /// <summary>
  /// 获取未读消息列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>未读消息列表</returns>
  [HttpGet("unread/{userId}")]
  [ProducesResponseType(typeof(LeanApiResult<List<LeanMessageDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<List<LeanMessageDto>>> GetUnreadMessages([FromRoute] long userId)
  {
    var result = await _messageService.GetUnreadMessagesAsync(userId);
    return Success(result);
  }

  /// <summary>
  /// 标记与指定用户的所有消息为已读
  /// </summary>
  /// <param name="userId">当前用户ID</param>
  /// <param name="targetUserId">目标用户ID</param>
  /// <returns>是否成功</returns>
  [HttpPost("mark-all-read/{userId}/{targetUserId}")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> MarkAllAsRead(
      [FromRoute] long userId,
      [FromRoute] long targetUserId)
  {
    var result = await _messageService.MarkAllAsReadAsync(userId, targetUserId);
    return Success(result);
  }

  #endregion
}