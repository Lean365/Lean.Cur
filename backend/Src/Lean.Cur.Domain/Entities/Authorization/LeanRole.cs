using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Authorization;

/// <summary>
/// 角色实体类
/// </summary>
/// <remarks>
/// 角色管理模块的核心实体，包含角色的基本信息和权限分配
/// 
/// 数据库映射说明：
/// 1. 表名：lean_role
/// 2. 主键：id (自增长)
/// 3. 唯一索引：role_code (角色编码)
/// 4. 索引：role_name (角色名称)
/// 
/// 业务规则：
/// 1. 角色编码全局唯一，不区分大小写
/// 2. 角色名称在同一租户下唯一
/// 3. 角色状态默认为启用
/// 4. 角色类型默认为自定义
/// 5. 内置角色不允许删除和修改
/// 6. 角色与权限的关系通过role_permission关系表维护
/// </remarks>
[SugarTable("lean_role", "角色表")]
public class LeanRole : LeanBaseEntity
{
  /// <summary>
  /// 角色编码
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 只能包含字母、数字、下划线
  /// 3. 全局唯一，不区分大小写
  /// 4. 创建后不允许修改
  /// 5. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "role_code", ColumnDescription = "角色编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("角色编码")]
  public string RoleCode { get; set; } = string.Empty;

  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 允许中文、字母、数字、下划线
  /// 3. 同一租户下唯一
  /// 4. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "role_name", ColumnDescription = "角色名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("角色名称")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色类型（0-内置，1-自定义）
  /// </summary>
  /// <remarks>
  /// 1. 内置角色不允许删除和修改
  /// 2. 必填字段
  /// 3. 默认为自定义(1)
  /// </remarks>
  [SugarColumn(ColumnName = "role_type", ColumnDescription = "角色类型", ColumnDataType = "int", IsNullable = false)]
  [Description("角色类型")]
  public int RoleType { get; set; }

  /// <summary>
  /// 数据范围（1-全部数据，2-本部门及以下数据，3-本部门数据，4-仅本人数据，5-自定义数据）
  /// </summary>
  /// <remarks>
  /// 1. 用于数据权限控制
  /// 2. 必填字段
  /// 3. 默认为仅本人数据(4)
  /// </remarks>
  [SugarColumn(ColumnName = "data_scope", ColumnDescription = "数据范围", ColumnDataType = "int", IsNullable = false)]
  [Description("数据范围")]
  public int DataScope { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  /// <remarks>
  /// 1. 用于角色列表的排序
  /// 2. 必填字段
  /// 3. 默认值：0
  /// </remarks>
  [SugarColumn(ColumnName = "role_sort", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
  [Description("显示顺序")]
  public int RoleSort { get; set; }

  /// <summary>
  /// 状态（0-禁用，1-正常）
  /// </summary>
  /// <remarks>
  /// 1. 禁用后该角色下的用户将无法登录
  /// 2. 必填字段
  /// 3. 默认为正常(1)
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  [Description("状态")]
  public int Status { get; set; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRole()
  {
    RoleCode = string.Empty;
    RoleName = string.Empty;
    RoleType = 1; // 默认为自定义角色
    DataScope = 4; // 默认为仅本人数据
    RoleSort = 0;
    Status = 1; // 默认为正常状态
    IsDeleted = 0;
  }
}