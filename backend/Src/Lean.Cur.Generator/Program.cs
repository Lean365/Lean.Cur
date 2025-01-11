using Lean.Cur.Generator;

// 定义模板路径和输出路径
var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

// 创建代码生成器实例
var generator = new CodeGenerator(templatePath, outputPath);

// 定义实体属性
var properties = new List<PropertyInfo>
{
    new() { Name = "Title", Type = "string", Comment = "通知标题", IsNullable = false },
    new() { Name = "Content", Type = "string", Comment = "通知内容", IsNullable = false },
    new() { Name = "Type", Type = "int", Comment = "通知类型：1=系统通知，2=待办通知", IsNullable = false },
    new() { Name = "Status", Type = "bool", Comment = "状态：true=启用，false=禁用", IsNullable = false },
    new() { Name = "PublishTime", Type = "DateTime", Comment = "发布时间", IsNullable = true },
    new() { Name = "Publisher", Type = "string", Comment = "发布人", IsNullable = true }
};

// 生成代码
await generator.GenerateAsync("Notice", properties);

Console.WriteLine("代码生成完成！");
Console.WriteLine($"输出目录：{outputPath}");