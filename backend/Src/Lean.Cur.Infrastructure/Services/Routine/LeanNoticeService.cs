using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Entities.Routine;
using Lean.Cur.Application.Services.Admin;
using Mapster;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Infrastructure.Services.Routine;

/// <summary>
/// 通知公告服务实现
/// </summary>
public class LeanNoticeService : ILeanNoticeService
{
  private readonly SqlSugarClient _db;
  private readonly LeanExcelHelper _excel;
  private readonly ILeanUserService _userService;
  private readonly ILeanNoticePushService _noticePushService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">数据库访问对象</param>
  /// <param name="excel">Excel操作对象</param>
  /// <param name="userService">用户服务</param>
  /// <param name="noticePushService">通知推送服务</param>
  public LeanNoticeService(SqlSugarClient db, LeanExcelHelper excel, ILeanUserService userService, ILeanNoticePushService noticePushService)
  {
    _db = db;
    _excel = excel;
    _userService = userService;
    _noticePushService = noticePushService;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<List<LeanNoticeDto>> GetListAsync(LeanNoticeQueryDto queryDto)
  {
    var query = _db.Queryable<LeanNotice>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.NoticeTitle), n => n.NoticeTitle.Contains(queryDto.NoticeTitle!))
        .WhereIF(queryDto.NoticeType.HasValue, n => n.NoticeType == queryDto.NoticeType)
        .WhereIF(queryDto.Status.HasValue, n => n.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, n => n.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, n => n.CreateTime <= queryDto.EndTime)
        .OrderByDescending(n => n.CreateTime);

