using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeanPositionController : LeanBaseController
{
  private readonly ILeanPositionService _positionService;

  public LeanPositionController(ILeanPositionService positionService)
  {
    _positionService = positionService;
  }

  [HttpGet("list")]
  public async Task<IActionResult> GetPositionList([FromQuery] string? keyword)
  {
    var positions = await _positionService.GetPositionListAsync(keyword);
    return Ok(positions);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetPosition(long id)
  {
    var position = await _positionService.GetPositionAsync(id);
    return Ok(position);
  }

  [HttpPost]
  public async Task<IActionResult> CreatePosition([FromBody] LeanPosition position)
  {
    await _positionService.CreatePositionAsync(position);
    return Ok();
  }

  [HttpPut]
  public async Task<IActionResult> UpdatePosition([FromBody] LeanPosition position)
  {
    await _positionService.UpdatePositionAsync(position);
    return Ok();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePosition(long id)
  {
    await _positionService.DeletePositionAsync(id);
    return Ok();
  }

  [HttpGet("{id}/users")]
  public async Task<IActionResult> GetPositionUsers(long id)
  {
    var users = await _positionService.GetPositionUsersAsync(id);
    return Ok(users);
  }
}