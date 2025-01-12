using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Admin;

/// <summary>
/// 用户管理控制器
/// </summary>
[ApiController]
[Route("api/admin/user")]
[Authorize]
public class LeanUserController : ControllerBase
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
  public async Task<PagedResult<LeanUserDto>> GetPagedListAsync([FromQuery] LeanUserQueryDto queryDto)
  {
    return await _userService.GetPagedListAsync(queryDto);
  }

  /// <summary>
  /// 获取用户详情
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>用户详情</returns>
  [HttpGet("{id}")]
  public async Task<LeanUserDto> GetByIdAsync(long id)
  {
    return await _userService.GetByIdAsync(id);
  }

  /// <summary>
  /// 创建用户
  /// </summary>
  /// <param name="createDto">创建参数</param>
  /// <returns>用户ID</returns>
  [HttpPost]
  public async Task<long> CreateAsync(LeanUserCreateDto createDto)
  {
    return await _userService.CreateAsync(createDto);
  }

  /// <summary>
  /// 更新用户
  /// </summary>
  /// <param name="updateDto">更新参数</param>
  /// <returns>是否成功</returns>
  [HttpPut]
  public async Task<bool> UpdateAsync(LeanUserUpdateDto updateDto)
  {
    return await _userService.UpdateAsync(updateDto);
  }

  /// <summary>
  /// 删除用户
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteAsync(long id)
  {
    return await _userService.DeleteAsync(id);
  }

  /// <summary>
  /// 更新用户状态
  /// </summary>
  /// <param name="statusDto">状态参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("status")]
  public async Task<bool> UpdateStatusAsync(LeanUserStatusDto statusDto)
  {
    return await _userService.UpdateStatusAsync(statusDto);
  }

  /// <summary>
  /// 重置用户密码
  /// </summary>
  /// <param name="resetDto">重置参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("reset-password")]
  public async Task<bool> ResetPasswordAsync(LeanUserResetPasswordDto resetDto)
  {
    return await _userService.ResetPasswordAsync(resetDto);
  }

  /// <summary>
  /// 修改用户密码
  /// </summary>
  /// <param name="updateDto">修改参数</param>
  /// <returns>是否成功</returns>
  [HttpPut("update-password")]
  public async Task<bool> UpdatePasswordAsync(LeanUserUpdatePasswordDto updateDto)
  {
    // TODO: 从Token中获取用户ID
    var userId = 1L;
    return await _userService.UpdatePasswordAsync(userId, updateDto);
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
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "用户导入模板.xlsx");
  }

  /// <summary>
  /// 导入用户
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  public async Task<object> ImportAsync(IFormFile file)
  {
    var (total, success, errors) = await _userService.ImportAsync(file);
    return new { total, success, errors };
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
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "用户列表.xlsx");
  }
  #endregion
}