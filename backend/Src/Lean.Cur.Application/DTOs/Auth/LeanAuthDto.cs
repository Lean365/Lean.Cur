using Lean.Cur.Application.DTOs.User;

namespace Lean.Cur.Application.DTOs.Auth;

public class LeanLoginRequestDto
{
  public string UserName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string? VerifyCode { get; set; }
}

public class LeanLoginResponseDto
{
  public string Token { get; set; } = null!;
  public string RefreshToken { get; set; } = null!;
  public Lean.Cur.Application.DTOs.User.LeanUserInfoDto UserInfo { get; set; } = null!;
}

public class LeanUserInfoDto
{
  public long Id { get; set; }
  public string UserName { get; set; } = null!;
  public string NickName { get; set; } = null!;
  public string? Avatar { get; set; }
  public List<string> Permissions { get; set; } = new();
  public List<string> Roles { get; set; } = new();
}