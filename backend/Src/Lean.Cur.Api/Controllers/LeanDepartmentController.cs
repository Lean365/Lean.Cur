using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanDepartmentController : LeanBaseController
{
  private readonly ILeanDepartmentService _departmentService;

  public LeanDepartmentController(ILeanDepartmentService departmentService)
  {
    _departmentService = departmentService;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeanDept>>> GetDeptListAsync([FromQuery] string? keyword)
  {
    return await _departmentService.GetDeptListAsync(keyword);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeanDept>> GetDeptAsync(long id)
  {
    var dept = await _departmentService.GetDeptAsync(id);
    if (dept == null)
    {
      return NotFound();
    }
    return dept;
  }

  [HttpPost]
  public async Task<ActionResult<LeanDept>> CreateDeptAsync([FromBody] LeanDept dept)
  {
    return await _departmentService.CreateDeptAsync(dept);
  }

  [HttpPut]
  public async Task<ActionResult<LeanDept>> UpdateDeptAsync([FromBody] LeanDept dept)
  {
    return await _departmentService.UpdateDeptAsync(dept);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteDeptAsync(long id)
  {
    await _departmentService.DeleteDeptAsync(id);
    return Ok();
  }

  [HttpGet("{id}/users")]
  public async Task<ActionResult<List<LeanUser>>> GetDeptUsersAsync(long id)
  {
    return await _departmentService.GetDeptUsersAsync(id);
  }

  [HttpPost("{id}/users")]
  public async Task<ActionResult> AssignUsersAsync(long id, [FromBody] List<long> userIds)
  {
    await _departmentService.AssignUsersAsync(id, userIds);
    return Ok();
  }
}