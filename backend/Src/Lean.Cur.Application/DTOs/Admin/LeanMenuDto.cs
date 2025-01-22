using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Admin;

#region 基础操作

/// <summary>
/// 菜单 DTO
/// </summary>
public class LeanMenuDto
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [Description("主键ID")]
    public long Id { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    [Description("父级ID")]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Description("菜单名称")]
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码
    /// </summary>
    [Description("菜单编码")]
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    [Description("菜单类型")]
    public LeanMenuType MenuType { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [Description("路由地址")]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [Description("组件路径")]
    public string? Component { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [Description("权限标识")]
    public string? Permission { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Description("图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    [Description("显示顺序")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Description("状态")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    [Description("是否可见")]
    public int Visible { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    [Description("是否缓存")]
    public int Cache { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    [Description("是否外链")]
    public int External { get; set; }

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

    /// <summary>
    /// 子菜单
    /// </summary>
    [Description("子菜单")]
    public List<LeanMenuDto>? Children { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [Description("翻译键")]
    public string? TransKey { get; set; }
}

/// <summary>
/// 菜单查询 DTO
/// </summary>
public class LeanMenuQueryDto : LeanPagedRequest
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    [Description("菜单名称")]
    public string? MenuName { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [Description("菜单编码")]
    public string? MenuCode { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    [Description("菜单类型")]
    public LeanMenuType? MenuType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Description("状态")]
    public LeanStatus? Status { get; set; }

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
/// 菜单创建 DTO
/// </summary>
public class LeanMenuCreateDto
{
    /// <summary>
    /// 父级ID
    /// </summary>
    [Description("父级ID")]
    [Required(ErrorMessage = "父级ID不能为空")]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Description("菜单名称")]
    [Required(ErrorMessage = "菜单名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单名称长度必须在2-50个字符之间")]
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码
    /// </summary>
    [Description("菜单编码")]
    [Required(ErrorMessage = "菜单编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "菜单编码只能包含字母、数字、下划线")]
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    [Description("菜单类型")]
    [Required(ErrorMessage = "菜单类型不能为空")]
    public LeanMenuType MenuType { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [Description("路由地址")]
    [StringLength(200, ErrorMessage = "路由地址长度不能超过200个字符")]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [Description("组件路径")]
    [StringLength(200, ErrorMessage = "组件路径长度不能超过200个字符")]
    public string? Component { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [Description("权限标识")]
    [StringLength(100, ErrorMessage = "权限标识长度不能超过100个字符")]
    public string? Permission { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Description("图标")]
    [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
    public string? Icon { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    [Description("显示顺序")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Description("状态")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    [Description("是否可见")]
    public int Visible { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    [Description("是否缓存")]
    public int Cache { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    [Description("是否外链")]
    public int External { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [Description("翻译键")]
    [StringLength(100, ErrorMessage = "翻译键长度不能超过100个字符")]
    public string? TransKey { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Description("备注")]
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 菜单更新 DTO
/// </summary>
public class LeanMenuUpdateDto : LeanMenuCreateDto
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [Description("主键ID")]
    [Required(ErrorMessage = "菜单ID不能为空")]
    public long Id { get; set; }
}

/// <summary>
/// 菜单状态 DTO
/// </summary>
public class LeanMenuStatusDto
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [Description("主键ID")]
    [Required(ErrorMessage = "菜单ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Description("状态")]
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 菜单批量删除 DTO
/// </summary>
public class LeanMenuBatchDeleteDto
{
    /// <summary>
    /// 菜单ID列表
    /// </summary>
    [Description("菜单ID列表")]
    [Required(ErrorMessage = "菜单ID列表不能为空")]
    public List<long> Ids { get; set; } = new();
}

#endregion

#region 导入导出

/// <summary>
/// 菜单导入DTO
/// </summary>
/// <remarks>
/// 用于Excel导入菜单数据，包含以下特点：
/// 1. 包含所有可导入的字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanMenuImportDto
{
    /// <summary>
    /// 父级菜单名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 必须是已存在的菜单名称
    /// </remarks>
    [Required(ErrorMessage = "父级菜单名称不能为空")]
    [StringLength(50, ErrorMessage = "父级菜单名称长度不能超过50个字符")]
    public string ParentMenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一父级下唯一
    /// </remarks>
    [Required(ErrorMessage = "菜单名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单名称长度必须在2-50个字符之间")]
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// </remarks>
    [Required(ErrorMessage = "菜单编码不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单编码长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "菜单编码只能包含字母、数字和下划线")]
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 可选值：目录、菜单、按钮
    /// </remarks>
    [Required(ErrorMessage = "菜单类型不能为空")]
    public LeanMenuType MenuType { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大200个字符
    /// </remarks>
    [StringLength(200, ErrorMessage = "路由地址长度不能超过200个字符")]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大200个字符
    /// </remarks>
    [StringLength(200, ErrorMessage = "组件路径长度不能超过200个字符")]
    public string? Component { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大100个字符
    /// </remarks>
    [StringLength(100, ErrorMessage = "权限标识长度不能超过100个字符")]
    public string? Permission { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大100个字符
    /// </remarks>
    [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
    public string? Icon { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 示例值：1
    /// 必填，数字，值越小越靠前
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
    /// 是否可见
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 0-隐藏，1-显示
    /// </remarks>
    [Required(ErrorMessage = "是否可见不能为空")]
    public int Visible { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 0-不缓存，1-缓存
    /// </remarks>
    [Required(ErrorMessage = "是否缓存不能为空")]
    public int Cache { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 0-否，1-是
    /// </remarks>
    [Required(ErrorMessage = "是否外链不能为空")]
    public int External { get; set; }

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
    /// <remarks>
    /// 用于记录导入过程中的错误信息
    /// </remarks>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 菜单导出DTO
/// </summary>
/// <remarks>
/// 用于Excel导出菜单数据，包含以下特点：
/// 1. 所有字段都是string类型，便于Excel格式化
/// 2. 包含所有需要导出的字段
/// 3. 时间字段会被格式化为指定格式
/// </remarks>
public class LeanMenuExportDto
{
    /// <summary>
    /// 父级菜单名称
    /// </summary>
    public string ParentMenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    /// <remarks>
    /// 会被格式化为中文：目录、菜单、按钮
    /// </remarks>
    public string MenuType { get; set; } = string.Empty;

    /// <summary>
    /// 路由地址
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    public string? Permission { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

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
    /// 是否可见
    /// </summary>
    /// <remarks>
    /// 会被格式化为中文：是、否
    /// </remarks>
    public string Visible { get; set; } = string.Empty;

    /// <summary>
    /// 是否缓存
    /// </summary>
    /// <remarks>
    /// 会被格式化为中文：是、否
    /// </remarks>
    public string Cache { get; set; } = string.Empty;

    /// <summary>
    /// 是否外链
    /// </summary>
    /// <remarks>
    /// 会被格式化为中文：是、否
    /// </remarks>
    public string External { get; set; } = string.Empty;

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
/// 菜单导入结果DTO
/// </summary>
/// <remarks>
/// 用于返回导入结果，包含以下特点：
/// 1. 包含导入的统计信息
/// 2. 包含详细的错误信息
/// </remarks>
public class LeanMenuImportResultDto
{
    /// <summary>
    /// 总数
    /// </summary>
    /// <remarks>
    /// 导入文件中的总记录数
    /// </remarks>
    public int TotalCount { get; set; }

    /// <summary>
    /// 成功数
    /// </summary>
    /// <remarks>
    /// 成功导入的记录数
    /// </remarks>
    public int SuccessCount { get; set; }

    /// <summary>
    /// 失败数
    /// </summary>
    /// <remarks>
    /// 导入失败的记录数
    /// </remarks>
    public int FailureCount { get; set; }

    /// <summary>
    /// 失败记录列表
    /// </summary>
    /// <remarks>
    /// 记录导入失败的数据
    /// </remarks>
    public List<LeanMenuImportDto> FailureItems { get; set; } = new();
}

/// <summary>
/// 菜单导入模板DTO
/// </summary>
/// <remarks>
/// 用于生成导入模板，包含以下特点：
/// 1. 包含所有可导入字段的示例值
/// 2. 包含字段说明和验证规则
/// </remarks>
public class LeanMenuImportTemplateDto
{
    /// <summary>
    /// 父级菜单名称
    /// </summary>
    /// <remarks>
    /// 示例值：系统管理
    /// 必填，必须是已存在的菜单名称
    /// </remarks>
    public string ParentMenuName { get; set; } = "系统管理";

    /// <summary>
    /// 菜单名称
    /// </summary>
    /// <remarks>
    /// 示例值：用户管理
    /// 必填，2-50个字符
    /// </remarks>
    public string MenuName { get; set; } = "用户管理";

    /// <summary>
    /// 菜单编码
    /// </summary>
    /// <remarks>
    /// 示例值：system_user
    /// 必填，2-50个字符，只能包含字母、数字和下划线
    /// </remarks>
    public string MenuCode { get; set; } = "system_user";

    /// <summary>
    /// 菜单类型
    /// </summary>
    /// <remarks>
    /// 示例值：菜单
    /// 必填，可选值：目录、菜单、按钮
    /// </remarks>
    public string MenuType { get; set; } = "菜单";

    /// <summary>
    /// 路由地址
    /// </summary>
    /// <remarks>
    /// 示例值：/system/user
    /// 选填，最多200个字符
    /// </remarks>
    public string? Path { get; set; } = "/system/user";

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <remarks>
    /// 示例值：system/user/index
    /// 选填，最多200个字符
    /// </remarks>
    public string? Component { get; set; } = "system/user/index";

    /// <summary>
    /// 权限标识
    /// </summary>
    /// <remarks>
    /// 示例值：system:user:list
    /// 选填，最多100个字符
    /// </remarks>
    public string? Permission { get; set; } = "system:user:list";

    /// <summary>
    /// 图标
    /// </summary>
    /// <remarks>
    /// 示例值：user
    /// 选填，最多100个字符
    /// </remarks>
    public string? Icon { get; set; } = "user";

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
    /// 是否可见
    /// </summary>
    /// <remarks>
    /// 示例值：是
    /// 必填，可选值：是、否
    /// </remarks>
    public string Visible { get; set; } = "是";

    /// <summary>
    /// 是否缓存
    /// </summary>
    /// <remarks>
    /// 示例值：是
    /// 必填，可选值：是、否
    /// </remarks>
    public string Cache { get; set; } = "是";

    /// <summary>
    /// 是否外链
    /// </summary>
    /// <remarks>
    /// 示例值：否
    /// 必填，可选值：是、否
    /// </remarks>
    public string External { get; set; } = "否";

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>
    /// 示例值：用户管理菜单
    /// 选填，最多500个字符
    /// </remarks>
    public string? Remark { get; set; } = "用户管理菜单";
}

#endregion 