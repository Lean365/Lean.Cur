// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Entities.Routine;
using SqlSugar;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Lean.Cur.Infrastructure.Hubs;
using Mapster;

namespace Lean.Cur.Infrastructure.Services.Routine;

/// <summary>
/// 消息服务实现类
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：实现消息相关的所有服务方法
/// </remarks>
public class LeanMessageService : ILeanMessageService
{
  private readonly SqlSugarClient _db;
  private readonly IHubContext<LeanMessageHub> _messageHub;
  private readonly ILeanOnlineService _onlineService;
  private readonly ILeanNoticePushService _pushService;

  public LeanMessageService(
    SqlSugarClient db,
    IHubContext<LeanMessageHub> messageHub,
    ILeanOnlineService onlineService,
    ILeanNoticePushService pushService)
  {
    _db = db;
    _messageHub = messageHub;
    _onlineService = onlineService;
    _pushService = pushService;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanMessageDto>> GetPagedListAsync(LeanMessageQueryDto queryDto)
  {
    var query = _db.Queryable<LeanMessage>();

    // 应用查询条件
    if (queryDto.TargetUserId > 0)
    {
      query = query.Where(x => x.SenderId == queryDto.TargetUserId || x.ReceiverId == queryDto.TargetUserId);
    }
    if (queryDto.Type.HasValue)
    {
      query = query.Where(x => x.MessageType == queryDto.Type.Value);
    }
    if (!string.IsNullOrEmpty(queryDto.DeviceType))
    {
      query = query.Where(x => x.DeviceType == queryDto.DeviceType);
    }
    if (queryDto.StartTime.HasValue)
    {
      query = query.Where(x => x.SendTime >= queryDto.StartTime.Value);
    }
    if (queryDto.EndTime.HasValue)
    {
      query = query.Where(x => x.SendTime <= queryDto.EndTime.Value);
    }

    // 获取总记录数
    var total = await query.CountAsync();

    // 获取分页数据
    var items = await query
        .OrderByDescending(x => x.SendTime)
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .Select(x => new LeanMessageDto
        {
          Id = x.Id,
          SenderId = x.SenderId,
          SenderName = x.SenderName,
          ReceiverId = x.ReceiverId,
          ReceiverName = x.ReceiverName,
          Content = x.Content,
          MessageType = x.MessageType,
          IsRead = x.IsRead,
          ReadTime = x.ReadTime,
          SendTime = x.SendTime,
          DeviceType = x.DeviceType,
          DeviceName = x.DeviceName,
          IpAddress = x.IpAddress,
          Browser = x.Browser,
          Os = x.Os,
          Location = x.Location,
          CreateTime = x.CreateTime,
          UpdateTime = x.UpdateTime
        })
        .ToListAsync();

    return new LeanPagedResult<LeanMessageDto>
    {
      Items = items,
      Total = total,
      PageIndex = queryDto.PageIndex,
      PageSize = queryDto.PageSize
    };
  }

  /// <inheritdoc/>
  public async Task<List<LeanMessageDto>> GetListAsync(LeanMessageQueryDto queryDto)
  {
    var query = _db.Queryable<LeanMessage>();

    // 应用查询条件
    if (queryDto.TargetUserId > 0)
    {
      query = query.Where(x => x.SenderId == queryDto.TargetUserId || x.ReceiverId == queryDto.TargetUserId);
    }
    if (queryDto.Type.HasValue)
    {
      query = query.Where(x => x.MessageType == queryDto.Type.Value);
    }
    if (!string.IsNullOrEmpty(queryDto.DeviceType))
    {
      query = query.Where(x => x.DeviceType == queryDto.DeviceType);
    }
    if (queryDto.StartTime.HasValue)
    {
      query = query.Where(x => x.SendTime >= queryDto.StartTime.Value);
    }
    if (queryDto.EndTime.HasValue)
    {
      query = query.Where(x => x.SendTime <= queryDto.EndTime.Value);
    }

    return await query
        .OrderByDescending(x => x.SendTime)
        .Select(x => new LeanMessageDto
        {
          Id = x.Id,
          SenderId = x.SenderId,
          SenderName = x.SenderName,
          ReceiverId = x.ReceiverId,
          ReceiverName = x.ReceiverName,
          Content = x.Content,
          MessageType = x.MessageType,
          IsRead = x.IsRead,
          ReadTime = x.ReadTime,
          SendTime = x.SendTime,
          DeviceType = x.DeviceType,
          DeviceName = x.DeviceName,
          IpAddress = x.IpAddress,
          Browser = x.Browser,
          Os = x.Os,
          Location = x.Location,
          CreateTime = x.CreateTime,
          UpdateTime = x.UpdateTime
        })
        .ToListAsync();
  }

  /// <inheritdoc/>
  public async Task<LeanMessageDto> GetByIdAsync(long id)
  {
    var entity = await _db.Queryable<LeanMessage>()
        .Where(x => x.Id == id)
        .FirstAsync();

    if (entity == null)
    {
      throw new ArgumentException($"消息不存在：{id}");
    }

    return new LeanMessageDto
    {
      Id = entity.Id,
      SenderId = entity.SenderId,
      SenderName = entity.SenderName,
      ReceiverId = entity.ReceiverId,
      ReceiverName = entity.ReceiverName,
      Content = entity.Content,
      MessageType = entity.MessageType,
      IsRead = entity.IsRead,
      ReadTime = entity.ReadTime,
      SendTime = entity.SendTime,
      DeviceType = entity.DeviceType,
      DeviceName = entity.DeviceName,
      IpAddress = entity.IpAddress,
      Browser = entity.Browser,
      Os = entity.Os,
      Location = entity.Location,
      CreateTime = entity.CreateTime,
      UpdateTime = entity.UpdateTime
    };
  }

  /// <inheritdoc/>
  public async Task<long> SendMessageAsync(LeanMessageCreateDto createDto)
  {
    // 1. 保存消息到数据库
    var message = new LeanMessage
    {
      SenderId = createDto.SenderId,
      ReceiverId = createDto.ReceiverId,
      Content = createDto.Content,
      MessageType = createDto.MessageType,
      SendTime = DateTime.Now,
      IsRead = false
    };
    await _db.Insertable(message).ExecuteCommandAsync();

    // 2. 检查接收者是否在线
    var isOnline = await _onlineService.IsUserOnlineAsync(createDto.ReceiverId);

    // 3. 如果在线，实时推送消息
    if (isOnline)
    {
      await _messageHub.Clients.Group($"User_{createDto.ReceiverId}")
        .SendAsync("ReceiveMessage", message);
    }
    // 4. 如果不在线，创建离线通知
    else
    {
      await _pushService.CreateMessageNotificationAsync(
        createDto.ReceiverId,
        "新消息提醒",
        $"您有来自 {createDto.SenderName} 的新消息",
        message.Id);
    }

    return message.Id;
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    return await _db.Deleteable<LeanMessage>()
        .Where(x => x.Id == id)
        .ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchDeleteAsync(List<long> ids)
  {
    return await _db.Deleteable<LeanMessage>()
        .Where(x => ids.Contains(x.Id))
        .ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> MarkAsReadAsync(long messageId)
  {
    var message = await _db.Queryable<LeanMessage>()
        .Where(x => x.Id == messageId)
        .FirstAsync();

    if (message != null)
    {
      message.IsRead = true;
      message.ReadTime = DateTime.Now;
      await _db.Updateable(message)
        .UpdateColumns(x => new { x.IsRead, x.ReadTime })
        .ExecuteCommandAsync();

      // 发送已读回执
      await _messageHub.Clients.Group($"User_{message.SenderId}")
        .SendAsync("MessageRead", messageId);

      return true;
    }

    return false;
  }

  /// <inheritdoc/>
  public async Task<bool> BatchMarkAsReadAsync(List<long> messageIds)
  {
    return await _db.Updateable<LeanMessage>()
        .SetColumns(x => new LeanMessage
        {
          IsRead = true,
          ReadTime = DateTime.Now,
          UpdateTime = DateTime.Now
        })
        .Where(x => messageIds.Contains(x.Id))
        .ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> MarkAllAsReadAsync(long userId, long targetUserId)
  {
    return await _db.Updateable<LeanMessage>()
        .SetColumns(x => new LeanMessage
        {
          IsRead = true,
          ReadTime = DateTime.Now,
          UpdateTime = DateTime.Now
        })
        .Where(x => x.ReceiverId == userId && x.SenderId == targetUserId && !x.IsRead)
        .ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<int> GetUnreadCountAsync(long userId)
  {
    return await _db.Queryable<LeanMessage>()
        .Where(x => x.ReceiverId == userId && !x.IsRead)
        .CountAsync();
  }

  #endregion

  #region 聊天记录管理

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanMessageDto>> GetChatHistoryAsync(LeanMessageQueryDto queryDto)
  {
    var query = _db.Queryable<LeanMessage>()
        .Where(x => (x.SenderId == queryDto.TargetUserId && x.ReceiverId == queryDto.TargetUserId) ||
                   (x.SenderId == queryDto.TargetUserId && x.ReceiverId == queryDto.TargetUserId));

    // 获取总记录数
    var total = await query.CountAsync();

    // 获取分页数据
    var items = await query
        .OrderByDescending(x => x.SendTime)
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .Select(x => new LeanMessageDto
        {
          Id = x.Id,
          SenderId = x.SenderId,
          SenderName = x.SenderName,
          ReceiverId = x.ReceiverId,
          ReceiverName = x.ReceiverName,
          Content = x.Content,
          MessageType = x.MessageType,
          IsRead = x.IsRead,
          ReadTime = x.ReadTime,
          SendTime = x.SendTime,
          DeviceType = x.DeviceType,
          DeviceName = x.DeviceName,
          IpAddress = x.IpAddress,
          Browser = x.Browser,
          Os = x.Os,
          Location = x.Location,
          CreateTime = x.CreateTime,
          UpdateTime = x.UpdateTime
        })
        .ToListAsync();

    return new LeanPagedResult<LeanMessageDto>
    {
      Items = items,
      Total = total,
      PageIndex = queryDto.PageIndex,
      PageSize = queryDto.PageSize
    };
  }

  /// <inheritdoc/>
  public async Task<List<LeanMessageContactDto>> GetRecentContactsAsync(long userId, int count = 20)
  {
    var messages = await _db.Queryable<LeanMessage>()
        .Where(x => x.SenderId == userId || x.ReceiverId == userId)
        .OrderByDescending(x => x.SendTime)
        .ToListAsync();

    var contacts = new List<LeanMessageContactDto>();
    foreach (var group in messages.GroupBy(x => x.SenderId == userId ? x.ReceiverId : x.SenderId).Take(count))
    {
      var lastMessage = group.OrderByDescending(m => m.SendTime).First();
      var unreadCount = await _db.Queryable<LeanMessage>()
          .Where(x => x.SenderId == group.Key && x.ReceiverId == userId && !x.IsRead)
          .CountAsync();

      contacts.Add(new LeanMessageContactDto
      {
        UserId = group.Key,
        UserName = lastMessage.SenderId == userId ? lastMessage.ReceiverName : lastMessage.SenderName,
        LastMessage = lastMessage.Content,
        LastTime = lastMessage.SendTime,
        UnreadCount = unreadCount,
        DeviceType = lastMessage.DeviceType,
        Location = lastMessage.Location
      });
    }

    return contacts;
  }

  #endregion

  #region 消息状态管理

  /// <inheritdoc/>
  public async Task<List<LeanMessageDto>> GetUnreadMessagesAsync(long userId)
  {
    return await _db.Queryable<LeanMessage>()
        .Where(x => x.ReceiverId == userId && !x.IsRead)
        .OrderByDescending(x => x.SendTime)
        .Select(x => new LeanMessageDto
        {
          Id = x.Id,
          SenderId = x.SenderId,
          SenderName = x.SenderName,
          ReceiverId = x.ReceiverId,
          ReceiverName = x.ReceiverName,
          Content = x.Content,
          MessageType = x.MessageType,
          IsRead = x.IsRead,
          ReadTime = x.ReadTime,
          SendTime = x.SendTime,
          DeviceType = x.DeviceType,
          DeviceName = x.DeviceName,
          IpAddress = x.IpAddress,
          Browser = x.Browser,
          Os = x.Os,
          Location = x.Location,
          CreateTime = x.CreateTime,
          UpdateTime = x.UpdateTime
        })
        .ToListAsync();
  }

  /// <summary>
  /// 获取用户的离线消息
  /// </summary>
  public async Task<List<LeanMessageDto>> GetOfflineMessagesAsync(long userId)
  {
    var messages = await _db.Queryable<LeanMessage>()
        .Where(x => x.ReceiverId == userId && !x.IsRead)
        .OrderBy(x => x.SendTime)
        .ToListAsync();

    return messages.Select(x => new LeanMessageDto
    {
      Id = x.Id,
      SenderId = x.SenderId,
      SenderName = x.SenderName,
      ReceiverId = x.ReceiverId,
      ReceiverName = x.ReceiverName,
      Content = x.Content,
      MessageType = x.MessageType,
      IsRead = x.IsRead,
      ReadTime = x.ReadTime,
      SendTime = x.SendTime,
      DeviceType = x.DeviceType,
      DeviceName = x.DeviceName,
      IpAddress = x.IpAddress,
      Browser = x.Browser,
      Os = x.Os,
      Location = x.Location,
      CreateTime = x.CreateTime,
      UpdateTime = x.UpdateTime
    }).ToList();
  }

  #endregion
}