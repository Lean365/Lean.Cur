using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// 通知公告服务接口
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本接口定义了通知公告相关的所有服务方法，包括：
/// 1. 基础操作（查询、创建、更新、删除等）
/// 2. 发布管理（发布、撤回）
/// 3. 通知管理（我的通知、已读未读等）
/// 4. 导入导出功能
/// </remarks>
public interface ILeanNoticeService
{
  #region 基础操作

  /// <summary>
  /// 获取通知公告列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>通知公告列表</returns>
  Task<List<LeanNoticeDto>> GetListAsync(LeanNoticeQueryDto queryDto);

  /// <summary>
  /// 获取通知公告分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  Task<LeanPagedResult<LeanNoticeDto>> GetPagedListAsync(LeanNoticeQueryDto queryDto);

  /// <summary>
  /// 获取通知公告详情
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>通知公告详情</returns>
  Task<LeanNoticeDto> GetByIdAsync(long id);

  /// <summary>
  /// 创建通知公告
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>通知公告ID</returns>
  Task<long> CreateAsync(LeanNoticeCreateDto createDto);

  /// <summary>
  /// 更新通知公告
  /// </summary>
  /// <param name="updateDto">更新参数</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateAsync(LeanNoticeUpdateDto updateDto);

  /// <summary>
  /// 删除通知公告
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 批量删除通知公告
  /// </summary>
  /// <param name="deleteDto">删除参数</param>
  /// <returns>是否成功</returns>
  Task<bool> BatchDeleteAsync(LeanNoticeBatchDeleteDto deleteDto);

  /// <summary>
  /// 更新通知公告状态
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <param name="status">状态</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateStatusAsync(long id, bool status);

  #endregion

  #region 发布管理

  /// <summary>
  /// 发布通知公告
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  Task<bool> PublishAsync(long id);

  /// <summary>
  /// 撤回通知公告
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  Task<bool> WithdrawAsync(long id);

  #endregion

  #region 通知管理

  /// <summary>
  /// 获取我的通知公告列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  Task<LeanPagedResult<LeanNoticeDto>> GetMyNoticesAsync(LeanNoticeQueryDto queryDto);

  /// <summary>
  /// 标记通知公告已读
  /// </summary>
  /// <param name="id">通知公告ID</param>
  /// <returns>是否成功</returns>
  Task<bool> MarkAsReadAsync(long id);

  /// <summary>
  /// 批量标记通知公告已读
  /// </summary>
  /// <param name="ids">通知公告ID列表</param>
  /// <returns>是否成功</returns>
  Task<bool> BatchMarkAsReadAsync(List<long> ids);

  /// <summary>
  /// 获取未读通知公告数量
  /// </summary>
  /// <returns>未读数量</returns>
  Task<int> GetUnreadCountAsync();

  #endregion

  #region 导入导出

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  Task<byte[]> GetImportTemplateAsync();

  /// <summary>
  /// 导入通知公告
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  Task<LeanNoticeImportResultDto> ImportAsync(IFormFile file);

  /// <summary>
  /// 导出通知公告
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>导出文件</returns>
  Task<byte[]> ExportAsync(LeanNoticeQueryDto queryDto);

  #endregion

  #region 附件管理

  /// <summary>
  /// 上传附件
  /// </summary>
  /// <param name="noticeId">通知公告ID</param>
  /// <param name="attachments">附件列表</param>
  /// <returns>是否成功</returns>
  Task<bool> UploadAttachmentsAsync(long noticeId, List<LeanNoticeAttachmentCreateDto> attachments);

  /// <summary>
  /// 删除附件
  /// </summary>
  /// <param name="noticeId">通知公告ID</param>
  /// <param name="fileName">附件名称</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAttachmentAsync(long noticeId, string fileName);

  /// <summary>
  /// 获取附件列表
  /// </summary>
  /// <param name="noticeId">通知公告ID</param>
  /// <returns>附件列表</returns>
  Task<List<LeanNoticeAttachmentCreateDto>> GetAttachmentsAsync(long noticeId);

  #endregion
}