using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Routine;

/// <summary>
/// 通知公告控制器
/// </summary>
[ApiController]
[Route("api/routine/notice")]
[Authorize]
public class LeanNoticeController : LeanBaseController
{
  private readonly ILeanNoticeService _noticeService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="noticeService">通知公告服务</param>
  public LeanNoticeController(ILeanNoticeService noticeService)
  {
    _noticeService = noticeService;
  }

  #region 基础操作

  /// <summary>
  /// 获取通知公告列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>通知公告列表</returns>
  [HttpGet("list")]
  [ProducesResponseType(typeof(LeanApiResult<List<LeanNoticeDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<List<LeanNoticeDto>>> GetListAsync([FromQuery] LeanNoticeQueryDto queryDto)
  {
    var result = await _noticeService.GetListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取通知公告分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  [HttpGet("page")]
  [ProducesResponseType(typeof(LeanApiResult<LeanPagedResult<LeanNoticeDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanPagedResult<LeanNoticeDto>>> GetPagedListAsync([FromQuery] LeanNoticeQueryDto queryDto)
  {
    var result = await _noticeService.GetPagedListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取通知公告详情
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>通知公告详情</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(LeanApiResult<LeanNoticeDto>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanNoticeDto>> GetByIdAsync(long id)
  {
    var result = await _noticeService.GetByIdAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 创建通知公告
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>通知公告ID</returns>
  [HttpPost]
  [ProducesResponseType(typeof(LeanApiResult<long>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<long>> CreateAsync([FromBody] LeanNoticeCreateDto createDto)
  {
    var result = await _noticeService.CreateAsync(createDto);
    return Success(result);
  }

  /// <summary>
  /// 更新通知公告
  /// </summary>
  /// <param name="updateDto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> UpdateAsync([FromBody] LeanNoticeUpdateDto updateDto)
  {
    var result = await _noticeService.UpdateAsync(updateDto);
    return Success(result);
  }

  /// <summary>
  /// 删除通知公告
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> DeleteAsync(long id)
  {
    var result = await _noticeService.DeleteAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 批量删除通知公告
  /// </summary>
  /// <param name="deleteDto">删除参数</param>
  /// <returns>是否成功</returns>
  [HttpDelete("batch")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> BatchDeleteAsync([FromBody] LeanNoticeBatchDeleteDto deleteDto)
  {
    var result = await _noticeService.BatchDeleteAsync(deleteDto);
    return Success(result);
  }

  /// <summary>
  /// 更新通知公告状态
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <param name="status">状态</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}/status")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> UpdateStatusAsync(long id, bool status)
  {
    var result = await _noticeService.UpdateStatusAsync(id, status);
    return Success(result, "更新状态成功");
  }

  #endregion

  #region 发布管理

  /// <summary>
  /// 发布通知公告
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}/publish")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> PublishAsync(long id)
  {
    var result = await _noticeService.PublishAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 撤回通知公告
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}/withdraw")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> WithdrawAsync(long id)
  {
    var result = await _noticeService.WithdrawAsync(id);
    return Success(result);
  }

  #endregion

  #region 通知管理

  /// <summary>
  /// 获取我的通知公告列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  [HttpGet("my")]
  [ProducesResponseType(typeof(LeanApiResult<LeanPagedResult<LeanNoticeDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanPagedResult<LeanNoticeDto>>> GetMyNoticesAsync([FromQuery] LeanNoticeQueryDto queryDto)
  {
    var result = await _noticeService.GetMyNoticesAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 标记通知公告已读
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}/read")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> MarkAsReadAsync(long id)
  {
    var result = await _noticeService.MarkAsReadAsync(id);
    return Success(result, "标记已读成功");
  }

  /// <summary>
  /// 批量标记通知公告已读
  /// </summary>
  /// <param name="ids">通知公告ID列表</param>
  /// <returns>是否成功</returns>
  [HttpPut("batch-read")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> BatchMarkAsReadAsync([FromBody] List<long> ids)
  {
    var result = await _noticeService.BatchMarkAsReadAsync(ids);
    return Success(result, "标记已读成功");
  }

  /// <summary>
  /// 获取未读通知公告数量
  /// </summary>
  /// <returns>未读数量</returns>
  [HttpGet("unread/count")]
  [ProducesResponseType(typeof(LeanApiResult<int>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<int>> GetUnreadCountAsync()
  {
    var result = await _noticeService.GetUnreadCountAsync();
    return Success(result);
  }

  #endregion

  #region 导入导出

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  [HttpGet("import/template")]
  [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _noticeService.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "通知公告导入模板.xlsx");
  }

  /// <summary>
  /// 导入通知公告
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [ProducesResponseType(typeof(LeanApiResult<LeanNoticeImportResultDto>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<LeanNoticeImportResultDto>> ImportAsync(IFormFile file)
  {
    var result = await _noticeService.ImportAsync(file);
    return Success(result, $"成功导入{result.SuccessCount}条数据，失败{result.FailureCount}条");
  }

  /// <summary>
  /// 导出通知公告
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>Excel文件</returns>
  [HttpGet("export")]
  [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanNoticeQueryDto queryDto)
  {
    var bytes = await _noticeService.ExportAsync(queryDto);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "通知公告列表.xlsx");
  }

  #endregion

  #region 附件管理

  /// <summary>
  /// 上传附件
  /// </summary>
  /// <param name="noticeId">通知公告ID</param>
  /// <param name="attachments">附件列表</param>
  /// <returns>操作结果</returns>
  [HttpPost("{noticeId}/attachments")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> UploadAttachments([FromRoute] long noticeId, [FromBody] List<LeanNoticeAttachmentCreateDto> attachments)
  {
    var result = await _noticeService.UploadAttachmentsAsync(noticeId, attachments);
    return Success(result, "上传成功");
  }

  /// <summary>
  /// 删除附件
  /// </summary>
  /// <param name="noticeId">通知公告ID</param>
  /// <param name="fileName">附件名称</param>
  /// <returns>操作结果</returns>
  [HttpDelete("{noticeId}/attachments/{fileName}")]
  [ProducesResponseType(typeof(LeanApiResult<bool>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<bool>> DeleteAttachment([FromRoute] long noticeId, [FromRoute] string fileName)
  {
    var result = await _noticeService.DeleteAttachmentAsync(noticeId, fileName);
    return Success(result, "删除成功");
  }

  /// <summary>
  /// 获取附件列表
  /// </summary>
  /// <param name="noticeId">通知公告ID</param>
  /// <returns>附件列表</returns>
  [HttpGet("{noticeId}/attachments")]
  [ProducesResponseType(typeof(LeanApiResult<List<LeanNoticeAttachmentCreateDto>>), StatusCodes.Status200OK)]
  public async Task<LeanApiResult<List<LeanNoticeAttachmentCreateDto>>> GetAttachments([FromRoute] long noticeId)
  {
    var result = await _noticeService.GetAttachmentsAsync(noticeId);
    return Success(result);
  }

  #endregion
}