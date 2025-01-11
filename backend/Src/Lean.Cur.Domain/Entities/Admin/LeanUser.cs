/**
 * @description 用户实体类
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.Security.Cryptography;
using System.Text;
using Lean.Cur.Common.Enums;
using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 用户实体类
/// </summary>
/// <remarks>
/// 该实体类映射到数据库表 lean_user，用于存储用户相关数据
/// 
/// 数据库映射说明：
/// 1. 表名：lean_user
/// 2. 主键：Id (自增长)
/// 3. 基础字段：继承自LeanBaseEntity，包含创建时间、创建人、更新时间、更新人等
/// </remarks>
[SugarTable("lean_user", TableDescription = "用户表")]
public class LeanUser : LeanBaseEntity
{
  /// <summary>
  /// 用户名
  /// </summary>
  [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string UserName { get; set; } = null!;

  /// <summary>
  /// 英文名称
  /// </summary>
  [SugarColumn(ColumnName = "english_name", ColumnDescription = "英文名称", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
  public string? EnglishName { get; set; }

  /// <summary>
  /// 密码(MD5加密)
  /// </summary>
  [SugarColumn(ColumnName = "password", ColumnDescription = "密码", ColumnDataType = "varchar", Length = 32, IsNullable = false)]
  public string Password { get; set; } = null!;

  /// <summary>
  /// 密码盐值
  /// </summary>
  [SugarColumn(ColumnName = "salt", ColumnDescription = "密码盐值", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
  public string Salt { get; set; } = null!;

  /// <summary>
  /// 昵称
  /// </summary>
  [SugarColumn(ColumnName = "nick_name", ColumnDescription = "昵称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string NickName { get; set; } = null!;

  /// <summary>
  /// 邮箱
  /// </summary>
  [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  public string? Email { get; set; }

  /// <summary>
  /// 手机号
  /// </summary>
  [SugarColumn(ColumnName = "phone", ColumnDescription = "手机号", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
  public string? Phone { get; set; }

  /// <summary>
  /// 性别
  /// </summary>
  [SugarColumn(ColumnName = "gender", ColumnDescription = "性别", ColumnDataType = "int", IsNullable = false)]
  public Gender Gender { get; set; } = Gender.Unknown;

  /// <summary>
  /// 头像URL
  /// </summary>
  [SugarColumn(ColumnName = "avatar", ColumnDescription = "头像URL", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
  public string? Avatar { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  public Status Status { get; set; } = Status.Normal;

  /// <summary>
  /// 部门ID
  /// </summary>
  [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = false)]
  public long DeptId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  [SugarColumn(ColumnName = "position_id", ColumnDescription = "岗位ID", ColumnDataType = "bigint", IsNullable = false)]
  public long PositionId { get; set; }

  /// <summary>
  /// 最后登录IP
  /// </summary>
  [SugarColumn(ColumnName = "login_ip", ColumnDescription = "最后登录IP", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
  public string? LoginIp { get; set; }

  /// <summary>
  /// 最后登录时间
  /// </summary>
  [SugarColumn(ColumnName = "login_date", ColumnDescription = "最后登录时间", ColumnDataType = "datetime", IsNullable = true)]
  public DateTime? LoginDate { get; set; }

  /// <summary>
  /// 用户角色关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserRole.UserId))]
  public virtual List<LeanRole> Roles { get; set; } = new();

  /// <summary>
  /// 用户部门关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserDept.UserId))]
  public virtual List<LeanDept> Depts { get; set; } = new();

  /// <summary>
  /// 用户岗位关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserPosition.UserId))]
  public virtual List<LeanPosition> Positions { get; set; } = new();

  /// <summary>
  /// 用户类型
  /// </summary>
  [SugarColumn(ColumnName = "user_type", ColumnDescription = "用户类型", ColumnDataType = "int", IsNullable = false)]
  public UserType UserType { get; set; } = UserType.User;

  /// <summary>
  /// 验证密码
  /// </summary>
  /// <param name="password">待验证的密码</param>
  /// <returns>true:验证通过,false:验证失败</returns>
  public bool ValidatePassword(string password)
  {
    using var md5 = MD5.Create();
    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password + Salt));
    var hashedPassword = BitConverter.ToString(hash).Replace("-", "").ToLower();
    return Password == hashedPassword;
  }
}