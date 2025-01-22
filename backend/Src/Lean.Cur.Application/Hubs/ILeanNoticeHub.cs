using Lean.Cur.Application.Dtos.Routine;

namespace Lean.Cur.Application.Hubs;

/// <summary>
/// 通知Hub接口
/// </summary>
public interface ILeanNoticeHub
{
  /// <summary>
  /// 接收通知
  /// </summary>
  /// <param name="notice">通知内容</param>
  Task ReceiveNotice(LeanNoticeDto notice);

  /// <summary>
  /// 标记通知为已读
  /// </summary>
  /// <param name="noticeId">通知ID</param>
  Task MarkAsRead(long noticeId);

  /// <summary>
  /// 批量标记通知为已读
  /// </summary>
  /// <param name="noticeIds">通知ID列表</param>
  Task BatchMarkAsRead(List<long> noticeIds);

  /// <summary>
  /// 删除通知
  /// </summary>
  /// <param name="noticeId">通知ID</param>
  Task DeleteNotice(long noticeId);

  /// <summary>
  /// 批量删除通知
  /// </summary>
  /// <param name="noticeIds">通知ID列表</param>
  Task BatchDeleteNotices(List<long> noticeIds);
}