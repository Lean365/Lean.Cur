using System.Security.Cryptography;
using Lean.Cur.Application.Dtos.Common;
using Lean.Cur.Application.Services.Common;
using Lean.Cur.Common.Enums;
using Lean.Cur.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Common;

/// <summary>
/// 文件服务
/// </summary>
public class LeanFileService : ILeanFileService
{
  private readonly ISqlSugarClient _db;
  private readonly ILeanStorageProvider _storageProvider;
  private readonly ILogger<LeanFileService> _logger;
  private readonly IConfiguration _configuration;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">数据库</param>
  /// <param name="storageProvider">存储提供者</param>
  /// <param name="logger">日志</param>
  /// <param name="configuration">配置</param>
  public LeanFileService(ISqlSugarClient db, ILeanStorageProvider storageProvider, ILogger<LeanFileService> logger, IConfiguration configuration)
  {
    _db = db;
    _storageProvider = storageProvider;
    _logger = logger;
    _configuration = configuration;
  }

  /// <summary>
  /// 上传文件
  /// </summary>
  /// <param name="file">文件</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <param name="isTemp">是否临时文件</param>
  /// <returns>文件信息</returns>
  public async Task<LeanFileDto> UploadAsync(IFormFile file, LeanBusinessType businessType, long? businessId = null, bool isTemp = false)
  {
    // 验证文件大小
    var maxSize = _configuration.GetValue<long>("FileSettings:MaxSize", 10 * 1024 * 1024);
    if (file.Length > maxSize)
    {
      throw new Exception($"文件大小超过限制: {maxSize / 1024 / 1024}MB");
    }

    // 验证文件扩展名
    var extension = Path.GetExtension(file.FileName).ToLower();
    if (!Enum.TryParse<LeanFileExtension>(extension.TrimStart('.').ToUpper(), out var fileExtension))
    {
      throw new Exception($"不支持的文件类型: {extension}");
    }

    // 获取文件类型
    var fileType = GetFileType(fileExtension);

    // 生成存储文件名
    var storageName = $"{Guid.NewGuid():N}{extension}";

    // 获取存储路径
    var storagePath = GetStoragePath(businessType);

    // 计算文件MD5
    using var stream = file.OpenReadStream();
    var md5 = await _storageProvider.GetMd5Async(stream);

    // 检查文件是否已存在
    var existFile = await _db.Queryable<LeanFile>()
        .FirstAsync(x => x.FileMd5 == md5 && x.FileSize == file.Length);

    if (existFile != null)
    {
      // 返回已存在的文件信息
      return await GetFileInfoAsync(existFile.Id);
    }

    // 保存文件
    stream.Position = 0;
    var success = await _storageProvider.SaveAsync(stream, storagePath, storageName);
    if (!success)
    {
      throw new Exception("保存文件失败");
    }

    // 保存文件信息
    var fileInfo = new LeanFile
    {
      OriginalName = file.FileName,
      StorageName = storageName,
      FileSize = file.Length,
      FileType = fileType,
      FileExtension = fileExtension,
      FileMd5 = md5,
      StorageType = _storageProvider.StorageType,
      StoragePath = storagePath,
      BusinessType = businessType,
      BusinessId = businessId,
      IsTemp = isTemp
    };

    await _db.Insertable(fileInfo).ExecuteReturnEntityAsync();
    return await GetFileInfoAsync(fileInfo.Id);
  }

  /// <summary>
  /// 下载文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件流</returns>
  public async Task<(string fileName, string contentType, Stream fileStream)> DownloadAsync(long id)
  {
    var file = await _db.Queryable<LeanFile>()
        .FirstAsync(x => x.Id == id);

    if (file == null)
    {
      throw new Exception($"文件不存在: {id}");
    }

    var stream = await _storageProvider.GetAsync(file.StoragePath, file.StorageName);
    var contentType = GetContentType(file.FileExtension);

    return (file.OriginalName, contentType, stream);
  }

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>是否成功</returns>
  public async Task<bool> DeleteAsync(long id)
  {
    var file = await _db.Queryable<LeanFile>()
        .FirstAsync(x => x.Id == id);

    if (file == null)
    {
      return true;
    }

    // 删除物理文件
    var success = await _storageProvider.DeleteAsync(file.StoragePath, file.StorageName);
    if (!success)
    {
      return false;
    }

    // 删除数据库记录
    await _db.Deleteable<LeanFile>()
        .Where(x => x.Id == id)
        .ExecuteCommandAsync();

    return true;
  }

  /// <summary>
  /// 清理临时文件
  /// </summary>
  /// <param name="days">保留天数</param>
  /// <returns>清理数量</returns>
  public async Task<int> CleanTempFilesAsync(int days = 7)
  {
    var files = await _db.Queryable<LeanFile>()
        .Where(x => x.IsTemp && x.CreateTime < DateTime.Now.AddDays(-days))
        .ToListAsync();

    var count = 0;
    foreach (var file in files)
    {
      if (await DeleteAsync(file.Id))
      {
        count++;
      }
    }

    return count;
  }

  /// <summary>
  /// 关联业务
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <returns>是否成功</returns>
  public async Task<bool> AssociateBusinessAsync(long id, LeanBusinessType businessType, long businessId)
  {
    var file = await _db.Queryable<LeanFile>()
        .FirstAsync(x => x.Id == id);

    if (file == null)
    {
      throw new Exception($"文件不存在: {id}");
    }

    file.BusinessType = businessType;
    file.BusinessId = businessId;
    file.IsTemp = false;

    await _db.Updateable(file).ExecuteCommandAsync();
    return true;
  }

