using System.Security.Claims;
using Lean.Cur.Application.Dtos.Auth;
using Lean.Cur.Application.Services.Auth;
using Lean.Cur.Common.Auth.Jwt;
using Lean.Cur.Common.Auth.SlideVerify;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Security;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Lean.Cur.Domain.Entities.Logging;

namespace Lean.Cur.Infrastructure.Services.Auth;

/// <summary>
/// 认证服务实现
/// </summary>
public class AuthService : IAuthService
{
  private readonly SqlSugarClient _db;
  private readonly SlideVerifyHelper _verifyHelper;
  private readonly JwtHelper _jwtHelper;
  private readonly IMemoryCache _cache;
  private readonly ILogger<AuthService> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public AuthService(
      SqlSugarClient db,
      SlideVerifyHelper verifyHelper,
      JwtHelper jwtHelper,
      IMemoryCache cache,
      ILogger<AuthService> logger,
      IHttpContextAccessor httpContextAccessor)
  {
    _db = db;
    _verifyHelper = verifyHelper;
    _jwtHelper = jwtHelper;
    _cache = cache;
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 生成滑块验证码
  /// </summary>
  public async Task<SlideVerifyResult> GenerateVerifyCodeAsync()
  {
    var result = _verifyHelper.Generate();
    return result;
  }

  /// <summary>
  /// 登录
  /// </summary>
  public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
  {
    try
    {
      // 查询用户
      var user = await _db.Queryable<LeanUser>()
          .Where(u => u.UserName == loginDto.UserName && u.IsDeleted == 0)
          .FirstAsync();

      // 验证滑块验证码
      if (!_verifyHelper.Verify(loginDto.VerifyId, loginDto.X))
      {
        await RecordLoginLogAsync(user?.Id ?? 0, loginDto.UserName, 1, false, "验证码错误");
        throw new LeanUserFriendlyException("验证码错误");
      }

      if (user == null)
      {
        await RecordLoginLogAsync(0, loginDto.UserName, 1, false, "用户名不存在");
        await UpdateLoginFailedAsync(loginDto.UserName);
        throw new LeanUserFriendlyException("用户名或密码错误");
      }

      // 验证密码
      if (!LeanPassword.VerifyPassword(loginDto.Password, user.Password, user.PasswordSalt))
      {
        await RecordLoginLogAsync(user.Id, user.UserName, 1, false, "密码错误");
        await UpdateLoginFailedAsync(user.Id);
        throw new LeanUserFriendlyException("用户名或密码错误");
      }

      // 检查账户锁定状态
      var userExtend = await _db.Queryable<LeanUserExtend>()
          .Where(ue => ue.UserId == user.Id)
          .FirstAsync();

      if (userExtend?.LockEndTime != null && userExtend.LockEndTime > DateTime.Now)
      {
        throw new LeanUserFriendlyException($"账户已锁定，请在{userExtend.LockEndTime.Value:yyyy-MM-dd HH:mm:ss}后重试");
      }

      // 更新登录信息
      await UpdateLoginSuccessAsync(user.Id);

      // 生成令牌
      var accessToken = GenerateAccessToken(user);
      var refreshToken = GenerateRefreshToken(user);

      // 缓存刷新令牌
      var cacheKey = $"refresh_token:{user.Id}";
      _cache.Set(cacheKey, refreshToken, TimeSpan.FromDays(7));

      // 记录登录成功日志
      await RecordLoginLogAsync(user.Id, user.UserName, 1, true);

      return new LoginResultDto
      {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        ExpiresIn = 7200, // 2小时
        User = new LoginUserDto
        {
          Id = user.Id,
          UserName = user.UserName,
          NickName = user.NickName,
          EnglishName = user.EnglishName,
          Avatar = user.Avatar,
          UserType = user.UserType
        }
      };
    }
    catch (Exception ex) when (ex is not LeanUserFriendlyException)
    {
      await RecordLoginLogAsync(0, loginDto.UserName, 1, false, ex.Message);
      throw;
    }
  }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public async Task<LoginResultDto> RefreshTokenAsync(string refreshToken)
  {
    try
    {
      // 验证刷新令牌
      var principal = _jwtHelper.ValidateToken(refreshToken);
      if (principal == null)
      {
        throw new LeanUserFriendlyException("无效的刷新令牌");
      }

      // 获取用户ID
      var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
      if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
      {
        throw new LeanUserFriendlyException("无效的刷新令牌");
      }

      // 验证缓存的刷新令牌
      var cacheKey = $"refresh_token:{userId}";
      if (!_cache.TryGetValue(cacheKey, out string? cachedToken) || cachedToken != refreshToken)
      {
        // 令牌已失效，标记为异常退出
        await HandleSessionExpiredAsync(userId);
        throw new LeanUserFriendlyException("刷新令牌已失效");
      }

      // 查询用户
      var user = await _db.Queryable<LeanUser>()
          .Where(u => u.Id == userId && u.IsDeleted == 0)
          .FirstAsync();

      if (user == null)
      {
        await RecordLoginLogAsync(userId, "未知用户", 3, false, "用户不存在或已被删除");
        throw new LeanUserFriendlyException("用户不存在或已被删除");
      }

      // 生成新令牌
      var accessToken = GenerateAccessToken(user);
      var newRefreshToken = GenerateRefreshToken(user);

      // 更新缓存的刷新令牌
      _cache.Set(cacheKey, newRefreshToken, TimeSpan.FromDays(7));

      // 更新用户活跃时间
      var userExtend = await _db.Queryable<LeanUserExtend>()
          .Where(ue => ue.UserId == userId)
          .FirstAsync();

      if (userExtend != null)
      {
        userExtend.LastActiveTime = DateTime.Now;
        userExtend.LastActiveIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        userExtend.LastHeartbeatTime = DateTime.Now;
        await _db.Updateable(userExtend).ExecuteCommandAsync();
      }

      // 记录刷新令牌日志
      await RecordLoginLogAsync(user.Id, user.UserName, 3, true);

      return new LoginResultDto
      {
        AccessToken = accessToken,
        RefreshToken = newRefreshToken,
        ExpiresIn = 7200,
        User = new LoginUserDto
        {
          Id = user.Id,
          UserName = user.UserName,
          NickName = user.NickName,
          EnglishName = user.EnglishName,
          Avatar = user.Avatar,
          UserType = user.UserType
        }
      };
    }
    catch (Exception ex) when (ex is not LeanUserFriendlyException)
    {
      _logger.LogError(ex, "刷新令牌时发生错误");
      throw new LeanUserFriendlyException("刷新令牌失败");
    }
  }

  /// <summary>
  /// 退出登录
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>是否成功</returns>
  public async Task<bool> LogoutAsync(long userId)
  {
    var user = await _db.Queryable<LeanUser>()
        .Where(u => u.Id == userId)
        .FirstAsync() ?? throw new BusinessException("用户不存在");

    // 更新用户扩展信息
    var userExtend = await _db.Queryable<LeanUserExtend>()
        .Where(ue => ue.UserId == userId)
        .FirstAsync();

    if (userExtend != null)
    {
      userExtend.OnlineStatus = 0;
      userExtend.LastActiveTime = DateTime.Now;
      await _db.Updateable(userExtend).ExecuteCommandAsync();
    }

    // 记录登出日志
    await RecordLoginLogAsync(userId, string.Empty, 2, true);

    return true;
  }

  /// <summary>
  /// 处理会话过期
  /// </summary>
  private async Task HandleSessionExpiredAsync(long userId)
  {
    try
    {
      // 移除刷新令牌缓存
      var cacheKey = $"refresh_token:{userId}";
      _cache.Remove(cacheKey);

      // 更新用户扩展信息
      var userExtend = await _db.Queryable<LeanUserExtend>()
          .Where(ue => ue.UserId == userId)
          .FirstAsync();

      if (userExtend != null)
      {
        userExtend.LastLogoutTime = DateTime.Now;
        userExtend.OnlineStatus = 0; // 设置为离线状态

        // 计算本次在线时长（分钟）
        if (userExtend.LastLoginTime.HasValue)
        {
          var onlineMinutes = (int)(DateTime.Now - userExtend.LastLoginTime.Value).TotalMinutes;
          userExtend.TotalOnlineTime += onlineMinutes;
        }

        await _db.Updateable(userExtend).ExecuteCommandAsync();
      }

      // 记录会话过期日志
      await RecordLoginLogAsync(userId, null, 4, true, "会话过期");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "处理会话过期时发生错误: {UserId}", userId);
    }
  }

