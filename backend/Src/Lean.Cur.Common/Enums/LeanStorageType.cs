using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 存储类型枚举
/// </summary>
public enum LeanStorageType
{
  /// <summary>
  /// 本地存储
  /// </summary>
  [Description("本地存储")]
  Local = 1,

  /// <summary>
  /// 阿里云OSS
  /// </summary>
  [Description("阿里云OSS")]
  AliyunOSS = 2,

  /// <summary>
  /// 腾讯云COS
  /// </summary>
  [Description("腾讯云COS")]
  TencentCOS = 3,

  /// <summary>
  /// MinIO
  /// </summary>
  [Description("MinIO")]
  MinIO = 4
}