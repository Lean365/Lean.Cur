using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;
using System.Security.Claims;
using System.Threading;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 用户实体类
/// </summary>
/// <remarks>
/// 用户管理模块的核心实体，包含用户的基本信息和权限信息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_user
/// 2. 主键：id (自增长)
/// 3. 唯一索引：username (用户名)
/// 4. 索引：nick_name (昵称), english_name (英文名), phone (手机号), email (邮箱)
/// 
/// 业务规则：
/// 1. 用户名全局唯一，不区分大小写
/// 2. 密码需要加密存储，使用盐值加密
/// 3. 除头像外，所有字段都必填
/// 4. 性别默认为未知
/// 5. 状态默认为启用
/// 6. 用户类型默认为普通用户
/// 7. 用户与部门的关系通过user_dept关系表维护
/// </remarks>
/// <author>CodeGenerator</author>
/// <date>2024-01-17</date>
/// <version>1.0.0</version>
/// <copyright>© 2024 Lean. All rights reserved</copyright>
[SugarTable("lean_user", "用户表")]
public class LeanUser : LeanBaseEntity
{
  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 只能包含字母、数字、下划线
  /// 3. 不能与系统保留用户名冲突
  /// 4. 创建后不允许修改
  /// 5. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "username", ColumnDescription = "用户名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("用户名")]
  public string UserName { get; set; }

  /// <summary>
  /// 密码
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：6-100个字符
  /// 2. 必须包含字母和数字
  /// 3. 存储时使用盐值进行加密
  /// 4. 不允许明文传输
  /// 5. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "password", ColumnDescription = "密码", ColumnDataType = "varchar", Length = 64, IsNullable = false)]
  [Description("密码")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// 密码盐值
  /// </summary>
  /// <remarks>
  /// 1. 长度固定：64个字符
  /// 2. 用于密码加密
  /// 3. 创建用户时随机生成
  /// 4. 每个用户唯一
  /// 5. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "password_salt", ColumnDescription = "密码盐值", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
  [Description("密码盐值")]
  public string PasswordSalt { get; set; } = string.Empty;

  /// <summary>
  /// 昵称
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 允许中文、字母、数字、下划线
  /// 3. 用于显示的友好名称
  /// 4. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "nick_name", ColumnDescription = "昵称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("昵称")]
  public string NickName { get; set; }

  /// <summary>
  /// 英文名称
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 只能包含英文字母、空格和点号
  /// 3. 用于国际化场景下的显示
  /// 4. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "english_name", ColumnDescription = "英文名称", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("英文名称")]
  public string EnglishName { get; set; } = string.Empty;

  /// <summary>
  /// 头像
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：最大200个字符
  /// 2. 存储头像的URL地址
  /// 3. 支持相对路径和绝对路径
  /// 4. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "avatar", ColumnDescription = "头像URL", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
  [Description("头像")]
  public string? Avatar { get; set; }

  /// <summary>
  /// 性别
  /// </summary>
  /// <remarks>
  /// 用户的性别信息，使用Gender枚举
  /// 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "gender", ColumnDescription = "性别", ColumnDataType = "int", IsNullable = false)]
  [Description("性别")]
  public LeanGender Gender { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：最大100个字符
  /// 2. 必须符合邮箱格式
  /// 3. 用于接收系统通知
  /// 4. 可用于找回密码
  /// 5. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  [Description("邮箱")]
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// 手机号
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：最大20个字符
  /// 2. 必须符合手机号格式
  /// 3. 用于接收系统通知
  /// 4. 可用于找回密码
  /// 5. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "phone", ColumnDescription = "手机号", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
  [Description("手机号")]
  public string Phone { get; set; } = string.Empty;

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 用户账号的状态，使用LeanStatus枚举
  /// 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  [Description("状态")]
  public LeanStatus Status { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  /// <remarks>
  /// 用户的类型，使用UserType枚举：
  /// - Admin(0)：系统管理员
  /// - User(1)：普通用户
  /// - Guest(2)：访客用户
  /// 默认为普通用户
  /// 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "user_type", ColumnDescription = "用户类型", ColumnDataType = "int", IsNullable = false)]
  [Description("用户类型")]
  public LeanUserType UserType { get; set; }

  /// <summary>
  /// 导航属性：用户扩展信息
  /// </summary>
  /// <remarks>
  /// 1. 一对一关联到用户扩展信息表
  /// 2. 主表（用户表）通过Id关联从表（用户扩展信息表）的UserId
  /// 3. 延迟加载
  /// 4. 包含用户的登录历史、在线状态等扩展信息
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(Id))]
  public LeanUserExtend? UserExtend { get; set; }

  /// <summary>
  /// 导航属性：用户设备信息
  /// </summary>
  /// <remarks>
  /// 1. 一对多关联到用户设备信息表
  /// 2. 主表（用户表）通过Id关联从表（用户设备信息表）的UserId
  /// 3. 延迟加载
  /// 4. 包含用户的所有登录设备信息
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserDevice.UserId))]
  public List<LeanUserDevice>? UserDevices { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? DeptId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? PostId { get; set; }

  /// <summary>
  /// 部门信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(DeptId))]
  public LeanDept? Dept { get; set; }

  /// <summary>
  /// 岗位信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(PostId))]
  public LeanPost? Post { get; set; }

  /// <summary>
  /// 用户角色关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserRole.UserId))]
  public List<LeanUserRole>? UserRoles { get; set; }

  /// <summary>
  /// 获取当前登录用户ID
  /// </summary>
  /// <returns>当前用户ID，如果未登录则返回0</returns>
  public static long GetCurrentUserId()
  {
    var identity = Thread.CurrentPrincipal?.Identity as ClaimsIdentity;
    var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);
    return userIdClaim != null ? long.Parse(userIdClaim.Value) : 0;
  }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanUser()
  {
    UserName = string.Empty;
    Password = string.Empty;
    PasswordSalt = string.Empty;
    NickName = string.Empty;
    EnglishName = string.Empty;
    Email = string.Empty;
    Phone = string.Empty;
    Gender = LeanGender.Unknown;
    Status = LeanStatus.Normal;
    UserType = LeanUserType.User;
    IsDeleted = 0;
  }
}