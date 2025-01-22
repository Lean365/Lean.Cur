using Lean.Cur.Application.Dtos.Common;
using Lean.Cur.Application.Services.Common;
using Lean.Cur.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Common;

/// <summary>
/// 文件控制器
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeanFileController : ControllerBase
{
  private readonly ILeanFileService _fileService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="fileService">文件服务</param>
  public LeanFileController(ILeanFileService fileService)
  {
    _fileService = fileService;
  }

  /// <summary>
  /// 上传文件
  /// </summary>
  /// <param name="file">文件</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <param name="isTemp">是否临时文件</param>
  /// <returns>文件信息</returns>
  [HttpPost("upload")]
  public async Task<LeanFileDto> UploadAsync(IFormFile file, LeanBusinessType businessType, long? businessId = null, bool isTemp = false)
  {
    return await _fileService.UploadAsync(file, businessType, businessId, isTemp);
  }

  /// <summary>
  /// 下载文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件流</returns>
  [HttpGet("download/{id}")]
  public async Task<IActionResult> DownloadAsync(long id)
  {
    var (fileName, contentType, fileStream) = await _fileService.DownloadAsync(id);
    return File(fileStream, contentType, fileName);
  }

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>是否成功</returns>
  [HttpDelete("{id}")]
  public async Task<bool> DeleteAsync(long id)
  {
    return await _fileService.DeleteAsync(id);
  }

  /// <summary>
  /// 清理临时文件
  /// </summary>
  /// <param name="days">保留天数</param>
  /// <returns>清理数量</returns>
  [HttpDelete("temp")]
  public async Task<int> CleanTempFilesAsync(int days = 7)
  {
    return await _fileService.CleanTempFilesAsync(days);
  }

  /// <summary>
  /// 关联业务
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <returns>是否成功</returns>
  [HttpPut("{id}/business")]
  public async Task<bool> AssociateBusinessAsync(long id, LeanBusinessType businessType, long businessId)
  {
    return await _fileService.AssociateBusinessAsync(id, businessType, businessId);
  }

  /// <summary>
  /// 获取文件信息
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件信息</returns>
  [HttpGet("{id}")]
  public async Task<LeanFileDto> GetFileInfoAsync(long id)
  {
    return await _fileService.GetFileInfoAsync(id);
  }

  /// <summary>
  /// 获取业务相关文件列表
  /// </summary>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <returns>文件列表</returns>
  [HttpGet("business")]
  public async Task<List<LeanFileDto>> GetBusinessFilesAsync(LeanBusinessType businessType, long businessId)
  {
    return await _fileService.GetBusinessFilesAsync(businessType, businessId);
  }

  /// <summary>
  /// 获取临时文件列表
  /// </summary>
  /// <param name="days">创建天数</param>
  /// <returns>文件列表</returns>
  [HttpGet("temp")]
  public async Task<List<LeanFileDto>> GetTempFilesAsync(int days = 7)
  {
    return await _fileService.GetTempFilesAsync(days);
  }
}