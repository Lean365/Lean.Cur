using System;
using System.IO;
using System.Threading.Tasks;
using IP2Region.Net.XDB;
using Microsoft.Extensions.Options;
using Lean.Cur.Common.Configs;
using Lean.Cur.Application.Services.Common;

namespace Lean.Cur.Infrastructure.Services.Common;

/// <summary>
/// IP地址解析服务
/// </summary>
public class LeanIp2RegionService : ILeanIp2RegionService
{
  private readonly Searcher _searcher;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="options">配置选项</param>
  public LeanIp2RegionService(IOptions<LeanSecuritySettings> options)
  {
    var dbPath = options.Value.Ip2RegionDbPath;
    if (!File.Exists(dbPath))
    {
      throw new FileNotFoundException("IP2Region 数据库文件不存在", dbPath);
    }

    _searcher = new Searcher(CachePolicy.Content, dbPath);
  }

  /// <inheritdoc/>
  public async Task<string> SearchAsync(string ip)
  {
    try
    {
      if (string.IsNullOrWhiteSpace(ip) || ip.Equals("::1") || ip.StartsWith("127.0.0.1"))
      {
        return "本地";
      }

      var region = await Task.Run(() => _searcher.Search(ip));
      if (region == null)
      {
        return "未知";
      }

      // 解析结果
      var parts = region.Split('|');
      if (parts.Length < 4)
      {
        return "未知";
      }

      var country = parts[0].Trim();
      var province = parts[2].Trim();
      var city = parts[3].Trim();

      // 格式化输出
      if (country == "中国")
      {
        if (province == "0")
        {
          return country;
        }

        if (city == "0")
        {
          return province;
        }

        if (province == city)
        {
          return province;
        }

        return $"{province}{city}";
      }

      return country == "0" ? "未知" : country;
    }
    catch (Exception ex)
    {
      throw new InvalidOperationException($"IP地址解析失败：{ip}", ex);
    }
  }
}