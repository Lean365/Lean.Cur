using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeanRoleController : LeanBaseController
{
  private readonly ILeanRoleService _roleService;

  public LeanRoleController(ILeanRoleService roleService)
  {
    _roleService = roleService;
  }

  [HttpGet("list")]
  public async Task<IActionResult> GetRoleList([FromQuery] string? keyword)
  {
    var roles = await _roleService.GetRoleListAsync(keyword);
    return Ok(roles);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetRole(long id)
  {
    var role = await _roleService.GetRoleAsync(id);
    return Ok(role);
  }

  [HttpPost]
  public async Task<IActionResult> CreateRole([FromBody] LeanRole role)
  {
    await _roleService.CreateRoleAsync(role);
    return Ok();
  }

  [HttpPut]
  public async Task<IActionResult> UpdateRole([FromBody] LeanRole role)
  {
    await _roleService.UpdateRoleAsync(role);
    return Ok();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteRole(long id)
  {
    await _roleService.DeleteRoleAsync(id);
    return Ok();
  }

  [HttpPost("{roleId}/permissions")]
  public async Task<IActionResult> AssignPermissions(long roleId, [FromBody] List<long> permissionIds)
  {
    await _roleService.AssignPermissionsAsync(roleId, permissionIds);
    return Ok();
  }

  [HttpGet("{roleId}/permissions")]
  public async Task<IActionResult> GetRolePermissions(long roleId)
  {
    var permissions = await _roleService.GetRolePermissionsAsync(roleId);
    return Ok(permissions);
  }
}