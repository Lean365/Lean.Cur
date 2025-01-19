using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 用户岗位关联实体
/// </summary>
/// <remarks>
/// 用于维护用户和岗位之间的多对多关系
/// </remarks>
[SugarTable("lean_user_post")]
public class LeanUserPost : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
  public long UserId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", ColumnDataType = "bigint", IsNullable = false)]
  public long PostId { get; set; }

  /// <summary>
  /// 用户信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public LeanUser? User { get; set; }

  /// <summary>
  /// 岗位信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(PostId))]
  public LeanPost? Post { get; set; }
}