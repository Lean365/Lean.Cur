using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

public abstract class LeanBaseController : ControllerBase
{
  protected long CurrentUserId => long.Parse(User.FindFirst("sub")?.Value ?? "0");
  protected string? CurrentUserName => User.FindFirst("name")?.Value;
}