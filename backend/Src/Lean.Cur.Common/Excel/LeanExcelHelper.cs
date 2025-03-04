using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Common.Excel
{
  /// <summary>
  /// Excel帮助类
  /// </summary>
  public class LeanExcelHelper
  {
    private readonly LeanExcelOptions _options;

    public LeanExcelHelper(LeanExcelOptions options)
    {
      _options = options;
      ExcelPackage.LicenseContext = options.LicenseContext;
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    public byte[] Export<T>(List<T> data, LeanExcelExportSettings? exportSettings = null, LeanExcelOfficeProperties? officeProperties = null) where T : class
    {
      using var package = new ExcelPackage();
      var settings = exportSettings ?? _options.Export;
      var office = officeProperties ?? _options.Office;

      // 设置Office属性
      office.ApplyTo(package.Workbook);

      // 获取类型的所有属性
      var properties = typeof(T).GetProperties()
          .Select(p => new
          {
            Property = p,
            Column = p.GetCustomAttributes(typeof(LeanExcelColumnAttribute), false)
                  .FirstOrDefault() as LeanExcelColumnAttribute
          })
          .Where(x => x.Column != null && x.Column.AllowExport)
          .OrderBy(x => x.Column!.Order)
          .ToList();

      // 计算需要的Sheet数量
      var totalRows = data.Count;
      var maxRowsPerSheet = settings.MaxRowsPerSheet;
      var sheetCount = (int)Math.Ceiling((double)totalRows / maxRowsPerSheet);

      // 创建所有工作表并写入数据
      for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
      {
        var sheetName = sheetCount == 1
            ? settings.DefaultSheetNamePrefix
            : $"{settings.DefaultSheetNamePrefix}{sheetIndex + 1}";
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        // 写入表头
        for (int i = 0; i < properties.Count; i++)
        {
          var column = properties[i].Column!;
          var cell = worksheet.Cells[1, i + 1];
          cell.Value = column.Name;

          // 设置列宽
          worksheet.Column(i + 1).Width = column.Width;

          // 应用表头样式
          var style = column.HeaderStyle ?? settings.DefaultHeaderStyle;
          var headerCell = cell.Style;
          headerCell.Font.Size = style.FontSize;
          headerCell.Font.Bold = style.IsBold;
          headerCell.Fill.PatternType = ExcelFillStyle.Solid;
          headerCell.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(style.BackgroundColor));
          headerCell.Font.Color.SetColor(ColorTranslator.FromHtml(style.FontColor));
          headerCell.VerticalAlignment = ExcelVerticalAlignment.Center;
          headerCell.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // 计算当前Sheet的数据范围
        var startIndex = sheetIndex * maxRowsPerSheet;
        var endIndex = Math.Min(startIndex + maxRowsPerSheet, totalRows);
        var currentSheetData = data.Skip(startIndex).Take(endIndex - startIndex);

        // 写入数据
        var rowIndex = 2;
        foreach (var item in currentSheetData)
        {
          for (int j = 0; j < properties.Count; j++)
          {
            var prop = properties[j].Property;
            var column = properties[j].Column!;
            var value = prop.GetValue(item);
            var cell = worksheet.Cells[rowIndex, j + 1];

            if (value != null)
            {
              cell.Value = value;
              if (value is DateTime)
              {
                cell.Style.Numberformat.Format = settings.DefaultDateFormat;
              }
              else if (value is decimal or double or float)
              {
                cell.Style.Numberformat.Format = settings.NumberFormat.Decimal;
              }
              else if (value is int or long or short)
              {
                cell.Style.Numberformat.Format = settings.NumberFormat.Integer;
              }
            }
          }
          rowIndex++;
        }
      }

      return package.GetAsByteArray();
    }

    /// <summary>
    /// 导入Excel
    /// </summary>
    public List<T> Import<T>(Stream stream, string fileName, LeanExcelImportSettings? importSettings = null) where T : class, new()
    {
      var settings = importSettings ?? _options.Import;

      // 验证文件扩展名
      var extension = Path.GetExtension(fileName).ToLowerInvariant();
      if (!settings.AllowedExtensions.Contains(extension))
      {
        throw new InvalidOperationException($"不支持的文件类型：{extension}，仅支持：{string.Join(", ", settings.AllowedExtensions)}");
      }

      // 验证文件大小
      if (stream.Length > settings.MaxFileSize)
      {
        throw new InvalidOperationException($"文件大小超过限制：{stream.Length}字节，最大允许：{settings.MaxFileSize}字节");
      }

      using var package = new ExcelPackage(stream);
      var worksheet = package.Workbook.Worksheets[0];
      if (worksheet == null)
      {
        throw new InvalidOperationException("Excel文件中没有工作表");
      }

      // 获取类型的所有属性
      var properties = typeof(T).GetProperties()
          .Select(p => new
          {
            Property = p,
            Column = p.GetCustomAttributes(typeof(LeanExcelColumnAttribute), false)
                  .FirstOrDefault() as LeanExcelColumnAttribute
          })
          .Where(x => x.Column != null && x.Column.AllowImport)
          .OrderBy(x => x.Column!.Order)
          .ToList();

      var data = new List<T>();
      var startRow = settings.SkipHeaderRow ? 2 : 1;
      var endRow = worksheet.Dimension?.End.Row ?? 0;

      for (int row = startRow; row <= endRow; row++)
      {
        var item = new T();
        var hasValue = false;
        var hasError = false;

        for (int col = 1; col <= properties.Count; col++)
        {
          var prop = properties[col - 1].Property;
          var column = properties[col - 1].Column!;
          var cell = worksheet.Cells[row, col];
          var value = cell.Value;

          if (value != null)
          {
            hasValue = true;
            try
            {
              var convertedValue = ConvertValue(value, prop.PropertyType, settings.DateFormats);
              prop.SetValue(item, convertedValue);
            }
            catch (Exception ex)
            {
              hasError = true;
              throw new InvalidOperationException($"第{row}行，列 {column.Name}: {ex.Message}");
            }
          }
          else if (column.Required)
          {
            hasError = true;
            throw new InvalidOperationException($"第{row}行，列 {column.Name} 不能为空");
          }
        }

        if ((hasValue || !settings.SkipEmptyRows) && !hasError)
        {
          data.Add(item);
        }
      }

      return data;
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    public byte[] GetTemplate<T>(LeanExcelExportSettings? exportSettings = null, LeanExcelOfficeProperties? officeProperties = null) where T : class
    {
      return Export<T>(new List<T>(), exportSettings, officeProperties);
    }

    private static object? ConvertValue(object value, Type targetType, string[] dateFormats)
    {
      if (value == null) return null;

      var stringValue = value.ToString();
      if (string.IsNullOrWhiteSpace(stringValue)) return null;

      try
      {
        if (targetType == typeof(string))
        {
          return stringValue;
        }
        else if (targetType == typeof(int) || targetType == typeof(int?))
        {
          return Convert.ToInt32(value);
        }
        else if (targetType == typeof(long) || targetType == typeof(long?))
        {
          return Convert.ToInt64(value);
        }
        else if (targetType == typeof(decimal) || targetType == typeof(decimal?))
        {
          return Convert.ToDecimal(value);
        }
        else if (targetType == typeof(double) || targetType == typeof(double?))
        {
          return Convert.ToDouble(value);
        }
        else if (targetType == typeof(float) || targetType == typeof(float?))
        {
          return Convert.ToSingle(value);
        }
        else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
        {
          if (value is DateTime dt) return dt;
          foreach (var format in dateFormats)
          {
            if (DateTime.TryParseExact(stringValue, format, null, System.Globalization.DateTimeStyles.None, out dt))
            {
              return dt;
            }
          }
          throw new Exception($"无效的日期格式，支持的格式：{string.Join(", ", dateFormats)}");
        }
        else if (targetType == typeof(bool) || targetType == typeof(bool?))
        {
          if (stringValue == "1" || stringValue.Equals("true", StringComparison.OrdinalIgnoreCase))
            return true;
          if (stringValue == "0" || stringValue.Equals("false", StringComparison.OrdinalIgnoreCase))
            return false;
          throw new Exception("无效的布尔值，支持的值：1/0, true/false");
        }
        else if (targetType.IsEnum)
        {
          return Enum.Parse(targetType, stringValue, true);
        }

        return Convert.ChangeType(value, targetType);
      }
      catch (Exception ex)
      {
        throw new Exception($"值 '{stringValue}' 无法转换为类型 {targetType.Name}: {ex.Message}");
      }
    }

    /// <summary>
    /// 导出 Excel
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">数据列表</param>
    /// <param name="headers">表头(可选)</param>
    /// <returns>Excel 文件字节数组</returns>
    public static byte[] Export<T>(IEnumerable<T> data, Dictionary<string, string>? headers = null)
    {
      using var package = new ExcelPackage();
      var worksheet = package.Workbook.Worksheets.Add("Sheet1");

      // 获取属性信息
      var properties = typeof(T).GetProperties()
          .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false)
          .ToList();

      // 设置表头样式
      var headerStyle = worksheet.Cells[1, 1, 1, properties.Count].Style;
      headerStyle.Font.Bold = true;
      headerStyle.Fill.PatternType = ExcelFillStyle.Solid;
      headerStyle.Fill.BackgroundColor.SetColor(Color.LightGray);
      headerStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;
      headerStyle.VerticalAlignment = ExcelVerticalAlignment.Center;

      // 设置表头
      for (int i = 0; i < properties.Count; i++)
      {
        var property = properties[i];
        var header = headers?.GetValueOrDefault(property.Name)
            ?? property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
            ?? property.Name;
        worksheet.Cells[1, i + 1].Value = header;
      }

      // 填充数据
      var row = 2;
      foreach (var item in data)
      {
        for (int i = 0; i < properties.Count; i++)
        {
          var value = properties[i].GetValue(item);
          var cell = worksheet.Cells[row, i + 1];

          if (value != null)
          {
            if (value is DateTime dateValue)
            {
              cell.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
              cell.Value = dateValue;
            }
            else if (value is decimal or double or float)
            {
              cell.Style.Numberformat.Format = "#,##0.00";
              cell.Value = value;
            }
            else if (value is int or long)
            {
              cell.Style.Numberformat.Format = "#,##0";
              cell.Value = value;
            }
            else
            {
              cell.Value = value.ToString();
            }
          }
        }
        row++;
      }

      // 自动调整列宽
      worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

      // 返回字节数组
      return package.GetAsByteArray();
    }

    /// <summary>
    /// 生成导入模板
    /// </summary>
    public async Task<byte[]> GenerateTemplateAsync<T>(Dictionary<string, string> headers) where T : class
    {
      using var package = new ExcelPackage();
      var worksheet = package.Workbook.Worksheets.Add("Template");

      // 写入表头
      var col = 1;
      foreach (var header in headers)
      {
        var cell = worksheet.Cells[1, col];
        cell.Value = header.Value;
        cell.Style.Font.Bold = true;
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Column(col).Width = 20;
        col++;
      }

      return await package.GetAsByteArrayAsync();
    }

    /// <summary>
    /// 生成导入模板
    /// </summary>
    public async Task<byte[]> GenerateTemplateAsync(Dictionary<string, string> headers)
    {
      return await GenerateTemplateAsync<object>(headers);
    }

    /// <summary>
    /// 导入Excel文件
    /// </summary>
    public List<T> Import<T>(IFormFile file) where T : class, new()
    {
      using var stream = file.OpenReadStream();
      using var package = new ExcelPackage(stream);
      var worksheet = package.Workbook.Worksheets[0];
      var rowCount = worksheet.Dimension?.End.Row ?? 0;
      var colCount = worksheet.Dimension?.End.Column ?? 0;

      var data = new List<T>();
      var properties = typeof(T).GetProperties();
      var propertyNames = properties.Select(p => p.Name).ToList();

      // 获取表头映射
      var headerRow = 1;
      var headerMap = new Dictionary<string, int>();
      for (int col = 1; col <= colCount; col++)
      {
        var headerCell = worksheet.Cells[headerRow, col].Value?.ToString();
        if (!string.IsNullOrEmpty(headerCell))
        {
          headerMap[headerCell] = col;
        }
      }

      // 读取数据
      for (int row = headerRow + 1; row <= rowCount; row++)
      {
        var item = new T();
        var hasValue = false;

        foreach (var property in properties)
        {
          var columnName = property.Name;
          if (headerMap.TryGetValue(columnName, out int col))
          {
            var cell = worksheet.Cells[row, col].Value;
            if (cell != null)
            {
              hasValue = true;
              var value = ConvertValue(cell, property.PropertyType, _options.Import.DateFormats);
              property.SetValue(item, value);
            }
          }
        }

        if (hasValue)
        {
          data.Add(item);
        }
      }

      return data;
    }

    /// <summary>
    /// 导出Excel文件
    /// </summary>
    public async Task<byte[]> ExportAsync<T>(Dictionary<string, string> headers, List<T> data) where T : class
    {
      using var package = new ExcelPackage();
      var worksheet = package.Workbook.Worksheets.Add("Sheet1");

      // 写入表头
      var col = 1;
      foreach (var header in headers)
      {
        var cell = worksheet.Cells[1, col];
        cell.Value = header.Value;
        cell.Style.Font.Bold = true;
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        worksheet.Column(col).Width = 20;
        col++;
      }

      // 写入数据
      if (data.Any())
      {
        var properties = typeof(T).GetProperties();
        var row = 2;
        foreach (var item in data)
        {
          col = 1;
          foreach (var header in headers)
          {
            var property = properties.FirstOrDefault(p => p.Name == header.Key);
            if (property != null)
            {
              var value = property.GetValue(item);
              worksheet.Cells[row, col].Value = value;

              // 设置日期格式
              if (value is DateTime)
              {
                worksheet.Cells[row, col].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
              }
            }
            col++;
          }
          row++;
        }
      }

      return await package.GetAsByteArrayAsync();
    }
  }
}

