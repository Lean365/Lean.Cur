using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services;

public interface ILeanUserService
{
  Task<List<LeanUser>> GetUserListAsync(string? keyword);
  Task<LeanUser?> GetUserAsync(long id);
  Task<LeanUser> CreateUserAsync(LeanUser user);
  Task<LeanUser> UpdateUserAsync(LeanUser user);
  Task DeleteUserAsync(long id);
  Task<bool> IsUsernameExistsAsync(string username, long? excludeId = null);
  Task<List<LeanRole>> GetUserRolesAsync(long userId);
  Task AssignRolesAsync(long userId, IEnumerable<long> roleIds);
  Task AssignDepartmentsAsync(long userId, IEnumerable<long> deptIds);
  Task AssignPositionsAsync(long userId, IEnumerable<long> positionIds);
}