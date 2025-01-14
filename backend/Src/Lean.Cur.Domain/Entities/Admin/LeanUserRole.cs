using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 用户角色关联实体
/// </summary>
/// <remarks>
/// 用于维护用户和角色之间的多对多关系
/// </remarks>
[SugarTable("lean_user_role")]
public class LeanUserRole : LeanBaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID")]
    public long UserId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID")]
    public long RoleId { get; set; }

    /// <summary>
    /// 用户信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public LeanUser? User { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public LeanRole? Role { get; set; }
} 