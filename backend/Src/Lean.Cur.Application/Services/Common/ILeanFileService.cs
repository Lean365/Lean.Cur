using Lean.Cur.Application.Dtos.Common;
using Lean.Cur.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Lean.Cur.Application.Services.Common;

/// <summary>
/// 文件服务接口
/// </summary>
public interface ILeanFileService
{
  /// <summary>
  /// 上传文件
  /// </summary>
  /// <param name="file">文件</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <param name="isTemp">是否临时文件</param>
  /// <returns>文件信息</returns>
  Task<LeanFileDto> UploadAsync(IFormFile file, LeanBusinessType businessType, long? businessId = null, bool isTemp = false);

  /// <summary>
  /// 下载文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件流</returns>
  Task<(string fileName, string contentType, Stream fileStream)> DownloadAsync(long id);

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(long id);

  /// <summary>
  /// 清理临时文件
  /// </summary>
  /// <param name="days">保留天数</param>
  /// <returns>清理数量</returns>
  Task<int> CleanTempFilesAsync(int days = 7);

  /// <summary>
  /// 关联业务
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <returns>是否成功</returns>
  Task<bool> AssociateBusinessAsync(long id, LeanBusinessType businessType, long businessId);

  /// <summary>
  /// 获取文件信息
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件信息</returns>
  Task<LeanFileDto> GetFileInfoAsync(long id);

  /// <summary>
  /// 获取业务相关文件列表
  /// </summary>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <returns>文件列表</returns>
  Task<List<LeanFileDto>> GetBusinessFilesAsync(LeanBusinessType businessType, long businessId);

  /// <summary>
  /// 获取临时文件列表
  /// </summary>
  /// <param name="days">创建天数</param>
  /// <returns>文件列表</returns>
  Task<List<LeanFileDto>> GetTempFilesAsync(int days = 7);
}