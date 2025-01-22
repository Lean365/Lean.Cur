using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Routine;

/// <summary>
/// 通知公告管理相关的DTO定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了通知公告管理相关的所有DTO，包括：
/// - 基础操作相关DTO（查询、创建、更新、删除等）
/// - 导入导出相关DTO（数据导入、导出、模板等）
/// </remarks>

#region 基础操作

/// <summary>
/// 通知公告基本信息DTO
/// </summary>
/// <remarks>
/// 用于返回通知公告的基本信息，包含以下特点：
/// 1. 包含通知公告的所有基础字段
/// 2. 所有字段都是只读的，不包含验证特性
/// 3. 用于列表展示和详情展示
/// </remarks>
public class LeanNoticeDto
{
  /// <summary>
  /// 主键ID
  /// </summary>
  [Description("主键ID")]
  public long Id { get; set; }

  /// <summary>
  /// 通知标题
  /// </summary>
  [Description("通知标题")]
  public string NoticeTitle { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [Description("通知内容")]
  public string NoticeContent { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型
  /// </summary>
  [Description("通知类型")]
  public LeanNoticeType NoticeType { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [Description("状态")]
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 发布时间
  /// </summary>
  [Description("发布时间")]
  public DateTime PublishTime { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  [Description("是否已读")]
  public bool IsRead { get; set; }

  /// <summary>
  /// 附件名称
  /// </summary>
  [Description("附件名称")]
  public string? FileName { get; set; }

  /// <summary>
  /// 附件路径
  /// </summary>
  [Description("附件路径")]
  public string? FilePath { get; set; }

  /// <summary>
  /// 附件大小（字节）
  /// </summary>
  [Description("附件大小")]
  public long? FileSize { get; set; }

  /// <summary>
  /// 附件类型
  /// </summary>
  [Description("附件类型")]
  public string? FileType { get; set; }

  /// <summary>
  /// 上传时间
  /// </summary>
  [Description("上传时间")]
  public DateTime? UploadTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [Description("备注")]
  public string? Remark { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [Description("创建时间")]
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  [Description("更新时间")]
  public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 通知公告查询DTO
/// </summary>
/// <remarks>
/// 用于通知公告列表查询，包含以下特点：
/// 1. 继承自LeanPagedRequest，支持分页查询
/// 2. 包含多个查询条件字段
/// 3. 所有字段都是可选的
/// </remarks>
public class LeanNoticeQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [Description("通知标题")]
  public string? NoticeTitle { get; set; }

  /// <summary>
  /// 通知类型
  /// </summary>
  [Description("通知类型")]
  public LeanNoticeType? NoticeType { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [Description("状态")]
  public LeanStatus? Status { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  [Description("是否已读")]
  public bool? IsRead { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  [Description("开始时间")]
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [Description("结束时间")]
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 通知公告创建DTO
/// </summary>
/// <remarks>
/// 用于创建通知公告，包含以下特点：
/// 1. 包含所有必填字段
/// 2. 所有字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanNoticeCreateDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [Description("通知标题")]
  [Required(ErrorMessage = "通知标题不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "通知标题长度必须在2-100个字符之间")]
  public string NoticeTitle { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [Description("通知内容")]
  [Required(ErrorMessage = "通知内容不能为空")]
  public string NoticeContent { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型
  /// </summary>
  [Description("通知类型")]
  [Required(ErrorMessage = "通知类型不能为空")]
  public LeanNoticeType NoticeType { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [Description("状态")]
  public LeanStatus Status { get; set; } = LeanStatus.Normal;

  /// <summary>
  /// 发布时间
  /// </summary>
  [Description("发布时间")]
  public DateTime? PublishTime { get; set; }

  /// <summary>
  /// 附件名称
  /// </summary>
  [Description("附件名称")]
  [StringLength(200, ErrorMessage = "附件名称长度不能超过200个字符")]
  public string? FileName { get; set; }

  /// <summary>
  /// 附件路径
  /// </summary>
  [Description("附件路径")]
  [StringLength(500, ErrorMessage = "附件路径长度不能超过500个字符")]
  public string? FilePath { get; set; }

  /// <summary>
  /// 附件大小（字节）
  /// </summary>
  [Description("附件大小")]
  [Range(0, long.MaxValue, ErrorMessage = "附件大小必须大于等于0")]
  public long? FileSize { get; set; }

  /// <summary>
  /// 附件类型
  /// </summary>
  [Description("附件类型")]
  [StringLength(50, ErrorMessage = "附件类型长度不能超过50个字符")]
  public string? FileType { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [Description("备注")]
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }
}

/// <summary>
/// 通知公告附件创建DTO
/// </summary>
/// <remarks>
/// 用于创建通知公告附件，包含以下特点：
/// 1. 包含所有必填字段
/// 2. 所有字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanNoticeAttachmentCreateDto
{
  /// <summary>
  /// 附件名称
  /// </summary>
  [Description("附件名称")]
  [Required(ErrorMessage = "附件名称不能为空")]
  [StringLength(100, ErrorMessage = "附件名称长度不能超过100个字符")]
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// 附件路径
  /// </summary>
  [Description("附件路径")]
  [Required(ErrorMessage = "附件路径不能为空")]
  [StringLength(500, ErrorMessage = "附件路径长度不能超过500个字符")]
  public string FilePath { get; set; } = string.Empty;

  /// <summary>
  /// 附件大小（字节）
  /// </summary>
  [Description("附件大小")]
  [Required(ErrorMessage = "附件大小不能为空")]
  [Range(1, long.MaxValue, ErrorMessage = "附件大小必须大于0")]
  public long FileSize { get; set; }

  /// <summary>
  /// 附件类型
  /// </summary>
  [Description("附件类型")]
  [Required(ErrorMessage = "附件类型不能为空")]
  [StringLength(50, ErrorMessage = "附件类型长度不能超过50个字符")]
  public string FileType { get; set; } = string.Empty;
}

/// <summary>
/// 通知公告更新DTO
/// </summary>
/// <remarks>
/// 用于更新通知公告，包含以下特点：
/// 1. 继承自创建DTO，包含所有创建时的字段
/// 2. 增加了ID字段
/// 3. 所有字段都有验证特性
/// </remarks>
public class LeanNoticeUpdateDto : LeanNoticeCreateDto
{
  /// <summary>
  /// 主键ID
  /// </summary>
  [Description("主键ID")]
  [Required(ErrorMessage = "通知公告ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 通知公告批量删除DTO
/// </summary>
public class LeanNoticeBatchDeleteDto
{
  /// <summary>
  /// 通知公告ID列表
  /// </summary>
  [Description("通知公告ID列表")]
  [Required(ErrorMessage = "通知公告ID列表不能为空")]
  public List<long> Ids { get; set; } = new();
}

#endregion

#region 导入导出

/// <summary>
/// 通知公告导入DTO
/// </summary>
/// <remarks>
/// 用于Excel导入通知公告数据，包含以下特点：
/// 1. 包含所有可导入的字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanNoticeImportDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [Required(ErrorMessage = "通知标题不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "通知标题长度必须在2-100个字符之间")]
  public string NoticeTitle { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [Required(ErrorMessage = "通知内容不能为空")]
  public string NoticeContent { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型
  /// </summary>
  [Required(ErrorMessage = "通知类型不能为空")]
  public LeanNoticeType NoticeType { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [Required(ErrorMessage = "状态不能为空")]
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 发布时间
  /// </summary>
  public DateTime? PublishTime { get; set; }

  /// <summary>
  /// 附件名称
  /// </summary>
  [StringLength(100, ErrorMessage = "附件名称长度不能超过100个字符")]
  public string? FileName { get; set; }

  /// <summary>
  /// 附件路径
  /// </summary>
  [StringLength(500, ErrorMessage = "附件路径长度不能超过500个字符")]
  public string? FilePath { get; set; }

  /// <summary>
  /// 附件大小（字节）
  /// </summary>
  [Range(0, long.MaxValue, ErrorMessage = "附件大小必须大于等于0")]
  public long? FileSize { get; set; }

  /// <summary>
  /// 附件类型
  /// </summary>
  [StringLength(50, ErrorMessage = "附件类型长度不能超过50个字符")]
  public string? FileType { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }

  /// <summary>
  /// 错误信息
  /// </summary>
  public string? ErrorMessage { get; set; }
}

/// <summary>
/// 通知公告导出DTO
/// </summary>
/// <remarks>
/// 用于Excel导出通知公告数据，包含以下特点：
/// 1. 所有字段都是string类型，便于Excel格式化
/// 2. 包含所有需要导出的字段
/// 3. 时间字段会被格式化为指定格式
/// </remarks>
public class LeanNoticeExportDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  public string NoticeTitle { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  public string NoticeContent { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型
  /// </summary>
  public string NoticeType { get; set; } = string.Empty;

  /// <summary>
  /// 状态
  /// </summary>
  public string Status { get; set; } = string.Empty;

  /// <summary>
  /// 发布时间
  /// </summary>
  public string? PublishTime { get; set; }

  /// <summary>
  /// 发布人
  /// </summary>
  public string? Publisher { get; set; }

  /// <summary>
  /// 附件名称
  /// </summary>
  public string? FileName { get; set; }

  /// <summary>
  /// 附件路径
  /// </summary>
  public string? FilePath { get; set; }

  /// <summary>
  /// 附件大小
  /// </summary>
  public string? FileSize { get; set; }

  /// <summary>
  /// 附件类型
  /// </summary>
  public string? FileType { get; set; }

  /// <summary>
  /// 上传时间
  /// </summary>
  public string? UploadTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public string CreateTime { get; set; } = string.Empty;

  /// <summary>
  /// 更新时间
  /// </summary>
  public string? UpdateTime { get; set; }
}

/// <summary>
/// 通知公告导入结果DTO
/// </summary>
/// <remarks>
/// 用于返回导入结果，包含以下特点：
/// 1. 包含导入的统计信息
/// 2. 包含详细的错误信息
/// </remarks>
public class LeanNoticeImportResultDto
{
  /// <summary>
  /// 总数
  /// </summary>
  public int TotalCount { get; set; }

  /// <summary>
  /// 成功数
  /// </summary>
  public int SuccessCount { get; set; }

  /// <summary>
  /// 失败数
  /// </summary>
  public int FailureCount { get; set; }

  /// <summary>
  /// 失败记录列表
  /// </summary>
  public List<LeanNoticeImportDto> FailureItems { get; set; } = new();
}

#endregion