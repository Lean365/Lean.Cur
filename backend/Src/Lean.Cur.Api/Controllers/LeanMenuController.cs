using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanMenuController : LeanBaseController
{
  private readonly ILeanMenuService _menuService;

  public LeanMenuController(ILeanMenuService menuService)
  {
    _menuService = menuService;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeanPermission>>> GetMenuTreeAsync()
  {
    return await _menuService.GetMenuTreeAsync();
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeanPermission>> GetUserMenusAsync(long id)
  {
    return Ok(await _menuService.GetUserMenusAsync(id));
  }

  [HttpPost]
  public async Task<ActionResult<bool>> ValidatePermissionAsync([FromBody] string permissionCode)
  {
    return await _menuService.ValidatePermissionAsync(CurrentUserId, permissionCode);
  }
}