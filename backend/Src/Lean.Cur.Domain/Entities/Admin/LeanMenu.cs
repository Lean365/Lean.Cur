/**
 * @description 菜单实体类
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 菜单实体类
/// </summary>
/// <remarks>
/// 该实体类映射到数据库表 lean_menu，用于存储菜单相关数据
/// 
/// 数据库映射说明：
/// 1. 表名：lean_menu
/// 2. 主键：Id (自增长)
/// 3. 基础字段：继承自LeanBaseEntity，包含创建时间、创建人、更新时间、更新人等
/// </remarks>
[SugarTable("lean_menu", TableDescription = "菜单表")]
public class LeanMenu : LeanBaseEntity
{
  /// <summary>
  /// 菜单名称
  /// </summary>
  [SugarColumn(ColumnName = "menu_name", ColumnDescription = "菜单名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  public string MenuName { get; set; } = null!;

  /// <summary>
  /// 翻译键
  /// </summary>
  [SugarColumn(ColumnName = "trans_key", ColumnDescription = "翻译键", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  public string? TransKey { get; set; }

  /// <summary>
  /// 父菜单ID
  /// </summary>
  [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父菜单ID", ColumnDataType = "bigint", IsNullable = true)]
  public long? ParentId { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [SugarColumn(ColumnName = "ordernum", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  [SugarColumn(ColumnName = "path", ColumnDescription = "路由地址", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  [SugarColumn(ColumnName = "component", ColumnDescription = "组件路径", ColumnDataType = "nvarchar", Length = 255, IsNullable = true)]
  public string? Component { get; set; }

  /// <summary>
  /// 是否为外链(0:否,1:是)
  /// </summary>
  [SugarColumn(ColumnName = "is_frame", ColumnDescription = "是否为外链", ColumnDataType = "int", IsNullable = false)]
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存(0:否,1:是)
  /// </summary>
  [SugarColumn(ColumnName = "is_cache", ColumnDescription = "是否缓存", ColumnDataType = "int", IsNullable = false)]
  public int IsCache { get; set; }

  /// <summary>
  /// 菜单类型(M:目录,C:菜单,F:按钮)
  /// </summary>
  [SugarColumn(ColumnName = "menu_type", ColumnDescription = "菜单类型", ColumnDataType = "char", Length = 1, IsNullable = false)]
  public string MenuType { get; set; } = "M";

  /// <summary>
  /// 显示状态(0:隐藏,1:显示)
  /// </summary>
  [SugarColumn(ColumnName = "visible", ColumnDescription = "显示状态", ColumnDataType = "int", IsNullable = false)]
  public int Visible { get; set; } = 1;

  /// <summary>
  /// 菜单状态(0:禁用,1:正常)
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "菜单状态", ColumnDataType = "int", IsNullable = false)]
  public int Status { get; set; } = 1;

  /// <summary>
  /// 权限标识
  /// </summary>
  [SugarColumn(ColumnName = "perms", ColumnDescription = "权限标识", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  public string? Perms { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  [SugarColumn(ColumnName = "icon", ColumnDescription = "菜单图标", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  public string? Icon { get; set; }

  /// <summary>
  /// 子菜单
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(ParentId))]
  public List<LeanMenu> Children { get; set; } = new();
}