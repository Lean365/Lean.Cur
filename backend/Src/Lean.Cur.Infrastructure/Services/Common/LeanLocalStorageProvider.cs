using System.Security.Cryptography;
using Lean.Cur.Application.Services.Common;
using Lean.Cur.Common.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Infrastructure.Services.Common;

/// <summary>
/// 本地存储提供者
/// </summary>
public class LeanLocalStorageProvider : ILeanStorageProvider
{
  private readonly IConfiguration _configuration;
  private readonly ILogger<LeanLocalStorageProvider> _logger;
  private readonly string _rootPath;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="configuration">配置</param>
  /// <param name="logger">日志</param>
  public LeanLocalStorageProvider(IConfiguration configuration, ILogger<LeanLocalStorageProvider> logger)
  {
    _configuration = configuration;
    _logger = logger;
    _rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
  }

  /// <summary>
  /// 存储类型
  /// </summary>
  public LeanStorageType StorageType => LeanStorageType.Local;

  /// <summary>
  /// 保存文件
  /// </summary>
  /// <param name="stream">文件流</param>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>是否成功</returns>
  public async Task<bool> SaveAsync(Stream stream, LeanStoragePath storagePath, string storageName)
  {
    try
    {
      var path = GetFullPath(storagePath, storageName);
      var directory = Path.GetDirectoryName(path);
      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory!);
      }

      using var fileStream = new FileStream(path, FileMode.Create);
      await stream.CopyToAsync(fileStream);
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "保存文件失败: {StoragePath}/{StorageName}", storagePath, storageName);
      return false;
    }
  }

  /// <summary>
  /// 获取文件
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>文件流</returns>
  public Task<Stream> GetAsync(LeanStoragePath storagePath, string storageName)
  {
    var path = GetFullPath(storagePath, storageName);
    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"文件不存在: {storagePath}/{storageName}");
    }

    return Task.FromResult<Stream>(new FileStream(path, FileMode.Open, FileAccess.Read));
  }

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>是否成功</returns>
  public Task<bool> DeleteAsync(LeanStoragePath storagePath, string storageName)
  {
    try
    {
      var path = GetFullPath(storagePath, storageName);
      if (File.Exists(path))
      {
        File.Delete(path);
      }
      return Task.FromResult(true);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "删除文件失败: {StoragePath}/{StorageName}", storagePath, storageName);
      return Task.FromResult(false);
    }
  }

  /// <summary>
  /// 文件是否存在
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>是否存在</returns>
  public Task<bool> ExistsAsync(LeanStoragePath storagePath, string storageName)
  {
    var path = GetFullPath(storagePath, storageName);
    return Task.FromResult(File.Exists(path));
  }

  /// <summary>
  /// 获取文件大小
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>文件大小(字节)</returns>
  public Task<long> GetSizeAsync(LeanStoragePath storagePath, string storageName)
  {
    var path = GetFullPath(storagePath, storageName);
    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"文件不存在: {storagePath}/{storageName}");
    }

    var fileInfo = new FileInfo(path);
    return Task.FromResult(fileInfo.Length);
  }

  /// <summary>
  /// 获取文件MD5
  /// </summary>
  /// <param name="stream">文件流</param>
  /// <returns>文件MD5</returns>
  public async Task<string> GetMd5Async(Stream stream)
  {
    using var md5 = MD5.Create();
    var hash = await md5.ComputeHashAsync(stream);
    return BitConverter.ToString(hash).Replace("-", "").ToLower();
  }

  /// <summary>
  /// 获取完整路径
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>完整路径</returns>
  private string GetFullPath(LeanStoragePath storagePath, string storageName)
  {
    return Path.Combine(_rootPath, storagePath.ToString(), storageName);
  }
}