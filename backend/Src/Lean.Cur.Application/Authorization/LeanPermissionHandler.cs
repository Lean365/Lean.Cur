using Lean.Cur.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

public class LeanPermissionHandler : AuthorizationHandler<LeanPermissionRequirement>
{
  private readonly ILeanUserRepository _userRepository;

  public LeanPermissionHandler(ILeanUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LeanPermissionRequirement requirement)
  {
    if (context.User.Identity?.IsAuthenticated != true)
    {
      return;
    }

    var userId = long.Parse(context.User.FindFirst("sub")?.Value ?? "0");
    if (userId == 0)
    {
      return;
    }

    if (await _userRepository.HasPermissionAsync(userId, requirement.PermissionCode))
    {
      context.Succeed(requirement);
    }
  }
}