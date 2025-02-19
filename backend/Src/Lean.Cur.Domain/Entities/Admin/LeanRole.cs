using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 角色实体类
/// </summary>
/// <remarks>
/// 角色管理模块的核心实体，包含角色的基本信息和权限关联
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
/// 3. 所有字段都必填（备注除外）
/// 4. 状态默认为启用
/// 5. 排序值越小越靠前
/// 6. 角色与权限的关系通过role_permission关系表维护
/// 7. 角色与用户的关系通过user_role关系表维护
/// </remarks>
/// <author>CodeGenerator</author>
/// <date>2024-01-17</date>
/// <version>1.0.0</version>
/// <copyright>© 2024 Lean. All rights reserved</copyright>
[SugarTable("lean_role", "角色表")]
public class LeanRole : LeanBaseEntity
{
  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：2-50个字符
  /// 2. 允许中文、字母、数字
  /// 3. 同一租户下唯一
  /// 4. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "role_name", ColumnDescription = "角色名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("角色名称")]
  public string RoleName { get; set; } = string.Empty;

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
  /// 角色类型
  /// </summary>
  [SugarColumn(ColumnName = "role_type", ColumnDescription = "角色类型", ColumnDataType = "int", IsNullable = false)]
  [Description("角色类型")]
  public LeanRoleType RoleType { get; set; }

  /// <summary>
  /// 数据范围
  /// </summary>
  [SugarColumn(ColumnName = "data_scope", ColumnDescription = "数据范围", ColumnDataType = "int", IsNullable = false)]
  [Description("数据范围")]
  public LeanDataScope DataScope { get; set; } = LeanDataScope.Self;

  /// <summary>
  /// 部门ID列表（仅在自定义数据范围时使用）
  /// </summary>
  [SugarColumn(ColumnName = "dept_ids", ColumnDescription = "部门ID列表", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  [Description("部门ID列表")]
  public string? DeptIds { get; set; }

  /// <summary>
  /// 继承的角色ID列表
  /// </summary>
  /// <remarks>
  /// 1. 记录当前角色继承的其他角色ID
  /// 2. 多个ID用逗号分隔
  /// 3. 最大长度：500个字符
  /// </remarks>
  [SugarColumn(ColumnName = "inherited_role_ids", ColumnDescription = "继承的角色ID列表", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  [Description("继承的角色ID列表")]
  public string? InheritedRoleIds { get; set; }

  /// <summary>
  /// 继承类型（1：完全继承 2：部分继承）
  /// </summary>
  /// <remarks>
  /// 1. 记录角色继承的类型
  /// 2. 1表示完全继承所有权限
  /// 3. 2表示仅继承指定的权限
  /// </remarks>
  [SugarColumn(ColumnName = "inheritance_type", ColumnDescription = "继承类型", ColumnDataType = "int", IsNullable = false)]
  [Description("继承类型")]
  public int InheritanceType { get; set; }

  /// <summary>
  /// 继承的权限列表（部分继承时使用）
  /// </summary>
  /// <remarks>
  /// 1. 记录部分继承时要继承的具体权限
  /// 2. 多个权限用逗号分隔
  /// 3. 最大长度：1000个字符
  /// </remarks>
  [SugarColumn(ColumnName = "inherited_permissions", ColumnDescription = "继承的权限列表", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
  [Description("继承的权限列表")]
  public string? InheritedPermissions { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  /// <remarks>
  /// 1. 默认值：0
  /// 2. 值越小越靠前
  /// 3. 用于自定义显示顺序
  /// 4. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
  [Description("显示顺序")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 角色状态，使用LeanStatus枚举
  /// 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
  [Description("状态")]
  public LeanStatus Status { get; set; } = LeanStatus.Normal;

  /// <summary>
  /// 备注
  /// </summary>
  /// <remarks>
  /// 1. 长度限制：最大500个字符
  /// 2. 可选字段
  /// </remarks>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
  [Description("备注")]
  public string? Remark { get; set; }

  /// <summary>
  /// 用户角色关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserRole.RoleId))]
  public List<LeanUserRole>? UserRoles { get; set; }

  /// <summary>
  /// 角色菜单关联
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanRoleMenu.RoleId))]
  public List<LeanRoleMenu>? RoleMenus { get; set; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRole()
  {
    RoleName = string.Empty;
    RoleCode = string.Empty;
    OrderNum = 0;
    Status = LeanStatus.Normal;
    IsDeleted = 0;
  }
}