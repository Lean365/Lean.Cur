using System.ComponentModel.DataAnnotations;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.Dtos.Auth;

/// <summary>
/// 登录请求
/// </summary>
public class LoginDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// 验证码ID
  /// </summary>
  [Required(ErrorMessage = "验证码ID不能为空")]
  public string VerifyId { get; set; } = string.Empty;

  /// <summary>
  /// 滑块X坐标
  /// </summary>
  [Required(ErrorMessage = "请完成滑块验证")]
  public int X { get; set; }
}

/// <summary>
/// 登录结果
/// </summary>
public class LoginResultDto
{
  /// <summary>
  /// 访问令牌
  /// </summary>
  public string AccessToken { get; set; } = string.Empty;

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public string RefreshToken { get; set; } = string.Empty;

  /// <summary>
  /// 过期时间（秒）
  /// </summary>
  public int ExpiresIn { get; set; }

  /// <summary>
  /// 用户信息
  /// </summary>
  public LoginUserDto User { get; set; } = new();
}

/// <summary>
/// 登录用户信息
/// </summary>
public class LoginUserDto
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
  /// 英文名
  /// </summary>
  public string EnglishName { get; set; } = string.Empty;

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 用户类型
  /// </summary>
  public UserType UserType { get; set; }
}