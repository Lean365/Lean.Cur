using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Authorization;

/// <summary>
/// 角色权限关联实体类
/// </summary>
/// <remarks>
/// 角色与权限的多对多关联关系实体
/// 
/// 数据库映射说明：
/// 1. 表名：lean_role_permission
/// 2. 主键：id (自增长)
/// 3. 联合唯一索引：role_id, permission_id (角色ID, 权限ID)
/// 4. 索引：
///    - IX_RoleId (角色ID)
///    - IX_PermissionId (权限ID)
/// 
/// 业务规则：
/// 1. 一个角色可以拥有多个权限
/// 2. 一个权限可以分配给多个角色
/// 3. 同一角色不能重复分配同一权限
/// 4. 角色被删除时，相关的角色权限关联记录也会被删除
/// 5. 权限被删除时，相关的角色权限关联记录也会被删除
/// </remarks>
[SugarTable("lean_role_permission", "角色权限关联表")]
public class LeanRolePermission : LeanBaseEntity
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 1. 关联角色表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("角色ID")]
  public long RoleId { get; set; }

  /// <summary>
  /// 权限ID
  /// </summary>
  /// <remarks>
  /// 1. 关联权限表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "permission_id", ColumnDescription = "权限ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("权限ID")]
  public long PermissionId { get; set; }

  /// <summary>
  /// 导航属性：角色
  /// </summary>
  /// <remarks>
  /// 1. 一对多关联到角色表
  /// 2. 从表（角色权限关联表）通过RoleId关联主表（角色表）的Id
  /// 3. 延迟加载
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(RoleId))]
  public LeanRole? Role { get; set; }

  /// <summary>
  /// 导航属性：权限
  /// </summary>
  /// <remarks>
  /// 1. 一对多关联到权限表
  /// 2. 从表（角色权限关联表）通过PermissionId关联主表（权限表）的Id
  /// 3. 延迟加载
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(PermissionId))]
  public LeanPermission? Permission { get; set; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanRolePermission()
  {
    RoleId = 0;
    PermissionId = 0;
    IsDeleted = 0;
  }
}