  /// <summary>
  /// 获取用户信息
  /// </summary>
  public async Task<LoginUserDto> GetUserInfoAsync(long userId)
  {
    var user = await _db.Queryable<LeanUser>()
        .Where(u => u.Id == userId && u.IsDeleted == 0)
        .FirstAsync()
        ?? throw new LeanUserFriendlyException("用户不存在或已被删除");

    return new LoginUserDto
    {
      Id = user.Id,
      UserName = user.UserName,
      NickName = user.NickName,
      EnglishName = user.EnglishName,
      Avatar = user.Avatar,
      UserType = user.UserType
    };
  }

  /// <summary>
  /// 生成访问令牌
  /// </summary>
  private string GenerateAccessToken(LeanUser user)
  {
    var claims = new[]
    {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.GivenName, user.NickName),
            new Claim("english_name", user.EnglishName),
            new Claim("user_type", ((int)user.UserType).ToString())
        };

    return _jwtHelper.GenerateAccessToken(claims);
  }

  /// <summary>
  /// 生成刷新令牌
  /// </summary>
  private string GenerateRefreshToken(LeanUser user)
  {
    return _jwtHelper.GenerateRefreshToken(user.Id);
  }

  /// <summary>
  /// 更新登录失败信息
  /// </summary>
  private async Task UpdateLoginFailedAsync(long userId)
  {
    var userExtend = await _db.Queryable<LeanUserExtend>()
        .Where(ue => ue.UserId == userId)
        .FirstAsync();

    if (userExtend == null)
    {
      userExtend = new LeanUserExtend
      {
        UserId = userId,
        ErrorCount = 1,
        TotalErrorCount = 1,
        LastErrorTime = DateTime.Now
      };
      await _db.Insertable(userExtend).ExecuteCommandAsync();
    }
    else
    {
      userExtend.ErrorCount++;
      userExtend.TotalErrorCount++;
      userExtend.LastErrorTime = DateTime.Now;

      // 如果错误次数超过限制，锁定账户
      if (userExtend.ErrorCount >= 5)
      {
        userExtend.LockEndTime = DateTime.Now.AddMinutes(30); // 锁定30分钟
      }

      await _db.Updateable(userExtend).ExecuteCommandAsync();
    }
  }

  /// <summary>
  /// 更新登录成功信息
  /// </summary>
  private async Task UpdateLoginSuccessAsync(long userId)
  {
    var userExtend = await _db.Queryable<LeanUserExtend>()
        .Where(ue => ue.UserId == userId)
        .FirstAsync();

    var httpContext = _httpContextAccessor.HttpContext;
    var ip = httpContext?.Connection.RemoteIpAddress?.ToString();
    var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();

    if (userExtend == null)
    {
      userExtend = new LeanUserExtend
      {
        UserId = userId,
        FirstLoginTime = DateTime.Now,
        FirstLoginIp = ip,
        FirstLoginDevice = userAgent,
        LastLoginTime = DateTime.Now,
        LastLoginIp = ip,
        LastLoginDevice = userAgent,
        LoginCount = 1,
        OnlineStatus = 1,
        LastHeartbeatTime = DateTime.Now,
        LastActiveTime = DateTime.Now,
        LastActiveIp = ip,
        SessionId = Guid.NewGuid().ToString("N")
      };
      await _db.Insertable(userExtend).ExecuteCommandAsync();
    }
    else
    {
      userExtend.LastLoginTime = DateTime.Now;
      userExtend.LastLoginIp = ip;
      userExtend.LastLoginDevice = userAgent;
      userExtend.LoginCount++;
      userExtend.ErrorCount = 0; // 重置错误次数
      userExtend.LockEndTime = null; // 解除锁定
      userExtend.OnlineStatus = 1;
      userExtend.LastHeartbeatTime = DateTime.Now;
      userExtend.LastActiveTime = DateTime.Now;
      userExtend.LastActiveIp = ip;
      userExtend.SessionId = Guid.NewGuid().ToString("N");

      await _db.Updateable(userExtend).ExecuteCommandAsync();
    }
  }

  /// <summary>
  /// 更新登录失败信息（用户名不存在的情况）
  /// </summary>
  private async Task UpdateLoginFailedAsync(string userName)
  {
    // 记录到日志中，防止恶意用户名暴力破解
    _logger.LogWarning($"登录失败：用户名 {userName} 不存在");
  }

  /// <summary>
  /// 记录登录日志
  /// </summary>
  private async Task RecordLoginLogAsync(long userId, string userName, int loginType, bool success, string? message = null)
  {
    try
    {
      var httpContext = _httpContextAccessor.HttpContext;
      var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();
      var ip = httpContext?.Connection.RemoteIpAddress?.ToString();

      // 解析User-Agent获取浏览器和操作系统信息
      var (browser, os) = ParseUserAgent(userAgent);

      var log = new LeanLoginLog
      {
        UserId = userId,
        UserName = userName,
        LoginType = loginType,
        Status = success ? 1 : 2,
        LoginTime = DateTime.Now,
        IpAddress = ip,
        Browser = browser,
        Os = os,
        Device = userAgent,
        Message = message
      };

      await _db.Insertable(log).ExecuteCommandAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "记录登录日志时发生错误");
    }
  }

  /// <summary>
  /// 解析User-Agent
  /// </summary>
  private (string? browser, string? os) ParseUserAgent(string? userAgent)
  {
    if (string.IsNullOrEmpty(userAgent))
      return (null, null);

    string? browser = null;
    string? os = null;

    // 解析浏览器
    if (userAgent.Contains("Firefox"))
      browser = "Firefox";
    else if (userAgent.Contains("Chrome"))
      browser = "Chrome";
    else if (userAgent.Contains("Safari"))
      browser = "Safari";
    else if (userAgent.Contains("Edge"))
      browser = "Edge";
    else if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
      browser = "Internet Explorer";

    // 解析操作系统
    if (userAgent.Contains("Windows"))
      os = "Windows";
    else if (userAgent.Contains("Mac"))
      os = "MacOS";
    else if (userAgent.Contains("Linux"))
      os = "Linux";
    else if (userAgent.Contains("Android"))
      os = "Android";
    else if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
      os = "iOS";

    return (browser, os);
  }
}