// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// 邮件服务接口
/// </summary>
/// <remarks>
/// 提供邮件相关的业务操作，包括：
/// 1. 发送邮件
/// 2. 接收邮件
/// 3. 管理附件
/// 4. 邮件状态更新
/// </remarks>
public interface ILeanEmailService
{
  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="dto">邮件发送参数</param>
  /// <returns>发送结果</returns>
  Task<LeanApiResult> SendEmailAsync(LeanEmailSendDto dto);

  /// <summary>
  /// 获取收件箱
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>收件箱列表</returns>
  Task<LeanApiResult<LeanPagedResult<LeanEmailDto>>> GetInboxAsync(LeanEmailQueryDto queryDto);

  /// <summary>
  /// 获取发件箱
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>发件箱列表</returns>
  Task<LeanApiResult<LeanPagedResult<LeanEmailDto>>> GetOutboxAsync(LeanEmailQueryDto queryDto);

  /// <summary>
  /// 获取邮件详情
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>邮件详情</returns>
  Task<LeanApiResult<LeanEmailDetailDto>> GetEmailDetailAsync(long id);

  /// <summary>
  /// 标记为已读
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>标记结果</returns>
  Task<LeanApiResult> MarkAsReadAsync(long id);

  /// <summary>
  /// 删除邮件
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>删除结果</returns>
  Task<LeanApiResult> DeleteEmailAsync(long id);

  /// <summary>
  /// 上传附件
  /// </summary>
  /// <param name="dto">附件上传参数</param>
  /// <returns>附件信息</returns>
  Task<LeanApiResult<LeanEmailAttachmentDto>> UploadAttachmentAsync(LeanEmailAttachmentUploadDto dto);

  /// <summary>
  /// 下载附件
  /// </summary>
  /// <param name="id">附件ID</param>
  /// <returns>附件下载信息</returns>
  Task<LeanApiResult<LeanEmailAttachmentDownloadDto>> DownloadAttachmentAsync(long id);

  /// <summary>
  /// 删除附件
  /// </summary>
  /// <param name="id">附件ID</param>
  /// <returns>删除结果</returns>
  Task<LeanApiResult> DeleteAttachmentAsync(long id);

  /// <summary>
  /// 获取未读邮件数量
  /// </summary>
  /// <returns>未读邮件数量</returns>
  Task<LeanApiResult<int>> GetUnreadCountAsync();
}