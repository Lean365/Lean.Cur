// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Domain.Entities.Routine;
using SqlSugar;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Lean.Cur.Infrastructure.Services.Routine;

/// <summary>
/// 在线用户服务实现类
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：实现在线用户相关的所有服务方法
/// </remarks>
public class LeanOnlineService : ILeanOnlineService
{
  private readonly SqlSugarClient _db;

  public LeanOnlineService(SqlSugarClient db)
  {
    _db = db;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanOnlineUserDto>> GetPagedListAsync(LeanOnlineUserQueryDto queryDto)
  {
    var query = _db.Queryable<LeanOnlineUser>();

    // 应用查询条件
    if (!string.IsNullOrEmpty(queryDto.UserName))
    {
      query = query.Where(x => x.UserName.Contains(queryDto.UserName));
    }

    if (!string.IsNullOrEmpty(queryDto.IpAddress))
    {
      query = query.Where(x => x.IpAddress.Contains(queryDto.IpAddress));
    }

    if (!string.IsNullOrEmpty(queryDto.DeviceType))
    {
      query = query.Where(x => x.DeviceType == queryDto.DeviceType);
    }

    if (queryDto.StartTime.HasValue)
    {
      query = query.Where(x => x.CreateTime >= queryDto.StartTime.Value);
    }

    if (queryDto.EndTime.HasValue)
    {
      query = query.Where(x => x.CreateTime <= queryDto.EndTime.Value);
    }

    // 获取总记录数
    var total = await query.CountAsync();

    // 获取分页数据
    var items = await query
        .OrderByDescending(x => x.CreateTime)
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .Select(x => new LeanOnlineUserDto
        {
          UserId = x.UserId,
          UserName = x.UserName,
          ConnectionId = x.ConnectionId,
          IpAddress = x.IpAddress,
          DeviceType = x.DeviceType,
          DeviceName = x.DeviceName,
          Browser = x.Browser,
          Os = x.Os,
          LastActiveTime = x.LastActiveTime,
          PcTodayOnlineTime = x.PcTodayOnlineTime,
          PcTotalOnlineTime = x.PcTotalOnlineTime,
          MobileTodayOnlineTime = x.MobileTodayOnlineTime,
          MobileTotalOnlineTime = x.MobileTotalOnlineTime,
          TabletTodayOnlineTime = x.TabletTodayOnlineTime,
          TabletTotalOnlineTime = x.TabletTotalOnlineTime,
          OtherTodayOnlineTime = x.OtherTodayOnlineTime,
          OtherTotalOnlineTime = x.OtherTotalOnlineTime,
          CreateTime = x.CreateTime,
          UpdateTime = x.UpdateTime
        })
        .ToListAsync();

    return new LeanPagedResult<LeanOnlineUserDto>
    {
      Items = items,
      Total = total,
      PageIndex = queryDto.PageIndex,
      PageSize = queryDto.PageSize
    };
  }

  /// <inheritdoc/>
  public async Task<List<LeanOnlineUserDto>> GetListAsync(LeanOnlineUserQueryDto queryDto)
  {
    var query = _db.Queryable<LeanOnlineUser>();

    // 应用查询条件
    if (!string.IsNullOrEmpty(queryDto.UserName))
    {
      query = query.Where(x => x.UserName.Contains(queryDto.UserName));
    }

    if (!string.IsNullOrEmpty(queryDto.IpAddress))
    {
      query = query.Where(x => x.IpAddress.Contains(queryDto.IpAddress));
    }

    if (!string.IsNullOrEmpty(queryDto.DeviceType))
    {
      query = query.Where(x => x.DeviceType == queryDto.DeviceType);
    }

    if (queryDto.StartTime.HasValue)
    {
      query = query.Where(x => x.CreateTime >= queryDto.StartTime.Value);
    }

    if (queryDto.EndTime.HasValue)
    {
      query = query.Where(x => x.CreateTime <= queryDto.EndTime.Value);
    }

    // 获取数据
    return await query
        .OrderByDescending(x => x.CreateTime)
        .Select(x => new LeanOnlineUserDto
        {
          UserId = x.UserId,
          UserName = x.UserName,
          ConnectionId = x.ConnectionId,
          IpAddress = x.IpAddress,
          DeviceType = x.DeviceType,
          DeviceName = x.DeviceName,
          Browser = x.Browser,
          Os = x.Os,
          LastActiveTime = x.LastActiveTime,
          PcTodayOnlineTime = x.PcTodayOnlineTime,
          PcTotalOnlineTime = x.PcTotalOnlineTime,
          MobileTodayOnlineTime = x.MobileTodayOnlineTime,
          MobileTotalOnlineTime = x.MobileTotalOnlineTime,
          TabletTodayOnlineTime = x.TabletTodayOnlineTime,
          TabletTotalOnlineTime = x.TabletTotalOnlineTime,
          OtherTodayOnlineTime = x.OtherTodayOnlineTime,
          OtherTotalOnlineTime = x.OtherTotalOnlineTime,
          CreateTime = x.CreateTime,
          UpdateTime = x.UpdateTime
        })
        .ToListAsync();
  }

  /// <inheritdoc/>
  public async Task<LeanOnlineUserDto> GetByIdAsync(long id)
  {
    var entity = await _db.Queryable<LeanOnlineUser>()
        .FirstAsync(x => x.Id == id);

    if (entity == null)
    {
      return null;
    }

    return new LeanOnlineUserDto
    {
      UserId = entity.UserId,
      UserName = entity.UserName,
      ConnectionId = entity.ConnectionId,
      IpAddress = entity.IpAddress,
      DeviceType = entity.DeviceType,
      DeviceName = entity.DeviceName,
      Browser = entity.Browser,
      Os = entity.Os,
      LastActiveTime = entity.LastActiveTime,
      PcTodayOnlineTime = entity.PcTodayOnlineTime,
      PcTotalOnlineTime = entity.PcTotalOnlineTime,
      MobileTodayOnlineTime = entity.MobileTodayOnlineTime,
      MobileTotalOnlineTime = entity.MobileTotalOnlineTime,
      TabletTodayOnlineTime = entity.TabletTodayOnlineTime,
      TabletTotalOnlineTime = entity.TabletTotalOnlineTime,
      OtherTodayOnlineTime = entity.OtherTodayOnlineTime,
      OtherTotalOnlineTime = entity.OtherTotalOnlineTime,
      CreateTime = entity.CreateTime,
      UpdateTime = entity.UpdateTime
    };
  }

  /// <inheritdoc/>
  public async Task<int> GetOnlineCountAsync()
  {
    return await _db.Queryable<LeanOnlineUser>().CountAsync();
  }

  #endregion

  #region 在线状态管理

  /// <inheritdoc/>
  public async Task<bool> UserConnectedAsync(
      long userId,
      string userName,
      string connectionId,
      string ipAddress,
      string deviceType,
      string deviceName,
      string location,
      string? browser = null,
      string? os = null)
  {
    var entity = new LeanOnlineUser
    {
      UserId = userId,
      UserName = userName,
      ConnectionId = connectionId,
      IpAddress = ipAddress,
      DeviceType = deviceType,
      DeviceName = deviceName,
      Browser = browser,
      Os = os,
      LastActiveTime = DateTime.Now
    };

    return await _db.Insertable(entity).ExecuteCommandAsync() > 0;
  }

  /// <inheritdoc/>
  public async Task<bool> UserDisconnectedAsync(string connectionId)
  {
    return await _db.Deleteable<LeanOnlineUser>()
        .Where(x => x.ConnectionId == connectionId)
        .ExecuteCommandAsync() > 0;
  }

  /// <inheritdoc/>
  public async Task<bool> IsUserOnlineAsync(long userId)
  {
    return await _db.Queryable<LeanOnlineUser>()
        .AnyAsync(x => x.UserId == userId);
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateLastActiveTimeAsync(string connectionId)
  {
    return await _db.Updateable<LeanOnlineUser>()
        .SetColumns(x => x.LastActiveTime == DateTime.Now)
        .Where(x => x.ConnectionId == connectionId)
        .ExecuteCommandAsync() > 0;
  }

  /// <inheritdoc/>
  public async Task<Dictionary<string, int>> GetTodayOnlineTimeAsync(long userId)
  {
    var entity = await _db.Queryable<LeanOnlineUser>()
        .FirstAsync(x => x.UserId == userId);

    if (entity == null)
    {
      return new Dictionary<string, int>
      {
        { "PC", 0 },
        { "Mobile", 0 },
        { "Tablet", 0 },
        { "Other", 0 }
      };
    }

    return new Dictionary<string, int>
    {
      { "PC", entity.PcTodayOnlineTime },
      { "Mobile", entity.MobileTodayOnlineTime },
      { "Tablet", entity.TabletTodayOnlineTime },
      { "Other", entity.OtherTodayOnlineTime }
    };
  }

  /// <inheritdoc/>
  public async Task<Dictionary<string, int>> GetTotalOnlineTimeAsync(long userId)
  {
    var entity = await _db.Queryable<LeanOnlineUser>()
        .FirstAsync(x => x.UserId == userId);

    if (entity == null)
    {
      return new Dictionary<string, int>
      {
        { "PC", 0 },
        { "Mobile", 0 },
        { "Tablet", 0 },
        { "Other", 0 }
      };
    }

    return new Dictionary<string, int>
    {
      { "PC", entity.PcTotalOnlineTime },
      { "Mobile", entity.MobileTotalOnlineTime },
      { "Tablet", entity.TabletTotalOnlineTime },
      { "Other", entity.OtherTotalOnlineTime }
    };
  }

  /// <inheritdoc/>
  public async Task<int> CleanupTimeoutUsersAsync(int timeoutMinutes = 30)
  {
    var timeoutTime = DateTime.Now.AddMinutes(-timeoutMinutes);
    return await _db.Deleteable<LeanOnlineUser>()
        .Where(x => x.LastActiveTime < timeoutTime)
        .ExecuteCommandAsync();
  }

  /// <inheritdoc/>
  public async Task UpdateAllOnlineTimeAsync()
  {
    var users = await _db.Queryable<LeanOnlineUser>().ToListAsync();
    var now = DateTime.Now;

    foreach (var user in users)
    {
      var timeDiff = (now - user.LastActiveTime).TotalMinutes;
      if (timeDiff <= 0) continue;

      switch (user.DeviceType?.ToLower())
      {
        case "pc":
          user.PcTodayOnlineTime += (int)timeDiff;
          user.PcTotalOnlineTime += (int)timeDiff;
          break;
        case "mobile":
          user.MobileTodayOnlineTime += (int)timeDiff;
          user.MobileTotalOnlineTime += (int)timeDiff;
          break;
        case "tablet":
          user.TabletTodayOnlineTime += (int)timeDiff;
          user.TabletTotalOnlineTime += (int)timeDiff;
          break;
        default:
          user.OtherTodayOnlineTime += (int)timeDiff;
          user.OtherTotalOnlineTime += (int)timeDiff;
          break;
      }

      user.LastActiveTime = now;
      user.UpdateTime = now;
    }

    await _db.Updateable(users).ExecuteCommandAsync();
  }

  #endregion
}