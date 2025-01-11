using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;

namespace Lean.Cur.Application.Services.Impl;

public class LeanPositionService : ILeanPositionService
{
  private readonly ILeanRepository<LeanPosition> _positionRepository;
  private readonly ILeanUserRepository _userRepository;

  public LeanPositionService(
      ILeanRepository<LeanPosition> positionRepository,
      ILeanUserRepository userRepository)
  {
    _positionRepository = positionRepository;
    _userRepository = userRepository;
  }

  public async Task<List<LeanPosition>> GetPositionListAsync(string? keyword)
  {
    var positions = await _positionRepository.GetListAsync();
    if (!string.IsNullOrEmpty(keyword))
    {
      positions = positions.Where(p =>
          p.PositionName.Contains(keyword) ||
          p.PositionCode.Contains(keyword)).ToList();
    }
    return positions;
  }

  public async Task<LeanPosition?> GetPositionAsync(long id)
  {
    return await _positionRepository.GetByIdAsync(id);
  }

  public async Task<LeanPosition> CreatePositionAsync(LeanPosition position)
  {
    position.CreateTime = DateTime.Now;
    return await _positionRepository.CreateAsync(position);
  }

  public async Task<LeanPosition> UpdatePositionAsync(LeanPosition position)
  {
    position.UpdateTime = DateTime.Now;
    return await _positionRepository.UpdateAsync(position);
  }

  public async Task DeletePositionAsync(long id)
  {
    await _positionRepository.DeleteAsync(id);
  }

  public async Task<bool> IsPositionCodeExistsAsync(string positionCode, long? excludeId = null)
  {
    var positions = await _positionRepository.GetListAsync();
    return positions.Any(p => p.PositionCode == positionCode && (!excludeId.HasValue || p.Id != excludeId.Value));
  }

  public async Task<List<LeanUser>> GetPositionUsersAsync(long positionId)
  {
    var position = await _positionRepository.GetByIdAsync(positionId);
    return position?.Users.ToList() ?? new List<LeanUser>();
  }

  public async Task AssignUsersAsync(long positionId, IEnumerable<long> userIds)
  {
    // 实现用户分配到岗位的逻辑
    var position = await _positionRepository.GetByIdAsync(positionId);
    if (position == null)
    {
      throw new ArgumentException("Position not found", nameof(positionId));
    }

    // TODO: 实现分配逻辑
    await Task.CompletedTask;
  }
}