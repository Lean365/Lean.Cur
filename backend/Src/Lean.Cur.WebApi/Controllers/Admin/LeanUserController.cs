using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Admin;

[ApiController]
[Route("api/admin/user")]
[Authorize]
public class LeanUserController : LeanBaseController
{
  private readonly ILeanUserService _userService;

  public LeanUserController(ILeanUserService userService)
  {
    _userService = userService;
  }

  [HttpGet("page")]
  public async Task<LeanApiResult<LeanPagedResult<LeanUserDto>>> GetPagedListAsync([FromQuery] LeanUserQueryDto queryDto)
  {
    var result = await _userService.GetPagedListAsync(queryDto);
    return Success(result);
  }

  [HttpGet("{id}")]
  public async Task<LeanApiResult<LeanUserDto>> GetByIdAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<LeanUserDto>("用户ID必须大于0");
    }
    var result = await _userService.GetByIdAsync(id);
    if (result == null)
    {
      return BusinessError<LeanUserDto>("用户不存在");
    }
    return Success(result);
  }

  [HttpPost]
  [AuditLog("创建用户", "创建新用户")]
  public async Task<LeanApiResult<long>> CreateAsync(LeanUserCreateDto createDto)
  {
    if (string.IsNullOrEmpty(createDto.UserName))
    {
      return ValidateError<long>("用户名不能为空");
    }
    if (string.IsNullOrEmpty(createDto.Password))
    {
      return ValidateError<long>("密码不能为空");
    }
    var result = await _userService.CreateAsync(createDto);
    return Success(result, "创建用户成功");
  }

  [HttpPut]
  public async Task<LeanApiResult<bool>> UpdateAsync(LeanUserUpdateDto updateDto)
  {
    if (updateDto.Id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    var result = await _userService.UpdateAsync(updateDto);
    return Success(result, result ? "更新用户成功" : "更新用户失败");
  }

  [HttpDelete("{id}")]
  public async Task<LeanApiResult<bool>> DeleteAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    var result = await _userService.DeleteAsync(id);
    return Success(result, result ? "删除用户成功" : "删除用户失败");
  }

  [HttpPut("status")]
  public async Task<LeanApiResult<bool>> UpdateStatusAsync(LeanUserStatusDto statusDto)
  {
    if (statusDto.Id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    var result = await _userService.UpdateStatusAsync(statusDto);
    return Success(result, result ? "更新状态成功" : "更新状态失败");
  }

  [HttpPut("reset-password")]
  public async Task<LeanApiResult<bool>> ResetPasswordAsync(LeanUserResetPasswordDto resetDto)
  {
    if (resetDto.Id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    if (string.IsNullOrEmpty(resetDto.NewPassword))
    {
      return ValidateError<bool>("新密码不能为空");
    }
    var result = await _userService.ResetPasswordAsync(resetDto);
    return Success(result, result ? "重置密码成功" : "重置密码失败");
  }

  [HttpPut("update-password")]
  public async Task<LeanApiResult<bool>> UpdatePasswordAsync(LeanUserUpdatePasswordDto updateDto)
  {
    if (string.IsNullOrEmpty(updateDto.OldPassword))
    {
      return ValidateError<bool>("原密码不能为空");
    }
    if (string.IsNullOrEmpty(updateDto.NewPassword))
    {
      return ValidateError<bool>("新密码不能为空");
    }
    var userId = 1L;
    var result = await _userService.UpdatePasswordAsync(userId, updateDto);
    return Success(result, result ? "修改密码成功" : "修改密码失败");
  }

  [HttpGet("import/template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _userService.GetImportTemplateAsync();
    return ExcelResponse(bytes, "用户导入模板.xlsx");
  }

  [HttpPost("import")]
  public async Task<LeanApiResult<LeanUserImportResultDto>> ImportAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      return ValidateError<LeanUserImportResultDto>("请选择要导入的文件");
    }
    var result = await _userService.ImportAsync(file);
    return Success(result, $"成功导入{result.SuccessCount}条数据，失败{result.FailureCount}条");
  }

  // ... existing code ...
}