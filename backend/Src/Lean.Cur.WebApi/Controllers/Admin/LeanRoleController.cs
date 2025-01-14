using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Admin;

/// <summary>
/// 角色管理控制器
/// </summary>
[ApiController]
[Route("api/admin/role")]
[Authorize]
public class LeanRoleController : LeanBaseController
{
    private readonly ILeanRoleService _roleService;

    public LeanRoleController(ILeanRoleService roleService)
    {
        _roleService = roleService;
    }

    #region 基础操作

    /// <summary>
    /// 获取角色分页列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>角色分页列表</returns>
    [HttpGet("page")]
    public async Task<LeanApiResponse<PagedResult<LeanRoleDto>>> GetPagedListAsync([FromQuery] LeanRoleQueryDto queryDto)
    {
        var result = await _roleService.GetPagedListAsync(queryDto);
        return Success(result);
    }

    /// <summary>
    /// 获取角色详情
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>角色详情</returns>
    [HttpGet("{id}")]
    public async Task<LeanApiResponse<LeanRoleDto>> GetByIdAsync(long id)
    {
        if (id <= 0)
        {
            return ValidateError<LeanRoleDto>("角色ID必须大于0");
        }
        var result = await _roleService.GetByIdAsync(id);
        if (result == null)
        {
            return BusinessError<LeanRoleDto>("角色不存在");
        }
        return Success(result);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="createDto">创建参数</param>
    /// <returns>角色ID</returns>
    [HttpPost]
    public async Task<LeanApiResponse<long>> CreateAsync(LeanRoleCreateDto createDto)
    {
        if (string.IsNullOrEmpty(createDto.RoleName))
        {
            return ValidateError<long>("角色名称不能为空");
        }
        if (string.IsNullOrEmpty(createDto.RoleCode))
        {
            return ValidateError<long>("角色编码不能为空");
        }
        var result = await _roleService.CreateAsync(createDto);
        return Success(result, "创建角色成功");
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="updateDto">更新参数</param>
    /// <returns>是否成功</returns>
    [HttpPut]
    public async Task<LeanApiResponse<bool>> UpdateAsync(LeanRoleUpdateDto updateDto)
    {
        if (updateDto.Id <= 0)
        {
            return ValidateError<bool>("角色ID必须大于0");
        }
        var result = await _roleService.UpdateAsync(updateDto);
        return Success(result, result ? "更新角色成功" : "更新角色失败");
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id">角色ID</param>
    /// <returns>是否成功</returns>
    [HttpDelete("{id}")]
    public async Task<LeanApiResponse<bool>> DeleteAsync(long id)
    {
        if (id <= 0)
        {
            return ValidateError<bool>("角色ID必须大于0");
        }
        var result = await _roleService.DeleteAsync(id);
        return Success(result, result ? "删除角色成功" : "删除角色失败");
    }

    /// <summary>
    /// 批量删除角色
    /// </summary>
    /// <param name="ids">角色ID列表</param>
    /// <returns>是否成功</returns>
    [HttpDelete("batch")]
    public async Task<LeanApiResponse<bool>> BatchDeleteAsync([FromBody] List<long> ids)
    {
        if (ids == null || !ids.Any())
        {
            return ValidateError<bool>("请选择要删除的角色");
        }
        var result = await _roleService.BatchDeleteAsync(ids);
        return Success(result, result ? "删除角色成功" : "删除角色失败");
    }

    /// <summary>
    /// 更新角色状态
    /// </summary>
    /// <param name="statusDto">状态参数</param>
    /// <returns>是否成功</returns>
    [HttpPut("status")]
    public async Task<LeanApiResponse<bool>> UpdateStatusAsync(LeanRoleStatusDto statusDto)
    {
        if (statusDto.Id <= 0)
        {
            return ValidateError<bool>("角色ID必须大于0");
        }
        var result = await _roleService.UpdateStatusAsync(statusDto);
        return Success(result, result ? "更新状态成功" : "更新状态失败");
    }

    #endregion 基础操作

    #region 导入导出

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>导入模板</returns>
    [HttpGet("import/template")]
    public async Task<IActionResult> GetImportTemplateAsync()
    {
        var bytes = await _roleService.GetImportTemplateAsync();
        return ExcelResponse(bytes, "角色导入模板.xlsx");
    }

    /// <summary>
    /// 导入角色
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [HttpPost("import")]
    public async Task<LeanApiResponse<LeanRoleImportResultDto>> ImportAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ValidateError<LeanRoleImportResultDto>("请选择要导入的文件");
        }
        var result = await _roleService.ImportAsync(file);
        return Success(result, $"成功导入{result.SuccessCount}条数据，失败{result.FailureCount}条");
    }

    /// <summary>
    /// 导出角色
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>Excel文件</returns>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanRoleQueryDto queryDto)
    {
        var bytes = await _roleService.ExportAsync(queryDto);
        return ExcelResponse(bytes, "角色列表.xlsx");
    }

    #endregion 导入导出

    #region 角色权限

    /// <summary>
    /// 获取角色权限
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>权限列表</returns>
    [HttpGet("{roleId}/permissions")]
    public async Task<LeanApiResponse<List<string>>> GetRolePermissionsAsync(long roleId)
    {
        if (roleId <= 0)
        {
            return ValidateError<List<string>>("角色ID必须大于0");
        }
        var result = await _roleService.GetRolePermissionsAsync(roleId);
        return Success(result);
    }

    /// <summary>
    /// 更新角色权限
    /// </summary>
    /// <param name="permissionDto">权限参数</param>
    /// <returns>是否成功</returns>
    [HttpPut("permissions")]
    public async Task<LeanApiResponse<bool>> UpdateRolePermissionsAsync(LeanRoleMenuPermissionDto permissionDto)
    {
        if (permissionDto.RoleId <= 0)
        {
            return ValidateError<bool>("角色ID必须大于0");
        }
        var result = await _roleService.UpdateRolePermissionsAsync(permissionDto);
        return Success(result, result ? "更新权限成功" : "更新权限失败");
    }

    #endregion 角色权限

    #region 角色用户

    /// <summary>
    /// 获取角色用户列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>用户分页列表</returns>
    [HttpGet("users")]
    public async Task<LeanApiResponse<PagedResult<LeanRoleUserListDto>>> GetRoleUsersAsync([FromQuery] LeanRoleUserQueryDto queryDto)
    {
        if (queryDto.RoleId <= 0)
        {
            return ValidateError<PagedResult<LeanRoleUserListDto>>("角色ID必须大于0");
        }
        var result = await _roleService.GetRoleUsersAsync(queryDto);
        return Success(result);
    }

    /// <summary>
    /// 分配用户到角色
    /// </summary>
    /// <param name="assignDto">分配参数</param>
    /// <returns>是否成功</returns>
    [HttpPost("users")]
    public async Task<LeanApiResponse<bool>> AssignUsersAsync(LeanRoleUserAssignDto assignDto)
    {
        if (assignDto.RoleId <= 0)
        {
            return ValidateError<bool>("角色ID必须大于0");
        }
        if (assignDto.UserIds == null || !assignDto.UserIds.Any())
        {
            return ValidateError<bool>("请选择要分配的用户");
        }
        var result = await _roleService.AssignUsersAsync(assignDto);
        return Success(result, result ? "分配用户成功" : "分配用户失败");
    }

    #endregion 角色用户
} 