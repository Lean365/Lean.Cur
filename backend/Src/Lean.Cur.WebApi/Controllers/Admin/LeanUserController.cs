using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Admin;

/// <summary>
/// 用户管理控制器
/// </summary>
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

  #region 基础操作
  /// <summary>
  /// 获取用户分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>用户分页列表</returns>
  [HttpGet("page")]
  public async Task<LeanApiResponse<PagedResult<LeanUserDto>>> GetPagedListAsync([FromQuery] LeanUserQueryDto queryDto)
  {
    var result = await _userService.GetPagedListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取用户详情
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>用户详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanApiResponse<LeanUserDto>> GetByIdAsync(long id)
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

  /// <summary>
  /// 创建用户
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>用户ID</returns>
  [HttpPost]
  public async Task<LeanApiResponse<long>> CreateAsync(LeanUserCreateDto createDto)
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

  /// <summary>
  /// 更新用户
  /// </summary>
  /// <param name="updateDto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<LeanApiResponse<bool>> UpdateAsync(LeanUserUpdateDto updateDto)
  {
    if (updateDto.Id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    var result = await _userService.UpdateAsync(updateDto);
    return Success(result, result ? "更新用户成功" : "更新用户失败");
  }

  /// <summary>
  /// 删除用户
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<LeanApiResponse<bool>> DeleteAsync(long id)
  {
    if (id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    var result = await _userService.DeleteAsync(id);
    return Success(result, result ? "删除用户成功" : "删除用户失败");
  }

  /// <summary>
  /// 更新用户状态
  /// </summary>
  /// <param name="statusDto">状态参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("status")]
  public async Task<LeanApiResponse<bool>> UpdateStatusAsync(LeanUserStatusDto statusDto)
  {
    if (statusDto.Id <= 0)
    {
      return ValidateError<bool>("用户ID必须大于0");
    }
    var result = await _userService.UpdateStatusAsync(statusDto);
    return Success(result, result ? "更新状态成功" : "更新状态失败");
  }

  /// <summary>
  /// 重置用户密码
  /// </summary>
  /// <param name="resetDto">重置参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("reset-password")]
  public async Task<LeanApiResponse<bool>> ResetPasswordAsync(LeanUserResetPasswordDto resetDto)
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

  /// <summary>
  /// 修改用户密码
  /// </summary>
  /// <param name="updateDto">修改参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("update-password")]
  public async Task<LeanApiResponse<bool>> UpdatePasswordAsync(LeanUserUpdatePasswordDto updateDto)
  {
    if (string.IsNullOrEmpty(updateDto.OldPassword))
    {
      return ValidateError<bool>("原密码不能为空");
    }
    if (string.IsNullOrEmpty(updateDto.NewPassword))
    {
      return ValidateError<bool>("新密码不能为空");
    }
    // TODO: 从Token中获取用户ID
    var userId = 1L;
    var result = await _userService.UpdatePasswordAsync(userId, updateDto);
    return Success(result, result ? "修改密码成功" : "修改密码失败");
  }
  #endregion

  #region 导入导出
  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  [HttpGet("import/template")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _userService.GetImportTemplateAsync();
    return ExcelResponse(bytes, "用户导入模板.xlsx");
  }

  /// <summary>
  /// 导入用户
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  public async Task<LeanApiResponse<LeanUserImportResultDto>> ImportAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      return ValidateError<LeanUserImportResultDto>("请选择要导入的文件");
    }
    var result = await _userService.ImportAsync(file);
    return Success(result, $"成功导入{result.SuccessCount}条数据，失败{result.FailureCount}条");
  }

  /// <summary>
  /// 导出用户
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>Excel文件</returns>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanUserQueryDto queryDto)
  {
    var bytes = await _userService.ExportAsync(queryDto);
    return ExcelResponse(bytes, "用户列表.xlsx");
  }
  #endregion
}