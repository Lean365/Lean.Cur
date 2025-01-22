using Lean.Cur.Application.Dtos.Routine;

namespace Lean.Cur.Application.Hubs;

/// <summary>
/// 在线用户Hub接口
/// </summary>
public interface ILeanOnlineHub
{
  /// <summary>
  /// 用户上线通知
  /// </summary>
  /// <param name="user">用户信息</param>
  Task UserOnline(LeanOnlineUserDto user);

  /// <summary>
  /// 用户下线通知
  /// </summary>
  /// <param name="userId">用户ID</param>
  Task UserOffline(long userId);

  /// <summary>
  /// 更新用户状态
  /// </summary>
  /// <param name="user">用户信息</param>
  Task UpdateUserStatus(LeanOnlineUserDto user);

  /// <summary>
  /// 更新最后活动时间
  /// </summary>
  Task UpdateLastActiveTime();

  /// <summary>
  /// 强制用户下线
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="reason">原因</param>
  Task ForceOffline(long userId, string reason);
}