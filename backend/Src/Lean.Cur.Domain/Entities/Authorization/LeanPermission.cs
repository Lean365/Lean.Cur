using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Authorization;

/// <summary>
/// 权限实体类
/// </summary>
/// <remarks>
/// 权限管理模块的核心实体，包含权限的基本信息和分配规则
/// 
/// 数据库映射说明：
/// 1. 表名：lean_permission
/// 2. 主键：id (自增长)
/// 3. 唯一索引：permission_code (权限编码)
/// 4. 索引：permission_name (权限名称), menu_id (菜单ID)
/// 
/// 业务规则：
/// 1. 权限编码全局唯一，不区分大小写
/// 2. 权限名称在同一菜单下唯一
/// 3. 权限状态默认为启用
/// 4. 权限类型分为菜单权限和按钮权限
/// 5. 内置权限不允许删除和修改
/// 6. 权限与角色的关系通过role_permission关系表维护
/// </remarks>
[SugarTable("lean_permission", "权限表")]
public class LeanPermission : LeanBaseEntity
{
  /// <summary>
  /// 权限编码
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-100个字符
  /// 2. 只能包含字母、数字、下划线、冒号
  /// 3. 全局唯一，不区分大小写
  /// 4. 创建后不允许修改
  /// 5. 必填字段
  /// 6. 格式：模块:操作，如：user:list
  /// </remarks>
  [SugarColumn(ColumnName = "permission_code", ColumnDescription = "权限编码", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  [Description("权限编码")]
  public string PermissionCode { get; set; } = string.Empty;

  /// <summary>
  /// 权限名称
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 允许中文、字母、数字、下划线
  /// 3. 同一菜单下唯一
  /// 4. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "permission_name", ColumnDescription = "权限名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("权限名称")]
  public string PermissionName { get; set; } = string.Empty;

  /// <summary>
  /// 权限类型（1-菜单权限，2-按钮权限）
  /// </summary>
  /// <remarks>
  /// 1. 菜单权限控制整个页面的访问
  /// 2. 按钮权限控制具体操作的执行
  /// 3. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "permission_type", ColumnDescription = "权限类型", ColumnDataType = "int", IsNullable = false)]
  [Description("权限类型")]
  public int PermissionType { get; set; }

  /// <summary>
  /// 菜单ID
  /// </summary>
  /// <remarks>
  /// 1. 关联菜单表的主键
  /// 2. 必填字段
  /// 3. 用于构建权限树
  /// </remarks>
  [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("菜单ID")]
  public long MenuId { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  /// <remarks>
  /// 1. 用于权限列表的排序
  /// 2. 必填字段
  /// 3. 默认值：0
  /// </remarks>
  [SugarColumn(ColumnName = "permission_sort", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
  [Description("显示顺序")]
  public int PermissionSort { get; set; }

  /// <summary>
  /// 状态（0-禁用，1-正常）
  /// </summary>
  /// <remarks>
  /// 1. 禁用后该权限将无法使用
  /// 2. 必填字段
  /// 3. 默认为正常(1)
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  [Description("状态")]
  public int Status { get; set; }

  /// <summary>
  /// 是否内置（0-否，1-是）
  /// </summary>
  /// <remarks>
  /// 1. 内置权限不允许删除和修改
  /// 2. 必填字段
  /// 3. 默认为否(0)
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", ColumnDataType = "int", IsNullable = false)]
  [Description("是否内置")]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanPermission()
  {
    PermissionCode = string.Empty;
    PermissionName = string.Empty;
    PermissionType = 2; // 默认为按钮权限
    MenuId = 0;
    PermissionSort = 0;
    Status = 1; // 默认为正常状态
    IsBuiltin = 0; // 默认为非内置
    IsDeleted = 0;
  }
}