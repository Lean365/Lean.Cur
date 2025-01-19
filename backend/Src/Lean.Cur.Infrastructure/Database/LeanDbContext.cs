using SqlSugar;

namespace Lean.Cur.Infrastructure.Database;

/// <summary>
/// 数据库上下文
/// </summary>
public class LeanDbContext
{
  private readonly SqlSugarClient _db;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">数据库客户端</param>
  public LeanDbContext(SqlSugarClient db)
  {
    _db = db;
  }

  /// <summary>
  /// 获取数据库客户端
  /// </summary>
  public SqlSugarClient Db => _db;
}