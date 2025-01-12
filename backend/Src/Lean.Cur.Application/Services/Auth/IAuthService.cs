using Lean.Cur.Application.Dtos.Auth;
using Lean.Cur.Common.Auth.SlideVerify;

namespace Lean.Cur.Application.Services.Auth;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService
{
  /// <summary>
  /// 生成滑块验证码
  /// </summary>
  /// <returns>验证码结果</returns>
  Task<SlideVerifyResult> GenerateVerifyCodeAsync();

  /// <summary>
  /// 登录
  /// </summary>
  /// <param name="loginDto">登录信息</param>
  /// <returns>登录结果</returns>
  Task<LoginResultDto> LoginAsync(LoginDto loginDto);

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>登录结果</returns>
  Task<LoginResultDto> RefreshTokenAsync(string refreshToken);

  /// <summary>
  /// 退出登录
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>是否成功</returns>
  Task<bool> LogoutAsync(long userId);

  /// <summary>
  /// 获取用户信息
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户信息</returns>
  Task<LoginUserDto> GetUserInfoAsync(long userId);
}