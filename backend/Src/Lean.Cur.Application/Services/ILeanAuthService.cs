using Lean.Cur.Application.DTOs;

namespace Lean.Cur.Application.Services;

public interface ILeanAuthService
{
  Task<LeanLoginResponseDto> LoginAsync(LeanLoginRequestDto request);
  Task<bool> LogoutAsync();
  Task<string> RefreshTokenAsync(string refreshToken);
}