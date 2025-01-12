using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Authorization;

/// <summary>
/// 用户角色关联实体类
/// </summary>
/// <remarks>
/// 用户与角色的多对多关联关系实体
/// 
/// 数据库映射说明：
/// 1. 表名：lean_user_role
/// 2. 主键：id (自增长)
/// 3. 联合唯一索引：user_id, role_id (用户ID, 角色ID)
/// 4. 索引：
///    - IX_UserId (用户ID)
///    - IX_RoleId (角色ID)
/// 
/// 业务规则：
/// 1. 一个用户可以拥有多个角色
/// 2. 一个角色可以分配给多个用户
/// 3. 同一用户不能重复分配同一角色
/// 4. 用户被删除时，相关的用户角色关联记录也会被删除
/// 5. 角色被删除时，相关的用户角色关联记录也会被删除
/// </remarks>
[SugarTable("lean_user_role", "用户角色关联表")]
public class LeanUserRole : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
  [Description("用户ID")]
  public long UserId { get; set; }

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
  /// 导航属性：用户
  /// </summary>
  /// <remarks>
  /// 1. 一对多关联到用户表
  /// 2. 从表（用户角色关联表）通过UserId关联主表（用户表）的Id
  /// 3. 延迟加载
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public Admin.LeanUser? User { get; set; }

  /// <summary>
  /// 导航属性：角色
  /// </summary>
  /// <remarks>
  /// 1. 一对多关联到角色表
  /// 2. 从表（用户角色关联表）通过RoleId关联主表（角色表）的Id
  /// 3. 延迟加载
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(RoleId))]
  public LeanRole? Role { get; set; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanUserRole()
  {
    UserId = 0;
    RoleId = 0;
    IsDeleted = 0;
  }
}