  /// <summary>
  /// 获取文件信息
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件信息</returns>
  public async Task<LeanFileDto> GetFileInfoAsync(long id)
  {
    var file = await _db.Queryable<LeanFile>()
        .FirstAsync(x => x.Id == id);

    if (file == null)
    {
      throw new Exception($"文件不存在: {id}");
    }

    return new LeanFileDto
    {
      Id = file.Id,
      OriginalName = file.OriginalName,
      FileSize = file.FileSize,
      FileType = file.FileType,
      FileExtension = file.FileExtension,
      BusinessType = file.BusinessType,
      BusinessId = file.BusinessId,
      IsTemp = file.IsTemp,
      CreateTime = file.CreateTime
    };
  }

  /// <summary>
  /// 获取业务相关文件列表
  /// </summary>
  /// <param name="businessType">业务类型</param>
  /// <param name="businessId">业务ID</param>
  /// <returns>文件列表</returns>
  public async Task<List<LeanFileDto>> GetBusinessFilesAsync(LeanBusinessType businessType, long businessId)
  {
    var files = await _db.Queryable<LeanFile>()
        .Where(x => x.BusinessType == businessType && x.BusinessId == businessId)
        .OrderByDescending(x => x.CreateTime)
        .ToListAsync();

    return files.Select(x => new LeanFileDto
    {
      Id = x.Id,
      OriginalName = x.OriginalName,
      FileSize = x.FileSize,
      FileType = x.FileType,
      FileExtension = x.FileExtension,
      BusinessType = x.BusinessType,
      BusinessId = x.BusinessId,
      IsTemp = x.IsTemp,
      CreateTime = x.CreateTime
    }).ToList();
  }

  /// <summary>
  /// 获取临时文件列表
  /// </summary>
  /// <param name="days">创建天数</param>
  /// <returns>文件列表</returns>
  public async Task<List<LeanFileDto>> GetTempFilesAsync(int days = 7)
  {
    var files = await _db.Queryable<LeanFile>()
        .Where(x => x.IsTemp && x.CreateTime >= DateTime.Now.AddDays(-days))
        .OrderByDescending(x => x.CreateTime)
        .ToListAsync();

    return files.Select(x => new LeanFileDto
    {
      Id = x.Id,
      OriginalName = x.OriginalName,
      FileSize = x.FileSize,
      FileType = x.FileType,
      FileExtension = x.FileExtension,
      BusinessType = x.BusinessType,
      BusinessId = x.BusinessId,
      IsTemp = x.IsTemp,
      CreateTime = x.CreateTime
    }).ToList();
  }

  /// <summary>
  /// 获取文件类型
  /// </summary>
  /// <param name="extension">文件扩展名</param>
  /// <returns>文件类型</returns>
  private static LeanFileType GetFileType(LeanFileExtension extension)
  {
    return extension switch
    {
      LeanFileExtension.JPG or LeanFileExtension.JPEG or LeanFileExtension.PNG or LeanFileExtension.GIF => LeanFileType.Image,
      LeanFileExtension.DOC or LeanFileExtension.DOCX => LeanFileType.Word,
      LeanFileExtension.XLS or LeanFileExtension.XLSX => LeanFileType.Excel,
      LeanFileExtension.PDF => LeanFileType.PDF,
      LeanFileExtension.TXT => LeanFileType.Text,
      LeanFileExtension.ZIP or LeanFileExtension.RAR => LeanFileType.Archive,
      LeanFileExtension.MP3 or LeanFileExtension.WAV => LeanFileType.Audio,
      LeanFileExtension.MP4 or LeanFileExtension.AVI => LeanFileType.Video,
      _ => LeanFileType.Other
    };
  }

  /// <summary>
  /// 获取存储路径
  /// </summary>
  /// <param name="businessType">业务类型</param>
  /// <returns>存储路径</returns>
  private static LeanStoragePath GetStoragePath(LeanBusinessType businessType)
  {
    return businessType switch
    {
      LeanBusinessType.EmailAttachment => LeanStoragePath.EmailAttachment,
      LeanBusinessType.UserAvatar => LeanStoragePath.UserAvatar,
      LeanBusinessType.SystemDocument => LeanStoragePath.SystemDocument,
      LeanBusinessType.Import => LeanStoragePath.Import,
      LeanBusinessType.Export => LeanStoragePath.Export,
      LeanBusinessType.Temporary => LeanStoragePath.Temporary,
      _ => LeanStoragePath.Temporary
    };
  }

  /// <summary>
  /// 获取内容类型
  /// </summary>
  /// <param name="extension">文件扩展名</param>
  /// <returns>内容类型</returns>
  private static string GetContentType(LeanFileExtension extension)
  {
    return extension switch
    {
      LeanFileExtension.JPG or LeanFileExtension.JPEG => "image/jpeg",
      LeanFileExtension.PNG => "image/png",
      LeanFileExtension.GIF => "image/gif",
      LeanFileExtension.DOC => "application/msword",
      LeanFileExtension.DOCX => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
      LeanFileExtension.XLS => "application/vnd.ms-excel",
      LeanFileExtension.XLSX => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      LeanFileExtension.PDF => "application/pdf",
      LeanFileExtension.TXT => "text/plain",
      LeanFileExtension.ZIP => "application/zip",
      LeanFileExtension.RAR => "application/x-rar-compressed",
      LeanFileExtension.MP3 => "audio/mpeg",
      LeanFileExtension.WAV => "audio/wav",
      LeanFileExtension.MP4 => "video/mp4",
      LeanFileExtension.AVI => "video/x-msvideo",
      _ => "application/octet-stream"
    };
  }
}