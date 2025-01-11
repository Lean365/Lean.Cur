using Lean.Cur.Application.DTOs.Auth;
using Lean.Cur.Domain.Repositories;
using Lean.Cur.Domain.Cache;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.Extensions.Logging;
using Lean.Cur.Domain.Entities.Log;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace Lean.Cur.Application.Services.Impl;

/// <summary>
/// 认证服务实现
/// </summary>
public class LeanAuthService : ILeanAuthService
{
  private readonly ILeanUserRepository _userRepository;
  private readonly ILeanLoginLogRepository _loginLogRepository;
  private readonly ILeanCache _cache;
  private readonly IConfiguration _configuration;
  private readonly ILogger<LeanAuthService> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public LeanAuthService(
      ILeanUserRepository userRepository,
      ILeanLoginLogRepository loginLogRepository,
      ILeanCache cache,
      IConfiguration configuration,
      ILogger<LeanAuthService> logger,
      IHttpContextAccessor httpContextAccessor)
  {
    _userRepository = userRepository;
    _loginLogRepository = loginLogRepository;
    _cache = cache;
    _configuration = configuration;
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<LeanLoginResponseDto> LoginAsync(LeanLoginRequestDto request)
  {
    try
    {
      _logger.LogInformation("用户 {UserName} 正在尝试登录", request.UserName);

      var loginLog = new LeanLoginLog
      {
        UserName = request.UserName,
        IpAddress = GetClientIp(),
        LoginLocation = await GetLocationByIp(GetClientIp()),
        Browser = GetClientBrowser(),
        Os = GetClientOs(),
        LoginTime = DateTime.Now
      };

      try
      {
        // 验证验证码
        if (!string.IsNullOrEmpty(request.CaptchaCode) && !string.IsNullOrEmpty(request.CaptchaKey))
        {
          var captchaCode = await _cache.GetAsync<string>($"Captcha:{request.CaptchaKey}");
          if (captchaCode == null || !captchaCode.Equals(request.CaptchaCode, StringComparison.OrdinalIgnoreCase))
          {
            loginLog.Status = 1;
            loginLog.Message = "验证码错误或已过期";
            await _loginLogRepository.CreateAsync(loginLog);
            throw new Exception(loginLog.Message);
          }
          await _cache.RemoveAsync($"Captcha:{request.CaptchaKey}");
        }

        // 验证用户名密码
        var user = await _userRepository.GetByUsernameAsync(request.UserName);
        if (user == null)
        {
          loginLog.Status = 1;
          loginLog.Message = "用户名或密码错误";
          await _loginLogRepository.CreateAsync(loginLog);
          throw new Exception(loginLog.Message);
        }

        if (!VerifyPassword(user.Password, request.Password))
        {
          loginLog.Status = 1;
          loginLog.Message = "用户名或密码错误";
          await _loginLogRepository.CreateAsync(loginLog);
          throw new Exception(loginLog.Message);
        }

        // 生成令牌
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = await GenerateRefreshTokenAsync(user.Id);
        var expiresIn = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "120") * 60;

        // 获取用户信息
        var roles = await _userRepository.GetUserRolesAsync(user.Id);
        var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

        var response = new LeanLoginResponseDto
        {
          AccessToken = accessToken,
          RefreshToken = refreshToken,
          ExpiresIn = expiresIn,
          UserInfo = new LeanUserInfoDto
          {
            Id = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Avatar = user.Avatar,
            Roles = roles.Select(r => r.RoleCode).ToList(),
            Permissions = permissions.Select(p => p.PermissionCode).ToList()
          }
        };

        loginLog.Status = 0;
        loginLog.Message = "登录成功";
        await _loginLogRepository.CreateAsync(loginLog);

        _logger.LogInformation("用户 {UserName} 登录成功", request.UserName);
        return response;
      }
      catch (Exception)
      {
        if (loginLog.Status == 0)
        {
          loginLog.Status = 1;
          loginLog.Message = "登录失败";
          await _loginLogRepository.CreateAsync(loginLog);
        }
        throw;
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "用户 {UserName} 登录失败", request.UserName);
      throw;
    }
  }

