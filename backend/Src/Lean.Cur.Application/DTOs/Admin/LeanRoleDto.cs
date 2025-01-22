// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Admin;

/// <summary>
/// 角色管理相关的DTO定义
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本文件定义了角色管理相关的所有DTO，包括：
/// - 基础操作相关DTO（查询、创建、更新、删除等）
/// - 权限管理相关DTO（权限分配、权限查询等）
/// - 导入导出相关DTO（数据导入、导出、模板等）
/// </remarks>

#region 基础操作

/// <summary>
/// 角色基本信息DTO
/// </summary>
/// <remarks>
/// 用于返回角色的基本信息，包含以下特点：
/// 1. 包含角色的所有基础字段
/// 2. 所有字段都是只读的，不包含验证特性
/// 3. 用于列表展示和详情展示
/// </remarks>
public class LeanRoleDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; } = string.Empty;

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
/// 角色查询DTO
/// </summary>
/// <remarks>
/// 用于角色列表的查询条件，包含以下特点：
/// 1. 继承自LeanPagedRequest，支持分页查询
/// 2. 所有查询条件都是可选的
/// 3. 支持按时间范围查询
/// </remarks>
public class LeanRoleQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 角色名称
  /// </summary>
  public string? RoleName { get; set; }

  /// <summary>
  /// 角色编码
  /// </summary>
  public string? RoleCode { get; set; }

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
/// 角色创建DTO
/// </summary>
/// <remarks>
/// 用于创建新角色，包含以下特点：
/// 1. 包含创建角色所需的所有必要字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanRoleCreateDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 长度限制：2-50个字符
  /// 3. 同一租户下唯一
  /// </remarks>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 长度限制：2-50个字符
  /// 3. 只能包含字母、数字和下划线
  /// 4. 全局唯一，不区分大小写
  /// 5. 创建后不可修改
  /// </remarks>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "角色编码只能包含字母、数字和下划线")]
  public string RoleCode { get; set; } = string.Empty;

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
/// 角色更新DTO
/// </summary>
/// <remarks>
/// 用于更新现有角色，包含以下特点：
/// 1. 包含可以更新的所有字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// 4. 不允许修改角色编码
/// </remarks>
public class LeanRoleUpdateDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 长度限制：2-50个字符
  /// 3. 同一租户下唯一
  /// </remarks>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 长度限制：2-50个字符
  /// 3. 只能包含字母、数字和下划线
  /// 4. 全局唯一，不区分大小写
  /// </remarks>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "角色编码只能包含字母、数字和下划线")]
  public string RoleCode { get; set; } = string.Empty;

  /// <summary>
  /// 角色类型
  /// </summary>
  [Required(ErrorMessage = "角色类型不能为空")]
  public LeanRoleType RoleType { get; set; }

  /// <summary>
  /// 数据范围
  /// </summary>
  [Required(ErrorMessage = "数据范围不能为空")]
  public LeanDataScope DataScope { get; set; }

  /// <summary>
  /// 部门ID列表（仅在自定义数据范围时使用）
  /// </summary>
  public List<long>? DeptIds { get; set; }

  /// <summary>
  /// 继承的角色ID列表
  /// </summary>
  public List<long>? InheritedRoleIds { get; set; }

  /// <summary>
  /// 继承类型（1：完全继承 2：部分继承）
  /// </summary>
  public int? InheritanceType { get; set; }

  /// <summary>
  /// 继承的权限列表（部分继承时使用）
  /// </summary>
  public List<string>? InheritedPermissions { get; set; }

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
/// 角色状态DTO
/// </summary>
/// <remarks>
/// 用于更新角色状态，包含以下特点：
/// 1. 只包含ID和状态字段
/// 2. 所有字段都有验证特性
/// </remarks>
public class LeanRoleStatusDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
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
/// 角色批量删除DTO
/// </summary>
/// <remarks>
/// 用于批量删除角色，包含以下特点：
/// 1. 只包含ID列表
/// 2. ID列表不能为空
/// </remarks>
public class LeanRoleBatchDeleteDto
{
  /// <summary>
  /// 角色ID列表
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 不能为空列表
  /// </remarks>
  [Required(ErrorMessage = "角色ID列表不能为空")]
  public List<long> Ids { get; set; } = new();
}

/// <summary>
/// 角色权限DTO
/// </summary>
/// <remarks>
/// 用于角色权限管理，包含以下特点：
/// 1. 包含角色ID和权限列表
/// 2. 所有字段都有验证特性
/// </remarks>
public class LeanRolePermissionDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 权限列表
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 不能为空列表
  /// </remarks>
  [Required(ErrorMessage = "权限列表不能为空")]
  public List<string> Permissions { get; set; } = new();
}

