using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Lean.Cur.Application.Hubs;
using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Application.Services.Routine;
using System;
using System.Threading.Tasks;
using Lean.Cur.Infrastructure.Services.Common;

namespace Lean.Cur.Infrastructure.Hubs;

/// <summary>
/// 在线用户Hub实现
/// </summary>
[Authorize]
public class LeanOnlineHub : Hub, ILeanOnlineHub
{
  private readonly ILeanOnlineService _onlineService;
  private readonly ILeanUserService _userService;
  private readonly LeanIp2RegionService _ip2RegionService;

  public LeanOnlineHub(
      ILeanOnlineService onlineService,
      ILeanUserService userService,
      LeanIp2RegionService ip2RegionService)
  {
    _onlineService = onlineService;
    _userService = userService;
    _ip2RegionService = ip2RegionService;
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

  public async Task UserOnline(LeanOnlineUserDto user)
  {
    await _onlineService.UserConnectedAsync(
      user.UserId,
      user.UserName,
      Context.ConnectionId,
      user.IpAddress,
      user.DeviceType,
      user.DeviceName,
      user.Location,
      user.Browser,
      user.Os);

    await Clients.Others.SendAsync("UserOnline", user);
  }

  public async Task UserOffline(long userId)
  {
    await _onlineService.UserDisconnectedAsync(Context.ConnectionId);
    await Clients.Others.SendAsync("UserOffline", userId);
  }

  public async Task UpdateUserStatus(LeanOnlineUserDto user)
  {
    await Clients.All.SendAsync("UserStatusUpdated", user);
  }

  public async Task UpdateLastActiveTime()
  {
    await _onlineService.UpdateLastActiveTimeAsync(Context.ConnectionId);
  }

  public async Task ForceOffline(long userId, string reason)
  {
    await Clients.Group($"User_{userId}").SendAsync("ForceOffline", reason);
  }

  #region Helper Methods

  private string GetDeviceType()
  {
    var userAgent = Context.GetHttpContext()?.Request.Headers["User-Agent"].ToString();
    if (string.IsNullOrEmpty(userAgent)) return "Unknown";

    if (userAgent.Contains("Mobile")) return "Mobile";
    if (userAgent.Contains("Tablet")) return "Tablet";
    return "PC";
  }

  private string GetDeviceName()
  {
    var userAgent = Context.GetHttpContext()?.Request.Headers["User-Agent"].ToString();
    return userAgent ?? "Unknown";
  }

  private async Task<string> GetLocation(string ip)
  {
    var location = await _ip2RegionService.SearchAsync(ip);
    return location;
  }

  #endregion
}