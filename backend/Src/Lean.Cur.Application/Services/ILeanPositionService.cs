using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services;

public interface ILeanPositionService
{
  Task<List<LeanPosition>> GetPositionListAsync(string? keyword);
  Task<LeanPosition?> GetPositionAsync(long id);
  Task<LeanPosition> CreatePositionAsync(LeanPosition position);
  Task<LeanPosition> UpdatePositionAsync(LeanPosition position);
  Task DeletePositionAsync(long id);
  Task<bool> IsPositionCodeExistsAsync(string positionCode, long? excludeId = null);
  Task<List<LeanUser>> GetPositionUsersAsync(long positionId);
  Task AssignUsersAsync(long positionId, IEnumerable<long> userIds);
}