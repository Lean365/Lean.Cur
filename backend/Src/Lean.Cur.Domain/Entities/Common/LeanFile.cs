using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Common;

/// <summary>
/// 文件信息
/// </summary>
[SugarTable("lean_file", TableDescription = "文件信息表")]
public class LeanFile : LeanBaseEntity
{
  /// <summary>
  /// 原始文件名
  /// </summary>
  [SugarColumn(ColumnName = "original_name", ColumnDescription = "原始文件名", Length = 255, IsNullable = false)]
  [Description("原始文件名")]
  public string OriginalName { get; set; } = null!;

  /// <summary>
  /// 存储文件名
  /// </summary>
  [SugarColumn(ColumnName = "storage_name", ColumnDescription = "存储文件名", Length = 255, IsNullable = false)]
  [Description("存储文件名")]
  public string StorageName { get; set; } = null!;

  /// <summary>
  /// 存储路径
  /// </summary>
  [SugarColumn(ColumnName = "storage_path", ColumnDescription = "存储路径", IsNullable = false)]
  [Description("存储路径")]
  public LeanStoragePath StoragePath { get; set; }

  /// <summary>
  /// 文件大小(字节)
  /// </summary>
  [SugarColumn(ColumnName = "file_size", ColumnDescription = "文件大小(字节)", IsNullable = false)]
  [Description("文件大小(字节)")]
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  [SugarColumn(ColumnName = "file_type", ColumnDescription = "文件类型", IsNullable = false)]
  [Description("文件类型")]
  public LeanFileType FileType { get; set; }

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [SugarColumn(ColumnName = "file_extension", ColumnDescription = "文件扩展名", IsNullable = false)]
  [Description("文件扩展名")]
  public LeanFileExtension FileExtension { get; set; }

  /// <summary>
  /// 文件MD5
  /// </summary>
  [SugarColumn(ColumnName = "file_md5", ColumnDescription = "文件MD5", Length = 32, IsNullable = false)]
  [Description("文件MD5")]
  public string FileMd5 { get; set; } = null!;

  /// <summary>
  /// 存储类型
  /// </summary>
  [SugarColumn(ColumnName = "storage_type", ColumnDescription = "存储类型", IsNullable = false)]
  [Description("存储类型")]
  public LeanStorageType StorageType { get; set; }

  /// <summary>
  /// 业务类型
  /// </summary>
  [SugarColumn(ColumnName = "business_type", ColumnDescription = "业务类型", IsNullable = false)]
  [Description("业务类型")]
  public LeanBusinessType BusinessType { get; set; }

  /// <summary>
  /// 业务ID
  /// </summary>
  [SugarColumn(ColumnName = "business_id", ColumnDescription = "业务ID", IsNullable = true)]
  [Description("业务ID")]
  public long? BusinessId { get; set; }

  /// <summary>
  /// 是否临时文件
  /// </summary>
  [SugarColumn(ColumnName = "is_temp", ColumnDescription = "是否临时文件", IsNullable = false)]
  [Description("是否临时文件")]
  public bool IsTemp { get; set; }
}