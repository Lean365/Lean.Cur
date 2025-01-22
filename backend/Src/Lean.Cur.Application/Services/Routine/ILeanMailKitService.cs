// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using MimeKit;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// MailKit邮件服务接口
/// </summary>
public interface ILeanMailKitService
{
  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="message">邮件消息</param>
  /// <returns>发送结果</returns>
  Task<bool> SendEmailAsync(MimeMessage message);

  /// <summary>
  /// 创建邮件消息
  /// </summary>
  /// <param name="to">收件人列表</param>
  /// <param name="cc">抄送人列表</param>
  /// <param name="bcc">密送人列表</param>
  /// <param name="subject">主题</param>
  /// <param name="body">内容</param>
  /// <param name="attachments">附件列表</param>
  /// <returns>邮件消息</returns>
  MimeMessage CreateMessage(
      List<string> to,
      List<string>? cc = null,
      List<string>? bcc = null,
      string? subject = null,
      string? body = null,
      List<(string fileName, byte[] content, string contentType)>? attachments = null);

  /// <summary>
  /// 使用模板创建邮件
  /// </summary>
  /// <param name="templateCode">模板代码</param>
  /// <param name="parameters">模板参数</param>
  /// <param name="to">收件人列表</param>
  /// <param name="cc">抄送人列表</param>
  /// <param name="bcc">密送人列表</param>
  /// <param name="attachments">附件列表</param>
  /// <returns>邮件消息</returns>
  Task<MimeMessage> CreateMessageFromTemplateAsync(
      string templateCode,
      Dictionary<string, string> parameters,
      List<string> to,
      List<string>? cc = null,
      List<string>? bcc = null,
      List<(string fileName, byte[] content, string contentType)>? attachments = null);
}