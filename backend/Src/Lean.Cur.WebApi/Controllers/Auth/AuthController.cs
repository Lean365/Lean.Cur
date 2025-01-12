using Lean.Cur.Application.Dtos.Auth;
using Lean.Cur.Application.Services.Auth;
using Lean.Cur.Common.Auth.SlideVerify;
using Lean.Cur.Infrastructure.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using Lean.Cur.Common.Utils;

namespace Lean.Cur.WebApi.Controllers.Auth;

/// <summary>
/// 认证控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;
  private readonly ILogger<LoginLogAttribute> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ISqlSugarClient _db;
  private readonly Ip2RegionHelper _ip2Region;

  public AuthController(
    IAuthService authService,
    ILogger<LoginLogAttribute> logger,
    IHttpContextAccessor httpContextAccessor,
    ISqlSugarClient db,
    Ip2RegionHelper ip2Region)
  {
    _authService = authService;
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
    _db = db;
    _ip2Region = ip2Region;
  }

  /// <summary>
  /// 登录
  /// </summary>
  /// <param name="loginDto">登录参数</param>
  /// <returns>登录结果</returns>
  [HttpPost("login")]
  [AllowAnonymous]
  [ServiceFilter(typeof(LoginLogAttribute))]
  public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
  {
    var result = await _authService.LoginAsync(loginDto);
    return Ok(result);
  }

  /// <summary>
  /// 登出
  /// </summary>
  /// <returns></returns>
  [HttpPost("logout")]
  [Authorize]
  [ServiceFilter(typeof(LoginLogAttribute))]
  public async Task<IActionResult> LogoutAsync()
  {
    var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
    await _authService.LogoutAsync(userId);
    return Ok();
  }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>新的访问令牌</returns>
  [HttpPost("refresh-token")]
  [AllowAnonymous]
  [ServiceFilter(typeof(LoginLogAttribute))]
  public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken)
  {
    var token = await _authService.RefreshTokenAsync(refreshToken);
    return Ok(token);
  }

  /// <summary>
  /// 生成验证码
  /// </summary>
  /// <returns>验证码结果</returns>
  [HttpGet("verify-code")]
  [AllowAnonymous]
  public async Task<IActionResult> GenerateVerifyCodeAsync()
  {
    var result = await _authService.GenerateVerifyCodeAsync();
    return Ok(result);
  }

  /// <summary>
  /// 获取当前用户信息
  /// </summary>
  /// <returns>用户信息</returns>
  [HttpGet("user-info")]
  [Authorize]
  public async Task<IActionResult> GetUserInfoAsync()
  {
    var userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
    var result = await _authService.GetUserInfoAsync(userId);
    return Ok(result);
  }
}