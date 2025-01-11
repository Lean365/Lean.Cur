using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanUserController : LeanBaseController
{
  private readonly ILeanUserService _userService;

  public LeanUserController(ILeanUserService userService)
  {
    _userService = userService;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeanUser>>> GetUserListAsync([FromQuery] string? keyword)
  {
    return await _userService.GetUserListAsync(keyword);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeanUser>> GetUserAsync(long id)
  {
    var user = await _userService.GetUserAsync(id);
    if (user == null)
    {
      return NotFound();
    }
    return user;
  }

  [HttpPost]
  public async Task<ActionResult<LeanUser>> CreateUserAsync([FromBody] LeanUser user)
  {
    return await _userService.CreateUserAsync(user);
  }

  [HttpPut]
  public async Task<ActionResult<LeanUser>> UpdateUserAsync([FromBody] LeanUser user)
  {
    return await _userService.UpdateUserAsync(user);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteUserAsync(long id)
  {
    await _userService.DeleteUserAsync(id);
    return Ok();
  }

  [HttpGet("{id}/roles")]
  public async Task<ActionResult<List<LeanRole>>> GetUserRolesAsync(long id)
  {
    return await _userService.GetUserRolesAsync(id);
  }

  [HttpPost("{id}/roles")]
  public async Task<ActionResult> AssignRolesAsync(long id, [FromBody] List<long> roleIds)
  {
    await _userService.AssignRolesAsync(id, roleIds);
    return Ok();
  }

  [HttpPost("{id}/departments")]
  public async Task<ActionResult> AssignDepartmentsAsync(long id, [FromBody] List<long> deptIds)
  {
    await _userService.AssignDepartmentsAsync(id, deptIds);
    return Ok();
  }

  [HttpPost("{id}/positions")]
  public async Task<ActionResult> AssignPositionsAsync(long id, [FromBody] List<long> positionIds)
  {
    await _userService.AssignPositionsAsync(id, positionIds);
    return Ok();
  }
}