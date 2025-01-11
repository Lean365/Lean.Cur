using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lean.Cur.Application.DTOs.Auth;
using Lean.Cur.Application.Services;
using System.Security.Claims;

namespace Lean.Cur.Api.Controllers;

/// <summary>
/// 认证控制器
/// </summary>
[Route("api/auth")]
[ApiController]
public class LeanAuthController : ControllerBase
{
  private readonly ILeanAuthService _authService;
  private readonly ILogger<LeanAuthController> _logger;

  public LeanAuthController(ILeanAuthService authService, ILogger<LeanAuthController> logger)
  {
    _authService = authService;
    _logger = logger;
  }

  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="request">登录请求</param>
  /// <returns>登录响应</returns>
  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<ActionResult<LeanLoginResponseDto>> LoginAsync([FromBody] LeanLoginRequestDto request)
  {
    try
    {
      var response = await _authService.LoginAsync(request);
      return Ok(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "登录失败");
      return BadRequest(new { message = ex.Message });
    }
  }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>登录响应</returns>
  [HttpPost("refresh-token")]
  [AllowAnonymous]
  public async Task<ActionResult<LeanLoginResponseDto>> RefreshTokenAsync([FromBody] string refreshToken)
  {
    try
    {
      var response = await _authService.RefreshTokenAsync(refreshToken);
      return Ok(response);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "刷新令牌失败");
      return BadRequest(new { message = ex.Message });
    }
  }

  /// <summary>
  /// 用户登出
  /// </summary>
  /// <returns>操作结果</returns>
  [HttpPost("logout")]
  [Authorize]
  public async Task<IActionResult> LogoutAsync()
  {
    try
    {
      var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
      await _authService.LogoutAsync(userId);
      return Ok(new { message = "登出成功" });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "登出失败");
      return BadRequest(new { message = ex.Message });
    }
  }
}