using System;

namespace Lean.Cur.Common.Configs
{
  /// <summary>
  /// IP2Region 配置
  /// </summary>
  public class LeanIp2RegionSettings
  {
    /// <summary>
    /// 数据库文件路径
    /// </summary>
    public string DbPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用文件缓存
    /// </summary>
    public bool EnableFileCache { get; set; } = true;
  }
}