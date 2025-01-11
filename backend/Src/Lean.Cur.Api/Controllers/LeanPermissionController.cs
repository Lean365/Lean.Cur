using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanPermissionController : LeanBaseController
{
  private readonly ILeanPermissionService _permissionService;

  public LeanPermissionController(ILeanPermissionService permissionService)
  {
    _permissionService = permissionService;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeanPermission>>> GetPermissionListAsync([FromQuery] string? keyword)
  {
    return await _permissionService.GetPermissionListAsync(keyword);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeanPermission>> GetPermissionAsync(long id)
  {
    var permission = await _permissionService.GetPermissionAsync(id);
    if (permission == null)
    {
      return NotFound();
    }
    return permission;
  }

  [HttpPost]
  public async Task<ActionResult<LeanPermission>> CreatePermissionAsync([FromBody] LeanPermission permission)
  {
    return await _permissionService.CreatePermissionAsync(permission);
  }

  [HttpPut]
  public async Task<ActionResult<LeanPermission>> UpdatePermissionAsync([FromBody] LeanPermission permission)
  {
    return await _permissionService.UpdatePermissionAsync(permission);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeletePermissionAsync(long id)
  {
    await _permissionService.DeletePermissionAsync(id);
    return Ok();
  }

  [HttpGet("tree")]
  public async Task<ActionResult<List<LeanPermission>>> GetPermissionTreeAsync()
  {
    return await _permissionService.GetPermissionTreeAsync();
  }

  [HttpGet("user-menus/{userId}")]
  public async Task<ActionResult<List<LeanPermission>>> GetUserMenusAsync(long userId)
  {
    return await _permissionService.GetUserMenusAsync(userId);
  }

  [HttpGet("user-buttons/{userId}")]
  public async Task<ActionResult<List<LeanPermission>>> GetUserButtonsAsync(long userId)
  {
    return await _permissionService.GetUserButtonsAsync(userId);
  }

  [HttpPost("validate")]
  public async Task<ActionResult<bool>> ValidatePermissionAsync([FromBody] string permissionCode)
  {
    return await _permissionService.ValidatePermissionAsync(CurrentUserId, permissionCode);
  }

  [HttpGet("user-codes/{userId}")]
  public async Task<ActionResult<List<string>>> GetUserPermissionCodesAsync(long userId)
  {
    return await _permissionService.GetUserPermissionCodesAsync(userId);
  }

  [HttpPost("user-roles")]
  public async Task<ActionResult> AssignUserRolesAsync([FromQuery] long userId, [FromBody] List<long> roleIds)
  {
    await _permissionService.AssignUserRolesAsync(userId, roleIds);
    return Ok();
  }

  [HttpPost("role-permissions")]
  public async Task<ActionResult> AssignRolePermissionsAsync([FromQuery] long roleId, [FromBody] List<long> permissionIds)
  {
    await _permissionService.AssignRolePermissionsAsync(roleId, permissionIds);
    return Ok();
  }

  [HttpGet("check-code")]
  public async Task<ActionResult<bool>> CheckPermissionCodeAsync([FromQuery] string code)
  {
    return await _permissionService.CheckPermissionCodeAsync(code);
  }
}