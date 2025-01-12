using OfficeOpenXml;

namespace Lean.Cur.Common.Excel;

/// <summary>
/// Excel配置
/// </summary>
public class LeanExcelOptions
{
  /// <summary>
  /// 许可证类型
  /// </summary>
  public OfficeOpenXml.LicenseContext LicenseContext { get; set; } = OfficeOpenXml.LicenseContext.NonCommercial;

  /// <summary>
  /// 导出配置
  /// </summary>
  public LeanExcelExportSettings Export { get; set; } = new();

  /// <summary>
  /// 导入配置
  /// </summary>
  public LeanExcelImportSettings Import { get; set; } = new();

  /// <summary>
  /// Office属性配置
  /// </summary>
  public LeanExcelOfficeProperties Office { get; set; } = new();
}

/// <summary>
/// Excel导出配置
/// </summary>
public class LeanExcelExportSettings
{
  /// <summary>
  /// 每个Sheet最大行数
  /// </summary>
  public int MaxRowsPerSheet { get; set; } = 100000;

  /// <summary>
  /// 默认Sheet名称前缀
  /// </summary>
  public string DefaultSheetNamePrefix { get; set; } = "Sheet";

  /// <summary>
  /// 默认日期格式
  /// </summary>
  public string DefaultDateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

  /// <summary>
  /// 数字格式配置
  /// </summary>
  public LeanExcelNumberFormat NumberFormat { get; set; } = new();

  /// <summary>
  /// 默认表头样式
  /// </summary>
  public LeanExcelHeaderStyle DefaultHeaderStyle { get; set; } = new();
}

/// <summary>
/// Excel数字格式配置
/// </summary>
public class LeanExcelNumberFormat
{
  /// <summary>
  /// 整数格式
  /// </summary>
  public string Integer { get; set; } = "#,##0";

  /// <summary>
  /// 小数格式
  /// </summary>
  public string Decimal { get; set; } = "#,##0.00";

  /// <summary>
  /// 百分比格式
  /// </summary>
  public string Percentage { get; set; } = "0.00%";

  /// <summary>
  /// 货币格式
  /// </summary>
  public string Currency { get; set; } = "¥#,##0.00";

  /// <summary>
  /// 科学计数格式
  /// </summary>
  public string Scientific { get; set; } = "0.00E+00";
}

/// <summary>
/// Excel导入配置
/// </summary>
public class LeanExcelImportSettings
{
  /// <summary>
  /// 最大允许的文件大小(字节)
  /// </summary>
  public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 默认10MB

  /// <summary>
  /// 允许的文件扩展名
  /// </summary>
  public string[] AllowedExtensions { get; set; } = new[] { ".xlsx", ".xls" };

  /// <summary>
  /// 是否跳过空行
  /// </summary>
  public bool SkipEmptyRows { get; set; } = true;

  /// <summary>
  /// 是否跳过标题行
  /// </summary>
  public bool SkipHeaderRow { get; set; } = true;

  /// <summary>
  /// 默认日期格式
  /// </summary>
  public string[] DateFormats { get; set; } = new[]
  {
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd",
        "yyyy/MM/dd HH:mm:ss",
        "yyyy/MM/dd"
    };
}

/// <summary>
/// Office属性配置
/// </summary>
public class LeanExcelOfficeProperties
{
  /// <summary>
  /// 创建者
  /// </summary>
  public string? Creator { get; set; }

  /// <summary>
  /// 最后修改者
  /// </summary>
  public string? LastModifiedBy { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime Created { get; set; } = DateTime.Now;

  /// <summary>
  /// 修改时间
  /// </summary>
  public DateTime Modified { get; set; } = DateTime.Now;

  /// <summary>
  /// 公司
  /// </summary>
  public string? Company { get; set; }

  /// <summary>
  /// 管理者
  /// </summary>
  public string? Manager { get; set; }

  /// <summary>
  /// 标题
  /// </summary>
  public string? Title { get; set; }

  /// <summary>
  /// 主题
  /// </summary>
  public string? Subject { get; set; }

  /// <summary>
  /// 分类
  /// </summary>
  public string? Category { get; set; }

  /// <summary>
  /// 关键字
  /// </summary>
  public string? Keywords { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Comments { get; set; }

  /// <summary>
  /// 应用程序名称
  /// </summary>
  public string Application { get; set; } = "Lean.Cur";

  /// <summary>
  /// 应用程序版本
  /// </summary>
  public string AppVersion { get; set; } = "1.0.0";

  /// <summary>
  /// 应用Office属性到工作簿
  /// </summary>
  public void ApplyTo(ExcelWorkbook workbook)
  {
    var props = workbook.Properties;
    props.Author = Creator;
    props.LastModifiedBy = LastModifiedBy;
    props.Created = Created;
    props.Modified = Modified;
    props.Company = Company;
    props.Manager = Manager;
    props.Title = Title;
    props.Subject = Subject;
    props.Category = Category;
    props.Keywords = Keywords;
    props.Comments = Comments;
    props.Application = Application;
    props.AppVersion = AppVersion;
  }
}