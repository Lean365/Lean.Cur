using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Extensions;

/// <summary>
/// 部门扩展方法
/// </summary>
public static class DeptExtensions
{
  /// <summary>
  /// 获取部门的祖先ID列表
  /// </summary>
  /// <param name="dept">部门实体</param>
  /// <returns>祖先ID列表</returns>
  public static List<long> GetAncestors(this LeanDept dept)
  {
    if (string.IsNullOrEmpty(dept.Ancestors))
      return new List<long>();

    return dept.Ancestors.Split(',')
                       .Select(long.Parse)
                       .ToList();
  }
}