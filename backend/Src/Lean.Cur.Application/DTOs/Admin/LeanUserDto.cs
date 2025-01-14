// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;

namespace Lean.Cur.Application.Dtos.Admin;

#region 基础操作

/// <summary>
/// 用户基本信息DTO
/// </summary>
public class LeanUserDto
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
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

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
    public UserType UserType { get; set; }

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
/// 用户查询DTO
/// </summary>
public class LeanUserQueryDto : PagedRequest
{
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
    /// 性别
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public UserType? UserType { get; set; }

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
/// 用户创建DTO
/// </summary>
public class LeanUserCreateDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "用户名只能包含字母、数字和下划线")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6-100个字符之间")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "密码必须包含字母和数字")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    [Required(ErrorMessage = "昵称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "昵称长度必须在2-50个字符之间")]
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名称
    /// </summary>
    [Required(ErrorMessage = "英文名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "英文名称长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "英文名称只能包含英文字母、空格和点号")]
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    [StringLength(200, ErrorMessage = "头像URL长度不能超过200个字符")]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [Required(ErrorMessage = "性别不能为空")]
    public Gender Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    [StringLength(20, ErrorMessage = "手机号长度不能超过20个字符")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    [Required(ErrorMessage = "用户类型不能为空")]
    public UserType UserType { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 用户更新DTO
/// </summary>
public class LeanUserUpdateDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required(ErrorMessage = "用户ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Required(ErrorMessage = "昵称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "昵称长度必须在2-50个字符之间")]
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名称
    /// </summary>
    [Required(ErrorMessage = "英文名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "英文名称长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "英文名称只能包含英文字母、空格和点号")]
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    [StringLength(200, ErrorMessage = "头像URL长度不能超过200个字符")]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [Required(ErrorMessage = "性别不能为空")]
    public Gender Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    [StringLength(20, ErrorMessage = "手机号长度不能超过20个字符")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 用户状态更新DTO
/// </summary>
public class LeanUserStatusDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required(ErrorMessage = "用户ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 用户密码重置DTO
/// </summary>
public class LeanUserResetPasswordDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [Required(ErrorMessage = "用户ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6-100个字符之间")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "密码必须包含字母和数字")]
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// 用户密码修改DTO
/// </summary>
public class LeanUserUpdatePasswordDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    [Required(ErrorMessage = "旧密码不能为空")]
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6-100个字符之间")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "密码必须包含字母和数字")]
    public string NewPassword { get; set; } = string.Empty;
}

#endregion 基础操作

#region 导入导出

/// <summary>
/// 用户导入模板DTO
/// </summary>
public class LeanUserTempleteDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "用户名只能包含字母、数字和下划线")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    [Required(ErrorMessage = "昵称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "昵称长度必须在2-50个字符之间")]
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名称
    /// </summary>
    [Required(ErrorMessage = "英文名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "英文名称长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "英文名称只能包含英文字母、空格和点号")]
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 性别（0：未知，1：男，2：女）
    /// </summary>
    [Required(ErrorMessage = "性别不能为空")]
    public Gender Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    [StringLength(20, ErrorMessage = "手机号长度不能超过20个字符")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（0：管理员，1：普通用户，2：访客）
    /// </summary>
    [Required(ErrorMessage = "用户类型不能为空")]
    public UserType UserType { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 用户导入DTO
/// </summary>
public class LeanUserImportDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "用户名只能包含字母、数字和下划线")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    [Required(ErrorMessage = "昵称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "昵称长度必须在2-50个字符之间")]
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 英文名称
    /// </summary>
    [Required(ErrorMessage = "英文名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "英文名称长度必须在2-50个字符之间")]
    [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "英文名称只能包含英文字母、空格和点号")]
    public string EnglishName { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    [Required(ErrorMessage = "性别不能为空")]
    public Gender Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    [StringLength(20, ErrorMessage = "手机号长度不能超过20个字符")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型
    /// </summary>
    [Required(ErrorMessage = "用户类型不能为空")]
    public UserType UserType { get; set; }

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
/// 用户导出DTO
/// </summary>
public class LeanUserExportDto
{
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
    /// 性别
    /// </summary>
    public string Gender { get; set; } = string.Empty;

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
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型
    /// </summary>
    public string UserType { get; set; } = string.Empty;

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
/// 用户导入结果DTO
/// </summary>
/// <remarks>
/// 用于返回导入结果，包含以下特点：
/// 1. 包含导入的统计信息
/// 2. 包含详细的错误信息
/// </remarks>
public class LeanUserImportResultDto
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
    public List<LeanUserImportDto> FailureItems { get; set; } = new();
}

#endregion 导入导出