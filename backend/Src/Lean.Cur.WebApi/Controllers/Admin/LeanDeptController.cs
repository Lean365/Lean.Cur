using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Admin;

/// <summary>
/// 部门管理控制器
/// </summary>
[ApiController]
[Route("api/admin/dept")]
[Authorize]
public class LeanDeptController : LeanBaseController
{
    private readonly ILeanDeptService _deptService;

    public LeanDeptController(ILeanDeptService deptService)
    {
        _deptService = deptService;
    }

    #region 基础操作

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>部门列表</returns>
    [HttpGet("list")]
    public async Task<LeanApiResponse<List<LeanDeptDto>>> GetListAsync([FromQuery] LeanDeptQueryDto queryDto)
    {
        var result = await _deptService.GetListAsync(queryDto);
        return Success(result);
    }

    /// <summary>
    /// 获取部门详情
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>部门详情</returns>
    [HttpGet("{id}")]
    public async Task<LeanApiResponse<LeanDeptDto>> GetByIdAsync(long id)
    {
        if (id <= 0)
        {
            return ValidateError<LeanDeptDto>("部门ID必须大于0");
        }
        var result = await _deptService.GetByIdAsync(id);
        if (result == null)
        {
            return BusinessError<LeanDeptDto>("部门不存在");
        }
        return Success(result);
    }

    /// <summary>
    /// 创建部门
    /// </summary>
    /// <param name="createDto">创建参数</param>
    /// <returns>部门信息</returns>
    [HttpPost]
    public async Task<LeanApiResponse<LeanDeptDto>> CreateAsync(LeanDeptCreateDto createDto)
    {
        if (string.IsNullOrEmpty(createDto.DeptName))
        {
            return ValidateError<LeanDeptDto>("部门名称不能为空");
        }
        if (string.IsNullOrEmpty(createDto.DeptCode))
        {
            return ValidateError<LeanDeptDto>("部门编码不能为空");
        }
        var result = await _deptService.CreateAsync(createDto);
        return Success(result, "创建部门成功");
    }

    /// <summary>
    /// 更新部门
    /// </summary>
    /// <param name="updateDto">更新参数</param>
    /// <returns>部门信息</returns>
    [HttpPut]
    public async Task<LeanApiResponse<LeanDeptDto>> UpdateAsync(LeanDeptUpdateDto updateDto)
    {
        if (updateDto.Id <= 0)
        {
            return ValidateError<LeanDeptDto>("部门ID必须大于0");
        }
        var result = await _deptService.UpdateAsync(updateDto);
        return Success(result, "更新部门成功");
    }

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns>是否成功</returns>
    [HttpDelete("{id}")]
    public async Task<LeanApiResponse<bool>> DeleteAsync(long id)
    {
        if (id <= 0)
        {
            return ValidateError<bool>("部门ID必须大于0");
        }
        var result = await _deptService.DeleteAsync(id);
        return Success(result, result ? "删除部门成功" : "删除部门失败");
    }

    /// <summary>
    /// 更新部门状态
    /// </summary>
    /// <param name="statusDto">状态参数</param>
    /// <returns>是否成功</returns>
    [HttpPut("status")]
    public async Task<LeanApiResponse<bool>> UpdateStatusAsync(LeanDeptStatusDto statusDto)
    {
        if (statusDto.Id <= 0)
        {
            return ValidateError<bool>("部门ID必须大于0");
        }
        var result = await _deptService.UpdateStatusAsync(statusDto);
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
        var bytes = await _deptService.GetImportTemplateAsync();
        return ExcelResponse(bytes, "部门导入模板.xlsx");
    }

    /// <summary>
    /// 导入部门
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [HttpPost("import")]
    public async Task<LeanApiResponse<LeanDeptImportResultDto>> ImportAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ValidateError<LeanDeptImportResultDto>("请选择要导入的文件");
        }
        var result = await _deptService.ImportAsync(file);
        return Success(result, $"成功导入{result.SuccessCount}条数据，失败{result.FailureCount}条");
    }

    /// <summary>
    /// 导出部门
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>Excel文件</returns>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanDeptQueryDto queryDto)
    {
        var bytes = await _deptService.ExportAsync(queryDto);
        return ExcelResponse(bytes, "部门列表.xlsx");
    }

    #endregion 导入导出
} 