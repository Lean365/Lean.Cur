using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Lean.Cur.Common.Auth.Jwt;

namespace Lean.Cur.Common.Auth.Jwt;

/// <summary>
/// JWT工具类
/// </summary>
public class LeanJwtHelper
{
  private readonly LeanJwtOptions _options;
  private readonly SigningCredentials _credentials;

  public LeanJwtHelper(LeanJwtOptions options)
  {
    _options = options;
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
    _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
  }

  /// <summary>
  /// 生成访问令牌
  /// </summary>
  /// <param name="claims">声明集合</param>
  /// <returns>访问令牌</returns>
  public string GenerateAccessToken(IEnumerable<Claim> claims)
  {
    var token = new JwtSecurityToken(
        issuer: _options.Issuer,
        audience: _options.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpiryMinutes),
        signingCredentials: _credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  /// <summary>
  /// 生成刷新令牌
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>刷新令牌</returns>
  public string GenerateRefreshToken(long userId)
  {
    var claims = new[]
    {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

    var token = new JwtSecurityToken(
        issuer: _options.Issuer,
        audience: _options.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddDays(_options.RefreshTokenExpiryDays),
        signingCredentials: _credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  /// <summary>
  /// 验证令牌
  /// </summary>
  /// <param name="token">令牌</param>
  /// <returns>声明主体</returns>
  public ClaimsPrincipal? ValidateToken(string token)
  {
    try
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_options.SecretKey);
      var validationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = _options.Issuer,
        ValidateAudience = true,
        ValidAudience = _options.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
      return principal;
    }
    catch
    {
      return null;
    }
  }
}