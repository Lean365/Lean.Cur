using Microsoft.AspNetCore.Authorization;

namespace Lean.Cur.Application.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class LeanPermissionAttribute : AuthorizeAttribute
{
  public const string POLICY_PREFIX = "Permission";

  public LeanPermissionAttribute(string permissionCode) : base($"{POLICY_PREFIX}:{permissionCode}")
  {
  }
}