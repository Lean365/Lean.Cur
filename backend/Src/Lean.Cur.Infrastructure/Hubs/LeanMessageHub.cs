using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Lean.Cur.Application.Hubs;
using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Application.Services.Routine;

namespace Lean.Cur.Infrastructure.Hubs;

/// <summary>
/// 即时通讯消息Hub实现
/// </summary>
[Authorize]
public class LeanMessageHub : Hub, ILeanMessageHub
{
  private readonly ILeanMessageService _messageService;
  private readonly ILeanOnlineService _onlineService;
  private readonly ILeanUserService _userService;

  public LeanMessageHub(
      ILeanMessageService messageService,
      ILeanOnlineService onlineService,
      ILeanUserService userService)
  {
    _messageService = messageService;
    _onlineService = onlineService;
    _userService = userService;
  }

  public override async Task OnConnectedAsync()
  {
    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!string.IsNullOrEmpty(userId))
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
      var roleCode = await _userService.GetUserRoleCodeAsync(long.Parse(userId));
      if (!string.IsNullOrEmpty(roleCode))
      {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Role_{roleCode}");
      }
    }
    await base.OnConnectedAsync();
  }

  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!string.IsNullOrEmpty(userId))
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
      var roleCode = await _userService.GetUserRoleCodeAsync(long.Parse(userId));
      if (!string.IsNullOrEmpty(roleCode))
      {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Role_{roleCode}");
      }
    }
    await base.OnDisconnectedAsync(exception);
  }

  public async Task SendMessageToUser(LeanMessageDto message)
  {
    await Clients.Group($"User_{message.ReceiverId}")
        .SendAsync("ReceiveMessage", message);
  }

  public async Task SendMessageToRole(string roleCode, LeanMessageDto message)
  {
    await Clients.Group($"Role_{roleCode}")
        .SendAsync("ReceiveMessage", message);
  }

  public async Task SendMessageToAll(LeanMessageDto message)
  {
    await Clients.All.SendAsync("ReceiveMessage", message);
  }

  public async Task MarkMessageAsRead(long messageId)
  {
    var message = await _messageService.GetByIdAsync(messageId);
    if (message == null) return;

    await _messageService.MarkAsReadAsync(messageId);
    await Clients.Group($"User_{message.SenderId}")
        .SendAsync("MessageRead", messageId);
  }

  public async Task MarkAllMessagesAsRead(long targetUserId)
  {
    var userId = long.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    await _messageService.MarkAllAsReadAsync(userId, targetUserId);
    await Clients.Group($"User_{targetUserId}")
        .SendAsync("AllMessagesRead", userId);
  }

  public async Task UpdateLastActiveTime()
  {
    await _onlineService.UpdateLastActiveTimeAsync(Context.ConnectionId);
  }
}