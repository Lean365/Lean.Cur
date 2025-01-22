using Lean.Cur.Application.Dtos.Routine;

namespace Lean.Cur.Application.Hubs;

/// <summary>
/// 即时通讯消息Hub接口
/// </summary>
public interface ILeanMessageHub
{
  /// <summary>
  /// 发送消息到指定用户
  /// </summary>
  /// <param name="message">消息内容</param>
  Task SendMessageToUser(LeanMessageDto message);

  /// <summary>
  /// 发送消息到指定角色
  /// </summary>
  /// <param name="roleCode">角色代码</param>
  /// <param name="message">消息内容</param>
  Task SendMessageToRole(string roleCode, LeanMessageDto message);

  /// <summary>
  /// 发送消息到所有在线用户
  /// </summary>
  /// <param name="message">消息内容</param>
  Task SendMessageToAll(LeanMessageDto message);

  /// <summary>
  /// 标记消息为已读
  /// </summary>
  /// <param name="messageId">消息ID</param>
  Task MarkMessageAsRead(long messageId);

  /// <summary>
  /// 标记与指定用户的所有消息为已读
  /// </summary>
  /// <param name="targetUserId">目标用户ID</param>
  Task MarkAllMessagesAsRead(long targetUserId);

  /// <summary>
  /// 更新最后活动时间
  /// </summary>
  Task UpdateLastActiveTime();
}