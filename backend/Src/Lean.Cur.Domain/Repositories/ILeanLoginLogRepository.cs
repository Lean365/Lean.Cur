using Lean.Cur.Domain.Entities.Log;

namespace Lean.Cur.Domain.Repositories;

/// <summary>
/// 登录日志仓储接口
/// </summary>
public interface ILeanLoginLogRepository : ILeanBaseRepository<LeanLoginLog>
{
  /// <summary>
  /// 获取用户最后一次登录记录
  /// </summary>
  /// <param name="userName">用户名</param>
  /// <returns>登录日志</returns>
  Task<LeanLoginLog?> GetLastLoginAsync(string userName);

  /// <summary>
  /// 获取用户登录统计
  /// </summary>
  /// <param name="userName">用户名</param>
  /// <returns>登录次数</returns>
  Task<(int Total, int Success, int Failed)> GetLoginStatsAsync(string userName);

  /// <summary>
  /// 清理指定天数之前的登录日志
  /// </summary>
  /// <param name="days">天数</param>
  /// <returns>清理的记录数</returns>
  Task<int> CleanupAsync(int days);
}