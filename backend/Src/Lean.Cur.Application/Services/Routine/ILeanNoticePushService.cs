using Lean.Cur.Application.Dtos.Routine;
using System.Threading.Tasks;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// 通知推送服务接口
/// </summary>
public interface ILeanNoticePushService
{
  /// <summary>
  /// 向指定用户推送通知
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="notice">通知内容</param>
  Task PushToUserAsync(long userId, LeanNoticeDto notice);

  /// <summary>
  /// 向指定角色的所有用户推送通知
  /// </summary>
  /// <param name="roleCode">角色代码</param>
  /// <param name="notice">通知内容</param>
  Task PushToRoleAsync(string roleCode, LeanNoticeDto notice);

  /// <summary>
  /// 向所有在线用户推送通知
  /// </summary>
  /// <param name="notice">通知内容</param>
  Task PushToAllAsync(LeanNoticeDto notice);

  /// <summary>
  /// 创建消息通知
  /// </summary>
  /// <param name="userId">接收用户ID</param>
  /// <param name="title">通知标题</param>
  /// <param name="content">通知内容</param>
  /// <param name="messageId">关联的消息ID</param>
  Task CreateMessageNotificationAsync(long userId, string title, string content, long messageId);
}