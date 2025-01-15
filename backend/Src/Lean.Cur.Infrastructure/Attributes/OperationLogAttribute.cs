using Microsoft.AspNetCore.Mvc.Filters;
using Lean.Cur.Application.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Diagnostics;

namespace Lean.Cur.Infrastructure.Attributes;

/// <summary>
/// 操作日志记录特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class OperationLogAttribute : ActionFilterAttribute
{
  private readonly string _moduleName;
  private readonly string _operationType;
  private readonly string _description;
  private readonly Stopwatch _stopwatch;

  public OperationLogAttribute(string moduleName, string operationType, string description)
  {
    _moduleName = moduleName;
    _operationType = operationType;
    _description = description;
    _stopwatch = new Stopwatch();
  }

  /// <summary>
  /// 在动作执行前
  /// </summary>
  public override void OnActionExecuting(ActionExecutingContext context)
  {
    _stopwatch.Start();
    base.OnActionExecuting(context);
  }

  /// <summary>
  /// 在动作执行后异步处理
  /// </summary>
  public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    var executedContext = await next();

    try
    {
      // 获取用户ID
      var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
      if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
      {
        _stopwatch.Stop();

        // 创建新的作用域来解析作用域服务
        using var scope = context.HttpContext.RequestServices.CreateScope();
        var operationLogService = scope.ServiceProvider.GetRequiredService<IOperationLogService>();

        // 记录操作日志
        await operationLogService.AddOperationLogAsync(userId, _moduleName, _operationType, _description, context.HttpContext);
      }
    }
    catch
    {
      // 记录日志失败不影响主流程
    }
  }
}