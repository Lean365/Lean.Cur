// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Enums;
using Lean.Cur.Domain.Entities.Routine;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace Lean.Cur.Infrastructure.Services.Routine;

/// <summary>
/// 邮件服务实现类
/// </summary>
public class LeanEmailService : ILeanEmailService
{
  private readonly ISqlSugarClient _db;
  private readonly ILogger<LeanEmailService> _logger;
  private readonly IConfiguration _configuration;
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanEmailService(
      ISqlSugarClient db,
      ILogger<LeanEmailService> logger,
      IConfiguration configuration,
      IHttpContextAccessor httpContextAccessor)
  {
    _db = db;
    _logger = logger;
    _configuration = configuration;
    _httpContextAccessor = httpContextAccessor;
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> SendEmailAsync(LeanEmailSendDto dto)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      // 验证收件人是否存在
      var receivers = await _db.Queryable<LeanUser>()
          .Where(u => dto.ReceiverIds.Contains(u.Id))
          .ToListAsync();

      if (receivers.Count != dto.ReceiverIds.Count)
      {
        return LeanApiResult<LeanEmailSendDto>.Error(LeanErrorCode.DataNotFound, "部分收件人不存在");
      }

      // 验证附件是否存在
      if (dto.AttachmentIds?.Any() == true)
      {
        var attachments = await _db.Queryable<LeanEmailAttachment>()
            .Where(a => dto.AttachmentIds.Contains(a.Id))
            .ToListAsync();

        if (attachments.Count != dto.AttachmentIds.Count)
        {
          return LeanApiResult<LeanEmailSendDto>.Error(LeanErrorCode.DataNotFound, "部分附件不存在");
        }
      }

      // 开启事务
      await _db.Ado.BeginTranAsync();

      try
      {
        // 创建邮件
        var email = new LeanEmail
        {
          SenderId = currentUserId,
          Subject = dto.Subject,
          Content = dto.Content,
          SendTime = DateTime.Now,
          Status = LeanEmailStatus.Sent
        };

        // 保存邮件
        await _db.Insertable(email).ExecuteReturnEntityAsync();

        // 更新附件关联
        if (dto.AttachmentIds?.Any() == true)
        {
          await _db.Updateable<LeanEmailAttachment>()
              .SetColumns(a => a.EmailId == email.Id)
              .Where(a => dto.AttachmentIds.Contains(a.Id))
              .ExecuteCommandAsync();
        }

        // 创建收件人记录
        var receiverEmails = receivers.Select(r => new LeanEmail
        {
          SenderId = currentUserId,
          ReceiverId = r.Id,
          Subject = dto.Subject,
          Content = dto.Content,
          SendTime = DateTime.Now,
          Status = LeanEmailStatus.Sent,
          IsRead = false
        }).ToList();

        await _db.Insertable(receiverEmails).ExecuteCommandAsync();

        await _db.Ado.CommitTranAsync();
        return LeanApiResult<LeanEmailSendDto>.Success(dto);
      }
      catch (Exception ex)
      {
        await _db.Ado.RollbackTranAsync();
        _logger.LogError(ex, "发送邮件失败");
        return LeanApiResult<LeanEmailSendDto>.Error(LeanErrorCode.ServerError, "发送邮件失败");
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "发送邮件失败");
      return LeanApiResult<LeanEmailSendDto>.Error(LeanErrorCode.ServerError, "发送邮件失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanPagedResult<LeanEmailDto>>> GetInboxAsync(LeanEmailQueryDto queryDto)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var query = _db.Queryable<LeanEmail>()
          .WhereIF(!string.IsNullOrEmpty(queryDto.Keyword), e =>
              e.Subject.Contains(queryDto.Keyword) ||
              e.SenderName.Contains(queryDto.Keyword))
          .WhereIF(queryDto.SenderId > 0, e => e.SenderId == queryDto.SenderId)
          .WhereIF(queryDto.ReceiverId > 0, e => e.ReceiverId == queryDto.ReceiverId)
          .WhereIF(queryDto.StartTime.HasValue, e => e.SendTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, e => e.SendTime <= queryDto.EndTime)
          .WhereIF(queryDto.IsRead.HasValue, e => e.IsRead == queryDto.IsRead)
          .WhereIF(queryDto.HasAttachment.HasValue, e => e.HasAttachment == queryDto.HasAttachment)
          .Select(e => new LeanEmailDto
          {
            Id = e.Id,
            SenderId = e.SenderId,
            SenderName = e.SenderName,
            ReceiverId = e.ReceiverId,
            ReceiverName = e.ReceiverName,
            Subject = e.Subject,
            IsRead = e.IsRead,
            HasAttachment = e.HasAttachment,
            SendTime = e.SendTime
          });

      if (!string.IsNullOrEmpty(queryDto.OrderBy))
      {
        switch (queryDto.OrderBy.ToLower())
        {
          case "sendtime":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.SendTime) : query.OrderBy(e => e.SendTime);
            break;
          case "subject":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.Subject) : query.OrderBy(e => e.Subject);
            break;
          case "sendername":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.SenderName) : query.OrderBy(e => e.SenderName);
            break;
          case "receivername":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.ReceiverName) : query.OrderBy(e => e.ReceiverName);
            break;
          case "isread":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.IsRead) : query.OrderBy(e => e.IsRead);
            break;
          case "hasattachment":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.HasAttachment) : query.OrderBy(e => e.HasAttachment);
            break;
          default:
            query = query.OrderByDescending(e => e.SendTime);
            break;
        }
      }
      else
      {
        query = query.OrderByDescending(e => e.SendTime);
      }

      var total = await query.CountAsync();
      var items = await query.Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
          .Take(queryDto.PageSize)
          .ToListAsync();

      var result = new LeanPagedResult<LeanEmailDto>(items, total, queryDto.PageIndex, queryDto.PageSize);
      return LeanApiResult<LeanPagedResult<LeanEmailDto>>.Success(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取收件箱失败");
      return LeanApiResult<LeanPagedResult<LeanEmailDto>>.Error(LeanErrorCode.ServerError, "获取收件箱失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanPagedResult<LeanEmailDto>>> GetOutboxAsync(LeanEmailQueryDto queryDto)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var query = _db.Queryable<LeanEmail>()
          .Where(e => e.SenderId == currentUserId)
          .WhereIF(!string.IsNullOrEmpty(queryDto.Keyword), e =>
              e.Subject.Contains(queryDto.Keyword) ||
              e.Content.Contains(queryDto.Keyword) ||
              e.ReceiverName.Contains(queryDto.Keyword))
          .WhereIF(queryDto.StartTime.HasValue, e => e.SendTime >= queryDto.StartTime)
          .WhereIF(queryDto.EndTime.HasValue, e => e.SendTime <= queryDto.EndTime)
          .WhereIF(queryDto.IsRead.HasValue, e => e.IsRead == queryDto.IsRead)
          .WhereIF(queryDto.HasAttachment.HasValue, e => e.HasAttachment == queryDto.HasAttachment)
          .Select(e => new LeanEmailDto
          {
            Id = e.Id,
            SenderId = e.SenderId,
            SenderName = e.SenderName,
            ReceiverId = e.ReceiverId,
            ReceiverName = e.ReceiverName,
            Subject = e.Subject,
            IsRead = e.IsRead,
            HasAttachment = e.HasAttachment,
            SendTime = e.SendTime
          });

      if (!string.IsNullOrEmpty(queryDto.OrderBy))
      {
        switch (queryDto.OrderBy.ToLower())
        {
          case "sendtime":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.SendTime) : query.OrderBy(e => e.SendTime);
            break;
          case "subject":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.Subject) : query.OrderBy(e => e.Subject);
            break;
          case "sendername":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.SenderName) : query.OrderBy(e => e.SenderName);
            break;
          case "receivername":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.ReceiverName) : query.OrderBy(e => e.ReceiverName);
            break;
          case "isread":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.IsRead) : query.OrderBy(e => e.IsRead);
            break;
          case "hasattachment":
            query = queryDto.IsDesc ? query.OrderByDescending(e => e.HasAttachment) : query.OrderBy(e => e.HasAttachment);
            break;
          default:
            query = query.OrderByDescending(e => e.SendTime);
            break;
        }
      }
      else
      {
        query = query.OrderByDescending(e => e.SendTime);
      }

      var total = await query.CountAsync();
      var items = await query.Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
          .Take(queryDto.PageSize)
          .ToListAsync();

      return LeanApiResult<LeanPagedResult<LeanEmailDto>>.Success(new LeanPagedResult<LeanEmailDto>
      {
        Items = items,
        Total = total,
        PageIndex = queryDto.PageIndex,
        PageSize = queryDto.PageSize
      });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取发件箱失败");
      return LeanApiResult<LeanPagedResult<LeanEmailDto>>.Error(LeanErrorCode.ServerError, "获取发件箱失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanEmailDetailDto>> GetEmailDetailAsync(long id)
  {
    try
    {
      var email = await _db.Queryable<LeanEmail>()
          .Where(e => e.Id == id)
          .LeftJoin<LeanUser>((e, s) => e.SenderId == s.Id)
          .LeftJoin<LeanUser>((e, s, r) => e.ReceiverId == r.Id)
          .Select((e, s, r) => new LeanEmailDetailDto
          {
            Id = e.Id,
            SenderId = e.SenderId,
            SenderName = s.UserName,
            ReceiverId = e.ReceiverId,
            ReceiverName = r.UserName,
            Subject = e.Subject,
            Content = e.Content,
            IsRead = e.IsRead,
            SendTime = e.SendTime,
            HasAttachment = e.HasAttachment
          })
          .FirstAsync();

      if (email == null)
      {
        return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.DataNotFound, "邮件不存在");
      }

      return LeanApiResult<LeanEmailDetailDto>.Success(email);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取邮件详情失败");
      return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.ServerError, "获取邮件详情失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> MarkAsReadAsync(long id)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var email = await _db.Queryable<LeanEmail>()
          .Where(e => e.Id == id && e.ReceiverId == currentUserId)
          .FirstAsync();

      if (email == null)
      {
        return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.DataNotFound, "邮件不存在");
      }

      if (email.IsRead)
      {
        return LeanApiResult<LeanEmailDetailDto>.Success(null);
      }

      email.IsRead = true;
      email.ReadTime = DateTime.Now;

      await _db.Updateable(email).ExecuteCommandAsync();

      return LeanApiResult<LeanEmailDetailDto>.Success(null);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "标记邮件已读失败");
      return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.ServerError, "标记邮件已读失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> DeleteEmailAsync(long id)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var email = await _db.Queryable<LeanEmail>()
          .Where(e => e.Id == id && (e.SenderId == currentUserId || e.ReceiverId == currentUserId))
          .FirstAsync();

      if (email == null)
      {
        return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.DataNotFound, "邮件不存在");
      }

      // 开启事务
      await _db.Ado.BeginTranAsync();

      try
      {
        // 删除附件
        await _db.Deleteable<LeanEmailAttachment>()
            .Where(a => a.EmailId == id)
            .ExecuteCommandAsync();

        // 删除邮件
        await _db.Deleteable<LeanEmail>()
            .Where(e => e.Id == id)
            .ExecuteCommandAsync();

        await _db.Ado.CommitTranAsync();
        return LeanApiResult<LeanEmailDetailDto>.Success(null);
      }
      catch (Exception ex)
      {
        await _db.Ado.RollbackTranAsync();
        _logger.LogError(ex, "删除邮件失败");
        return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.ServerError, "删除邮件失败");
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "删除邮件失败");
      return LeanApiResult<LeanEmailDetailDto>.Error(LeanErrorCode.ServerError, "删除邮件失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanEmailAttachmentDto>> UploadAttachmentAsync(LeanEmailAttachmentUploadDto dto)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      // 验证文件
      if (dto.File == null || dto.File.Length == 0)
      {
        return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.BadRequest, "请选择要上传的文件");
      }

      // 验证文件大小
      var maxSize = _configuration.GetValue<long>("FileUpload:MaxSize");
      if (dto.File.Length > maxSize)
      {
        return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.BadRequest, "文件大小超过限制");
      }

      // 验证文件类型
      var allowedExtensions = _configuration.GetSection("FileUpload:AllowedExtensions").Get<string[]>();
      var extension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();
      if (!allowedExtensions.Contains(extension))
      {
        return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.BadRequest, "不支持的文件类型");
      }

      // 保存文件
      var fileName = $"{Guid.NewGuid():N}{extension}";
      var filePath = Path.Combine("uploads", "attachments", fileName);
      var directory = Path.GetDirectoryName(filePath);
      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await dto.File.CopyToAsync(stream);
      }

      // 创建附件记录
      var attachment = new LeanEmailAttachment
      {
        FileName = dto.File.FileName,
        FilePath = filePath,
        FileSize = dto.File.Length,
        FileType = dto.File.ContentType,
        FileExtension = extension,
        UploadTime = DateTime.Now
      };

      await _db.Insertable(attachment).ExecuteReturnEntityAsync();

      var result = new LeanEmailAttachmentDto
      {
        Id = attachment.Id,
        EmailId = attachment.EmailId,
        FileName = attachment.FileName,
        FileSize = attachment.FileSize,
        FileType = attachment.FileType,
        FileExtension = attachment.FileExtension,
        UploadTime = attachment.UploadTime
      };

      return LeanApiResult<LeanEmailAttachmentDto>.Success(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "上传附件失败");
      return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.ServerError, "上传附件失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanEmailAttachmentDownloadDto>> DownloadAttachmentAsync(long id)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var attachment = await _db.Queryable<LeanEmailAttachment>()
          .Where(a => a.Id == id)
          .FirstAsync();

      if (attachment == null)
      {
        return LeanApiResult<LeanEmailAttachmentDownloadDto>.Error(LeanErrorCode.DataNotFound, "附件不存在");
      }

      // 验证权限
      var email = await _db.Queryable<LeanEmail>()
          .Where(e => e.Id == attachment.EmailId)
          .FirstAsync();

      if (email == null || (email.SenderId != currentUserId && email.ReceiverId != currentUserId))
      {
        return LeanApiResult<LeanEmailAttachmentDownloadDto>.Error(LeanErrorCode.Forbidden, "无权访问此附件");
      }

      if (!File.Exists(attachment.FilePath))
      {
        return LeanApiResult<LeanEmailAttachmentDownloadDto>.Error(LeanErrorCode.DataNotFound, "附件文件不存在");
      }

      var result = new LeanEmailAttachmentDownloadDto
      {
        FileName = attachment.FileName,
        FileType = attachment.FileType,
        FilePath = attachment.FilePath
      };

      return LeanApiResult<LeanEmailAttachmentDownloadDto>.Success(result);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "下载附件失败");
      return LeanApiResult<LeanEmailAttachmentDownloadDto>.Error(LeanErrorCode.ServerError, "下载附件失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> DeleteAttachmentAsync(long id)
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var attachment = await _db.Queryable<LeanEmailAttachment>()
          .Where(a => a.Id == id)
          .FirstAsync();

      if (attachment == null)
      {
        return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.DataNotFound, "附件不存在");
      }

      // 验证权限
      var email = await _db.Queryable<LeanEmail>()
          .Where(e => e.Id == attachment.EmailId)
          .FirstAsync();

      if (email == null || email.SenderId != currentUserId)
      {
        return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.Forbidden, "无权删除此附件");
      }

      // 删除文件
      if (File.Exists(attachment.FilePath))
      {
        File.Delete(attachment.FilePath);
      }

      // 删除记录
      await _db.Deleteable<LeanEmailAttachment>()
          .Where(a => a.Id == id)
          .ExecuteCommandAsync();

      return LeanApiResult<LeanEmailAttachmentDto>.Success(null);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "删除附件失败");
      return LeanApiResult<LeanEmailAttachmentDto>.Error(LeanErrorCode.ServerError, "删除附件失败");
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<int>> GetUnreadCountAsync()
  {
    try
    {
      var currentUserId = LeanUser.GetCurrentUserId();

      var count = await _db.Queryable<LeanEmail>()
          .Where(e => e.ReceiverId == currentUserId && !e.IsRead)
          .CountAsync();

      return LeanApiResult<int>.Success(count);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取未读邮件数量失败");
      return LeanApiResult<int>.Error(LeanErrorCode.ServerError, "获取未读邮件数量失败");
    }
  }
}