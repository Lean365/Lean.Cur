using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Lean.Cur.Application.Hubs;
using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Application.Services.Routine;

namespace Lean.Cur.Infrastructure.Hubs;

/// <summary>
/// 通知Hub实现
/// </summary>
[Authorize]
public class LeanNoticeHub : Hub, ILeanNoticeHub
{
  private readonly ILeanNoticeService _noticeService;
  private readonly ILeanUserService _userService;

  public LeanNoticeHub(
      ILeanNoticeService noticeService,
      ILeanUserService userService)
  {
    _noticeService = noticeService;
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

  public async Task ReceiveNotice(LeanNoticeDto notice)
  {
    await Clients.Caller.SendAsync("ReceiveNotice", notice);
  }

  public async Task MarkAsRead(long noticeId)
  {
    await _noticeService.MarkAsReadAsync(noticeId);
  }

  public async Task BatchMarkAsRead(List<long> noticeIds)
  {
    await _noticeService.BatchMarkAsReadAsync(noticeIds);
  }

  public async Task DeleteNotice(long noticeId)
  {
    await _noticeService.DeleteAsync(noticeId);
  }

  public async Task BatchDeleteNotices(List<long> noticeIds)
  {
    var deleteDto = new LeanNoticeBatchDeleteDto { Ids = noticeIds };
    await _noticeService.BatchDeleteAsync(deleteDto);
  }
}