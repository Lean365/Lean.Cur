using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Application.Services;

public interface ILeanDepartmentService
{
  Task<List<LeanDept>> GetDeptListAsync(string? keyword);
  Task<LeanDept?> GetDeptAsync(long id);
  Task<LeanDept> CreateDeptAsync(LeanDept dept);
  Task<LeanDept> UpdateDeptAsync(LeanDept dept);
  Task DeleteDeptAsync(long id);
  Task<bool> IsDeptCodeExistsAsync(string deptCode, long? excludeId = null);
  Task<List<LeanUser>> GetDeptUsersAsync(long deptId);
  Task AssignUsersAsync(long deptId, IEnumerable<long> userIds);
}