// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Common.Pagination;
using System.Threading.Tasks;

namespace Lean.Cur.Application.Services.Routine;

/// <summary>
/// 在线用户服务接口
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本接口定义了在线用户相关的所有服务方法，包括：
/// 1. 基础操作（查询、统计等）
/// 2. 在线状态管理（上线、下线、活动时间更新等）
/// 3. 在线时长统计（今日在线时长、总在线时长等）
/// </remarks>
public interface ILeanOnlineService
{
  #region 基础操作

  /// <summary>
  /// 获取在线用户分页列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>分页结果</returns>
  Task<LeanPagedResult<LeanOnlineUserDto>> GetPagedListAsync(LeanOnlineUserQueryDto queryDto);

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>在线用户列表</returns>
  Task<List<LeanOnlineUserDto>> GetListAsync(LeanOnlineUserQueryDto queryDto);

  /// <summary>
  /// 获取在线用户详情
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>在线用户详情</returns>
  Task<LeanOnlineUserDto> GetByIdAsync(long id);

  /// <summary>
  /// 获取在线用户数量
  /// </summary>
  /// <returns>在线用户数量</returns>
  Task<int> GetOnlineCountAsync();

  #endregion

  #region 在线状态管理

  /// <summary>
  /// 用户上线
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="userName">用户名</param>
  /// <param name="connectionId">连接ID</param>
  /// <param name="ipAddress">IP地址</param>
  /// <param name="deviceType">设备类型</param>
  /// <param name="deviceName">设备名称</param>
  /// <param name="location">登录地点</param>
  /// <param name="browser">浏览器信息</param>
  /// <param name="os">操作系统信息</param>
  /// <returns>是否成功</returns>
  Task<bool> UserConnectedAsync(
      long userId,
      string userName,
      string connectionId,
      string ipAddress,
      string deviceType,
      string deviceName,
      string location,
      string? browser = null,
      string? os = null);

  /// <summary>
  /// 用户下线
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <returns>是否成功</returns>
  Task<bool> UserDisconnectedAsync(string connectionId);

  /// <summary>
  /// 更新用户最后活动时间
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <returns>是否成功</returns>
  Task<bool> UpdateLastActiveTimeAsync(string connectionId);

  #endregion

  #region 在线时长统计

  /// <summary>
  /// 获取用户今日在线时长
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>各设备类型的在线时长(分钟)</returns>
  /// <remarks>
  /// 返回字典中的键为设备类型:
  /// - PC: PC设备
  /// - Mobile: 移动设备
  /// - Tablet: 平板设备
  /// - Other: 其他设备
  /// </remarks>
  Task<Dictionary<string, int>> GetTodayOnlineTimeAsync(long userId);

  /// <summary>
  /// 获取用户总在线时长
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>各设备类型的在线时长(分钟)</returns>
  /// <remarks>
  /// 返回字典中的键为设备类型:
  /// - PC: PC设备
  /// - Mobile: 移动设备
  /// - Tablet: 平板设备
  /// - Other: 其他设备
  /// </remarks>
  Task<Dictionary<string, int>> GetTotalOnlineTimeAsync(long userId);

  /// <summary>
  /// 清理过期的在线用户记录
  /// </summary>
  /// <param name="timeoutMinutes">超时时间(分钟)</param>
  /// <returns>清理的记录数</returns>
  Task<int> CleanupTimeoutUsersAsync(int timeoutMinutes = 30);

  #endregion

  /// <summary>
  /// 检查用户是否在线
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>是否在线</returns>
  Task<bool> IsUserOnlineAsync(long userId);

  /// <summary>
  /// 更新所有在线用户的在线时长
  /// </summary>
  Task UpdateAllOnlineTimeAsync();
}