using Lean.Cur.Application.Services;
using Lean.Cur.Domain.Entities;
using Lean.Cur.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanNoticeController : LeanBaseController
{
  private readonly ILeanNoticeService _noticeService;

  public LeanNoticeController(ILeanNoticeService noticeService)
  {
    _noticeService = noticeService;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeanNotice>>> GetNoticeListAsync([FromQuery] string? keyword)
  {
    return await _noticeService.GetNoticeListAsync(keyword);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeanNotice>> GetNoticeAsync(long id)
  {
    var notice = await _noticeService.GetNoticeAsync(id);
    if (notice == null)
    {
      return NotFound();
    }
    return notice;
  }

  [HttpPost]
  public async Task<ActionResult<LeanNotice>> CreateNoticeAsync([FromBody] LeanNotice notice)
  {
    return await _noticeService.CreateNoticeAsync(notice);
  }

  [HttpPut]
  public async Task<ActionResult<LeanNotice>> UpdateNoticeAsync([FromBody] LeanNotice notice)
  {
    return await _noticeService.UpdateNoticeAsync(notice);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteNoticeAsync(long id)
  {
    await _noticeService.DeleteNoticeAsync(id);
    return Ok();
  }
}