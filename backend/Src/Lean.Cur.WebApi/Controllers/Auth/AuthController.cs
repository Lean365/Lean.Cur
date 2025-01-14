using System;
using System.Linq;
using System.Threading.Tasks;
using Lean.Cur.Application.Dtos.Auth;
using Lean.Cur.Application.Services.Auth;
using Lean.Cur.Common.Auth.SlideVerify;
using Lean.Cur.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Auth;

/// <summary>
/// 认证控制器
/// </summary>
[Route("api/auth")]
[ApiController]
public class AuthController : LeanBaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginDto">登录信息</param>
    /// <returns>登录结果</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<LeanApiResponse<LoginResultDto>> LoginAsync([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidateError<LoginResultDto>();
        }
        var result = await _authService.LoginAsync(loginDto);
        return Success(result);
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns>是否成功</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<LeanApiResponse<bool>> LogoutAsync()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
        if (userId <= 0)
        {
            return Unauthorized<bool>();
        }
        var result = await _authService.LogoutAsync(userId);
        return Success(result);
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <param name="refreshToken">刷新令牌</param>
    /// <returns>登录结果</returns>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<LeanApiResponse<LoginResultDto>> RefreshTokenAsync([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return ValidateError<LoginResultDto>("刷新令牌不能为空");
        }
        var token = await _authService.RefreshTokenAsync(refreshToken);
        return Success(token);
    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <returns>验证码结果</returns>
    [HttpGet("verify-code")]
    [AllowAnonymous]
    public async Task<LeanApiResponse<SlideVerifyResult>> GenerateVerifyCodeAsync()
    {
        var result = await _authService.GenerateVerifyCodeAsync();
        return Success(result);
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    /// <returns>用户信息</returns>
    [HttpGet("user-info")]
    [Authorize]
    public async Task<LeanApiResponse<LoginUserDto>> GetUserInfoAsync()
    {
        var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
        if (userId <= 0)
        {
            return Unauthorized<LoginUserDto>();
        }
        var result = await _authService.GetUserInfoAsync(userId);
        return Success(result);
    }
}