  public async Task<LeanLoginResponseDto> RefreshTokenAsync(string refreshToken)
  {
    try
    {
      _logger.LogInformation("正在刷新令牌");

      var userId = await ValidateRefreshTokenAsync(refreshToken);
      var user = await _userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        _logger.LogWarning("刷新令牌失败：用户不存在");
        throw new Exception("无效的刷新令牌");
      }

      var accessToken = await GenerateAccessTokenAsync(user);
      var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);
      var expiresIn = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "120") * 60;

      var roles = await _userRepository.GetUserRolesAsync(user.Id);
      var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

      var response = new LeanLoginResponseDto
      {
        AccessToken = accessToken,
        RefreshToken = newRefreshToken,
        ExpiresIn = expiresIn,
        UserInfo = new LeanUserInfoDto
        {
          Id = user.Id,
          UserName = user.UserName,
          NickName = user.NickName,
          Avatar = user.Avatar,
          Roles = roles.Select(r => r.RoleCode).ToList(),
          Permissions = permissions.Select(p => p.PermissionCode).ToList()
        }
      };

      _logger.LogInformation("令牌刷新成功");
      return response;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "刷新令牌失败");
      throw;
    }
  }

  public async Task LogoutAsync(long userId)
  {
    try
    {
      _logger.LogInformation("用户 {UserId} 正在登出", userId);
      // 清除刷新令牌
      var refreshTokens = await _cache.GetAsync<List<string>>($"UserRefreshTokens:{userId}") ?? new List<string>();
      foreach (var token in refreshTokens)
      {
        await _cache.RemoveAsync($"RefreshToken:{token}");
      }
      await _cache.RemoveAsync($"UserRefreshTokens:{userId}");
      _logger.LogInformation("用户 {UserId} 登出成功", userId);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "用户 {UserId} 登出失败", userId);
      throw;
    }
  }

  private async Task<string> GenerateAccessTokenAsync(LeanUser user)
  {
    var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

    // 添加角色声明
    var roles = await _userRepository.GetUserRolesAsync(user.Id);
    foreach (var role in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role.RoleCode));
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "120"));

    var token = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
        audience: _configuration["JwtSettings:Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private async Task<string> GenerateRefreshTokenAsync(long userId)
  {
    var refreshToken = Guid.NewGuid().ToString();
    await _cache.SetAsync($"RefreshToken:{refreshToken}", userId, TimeSpan.FromDays(7));

    // 保存用户的刷新令牌列表
    var userTokens = await _cache.GetAsync<List<string>>($"UserRefreshTokens:{userId}") ?? new List<string>();
    userTokens.Add(refreshToken);
    await _cache.SetAsync($"UserRefreshTokens:{userId}", userTokens, TimeSpan.FromDays(7));

    return refreshToken;
  }

  private async Task<long> ValidateRefreshTokenAsync(string refreshToken)
  {
    var userId = await _cache.GetAsync<long>($"RefreshToken:{refreshToken}");
    if (userId == 0)
    {
      throw new Exception("无效的刷新令牌");
    }
    return userId;
  }

  private bool VerifyPassword(string hashedPassword, string password)
  {
    // TODO: 实现密码验证逻辑
    return hashedPassword == password;
  }

  private string GetClientIp()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null) return "unknown";

    var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
            httpContext.Request.Headers["X-Real-IP"].FirstOrDefault() ??
            httpContext.Connection.RemoteIpAddress?.ToString() ??
            "unknown";

    return ip.Split(',')[0].Trim();
  }

  private async Task<string> GetLocationByIp(string ip)
  {
    try
    {
      // TODO: 实现IP地址定位
      // 可以使用第三方服务或本地IP地址库
      return await Task.FromResult("未知");
    }
    catch
    {
      return "未知";
    }
  }

  private string GetClientBrowser()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null) return "unknown";

    var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
    var uaParser = Parser.GetDefault();
    var clientInfo = uaParser.Parse(userAgent);

    return $"{clientInfo.UA.Family} {clientInfo.UA.Major}";
  }

  private string GetClientOs()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null) return "unknown";

    var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
    var uaParser = Parser.GetDefault();
    var clientInfo = uaParser.Parse(userAgent);

    return $"{clientInfo.OS.Family} {clientInfo.OS.Major}";
  }
}