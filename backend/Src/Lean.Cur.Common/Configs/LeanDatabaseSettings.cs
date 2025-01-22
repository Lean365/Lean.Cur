/**
 * @description 数据库配置类
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

namespace Lean.Cur.Common.Configs;

/// <summary>
/// 数据库配置
/// </summary>
public class LeanDatabaseSettings
{
  /// <summary>
  /// 数据库连接字符串
  /// </summary>
  public string ConnectionString { get; set; } = string.Empty;

  /// <summary>
  /// 是否初始化数据库
  /// </summary>
  public bool InitDb { get; set; } = true;

  /// <summary>
  /// 是否自动迁移
  /// </summary>
  public bool AutoMigrate { get; set; } = true;

  /// <summary>
  /// 是否初始化种子数据
  /// </summary>
  public bool SeedData { get; set; } = true;
}