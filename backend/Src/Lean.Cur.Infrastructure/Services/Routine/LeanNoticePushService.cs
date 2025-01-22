using Microsoft.AspNetCore.SignalR;
using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Infrastructure.Hubs;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Infrastructure.Services.Routine;

/// <summary>
/// 通知推送服务实现
/// </summary>
public class LeanNoticePushService : ILeanNoticePushService
{
  private readonly IHubContext<LeanNoticeHub> _hubContext;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="hubContext">SignalR Hub上下文</param>
  public LeanNoticePushService(IHubContext<LeanNoticeHub> hubContext)
  {
    _hubContext = hubContext;
  }

  /// <inheritdoc/>
  public async Task PushToUserAsync(long userId, LeanNoticeDto notice)
  {
    await _hubContext.Clients.Group($"User_{userId}")
        .SendAsync("ReceiveNotice", notice);
  }

  /// <inheritdoc/>
  public async Task PushToRoleAsync(string roleCode, LeanNoticeDto notice)
  {
    await _hubContext.Clients.Group($"Role_{roleCode}")
        .SendAsync("ReceiveNotice", notice);
  }

  /// <inheritdoc/>
  public async Task PushToAllAsync(LeanNoticeDto notice)
  {
    await _hubContext.Clients.All
        .SendAsync("ReceiveNotice", notice);
  }

  /// <inheritdoc/>
  public async Task PushToUsersAsync(List<long> userIds, LeanNoticeDto notice)
  {
    var tasks = userIds.Select(userId =>
        _hubContext.Clients.Group($"User_{userId}")
            .SendAsync("ReceiveNotice", notice));

    await Task.WhenAll(tasks);
  }

  /// <inheritdoc/>
  public async Task CreateMessageNotificationAsync(long userId, string title, string content, long messageId)
  {
    var notice = new LeanNoticeDto
    {
      NoticeTitle = title,
      NoticeContent = content,
      NoticeType = LeanNoticeType.System,
      Status = LeanStatus.Normal,
      PublishTime = DateTime.Now,
      CreateTime = DateTime.Now,
      UpdateTime = DateTime.Now
    };

    await PushToUserAsync(userId, notice);
  }
}