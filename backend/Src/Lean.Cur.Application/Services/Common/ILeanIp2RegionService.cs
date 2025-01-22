using System.Threading.Tasks;

namespace Lean.Cur.Application.Services.Common;

/// <summary>
/// IP地址解析服务接口
/// </summary>
public interface ILeanIp2RegionService
{
  /// <summary>
  /// 解析IP地址
  /// </summary>
  /// <param name="ip">IP地址</param>
  /// <returns>地理位置信息</returns>
  Task<string> SearchAsync(string ip);
}