using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers;

/// <summary>
/// 基类控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class LeanBaseController : ControllerBase
{
}