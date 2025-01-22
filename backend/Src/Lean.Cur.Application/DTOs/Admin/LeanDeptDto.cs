using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Admin;

/// <summary>
/// 部门管理相关的DTO定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了部门管理相关的所有DTO，包括：
/// - 基础操作相关DTO（查询、创建、更新、删除等）
/// - 导入导出相关DTO（数据导入、导出、模板等）
/// </remarks>

#region 基础操作

/// <summary>
/// 部门基本信息DTO
/// </summary>
/// <remarks>
/// 用于返回部门的基本信息，包含以下特点：
/// 1. 包含部门的所有基础字段
/// 2. 所有字段都是只读的，不包含验证特性
/// 3. 用于列表展示和详情展示
/// </remarks>
public class LeanDeptDto
{
    /// <summary>
    /// 部门ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 子部门列表
    /// </summary>
    public List<LeanDeptDto> Children { get; set; } = new();
}

/// <summary>
/// 部门查询DTO
/// </summary>
/// <remarks>
/// 用于部门列表的查询条件，包含以下特点：
/// 1. 继承自LeanLeanPagedRequestLeanPagedRequest，支持分页查询
/// 2. 所有查询条件都是可选的
/// 3. 支持按时间范围查询
/// </remarks>
public class LeanDeptQueryDto : LeanPagedRequest
{
    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 部门编码
    /// </summary>
    public string? DeptCode { get; set; }

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
/// 部门创建DTO
/// </summary>
/// <remarks>
/// 用于创建新部门，包含以下特点：
/// 1. 包含创建部门所需的所有必要字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanDeptCreateDto
{
    /// <summary>
    /// 父级ID
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 默认值：0（表示顶级部门）
    /// </remarks>
    [Required(ErrorMessage = "父级ID不能为空")]
    public long ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一父级下唯一
    /// </remarks>
    [Required(ErrorMessage = "部门名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// 5. 创建后不可修改
    /// </remarks>
    [Required(ErrorMessage = "部门编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "部门编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "部门编码只能包含字母、数字和下划线")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大50个字符
    /// </remarks>
    [StringLength(50, ErrorMessage = "负责人长度不能超过50个字符")]
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大20个字符
    /// </remarks>
    [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大50个字符
    /// </remarks>
    [StringLength(50, ErrorMessage = "邮箱长度不能超过50个字符")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

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
}

/// <summary>
/// 部门更新DTO
/// </summary>
/// <remarks>
/// 用于更新现有部门，包含以下特点：
/// 1. 包含可以更新的所有字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// 4. 不允许修改部门编码
/// </remarks>
public class LeanDeptUpdateDto
{
    /// <summary>
    /// 部门ID
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 必须大于0
    /// </remarks>
    [Required(ErrorMessage = "部门ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 不能是自己或自己的子部门
    /// </remarks>
    [Required(ErrorMessage = "父级ID不能为空")]
    public long ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一父级下唯一
    /// </remarks>
    [Required(ErrorMessage = "部门名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大50个字符
    /// </remarks>
    [StringLength(50, ErrorMessage = "负责人长度不能超过50个字符")]
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大20个字符
    /// </remarks>
    [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大50个字符
    /// </remarks>
    [StringLength(50, ErrorMessage = "邮箱长度不能超过50个字符")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

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
}

/// <summary>
/// 部门状态DTO
/// </summary>
/// <remarks>
/// 用于更新部门状态，包含以下特点：
/// 1. 只包含ID和状态两个字段
/// 2. 所有字段都是必填的
/// </remarks>
public class LeanDeptStatusDto
{
    /// <summary>
    /// 部门ID
    /// </summary>
    [Required(ErrorMessage = "部门ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 部门批量删除DTO
/// </summary>
/// <remarks>
/// 用于批量删除部门，包含以下特点：
/// 1. 只包含ID集合
/// 2. ID集合不能为空
/// </remarks>
public class LeanDeptBatchDeleteDto
{
    /// <summary>
    /// 部门ID集合
    /// </summary>
    [Required(ErrorMessage = "部门ID集合不能为空")]
    public List<long> Ids { get; set; } = new();
}

#endregion 基础操作

#region 导入导出

/// <summary>
/// 部门导入DTO
/// </summary>
/// <remarks>
/// 用于从Excel导入部门数据，包含以下特点：
/// 1. 包含所有可以导入的字段
/// 2. 必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanDeptImportDto
{
    /// <summary>
    /// 上级部门名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 必须是已存在的部门名称
    /// </remarks>
    [Required(ErrorMessage = "上级部门名称不能为空")]
    public string ParentDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    [Required(ErrorMessage = "部门名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    [Required(ErrorMessage = "部门编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "部门编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "部门编码只能包含字母、数字和下划线")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    [StringLength(50, ErrorMessage = "负责人长度不能超过50个字符")]
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [StringLength(50, ErrorMessage = "邮箱长度不能超过50个字符")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    [Required(ErrorMessage = "显示顺序不能为空")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    /// <remarks>
    /// 用于记录导入过程中的错误信息
    /// </remarks>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 部门导出DTO
/// </summary>
/// <remarks>
/// 用于导出部门数据到Excel，包含以下特点：
/// 1. 包含所有需要导出的字段
/// 2. 所有字段都是只读的
/// </remarks>
public class LeanDeptExportDto
{
    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 父级ID
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 负责人
    /// </summary>
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; } = string.Empty;

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
/// 部门导入结果DTO
/// </summary>
/// <remarks>
/// 用于返回导入结果，包含以下特点：
/// 1. 包含导入的统计信息
/// 2. 包含详细的错误信息
/// </remarks>
public class LeanDeptImportResultDto
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
    public List<LeanDeptImportDto> FailureItems { get; set; } = new();
}

#endregion 导入导出