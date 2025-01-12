using Lean.Cur.Domain.Entities;
using Lean.Cur.Domain.Repositories;

namespace Lean.Cur.Application.Services.Impl;

public class LeanNoticeService : ILeanNoticeService
{
  private readonly ILeanRepository<LeanNotice> _noticeRepository;

  public LeanNoticeService(ILeanRepository<LeanNotice> noticeRepository)
  {
    _noticeRepository = noticeRepository;
  }

  public async Task<List<LeanNotice>> GetNoticeListAsync(string? keyword)
  {
    var list = await _noticeRepository.GetListAsync();
    if (!string.IsNullOrEmpty(keyword))
    {
      // 在此添加关键字过滤逻辑
    }
    return list;
  }

  public async Task<LeanNotice?> GetNoticeAsync(long id)
  {
    return await _noticeRepository.GetByIdAsync(id);
  }

  public async Task<LeanNotice> CreateNoticeAsync(LeanNotice notice)
  {
    notice.CreateTime = DateTime.Now;
    return await _noticeRepository.CreateAsync(notice);
  }

  public async Task<LeanNotice> UpdateNoticeAsync(LeanNotice notice)
  {
    notice.UpdateTime = DateTime.Now;
    return await _noticeRepository.UpdateAsync(notice);
  }

  public async Task DeleteNoticeAsync(long id)
  {
    await _noticeRepository.DeleteAsync(id);
  }
}