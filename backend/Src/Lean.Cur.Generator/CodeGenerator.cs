using Scriban;
using System.Text;

namespace Lean.Cur.Generator
{
  public class CodeGenerator
  {
    private readonly string[] _supportedLanguages = new[]
    {
            "zh-CN", // 简体中文
            "zh-TW", // 繁体中文
            "en-US", // 英语
            "fr-FR", // 法语
            "es-ES", // 西班牙语
            "ru-RU", // 俄语
            "ar-SA", // 阿拉伯语
            "ja-JP", // 日语
            "ko-KR", // 韩语
            "vi-VN", // 越南语
            "th-TH"  // 泰语
        };

    private readonly string _templatePath;
    private readonly string _outputPath;

    public CodeGenerator(string templatePath, string outputPath)
    {
      _templatePath = templatePath;
      _outputPath = outputPath;
    }

    public async Task GenerateAsync(string entityName, List<PropertyInfo> properties)
    {
      await GenerateBackendAsync(entityName, properties);
      await GenerateFrontendAsync(entityName, properties);
    }

    private async Task GenerateBackendAsync(string entityName, List<PropertyInfo> properties)
    {
      // 生成实体类
      await GenerateFromTemplateAsync(
          "Domain/entity.sbn",
          $"Domain/Entities/Lean{entityName}.cs",
          new { entity_name = entityName, properties }
      );

      // 生成仓储接口
      await GenerateFromTemplateAsync(
          "Domain/repository_interface.sbn",
          $"Domain/Repositories/ILean{entityName}Repository.cs",
          new { entity_name = entityName }
      );

      // 生成仓储实现
      await GenerateFromTemplateAsync(
          "Infrastructure/repository_implementation.sbn",
          $"Infrastructure/Repositories/Lean{entityName}Repository.cs",
          new { entity_name = entityName }
      );

      // 生成服务接口
      await GenerateFromTemplateAsync(
          "Application/service_interface.sbn",
          $"Application/Services/ILean{entityName}Service.cs",
          new { entity_name = entityName }
      );

      // 生成服务实现
      await GenerateFromTemplateAsync(
          "Application/service_implementation.sbn",
          $"Application/Services/Impl/Lean{entityName}Service.cs",
          new { entity_name = entityName }
      );

      // 生成控制器
      await GenerateFromTemplateAsync(
          "Api/controller.sbn",
          $"Api/Controllers/Lean{entityName}Controller.cs",
          new { entity_name = entityName }
      );
    }

    private async Task GenerateFrontendAsync(string entityName, List<PropertyInfo> properties)
    {
      // 生成Vue组件
      await GenerateFromTemplateAsync(
          "Frontend/vue/index.sbn",
          $"Frontend/views/{entityName}/index.vue",
          new { entity_name = entityName, properties }
      );

      // 生成API请求
      await GenerateFromTemplateAsync(
          "Frontend/api/index.sbn",
          $"Frontend/api/{entityName}.ts",
          new { entity_name = entityName }
      );

      // 生成TypeScript类型定义
      await GenerateFromTemplateAsync(
          "Frontend/api/types.sbn",
          $"Frontend/api/types/{entityName}.ts",
          new { entity_name = entityName, properties }
      );

      // 生成多语言文件
      foreach (var langCode in _supportedLanguages)
      {
        await GenerateFromTemplateAsync(
            "Frontend/locales/locale.sbn",
            $"Frontend/locales/{langCode}/{entityName}.ts",
            new { entity_name = entityName, properties, lang_code = langCode }
        );
      }
    }

    private async Task GenerateFromTemplateAsync(string templateFile, string outputFile, object model)
    {
      var templatePath = Path.Combine(_templatePath, templateFile);
      var outputPath = Path.Combine(_outputPath, outputFile);

      // 确保输出目录存在
      Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

      // 读取模板文件
      var template = await File.ReadAllTextAsync(templatePath);

      // 解析并渲染模板
      var parsedTemplate = Template.Parse(template);
      var result = await parsedTemplate.RenderAsync(model);

      // 写入输出文件
      await File.WriteAllTextAsync(outputPath, result, Encoding.UTF8);
    }
  }

  public class PropertyInfo
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public string Comment { get; set; }
    public bool IsNullable { get; set; }
  }
}