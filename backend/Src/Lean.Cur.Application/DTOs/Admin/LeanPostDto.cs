using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Admin;

/// <summary>
/// 岗位管理相关的DTO定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了岗位管理相关的所有DTO，包括：
/// - 基础操作相关DTO（查询、创建、更新、删除等）
/// - 导入导出相关DTO（数据导入、导出、模板等）
/// </remarks>

#region 基础操作

/// <summary>
/// 岗位基本信息DTO
/// </summary>
/// <remarks>
/// 用于返回岗位的基本信息，包含以下特点：
/// 1. 包含岗位的所有基础字段
/// 2. 所有字段都是只读的，不包含验证特性
/// 3. 用于列表展示和详情展示
/// </remarks>
public class LeanPostDto
{
    /// <summary>
    /// 岗位ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 岗位名称
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 岗位查询DTO
/// </summary>
/// <remarks>
/// 用于岗位列表的查询条件，包含以下特点：
/// 1. 继承自PagedRequest，支持分页查询
/// 2. 所有查询条件都是可选的
/// 3. 支持按时间范围查询
/// </remarks>
public class LeanPostQueryDto : PagedRequest
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    public string? PostName { get; set; }

    /// <summary>
    /// 岗位编码
    /// </summary>
    public string? PostCode { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus? Status { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}

/// <summary>
/// 岗位创建DTO
/// </summary>
/// <remarks>
/// 用于创建新岗位，包含以下特点：
/// 1. 包含创建岗位所需的所有必要字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanPostCreateDto
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一租户下唯一
    /// </remarks>
    [Required(ErrorMessage = "岗位名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// 5. 创建后不可修改
    /// </remarks>
    [Required(ErrorMessage = "岗位编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "岗位编码只能包含字母、数字和下划线")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 值越小越靠前
    /// 3. 默认值：0
    /// </remarks>
    [Required(ErrorMessage = "显示顺序不能为空")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 默认值：正常
    /// </remarks>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大500个字符
    /// </remarks>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 岗位更新DTO
/// </summary>
/// <remarks>
/// 用于更新现有岗位，包含以下特点：
/// 1. 包含可以更新的所有字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// 4. 不允许修改岗位编码
/// </remarks>
public class LeanPostUpdateDto
{
    /// <summary>
    /// 岗位ID
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 必须大于0
    /// </remarks>
    [Required(ErrorMessage = "岗位ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 岗位名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一租户下唯一
    /// </remarks>
    [Required(ErrorMessage = "岗位名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// </remarks>
    [Required(ErrorMessage = "岗位编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "岗位编码只能包含字母、数字和下划线")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 值越小越靠前
    /// </remarks>
    [Required(ErrorMessage = "显示顺序不能为空")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// </remarks>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大500个字符
    /// </remarks>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 岗位状态DTO
/// </summary>
/// <remarks>
/// 用于更新岗位状态，包含以下特点：
/// 1. 只包含ID和状态字段
/// 2. 所有字段都有验证特性
/// </remarks>
public class LeanPostStatusDto
{
    /// <summary>
    /// 岗位ID
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 必须大于0
    /// </remarks>
    [Required(ErrorMessage = "岗位ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// </remarks>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 岗位批量删除DTO
/// </summary>
/// <remarks>
/// 用于批量删除岗位，包含以下特点：
/// 1. 只包含ID列表
/// 2. ID列表不能为空
/// </remarks>
public class LeanPostBatchDeleteDto
{
    /// <summary>
    /// 岗位ID列表
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 不能为空列表
    /// </remarks>
    [Required(ErrorMessage = "岗位ID列表不能为空")]
    public List<long> Ids { get; set; } = new();
}

#endregion

#region 导入导出

/// <summary>
/// 岗位导入DTO
/// </summary>
/// <remarks>
/// 用于Excel导入岗位数据，包含以下特点：
/// 1. 包含所有可导入的字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanPostImportDto
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一租户下唯一
    /// </remarks>
    [Required(ErrorMessage = "岗位名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// </remarks>
    [Required(ErrorMessage = "岗位编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "岗位编码只能包含字母、数字和下划线")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 值越小越靠前
    /// </remarks>
    [Required(ErrorMessage = "显示顺序不能为空")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 默认值：正常
    /// </remarks>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大500个字符
    /// </remarks>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 岗位导出DTO
/// </summary>
/// <remarks>
/// 用于Excel导出岗位数据，包含以下特点：
/// 1. 所有字段都是string类型，便于Excel格式化
/// 2. 包含所有需要导出的字段
/// 3. 时间字段会被格式化为指定格式
/// </remarks>
public class LeanPostExportDto
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 显示顺序
    /// </summary>
    public string OrderNum { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 会被格式化为中文：正常、停用
    /// </remarks>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    /// <remarks>
    /// 格式：yyyy-MM-dd HH:mm:ss
    /// </remarks>
    public string CreateTime { get; set; } = string.Empty;

    /// <summary>
    /// 更新时间
    /// </summary>
    /// <remarks>
    /// 格式：yyyy-MM-dd HH:mm:ss
    /// </remarks>
    public string? UpdateTime { get; set; }
}

/// <summary>
/// 岗位导入结果DTO
/// </summary>
/// <remarks>
/// 用于返回导入结果，包含以下特点：
/// 1. 包含导入的统计信息
/// 2. 包含详细的错误信息
/// </remarks>
public class LeanPostImportResultDto
{
    /// <summary>
    /// 总记录数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 成功记录数
    /// </summary>
    public int SuccessCount { get; set; }

    /// <summary>
    /// 失败记录数
    /// </summary>
    public int FailureCount { get; set; }

    /// <summary>
    /// 失败记录列表
    /// </summary>
    public List<LeanPostImportDto> FailureItems { get; set; } = new();
}

/// <summary>
/// 岗位导入模板DTO
/// </summary>
/// <remarks>
/// 用于生成导入模板，包含以下特点：
/// 1. 包含所有可导入字段的示例值
/// 2. 包含字段说明和验证规则
/// </remarks>
public class LeanPostImportTemplateDto
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    /// <remarks>
    /// 示例值：技术总监
    /// 必填，2-50个字符
    /// </remarks>
    public string PostName { get; set; } = "技术总监";

    /// <summary>
    /// 岗位编码
    /// </summary>
    /// <remarks>
    /// 示例值：tech_director
    /// 必填，2-50个字符，只能包含字母、数字和下划线
    /// </remarks>
    public string PostCode { get; set; } = "tech_director";

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 示例值：1
    /// 必填，数字，值越小越靠前
    /// </remarks>
    public string OrderNum { get; set; } = "1";

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 示例值：正常
    /// 必填，可选值：正常、停用
    /// </remarks>
    public string Status { get; set; } = "正常";

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>
    /// 示例值：负责技术团队管理
    /// 选填，最多500个字符
    /// </remarks>
    public string? Remark { get; set; } = "负责技术团队管理";
}

#endregion 