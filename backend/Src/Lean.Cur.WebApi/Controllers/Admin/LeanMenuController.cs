using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lean.Cur.WebApi.Controllers.Admin
{
  /// <summary>
  /// 菜单控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class LeanMenuController : LeanBaseController
  {
    private readonly ILeanMenuService _menuService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="menuService">菜单服务</param>
    public LeanMenuController(ILeanMenuService menuService)
    {
      _menuService = menuService;
    }

    /// <summary>
    /// 导入菜单数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [HttpPost("import")]
    public async Task<LeanApiResult<LeanMenuImportResultDto>> ImportAsync([FromForm] IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return ValidateError<LeanMenuImportResultDto>("请选择要导入的文件");
      }

      if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
      {
        return ValidateError<LeanMenuImportResultDto>("只支持导入.xlsx格式的文件");
      }

      var result = await _menuService.ImportAsync(file);
      if (result.FailureCount > 0)
      {
        return Error($"导入失败：成功{result.SuccessCount}条，失败{result.FailureCount}条", result);
      }

      return Success(result, $"成功导入{result.SuccessCount}条数据");
    }

    /// <summary>
    /// 导出菜单数据
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>Excel文件</returns>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] LeanMenuQueryDto queryDto)
    {
      var bytes = await _menuService.ExportAsync(queryDto);
      return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "菜单列表.xlsx");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel模板文件</returns>
    [HttpGet("import/template")]
    public async Task<IActionResult> GetImportTemplateAsync()
    {
      var bytes = await _menuService.GetImportTemplateAsync();
      return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "菜单导入模板.xlsx");
    }
  }
}