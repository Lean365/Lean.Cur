using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Repositories;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

public class LeanNoticeRepository : LeanBaseRepository<LeanNotice>, ILeanNoticeRepository
{
    public LeanNoticeRepository(ISqlSugarClient db) : base(db)
    {
    }

    // 在此实现特定的仓储方法
} 