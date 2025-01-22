// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Dtos;
using Lean.Cur.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Routine;

/// <summary>
/// 邮件控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanEmailController : ControllerBase
{
  private readonly ILeanEmailService _emailService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanEmailController(ILeanEmailService emailService)
  {
    _emailService = emailService;
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  /// <param name="dto">邮件发送信息</param>
  /// <returns>发送结果</returns>
  [HttpPost("send")]
  public async Task<LeanApiResult> SendEmailAsync([FromBody] LeanEmailSendDto dto)
  {
    return await _emailService.SendEmailAsync(dto);
  }

  /// <summary>
  /// 获取收件箱列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>收件箱邮件列表</returns>
  [HttpGet("inbox")]
  public async Task<LeanApiResult<LeanPagedResult<LeanEmailDto>>> GetInboxAsync([FromQuery] LeanEmailQueryDto queryDto)
  {
    return await _emailService.GetInboxAsync(queryDto);
  }

  /// <summary>
  /// 获取发件箱列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>发件箱邮件列表</returns>
  [HttpGet("outbox")]
  public async Task<LeanApiResult<LeanPagedResult<LeanEmailDto>>> GetOutboxAsync([FromQuery] LeanEmailQueryDto queryDto)
  {
    return await _emailService.GetOutboxAsync(queryDto);
  }

  /// <summary>
  /// 获取邮件详情
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>邮件详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanEmailDetailDto>> GetEmailDetailAsync(long id)
  {
    return await _emailService.GetEmailDetailAsync(id);
  }

  /// <summary>
  /// 标记邮件为已读
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>操作结果</returns>
  [HttpPut("{id}/read")]
  public async Task<LeanApiResult> MarkAsReadAsync(long id)
  {
    return await _emailService.MarkAsReadAsync(id);
  }

  /// <summary>
  /// 删除邮件
  /// </summary>
  /// <param name="id">邮件ID</param>
  /// <returns>操作结果</returns>
  [HttpDelete("{id}")]
  public async Task<LeanApiResult> DeleteEmailAsync(long id)
  {
    return await _emailService.DeleteEmailAsync(id);
  }

  /// <summary>
  /// 上传邮件附件
  /// </summary>
  /// <param name="file">文件</param>
  /// <param name="tempEmailId">临时邮件ID（可选）</param>
  /// <returns>上传结果</returns>
  [HttpPost("attachment")]
  public async Task<LeanApiResult<LeanEmailAttachmentDto>> UploadAttachmentAsync(IFormFile file, long? tempEmailId = null)
  {
    var dto = new LeanEmailAttachmentUploadDto
    {
      File = file,
      TempEmailId = tempEmailId
    };
    return await _emailService.UploadAttachmentAsync(dto);
  }

  /// <summary>
  /// 下载邮件附件
  /// </summary>
  /// <param name="id">附件ID</param>
  /// <returns>文件流</returns>
  [HttpGet("attachment/{id}")]
  public async Task<IActionResult> DownloadAttachmentAsync(long id)
  {
    var result = await _emailService.DownloadAttachmentAsync(id);
    if (result.Code != LeanErrorCode.Success)
    {
      return BadRequest(result.Message);
    }

    var fileInfo = result.Data;
    if (fileInfo == null)
    {
      return BadRequest("文件信息不存在");
    }

    var stream = new FileStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read);
    return File(stream, fileInfo.FileType, fileInfo.FileName);
  }

  /// <summary>
  /// 删除邮件附件
  /// </summary>
  /// <param name="id">附件ID</param>
  /// <returns>操作结果</returns>
  [HttpDelete("attachment/{id}")]
  public async Task<LeanApiResult> DeleteAttachmentAsync(long id)
  {
    return await _emailService.DeleteAttachmentAsync(id);
  }

  /// <summary>
  /// 获取未读邮件数量
  /// </summary>
  /// <returns>未读邮件数量</returns>
  [HttpGet("unread/count")]
  public async Task<LeanApiResult<int>> GetUnreadCountAsync()
  {
    return await _emailService.GetUnreadCountAsync();
  }
}