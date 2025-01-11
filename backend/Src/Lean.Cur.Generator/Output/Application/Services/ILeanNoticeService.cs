using Lean.Cur.Domain.Entities;

namespace Lean.Cur.Application.Services;

public interface ILeanNoticeService
{
    Task<List<LeanNotice>> GetNoticeListAsync(string? keyword);
    Task<LeanNotice?> GetNoticeAsync(long id);
    Task<LeanNotice> CreateNoticeAsync(LeanNotice notice);
    Task<LeanNotice> UpdateNoticeAsync(LeanNotice notice);
    Task DeleteNoticeAsync(long id);
} 