    var list = await query.ToListAsync();
    return list.Adapt<List<LeanNoticeDto>>();
  }

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanNoticeDto>> GetPagedListAsync(LeanNoticeQueryDto queryDto)
  {
    var query = _db.Queryable<LeanNotice>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.NoticeTitle), n => n.NoticeTitle.Contains(queryDto.NoticeTitle!))
        .WhereIF(queryDto.NoticeType.HasValue, n => n.NoticeType == queryDto.NoticeType)
        .WhereIF(queryDto.Status.HasValue, n => n.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, n => n.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, n => n.CreateTime <= queryDto.EndTime)
        .OrderByDescending(n => n.CreateTime);

    var result = await query.ToPagedListAsync(queryDto);
    var dtos = result.Items.Adapt<List<LeanNoticeDto>>();
    return new LeanPagedResult<LeanNoticeDto>(dtos, result.Total, result.PageIndex, result.PageSize);
  }

  /// <inheritdoc/>
  public async Task<LeanNoticeDto> GetByIdAsync(long id)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == id) ?? throw new BusinessException("通知公告不存在");

    return notice.Adapt<LeanNoticeDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanNoticeCreateDto createDto)
  {
    var notice = createDto.Adapt<LeanNotice>();
    notice.CreateTime = DateTime.Now;
    if (!string.IsNullOrEmpty(notice.FileName))
    {
      notice.UploadTime = DateTime.Now;
    }

    return await _db.Insertable(notice).ExecuteReturnIdentityAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanNoticeUpdateDto updateDto)
  {
    var entity = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == updateDto.Id) ?? throw new BusinessException("通知公告不存在");

    updateDto.Adapt(entity);
    entity.UpdateTime = DateTime.Now;

    return await _db.Updateable(entity).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == id) ?? throw new BusinessException("通知公告不存在");

    return await _db.Deleteable(notice).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchDeleteAsync(LeanNoticeBatchDeleteDto deleteDto)
  {
    if (deleteDto.Ids == null || !deleteDto.Ids.Any())
    {
      throw new BusinessException("请选择要删除的通知公告");
    }

    var notices = await _db.Queryable<LeanNotice>()
        .Where(n => deleteDto.Ids.Contains(n.Id))
        .ToListAsync();

    if (!notices.Any())
    {
      throw new BusinessException("所选通知公告不存在");
    }

    return await _db.Deleteable<LeanNotice>()
        .Where(n => deleteDto.Ids.Contains(n.Id))
        .ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateStatusAsync(long id, bool status)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == id) ?? throw new BusinessException("通知公告不存在");

    notice.Status = status ? LeanStatus.Normal : LeanStatus.Disabled;
    notice.UpdateTime = DateTime.Now;

    var result = await _db.Updateable(notice).ExecuteCommandHasChangeAsync();
    if (result && notice.Status == LeanStatus.Normal)
    {
      // 如果启用通知，则推送给所有用户
      await _noticePushService.PushToAllAsync(notice.Adapt<LeanNoticeDto>());
    }

    return result;
  }

  #endregion 基础操作

  #region 发布管理

  /// <inheritdoc/>
  public async Task<bool> PublishAsync(long id)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == id) ?? throw new BusinessException("通知公告不存在");

    if (notice.Status == LeanStatus.Normal)
    {
      throw new BusinessException("通知公告已发布");
    }

    notice.Status = LeanStatus.Normal;
    notice.PublishTime = DateTime.Now;
    notice.UpdateTime = DateTime.Now;

    var result = await _db.Updateable(notice).ExecuteCommandHasChangeAsync();
    if (result)
    {
      // 发送通知
      await _noticePushService.PushToAllAsync(notice.Adapt<LeanNoticeDto>());
    }

    return result;
  }

  /// <inheritdoc/>
  public async Task<bool> WithdrawAsync(long id)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == id) ?? throw new BusinessException("通知公告不存在");

    if (notice.Status != LeanStatus.Normal)
    {
      throw new BusinessException("通知公告未发布");
    }

    notice.Status = LeanStatus.Disabled;
    notice.UpdateTime = DateTime.Now;

    return await _db.Updateable(notice).ExecuteCommandHasChangeAsync();
  }

  #endregion 发布管理

  #region 通知管理

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanNoticeDto>> GetMyNoticesAsync(LeanNoticeQueryDto queryDto)
  {
    var currentUser = await _userService.GetCurrentUserAsync();

    var query = _db.Queryable<LeanNotice>()
        .Where(n => n.Status == LeanStatus.Normal) // 只查询已发布的通知
        .WhereIF(!string.IsNullOrEmpty(queryDto.NoticeTitle), n => n.NoticeTitle.Contains(queryDto.NoticeTitle!))
        .WhereIF(queryDto.NoticeType.HasValue, n => n.NoticeType == queryDto.NoticeType)
        .WhereIF(queryDto.StartTime.HasValue, n => n.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, n => n.CreateTime <= queryDto.EndTime)
        .OrderByDescending(n => n.CreateTime);

    var result = await query.ToPagedListAsync(queryDto);
    var dtos = result.Items.Adapt<List<LeanNoticeDto>>();
    return new LeanPagedResult<LeanNoticeDto>(dtos, result.Total, result.PageIndex, result.PageSize);
  }

  /// <inheritdoc/>
  public async Task<bool> MarkAsReadAsync(long id)
  {
    var currentUser = await _userService.GetCurrentUserAsync();
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == id) ?? throw new BusinessException("通知公告不存在");

    if (notice.Status != LeanStatus.Normal)
    {
      throw new BusinessException("通知公告未发布");
    }

    var read = new LeanNoticeRead
    {
      NoticeId = id,
      UserId = currentUser.Id,
      ReadTime = DateTime.Now,
      CreateTime = DateTime.Now
    };

    return await _db.Insertable(read).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchMarkAsReadAsync(List<long> ids)
  {
    if (ids == null || !ids.Any())
    {
      throw new BusinessException("请选择要标记的通知公告");
    }

    var currentUser = await _userService.GetCurrentUserAsync();
    var notices = await _db.Queryable<LeanNotice>()
        .Where(n => ids.Contains(n.Id))
        .Where(n => n.Status == LeanStatus.Normal)
        .ToListAsync();

    if (!notices.Any())
    {
      throw new BusinessException("所选通知公告不存在或未发布");
    }

    var reads = notices.Select(n => new LeanNoticeRead
    {
      NoticeId = n.Id,
      UserId = currentUser.Id,
      ReadTime = DateTime.Now,
      CreateTime = DateTime.Now
    }).ToList();

    return await _db.Insertable(reads).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<int> GetUnreadCountAsync()
  {
    var currentUser = await _userService.GetCurrentUserAsync();

    // 查询已发布的通知总数
    var totalCount = await _db.Queryable<LeanNotice>()
        .Where(n => n.Status == LeanStatus.Normal)
        .CountAsync();

    // 查询已读的通知数
    var readCount = await _db.Queryable<LeanNoticeRead>()
        .Where(r => r.UserId == currentUser.Id)
        .CountAsync();

    return totalCount - readCount;
  }

  #endregion 通知管理

  #region 导入导出

  /// <inheritdoc/>
  public async Task<byte[]> GetImportTemplateAsync()
  {
    var headers = new Dictionary<string, string>
    {
      { "NoticeTitle", "通知标题" },
      { "NoticeContent", "通知内容" },
      { "NoticeType", "通知类型" },
      { "Status", "状态" },
      { "PublishTime", "发布时间" },
      { "FileName", "附件名称" },
      { "FilePath", "附件路径" },
      { "FileSize", "附件大小" },
      { "FileType", "附件类型" },
      { "Remark", "备注" }
    };

    return await _excel.GenerateTemplateAsync(headers);
  }

  /// <inheritdoc/>
  public async Task<LeanNoticeImportResultDto> ImportAsync(IFormFile file)
  {
    var result = new LeanNoticeImportResultDto();
    var items = _excel.Import<LeanNoticeImportDto>(file);
    result.TotalCount = items.Count;

    foreach (var item in items)
    {
      try
      {
        var notice = new LeanNotice
        {
          NoticeTitle = item.NoticeTitle,
          NoticeContent = item.NoticeContent,
          NoticeType = item.NoticeType,
          Status = item.Status,
          PublishTime = item.PublishTime ?? DateTime.Now,
          FileName = item.FileName ?? string.Empty,
          FilePath = item.FilePath ?? string.Empty,
          FileSize = item.FileSize,
          FileType = item.FileType ?? string.Empty,
          Remark = item.Remark ?? string.Empty,
          CreateTime = DateTime.Now,
          UpdateTime = DateTime.Now
        };

        if (!string.IsNullOrEmpty(notice.FileName))
        {
          notice.UploadTime = DateTime.Now;
        }

        await _db.Insertable(notice).ExecuteCommandAsync();
        result.SuccessCount++;
      }
      catch (Exception ex)
      {
        item.ErrorMessage = ex.Message;
        result.FailureItems.Add(item);
        result.FailureCount++;
      }
    }

    return result;
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(LeanNoticeQueryDto queryDto)
  {
    var query = _db.Queryable<LeanNotice>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.NoticeTitle), n => n.NoticeTitle.Contains(queryDto.NoticeTitle))
        .WhereIF(queryDto.NoticeType.HasValue, n => n.NoticeType == queryDto.NoticeType)
        .WhereIF(queryDto.Status.HasValue, n => n.Status == queryDto.Status)
        .WhereIF(queryDto.StartTime.HasValue, n => n.PublishTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, n => n.PublishTime <= queryDto.EndTime);

    var list = await query.Select(n => new LeanNoticeExportDto
    {
      NoticeTitle = n.NoticeTitle,
      NoticeContent = n.NoticeContent,
      NoticeType = n.NoticeType.ToString(),
      Status = n.Status.ToString(),
      PublishTime = n.PublishTime.ToString("yyyy-MM-dd HH:mm:ss"),
      Publisher = n.Publisher ?? string.Empty,
      FileName = n.FileName ?? string.Empty,
      FilePath = n.FilePath ?? string.Empty,
      FileSize = n.FileSize.HasValue ? n.FileSize.Value.ToString("N0") : string.Empty,
      FileType = n.FileType ?? string.Empty,
      UploadTime = n.UploadTime.HasValue ? n.UploadTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
      Remark = n.Remark ?? string.Empty,
      CreateTime = n.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
      UpdateTime = n.UpdateTime.HasValue ? n.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty
    }).ToListAsync();

    var headers = new Dictionary<string, string>
    {
        { "NoticeTitle", "通知标题" },
        { "NoticeContent", "通知内容" },
        { "NoticeType", "通知类型" },
        { "Status", "状态" },
        { "PublishTime", "发布时间" },
        { "Publisher", "发布人" },
        { "FileName", "附件名称" },
        { "FilePath", "附件路径" },
        { "FileSize", "附件大小" },
        { "FileType", "附件类型" },
        { "UploadTime", "上传时间" },
        { "Remark", "备注" },
        { "CreateTime", "创建时间" },
        { "UpdateTime", "更新时间" }
    };

    return await _excel.ExportAsync(headers, list);
  }

  #endregion 导入导出

  #region 附件管理

  /// <inheritdoc/>
  public async Task<bool> UploadAttachmentsAsync(long noticeId, List<LeanNoticeAttachmentCreateDto> attachments)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == noticeId) ?? throw new BusinessException("通知公告不存在");

    // 已发布的通知不能修改附件
    if (notice.Status == LeanStatus.Normal)
    {
      throw new BusinessException("已发布的通知公告不能修改附件");
    }

    // 更新通知的附件信息
    notice.FileName = attachments.FirstOrDefault()?.FileName;
    notice.FilePath = attachments.FirstOrDefault()?.FilePath;
    notice.FileSize = attachments.FirstOrDefault()?.FileSize;
    notice.FileType = attachments.FirstOrDefault()?.FileType;
    notice.UploadTime = DateTime.Now;
    notice.UpdateTime = DateTime.Now;

    return await _db.Updateable(notice).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAttachmentAsync(long noticeId, string fileName)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == noticeId) ?? throw new BusinessException("通知公告不存在");

    // 已发布的通知不能删除附件
    if (notice.Status == LeanStatus.Normal)
    {
      throw new BusinessException("已发布的通知公告不能删除附件");
    }

    // 检查附件是否存在
    if (notice.FileName != fileName)
    {
      throw new BusinessException("附件不存在");
    }

    // 清空附件信息
    notice.FileName = null;
    notice.FilePath = null;
    notice.FileSize = null;
    notice.FileType = null;
    notice.UploadTime = null;
    notice.UpdateTime = DateTime.Now;

    return await _db.Updateable(notice).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<List<LeanNoticeAttachmentCreateDto>> GetAttachmentsAsync(long noticeId)
  {
    var notice = await _db.Queryable<LeanNotice>()
        .FirstAsync(n => n.Id == noticeId) ?? throw new BusinessException("通知公告不存在");

    if (string.IsNullOrEmpty(notice.FileName))
    {
      return new List<LeanNoticeAttachmentCreateDto>();
    }

    return new List<LeanNoticeAttachmentCreateDto>
    {
      new LeanNoticeAttachmentCreateDto
      {
        FileName = notice.FileName,
        FilePath = notice.FilePath,
        FileSize = notice.FileSize ?? 0,
        FileType = notice.FileType
      }
    };
  }

  #endregion 附件管理
}