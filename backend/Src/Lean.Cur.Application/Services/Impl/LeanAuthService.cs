using Lean.Cur.Application.DTOs;
using Lean.Cur.Domain.Repositories;
using Lean.Cur.Domain.Cache;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services.Impl;

public class LeanAuthService : ILeanAuthService
{
  private readonly ILeanUserRepository _userRepository;
  private readonly ILeanCache _cache;
  private readonly IConfiguration _configuration;

  public LeanAuthService(
      ILeanUserRepository userRepository,
      ILeanCache cache,
      IConfiguration configuration)
  {
    _userRepository = userRepository;
    _cache = cache;
    _configuration = configuration;
  }

  public async Task<LeanLoginResponseDto> LoginAsync(LeanLoginRequestDto request)
  {
    var user = await _userRepository.GetByUsernameAsync(request.UserName);
    if (user == null)
    {
      throw new Exception("用户不存在");
    }

    if (!user.ValidatePassword(request.Password))
    {
      throw new Exception("密码错误");
    }

    if (!user.Status)
    {
      throw new Exception("用户已被禁用");
    }

    var token = await GenerateTokenAsync(user.Id);
    var refreshToken = await GenerateRefreshTokenAsync(user.Id);

    var response = new LeanLoginResponseDto
    {
      Token = token,
      RefreshToken = refreshToken,
      User = new LeanUserDto
      {
        Id = user.Id,
        UserName = user.UserName,
        NickName = user.NickName,
        Email = user.Email,
        Phone = user.Phone,
        Sex = user.Sex,
        Avatar = user.Avatar,
        Status = user.Status,
        DeptId = user.DeptId,
        LoginIp = user.LoginIp,
        LoginDate = user.LoginDate,
        RoleIds = user.Roles.Select(r => r.Id).ToList(),
        DeptIds = user.Depts.Select(d => d.Id).ToList(),
        PositionIds = user.Positions.Select(p => p.Id).ToList(),
        Roles = user.Roles.Select(r => new LeanRoleDto
        {
          Id = r.Id,
          RoleName = r.RoleName,
          RoleCode = r.RoleCode,
          OrderNum = r.OrderNum,
          Status = r.Status,
          Remark = r.Remark
        }).ToList(),
        Permissions = user.Roles.SelectMany(r => r.Permissions).Select(p => new LeanPermissionDto
        {
          Id = p.Id,
          PermissionName = p.PermissionName,
          PermissionCode = p.PermissionCode,
          ParentId = p.ParentId,
          OrderNum = p.OrderNum,
          Path = p.Path,
          Component = p.Component,
          Icon = p.Icon,
          Status = p.Status,
          Visible = p.Visible,
          Remark = p.Remark
        }).Distinct().ToList()
      }
    };

    return response;
  }

  public async Task<bool> LogoutAsync()
  {
    // TODO: 实现注销逻辑
    return await Task.FromResult(true);
  }

  public async Task<string> RefreshTokenAsync(string refreshToken)
  {
    var userId = await ValidateRefreshTokenAsync(refreshToken);
    if (userId == 0)
    {
      throw new Exception("无效的刷新令牌");
    }

    return await GenerateTokenAsync(userId);
  }

  private async Task<string> GenerateTokenAsync(long userId)
  {
    var claims = new[]
    {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception("未配置JWT密钥")));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private async Task<string> GenerateRefreshTokenAsync(long userId)
  {
    var refreshToken = Guid.NewGuid().ToString();
    await _cache.SetAsync($"RefreshToken:{refreshToken}", userId, TimeSpan.FromDays(7));
    return refreshToken;
  }

  private async Task<long> ValidateRefreshTokenAsync(string refreshToken)
  {
    var userId = await _cache.GetAsync<long>($"RefreshToken:{refreshToken}");
    await _cache.RemoveAsync($"RefreshToken:{refreshToken}");
    return userId;
  }

  private async Task<ClaimsIdentity> GenerateClaimsAsync(LeanUser user)
  {
    var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Name, user.UserName),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    // 获取用户角色
    var roles = await _userRepository.GetUserRolesAsync(user.Id);
    foreach (var role in roles)
    {
      claims.Add(new Claim(ClaimTypes.Role, role.RoleCode));
    }

    // 获取用户权限
    var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
    foreach (var permission in permissions)
    {
      claims.Add(new Claim("permission", permission.PermissionCode));
    }

    return new ClaimsIdentity(claims);
  }
}