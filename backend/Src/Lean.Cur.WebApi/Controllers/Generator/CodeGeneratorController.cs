using Lean.Cur.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Lean.Cur.WebApi.Controllers.Generator
{
  /// <summary>
  /// 代码生成控制器
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class CodeGeneratorController : LeanBaseController
  {
    private readonly CodeGenerator _generator;

    public CodeGeneratorController(IConfiguration configuration)
    {
      var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
      var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
      _generator = new CodeGenerator(templatePath, outputPath, configuration);
    }

    /// <summary>
    /// 生成代码
    /// </summary>
    /// <param name="request">生成请求</param>
    /// <returns>生成结果</returns>
    [HttpPost("generate")]
    public async Task<ActionResult> Generate([FromBody] GenerateRequest request)
    {
      await _generator.GenerateAsync(request.EntityName, request.Properties);
      return Ok("代码生成成功");
    }
  }

  /// <summary>
  /// 生成请求
  /// </summary>
  public class GenerateRequest
  {
    /// <summary>
    /// 实体名称
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// 属性列表
    /// </summary>
    public List<PropertyInfo> Properties { get; set; } = new();
  }
}