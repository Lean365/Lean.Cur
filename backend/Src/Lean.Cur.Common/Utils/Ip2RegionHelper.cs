using IP2Region.Net.XDB;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Common.Utils;

/// <summary>
/// IP地理位置解析工具类
/// </summary>
public class Ip2RegionHelper
{
  private readonly ILogger<Ip2RegionHelper> _logger;
  private readonly string _dbPath;
  private readonly Searcher _searcher;

  /// <summary>
  /// 构造函数
  /// </summary>
  public Ip2RegionHelper(ILogger<Ip2RegionHelper> logger)
  {
    _logger = logger;

    // 获取xdb文件路径
    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
    _dbPath = Path.Combine(baseDir, "Resources", "ip2region.xdb");

    // 确保目录存在
    var dir = Path.GetDirectoryName(_dbPath);
    if (!Directory.Exists(dir))
    {
      Directory.CreateDirectory(dir!);
    }

    // 如果文件不存在，从嵌入资源复制
    if (!File.Exists(_dbPath))
    {
      var assembly = typeof(Ip2RegionHelper).Assembly;
      using var stream = assembly.GetManifestResourceStream("Lean.Cur.Common.Resources.ip2region.xdb");
      if (stream == null)
      {
        throw new FileNotFoundException("未找到ip2region.xdb资源文件");
      }

      using var fileStream = File.Create(_dbPath);
      stream.CopyTo(fileStream);
    }

    // 初始化searcher
    _searcher = new Searcher(CachePolicy.Content, _dbPath);
    _logger.LogInformation("IP2Region 数据库加载成功");
  }

  /// <summary>
  /// 解析IP地址
  /// </summary>
  /// <param name="ip">IP地址</param>
  /// <returns>地理位置信息</returns>
  public string Search(string? ip)
  {
    try
    {
      if (string.IsNullOrEmpty(ip) || ip == "::1" || ip.StartsWith("127.0"))
        return "本地";

      // 查询
      var region = _searcher.Search(ip);
      if (region == null) return "未知";

      // 解析结果
      var parts = region.Split('|');
      if (parts.Length < 4) return "未知";

      var country = parts[0].Trim();
      var province = parts[2].Trim();
      var city = parts[3].Trim();

      // 格式化输出
      if (country == "中国")
      {
        if (province == "0")
          return country;

        if (city == "0")
          return province;

        if (province == city)
          return province;

        return $"{province}{city}";
      }

      return country == "0" ? "未知" : country;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "解析IP地址失败: {IP}", ip);
      return "未知";
    }
  }
}