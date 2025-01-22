// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// 在线时长更新服务
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：定期更新用户在线时长的后台服务
/// </remarks>
public class LeanOnlineTimeUpdateService : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;

  public LeanOnlineTimeUpdateService(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        using (var scope = _serviceProvider.CreateScope())
        {
          var onlineService = scope.ServiceProvider.GetRequiredService<ILeanOnlineService>();
          await onlineService.UpdateAllOnlineTimeAsync();
        }
      }
      catch (Exception ex)
      {
        // 记录错误但继续运行
        // TODO: 添加日志记录
      }

      // 每5分钟更新一次
      await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
    }
  }
}