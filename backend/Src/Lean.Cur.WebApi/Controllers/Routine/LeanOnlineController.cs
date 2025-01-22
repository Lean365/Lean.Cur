// Copyright (c) 2024 Lean.Cur. All rights reserved.
// Licensed under the Lean.Cur license. See LICENSE file in the project root for full license information.

using Lean.Cur.Application.Dtos.Routine;
using Lean.Cur.Application.Services.Routine;
using Lean.Cur.Common.Models;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers.Routine;

/// <summary>
/// 在线用户控制器
/// </summary>
/// <remarks>
/// 创建时间：2024-01-19
/// 修改时间：2024-01-19
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：提供在线用户管理相关的API接口
/// </remarks>
[ApiController]
[Route("api/online")]
[Authorize]
public class LeanOnlineController : LeanBaseController
{
  private readonly ILeanOnlineService _onlineService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanOnlineController(ILeanOnlineService onlineService)
  {
    _onlineService = onlineService;
  }

  /// <summary>
  /// 获取分页列表
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>分页结果</returns>
  [HttpGet("page")]
  [ProducesResponseType(typeof(LeanApiResult<LeanPagedResult<LeanOnlineUserDto>>), 200)]
  public async Task<LeanApiResult<LeanPagedResult<LeanOnlineUserDto>>> GetPagedListAsync([FromQuery] LeanOnlineUserQueryDto queryDto)
  {
    var result = await _onlineService.GetPagedListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取列表
  /// </summary>
  /// <param name="queryDto">查询参数</param>
  /// <returns>在线用户列表</returns>
  [HttpGet("list")]
  [ProducesResponseType(typeof(LeanApiResult<List<LeanOnlineUserDto>>), 200)]
  public async Task<LeanApiResult<List<LeanOnlineUserDto>>> GetListAsync([FromQuery] LeanOnlineUserQueryDto queryDto)
  {
    var result = await _onlineService.GetListAsync(queryDto);
    return Success(result);
  }

  /// <summary>
  /// 获取详情
  /// </summary>
  /// <param name="id">ID</param>
  /// <returns>在线用户详情</returns>
  [HttpGet("{id}")]
  [ProducesResponseType(typeof(LeanApiResult<LeanOnlineUserDto>), 200)]
  public async Task<LeanApiResult<LeanOnlineUserDto>> GetByIdAsync(long id)
  {
    var result = await _onlineService.GetByIdAsync(id);
    return Success(result);
  }

  /// <summary>
  /// 获取在线用户数量
  /// </summary>
  /// <returns>在线用户数量</returns>
  [HttpGet("count")]
  [ProducesResponseType(typeof(LeanApiResult<int>), 200)]
  public async Task<LeanApiResult<int>> GetOnlineCountAsync()
  {
    var result = await _onlineService.GetOnlineCountAsync();
    return Success(result);
  }

  /// <summary>
  /// 获取今日在线时长
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>今日在线时长</returns>
  [HttpGet("today-online-time/{userId}")]
  [ProducesResponseType(typeof(LeanApiResult<Dictionary<string, int>>), 200)]
  public async Task<LeanApiResult<Dictionary<string, int>>> GetTodayOnlineTimeAsync(long userId)
  {
    var result = await _onlineService.GetTodayOnlineTimeAsync(userId);
    return Success(result);
  }

  /// <summary>
  /// 获取总在线时长
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>总在线时长</returns>
  [HttpGet("total-online-time/{userId}")]
  [ProducesResponseType(typeof(LeanApiResult<Dictionary<string, int>>), 200)]
  public async Task<LeanApiResult<Dictionary<string, int>>> GetTotalOnlineTimeAsync(long userId)
  {
    var result = await _onlineService.GetTotalOnlineTimeAsync(userId);
    return Success(result);
  }

  /// <summary>
  /// 清理超时用户
  /// </summary>
  /// <returns>清理数量</returns>
  [HttpPost("cleanup")]
  [ProducesResponseType(typeof(LeanApiResult<int>), 200)]
  public async Task<LeanApiResult<int>> CleanupTimeoutUsersAsync()
  {
    var result = await _onlineService.CleanupTimeoutUsersAsync();
    return Success(result);
  }
}