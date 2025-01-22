using Microsoft.AspNetCore.Http;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Application.Services.Common;

/// <summary>
/// 文件存储提供者接口
/// </summary>
public interface ILeanStorageProvider
{
  /// <summary>
  /// 存储类型
  /// </summary>
  LeanStorageType StorageType { get; }

  /// <summary>
  /// 保存文件
  /// </summary>
  /// <param name="stream">文件流</param>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>是否成功</returns>
  Task<bool> SaveAsync(Stream stream, LeanStoragePath storagePath, string storageName);

  /// <summary>
  /// 获取文件
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>文件流</returns>
  Task<Stream> GetAsync(LeanStoragePath storagePath, string storageName);

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>是否成功</returns>
  Task<bool> DeleteAsync(LeanStoragePath storagePath, string storageName);

  /// <summary>
  /// 文件是否存在
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>是否存在</returns>
  Task<bool> ExistsAsync(LeanStoragePath storagePath, string storageName);

  /// <summary>
  /// 获取文件大小
  /// </summary>
  /// <param name="storagePath">存储路径</param>
  /// <param name="storageName">存储文件名</param>
  /// <returns>文件大小(字节)</returns>
  Task<long> GetSizeAsync(LeanStoragePath storagePath, string storageName);

  /// <summary>
  /// 获取文件MD5
  /// </summary>
  /// <param name="stream">文件流</param>
  /// <returns>文件MD5</returns>
  Task<string> GetMd5Async(Stream stream);
}