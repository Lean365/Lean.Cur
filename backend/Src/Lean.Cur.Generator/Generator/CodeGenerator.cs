using System.Text;
using Scriban;
using SqlSugar;
using Microsoft.Extensions.Configuration;

namespace Lean.Cur.Generator;

/// <summary>
/// 代码生成器
/// </summary>
public class CodeGenerator
{
  private readonly string _templatePath;
  private readonly string _outputPath;
  private readonly SqlSugarClient _db;

  /// <summary>
  /// 初始化代码生成器
  /// </summary>
  /// <param name="templatePath">模板路径</param>
  /// <param name="outputPath">输出路径</param>
  /// <param name="configuration">配置</param>
  public CodeGenerator(string templatePath, string outputPath, IConfiguration configuration)
  {
    _templatePath = templatePath;
    _outputPath = outputPath;

    var connectionString = configuration.GetConnectionString("DbFirst");
    _db = new SqlSugarClient(new ConnectionConfig()
    {
      ConnectionString = connectionString,
      DbType = DbType.SqlServer,
      IsAutoCloseConnection = true
    });
  }

  /// <summary>
  /// 生成代码
  /// </summary>
  /// <param name="entityName">实体名称</param>
  /// <param name="properties">属性列表</param>
  public async Task GenerateAsync(string entityName, List<PropertyInfo> properties)
  {
    // 使用 DbFirst 生成实体
    _db.DbFirst
        .IsCreateAttribute()  // 生成特性
        .StringNullable()     // .NET 6+ 字符串可空
        .Where(entityName)    // 指定表名
        .CreateClassFile(_outputPath, "Lean.Cur.Domain.Entities"); // 生成到指定目录和命名空间

    var data = new
    {
      EntityName = entityName,
      Properties = properties,
      Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    };

    // 生成实体类
    await GenerateFileAsync($"Domain/Entities/{entityName}.cs", "entity.sbn", data);
    // 生成仓储接口
    await GenerateFileAsync($"Infrastructure/Repositories/I{entityName}Repository.cs", "repository_interface.sbn", data);
    // 生成仓储实现
    await GenerateFileAsync($"Infrastructure/Repositories/{entityName}Repository.cs", "repository_implementation.sbn", data);
    // 生成服务接口
    await GenerateFileAsync($"Application/Services/I{entityName}Service.cs", "service_interface.sbn", data);
    // 生成服务实现
    await GenerateFileAsync($"Application/Services/{entityName}Service.cs", "service_implementation.sbn", data);
    // 生成控制器
    await GenerateFileAsync($"WebApi/Controllers/{entityName}Controller.cs", "controller.sbn", data);
    // 生成DTO
    await GenerateFileAsync($"Application/Dtos/{entityName}Dto.cs", "dto.sbn", data);
    // 生成数据库脚本
    await GenerateFileAsync($"Database/{entityName}.sql", "table.sql.sbn", data);
    // 生成前端API
    await GenerateFileAsync($"Frontend/src/api/{entityName.ToLower()}.ts", "api.ts.sbn", data);
    // 生成前端类型定义
    await GenerateFileAsync($"Frontend/src/api/types/{entityName.ToLower()}.ts", "types.ts.sbn", data);
    // 生成前端列表页
    await GenerateFileAsync($"Frontend/src/views/{entityName.ToLower()}/list.vue", "list.vue.sbn", data);
    // 生成前端表单页
    await GenerateFileAsync($"Frontend/src/views/{entityName.ToLower()}/form.vue", "form.vue.sbn", data);
    // 生成前端详情页
    await GenerateFileAsync($"Frontend/src/views/{entityName.ToLower()}/detail.vue", "detail.vue.sbn", data);
    // 生成多语言资源
    await GenerateFileAsync($"Frontend/src/locales/zh-CN/{entityName.ToLower()}.ts", "zh-CN.sbn", data);
    await GenerateFileAsync($"Frontend/src/locales/en-US/{entityName.ToLower()}.ts", "en-US.sbn", data);
  }

  /// <summary>
  /// 生成单个文件
  /// </summary>
  /// <param name="relativePath">相对路径</param>
  /// <param name="templateName">模板文件</param>
  /// <param name="data">模板数据</param>
  private async Task GenerateFileAsync(string relativePath, string templateName, object data)
  {
    var templatePath = Path.Combine(_templatePath, templateName);
    var outputPath = Path.Combine(_outputPath, relativePath);

    // 确保输出目录存在
    var outputDir = Path.GetDirectoryName(outputPath);
    if (!Directory.Exists(outputDir))
    {
      Directory.CreateDirectory(outputDir!);
    }

    // 读取模板
    var templateText = await File.ReadAllTextAsync(templatePath);

    // 使用 Scriban 解析模板
    var template = Template.Parse(templateText);

    // 渲染模板
    var result = await template.RenderAsync(data);

    // 写入文件
    await File.WriteAllTextAsync(outputPath, result);
  }
}

/// <summary>
/// 属性信息
/// </summary>
public class PropertyInfo
{
  /// <summary>
  /// 属性名称
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// 属性类型
  /// </summary>
  public string Type { get; set; } = string.Empty;

  /// <summary>
  /// 属性注释
  /// </summary>
  public string Comment { get; set; } = string.Empty;

  /// <summary>
  /// 是否可空
  /// </summary>
  public bool IsNullable { get; set; }
}

