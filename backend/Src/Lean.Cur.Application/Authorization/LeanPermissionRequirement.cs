using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

public class LeanPermissionRequirement : IAuthorizationRequirement
{
  public string PermissionCode { get; }

  public LeanPermissionRequirement(string permissionCode)
  {
    PermissionCode = permissionCode;
  }
}