/**
 * @description 数据范围过滤器
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Api.Filters;

/// <summary>
/// 数据范围特性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class LeanDataScopeAttribute : Attribute
{
  /// <summary>
  /// 数据范围类型
  /// </summary>
  public DataScopeType ScopeType { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDataScopeAttribute(DataScopeType scopeType = DataScopeType.All)
  {
    ScopeType = scopeType;
  }
}

/// <summary>
/// 数据范围类型
/// </summary>
public enum DataScopeType
{
  /// <summary>
  /// 全部数据
  /// </summary>
  All = 1,

  /// <summary>
  /// 本部门及以下数据
  /// </summary>
  DeptAndChild = 2,

  /// <summary>
  /// 本部门数据
  /// </summary>
  Dept = 3,

  /// <summary>
  /// 仅本人数据
  /// </summary>
  Self = 4,

  /// <summary>
  /// 自定义数据
  /// </summary>
  Custom = 5
}

/// <summary>
/// 数据范围过滤器
/// </summary>
public class LeanDataScopeFilter : IAsyncActionFilter
{
  private readonly ILogger<LeanDataScopeFilter> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanDataScopeFilter(ILogger<LeanDataScopeFilter> logger, IHttpContextAccessor httpContextAccessor)
  {
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 数据范围过滤
  /// </summary>
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    try
    {
      // 获取数据范围特性
      var dataScopeAttribute = context.ActionDescriptor.EndpointMetadata
          .OfType<LeanDataScopeAttribute>()
          .FirstOrDefault();

      if (dataScopeAttribute == null)
      {
        await next();
        return;
      }

      // 获取当前用户信息
      var userType = GetCurrentUserType();

      // 超级管理员可以访问所有数据
      if (userType == UserType.SuperAdmin)
      {
        await next();
        return;
      }

      // 根据数据范围类型构建过滤条件
      var filterCondition = await BuildFilterCondition(dataScopeAttribute.ScopeType);

      // 将过滤条件添加到请求参数中
      foreach (var param in context.ActionArguments)
      {
        if (param.Value is ILeanDataScopeFilter filter)
        {
          filter.DataScope = filterCondition;
        }
      }

      await next();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "数据范围过滤失败");
      context.Result = new StatusCodeResult(500);
    }
  }

  private UserType GetCurrentUserType()
  {
    // TODO: 从Token或Session中获取用户类型
    return UserType.User;
  }

  private Task<string> BuildFilterCondition(DataScopeType scopeType)
  {
    // TODO: 根据数据范围类型构建SQL过滤条件
    return Task.FromResult(string.Empty);
  }
}

/// <summary>
/// 数据范围过滤接口
/// </summary>
public interface ILeanDataScopeFilter
{
  /// <summary>
  /// 数据范围过滤条件
  /// </summary>
  string DataScope { get; set; }
}