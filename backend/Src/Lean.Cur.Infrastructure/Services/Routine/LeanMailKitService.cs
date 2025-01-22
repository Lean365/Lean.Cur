// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.Text;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Domain.Entities.Routine;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Routine;

/// <summary>
/// MailKit邮件服务实现类
/// </summary>
public class LeanMailKitService : ILeanMailKitService
{
  private readonly IConfiguration _configuration;
  private readonly ILogger<LeanMailKitService> _logger;
  private readonly ISqlSugarClient _db;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMailKitService(
      IConfiguration configuration,
      ILogger<LeanMailKitService> logger,
      ISqlSugarClient db)
  {
    _configuration = configuration;
    _logger = logger;
    _db = db;
  }

  /// <inheritdoc/>
  public async Task<bool> SendEmailAsync(MimeMessage message)
  {
    try
    {
      var smtpSettings = _configuration.GetSection("EmailSettings:SmtpSettings");
      var server = smtpSettings["Server"];
      var port = int.Parse(smtpSettings["Port"] ?? "587");
      var username = smtpSettings["Username"];
      var password = smtpSettings["Password"];
      var enableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");

      using var client = new SmtpClient();
      await client.ConnectAsync(server, port, enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
      await client.AuthenticateAsync(username, password);
      await client.SendAsync(message);
      await client.DisconnectAsync(true);

      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "发送邮件失败");
      return false;
    }
  }

  /// <inheritdoc/>
  public MimeMessage CreateMessage(
      List<string> to,
      List<string>? cc = null,
      List<string>? bcc = null,
      string? subject = null,
      string? body = null,
      List<(string fileName, byte[] content, string contentType)>? attachments = null)
  {
    var message = new MimeMessage();

    // 设置发件人
    var smtpSettings = _configuration.GetSection("EmailSettings:SmtpSettings");
    var fromName = smtpSettings["FromName"] ?? "Lean.Cur System";
    var fromEmail = smtpSettings["FromEmail"];
    message.From.Add(new MailboxAddress(fromName, fromEmail));

    // 设置收件人
    foreach (var address in to)
    {
      message.To.Add(MailboxAddress.Parse(address));
    }

    // 设置抄送人
    if (cc?.Any() == true)
    {
      foreach (var address in cc)
      {
        message.Cc.Add(MailboxAddress.Parse(address));
      }
    }

    // 设置密送人
    if (bcc?.Any() == true)
    {
      foreach (var address in bcc)
      {
        message.Bcc.Add(MailboxAddress.Parse(address));
      }
    }

    // 设置主题
    message.Subject = subject ?? string.Empty;

    // 创建邮件主体
    var builder = new BodyBuilder();

    // 设置HTML内容
    builder.HtmlBody = body;

    // 添加附件
    if (attachments?.Any() == true)
    {
      foreach (var (fileName, content, contentType) in attachments)
      {
        builder.Attachments.Add(fileName, content, ContentType.Parse(contentType));
      }
    }

    message.Body = builder.ToMessageBody();

    return message;
  }

  /// <summary>
  /// 使用模板创建邮件
  /// </summary>
  public async Task<MimeMessage> CreateMessageFromTemplateAsync(
      string templateCode,
      Dictionary<string, string> parameters,
      List<string> to,
      List<string>? cc = null,
      List<string>? bcc = null,
      List<(string fileName, byte[] content, string contentType)>? attachments = null)
  {
    // 获取模板
    var template = await _db.Queryable<LeanEmailTemplate>()
        .Where(t => t.Code == templateCode && t.IsEnabled)
        .FirstAsync() ?? throw new Exception($"邮件模板[{templateCode}]不存在或未启用");

    // 替换参数
    var subject = template.Subject;
    var content = template.Content;
    foreach (var param in parameters)
    {
      subject = subject.Replace($"{{{param.Key}}}", param.Value);
      content = content.Replace($"{{{param.Key}}}", param.Value);
    }

    // 创建邮件
    return CreateMessage(to, cc, bcc, subject, content, attachments);
  }
}