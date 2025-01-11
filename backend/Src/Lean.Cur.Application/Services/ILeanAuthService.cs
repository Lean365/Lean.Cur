using Lean.Cur.Application.DTOs.Auth;

namespace Lean.Cur.Application.Services;

/// <summary>
/// 认证服务接口
/// </summary>
public interface ILeanAuthService
{
  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="request">登录请求</param>
  /// <returns>登录响应</returns>
  Task<LeanLoginResponseDto> LoginAsync(LeanLoginRequestDto request);

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>登录响应</returns>
  Task<LeanLoginResponseDto> RefreshTokenAsync(string refreshToken);

  /// <summary>
  /// 登出
  /// </summary>
  /// <param name="userId">用户ID</param>
  Task LogoutAsync(long userId);
}