/// <summary>
/// 角色菜单DTO
/// </summary>
/// <remarks>
/// 用于角色菜单管理，包含以下特点：
/// 1. 包含角色ID和菜单ID列表
/// 2. 所有字段都有验证特性
/// 3. 用于分配和查询角色菜单权限
/// </remarks>
public class LeanRoleMenuDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 不能为空列表
  /// </remarks>
  [Required(ErrorMessage = "菜单ID列表不能为空")]
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 角色菜单树DTO
/// </summary>
/// <remarks>
/// 用于返回角色菜单树结构，包含以下特点：
/// 1. 包含菜单的层级结构
/// 2. 包含菜单的选中状态
/// 3. 用于前端展示菜单树
/// </remarks>
public class LeanRoleMenuTreeDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 父级ID
  /// </summary>
  public long ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// 菜单编码
  /// </summary>
  public string Code { get; set; } = string.Empty;

  /// <summary>
  /// 菜单类型
  /// </summary>
  [Description("菜单类型")]
  public LeanMenuType Type { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 图标
  /// </summary>
  public string? Icon { get; set; }

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
  /// 是否外链
  /// </summary>
  [Description("是否外链")]
  public int IsExternal { get; set; }

  /// <summary>
  /// 是否缓存
  /// </summary>
  [Description("是否缓存")]
  public int IsCache { get; set; }

  /// <summary>
  /// 是否可见
  /// </summary>
  [Description("是否可见")]
  public int IsVisible { get; set; }

  /// <summary>
  /// 是否选中
  /// </summary>
  [Description("是否选中")]
  public int IsChecked { get; set; }

  /// <summary>
  /// 子菜单列表
  /// </summary>
  public List<LeanRoleMenuTreeDto> Children { get; set; } = new();
}

/// <summary>
/// 角色菜单权限DTO
/// </summary>
/// <remarks>
/// 用于更新角色的菜单权限，包含以下特点：
/// 1. 包含角色ID和菜单ID列表
/// 2. 包含每个菜单的权限标识
/// </remarks>
public class LeanRoleMenuPermissionDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();

  /// <summary>
  /// 菜单权限列表
  /// </summary>
  public List<MenuPermission> Permissions { get; set; } = new();
}

/// <summary>
/// 菜单权限
/// </summary>
public class MenuPermission
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  public long MenuId { get; set; }

  /// <summary>
  /// 权限标识
  /// </summary>
  public string Permission { get; set; } = string.Empty;
}

/// <summary>
/// 角色用户DTO
/// </summary>
/// <remarks>
/// 用于角色用户管理，包含以下特点：
/// 1. 包含角色ID和用户ID列表
/// 2. 所有字段都有验证特性
/// 3. 用于分配和查询角色用户关系
/// </remarks>
public class LeanRoleUserDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 用户ID列表
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 不能为空列表
  /// </remarks>
  [Required(ErrorMessage = "用户ID列表不能为空")]
  public List<long> UserIds { get; set; } = new();
}

/// <summary>
/// 角色用户列表DTO
/// </summary>
/// <remarks>
/// 用于返回角色的用户列表，包含以下特点：
/// 1. 包含用户的基本信息
/// 2. 用于前端展示角色下的用户列表
/// </remarks>
public class LeanRoleUserListDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 昵称
  /// </summary>
  public string NickName { get; set; } = string.Empty;

  /// <summary>
  /// 英文名称
  /// </summary>
  public string EnglishName { get; set; } = string.Empty;

  /// <summary>
  /// 头像URL
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// 手机号
  /// </summary>
  public string Phone { get; set; } = string.Empty;

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  public LeanUserType UserType { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }
}

