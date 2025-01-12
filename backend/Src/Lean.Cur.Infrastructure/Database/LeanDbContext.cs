using SqlSugar;

namespace Lean.Cur.Infrastructure.Database;

/// <summary>
/// 数据库上下文
/// </summary>
public class LeanDbContext
{
    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">数据库客户端</param>
    public LeanDbContext(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 获取数据库客户端
    /// </summary>
    public ISqlSugarClient Db => _db;
} 