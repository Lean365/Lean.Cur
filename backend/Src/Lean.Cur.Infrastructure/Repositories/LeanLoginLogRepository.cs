using Lean.Cur.Domain.Entities.Log;
using Lean.Cur.Domain.Repositories;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Repositories;

/// <summary>
/// 登录日志仓储实现
/// </summary>
public class LeanLoginLogRepository : LeanBaseRepository<LeanLoginLog>, ILeanLoginLogRepository
{
  private readonly ILogger<LeanLoginLogRepository> _logger;

  public LeanLoginLogRepository(ISqlSugarClient db, ILogger<LeanLoginLogRepository> logger) : base(db)
  {
    _logger = logger;
  }

  public async Task<LeanLoginLog?> GetLastLoginAsync(string userName)
  {
    try
    {
      _logger.LogInformation("正在获取用户 {UserName} 的最后一次登录记录", userName);
      var log = await EntityDb.AsQueryable()
          .Where(x => x.UserName == userName && x.Status == 0)
          .OrderByDescending(x => x.LoginTime)
          .FirstAsync();
      return log;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取用户 {UserName} 的最后一次登录记录失败", userName);
      throw;
    }
  }

  public async Task<(int Total, int Success, int Failed)> GetLoginStatsAsync(string userName)
  {
    try
    {
      _logger.LogInformation("正在获取用户 {UserName} 的登录统计", userName);
      var stats = await EntityDb.AsQueryable()
          .Where(x => x.UserName == userName)
          .GroupBy(x => x.Status)
          .Select(x => new { Status = x.Key, Count = SqlFunc.AggregateCount(x.Id) })
          .ToListAsync();

      var total = stats.Sum(x => x.Count);
      var success = stats.FirstOrDefault(x => x.Status == 0)?.Count ?? 0;
      var failed = stats.FirstOrDefault(x => x.Status == 1)?.Count ?? 0;

      return (total, success, failed);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取用户 {UserName} 的登录统计失败", userName);
      throw;
    }
  }

  public async Task<int> CleanupAsync(int days)
  {
    try
    {
      _logger.LogInformation("正在清理 {Days} 天前的登录日志", days);
      var cutoff = DateTime.Now.AddDays(-days);
      var count = await EntityDb.AsDeleteable()
          .Where(x => x.LoginTime < cutoff)
          .ExecuteCommandAsync();
      _logger.LogInformation("成功清理 {Count} 条登录日志", count);
      return count;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "清理登录日志失败");
      throw;
    }
  }
}