/// <summary>
/// 角色用户查询DTO
/// </summary>
/// <remarks>
/// 用于查询角色下的用户列表，包含以下特点：
/// 1. 继承自LeanPagedRequest，支持分页查询
/// 2. 所有查询条件都是可选的
/// 3. 支持按用户信息和时间范围查询
/// </remarks>
public class LeanRoleUserQueryDto : LeanPagedRequest
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string? UserName { get; set; }

  /// <summary>
  /// 昵称
  /// </summary>
  public string? NickName { get; set; }

  /// <summary>
  /// 英文名称
  /// </summary>
  public string? EnglishName { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanStatus? Status { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  public LeanUserType? UserType { get; set; }

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
/// 角色用户分配DTO
/// </summary>
/// <remarks>
/// 用于角色用户分配，包含以下特点：
/// 1. 包含角色ID和用户ID列表
/// 2. 支持批量分配和取消分配
/// 3. 所有字段都有验证特性
/// </remarks>
public class LeanRoleUserAssignDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 必须大于0
  /// </remarks>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 用户ID列表
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 不能为空列表
  /// </remarks>
  [Required(ErrorMessage = "用户ID列表不能为空")]
  public List<long> UserIds { get; set; } = new();

  /// <summary>
  /// 是否分配
  /// </summary>
  /// <remarks>
  /// true: 分配角色给用户
  /// false: 取消用户的角色分配
  /// </remarks>
  [Description("是否分配")]
  public int IsAssign { get; set; }
}

/// <summary>
/// 角色继承关系更新DTO
/// </summary>
/// <remarks>
/// 用于更新角色的继承关系，包含以下特点：
/// 1. 包含角色ID和继承的角色ID列表
/// 2. 支持完全继承和部分继承
/// 3. 所有字段都有验证特性
/// </remarks>
public class LeanRoleInheritanceUpdateDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 继承的角色ID列表
  /// </summary>
  [Required(ErrorMessage = "继承的角色ID列表不能为空")]
  public List<long> InheritedRoleIds { get; set; } = new();

  /// <summary>
  /// 继承类型（1：完全继承 2：部分继承）
  /// </summary>
  [Required(ErrorMessage = "继承类型不能为空")]
  public int InheritanceType { get; set; }

  /// <summary>
  /// 继承的权限列表（部分继承时使用）
  /// </summary>
  public List<string>? InheritedPermissions { get; set; }
}

#endregion

#region 导入导出

/// <summary>
/// 角色导入DTO
/// </summary>
/// <remarks>
/// 用于Excel导入角色数据，包含以下特点：
/// 1. 包含所有可导入的字段
/// 2. 所有必填字段都有验证特性
/// 3. 包含详细的错误提示信息
/// </remarks>
public class LeanRoleImportDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 长度限制：2-50个字符
  /// 3. 同一租户下唯一
  /// </remarks>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  /// <remarks>
  /// 1. 必填字段
  /// 2. 长度限制：2-50个字符
  /// 3. 只能包含字母、数字和下划线
  /// 4. 全局唯一，不区分大小写
  /// </remarks>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "角色编码只能包含字母、数字和下划线")]
  public string RoleCode { get; set; } = string.Empty;

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

  /// <summary>
  /// 错误信息
  /// </summary>
  public string? ErrorMessage { get; set; }
}

/// <summary>
/// 角色导出DTO
/// </summary>
/// <remarks>
/// 用于Excel导出角色数据，包含以下特点：
/// 1. 所有字段都是string类型，便于Excel格式化
/// 2. 包含所有需要导出的字段
/// 3. 时间字段会被格式化为指定格式
/// </remarks>
public class LeanRoleExportDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; } = string.Empty;

  /// <summary>
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public string Status { get; set; } = string.Empty;

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
/// 角色导入结果DTO
/// </summary>
/// <remarks>
/// 用于返回导入结果，包含以下特点：
/// 1. 包含导入的统计信息
/// 2. 包含详细的错误信息
/// </remarks>
public class LeanRoleImportResultDto
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
  public List<LeanRoleImportDto> FailureItems { get; set; } = new();
}

/// <summary>
/// 角色导入模板DTO
/// </summary>
/// <remarks>
/// 用于生成导入模板，包含以下特点：
/// 1. 包含所有可导入字段的示例值
/// 2. 包含字段说明和验证规则
/// </remarks>
public class LeanRoleImportTemplateDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 示例值：系统管理员
  /// 必填，2-50个字符
  /// </remarks>
  public string RoleName { get; set; } = "系统管理员";

  /// <summary>
  /// 角色编码
  /// </summary>
  /// <remarks>
  /// 示例值：admin
  /// 必填，2-50个字符，只能包含字母、数字和下划线
  /// </remarks>
  public string RoleCode { get; set; } = "admin";

  /// <summary>
  /// 显示顺序
  /// </summary>
  /// <remarks>
  /// 示例值：0
  /// 必填，数字，值越小越靠前
  /// </remarks>
  public string OrderNum { get; set; } = "0";

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
  /// 示例值：系统内置管理员角色
  /// 选填，最多500个字符
  /// </remarks>
  public string? Remark { get; set; } = "系统内置管理员角色";
}

#endregion