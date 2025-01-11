using System.ComponentModel.DataAnnotations;

namespace Lean.Cur.Application.DTOs.Auth;

/// <summary>
/// 登录请求DTO
/// </summary>
public class LeanLoginRequestDto
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
  /// 验证码
  /// </summary>
  public string? CaptchaCode { get; set; }

  /// <summary>
  /// 验证码键值
  /// </summary>
  public string? CaptchaKey { get; set; }
}

/// <summary>
/// 登录响应DTO
/// </summary>
public class LeanLoginResponseDto
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
  public LeanUserInfoDto UserInfo { get; set; } = new();
}

/// <summary>
/// 用户信息DTO
/// </summary>
public class LeanUserInfoDto
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
  public string? NickName { get; set; }

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 角色列表
  /// </summary>
  public List<string> Roles { get; set; } = new();

  /// <summary>
  /// 权限列表
  /// </summary>
  public List<string> Permissions { get; set; } = new();
}