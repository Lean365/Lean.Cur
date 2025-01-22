using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 邮件模板实体
/// </summary>
[SugarTable("lean_email_template", "邮件模板表")]
public class LeanEmailTemplate : LeanBaseEntity
{
  /// <summary>
  /// 模板代码
  /// </summary>
  [SugarColumn(ColumnName = "code", ColumnDescription = "模板代码", Length = 50, IsNullable = false)]
  [Description("模板代码")]
  public string Code { get; set; } = null!;

  /// <summary>
  /// 模板名称
  /// </summary>
  [SugarColumn(ColumnName = "name", ColumnDescription = "模板名称", Length = 100, IsNullable = false)]
  [Description("模板名称")]
  public string Name { get; set; } = null!;

  /// <summary>
  /// 模板主题
  /// </summary>
  [SugarColumn(ColumnName = "subject", ColumnDescription = "模板主题", Length = 200, IsNullable = false)]
  [Description("模板主题")]
  public string Subject { get; set; } = null!;

  /// <summary>
  /// 模板内容
  /// </summary>
  [SugarColumn(ColumnName = "content", ColumnDescription = "模板内容", ColumnDataType = "nvarchar(max)", IsNullable = false)]
  [Description("模板内容")]
  public string Content { get; set; } = null!;

  /// <summary>
  /// 参数说明
  /// </summary>
  [SugarColumn(ColumnName = "params_desc", ColumnDescription = "参数说明", Length = 500, IsNullable = true)]
  [Description("参数说明")]
  public string? ParamsDesc { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  [SugarColumn(ColumnName = "is_enabled", ColumnDescription = "是否启用", IsNullable = false)]
  [Description("是否启用")]
  public bool IsEnabled { get; set; } = true;
}