namespace Lean.Cur.Common.Enums;

/// <summary>
/// 数据范围枚举
/// </summary>
public enum LeanDataScope
{
  /// <summary>
  /// 全部数据权限
  /// </summary>
  All = 1,

  /// <summary>
  /// 自定义数据权限
  /// </summary>
  Custom = 2,

  /// <summary>
  /// 本部门数据权限
  /// </summary>
  Dept = 3,

  /// <summary>
  /// 本部门及以下数据权限
  /// </summary>
  DeptAndChild = 4,

  /// <summary>
  /// 仅本人数据权限
  /// </summary>
  Self = 5
}