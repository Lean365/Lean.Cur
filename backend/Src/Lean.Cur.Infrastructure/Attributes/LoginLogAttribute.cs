using Microsoft.AspNetCore.Mvc.Filters;
using Lean.Cur.Application.Services.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Lean.Cur.Infrastructure.Attributes;

/// <summary>
/// 登录日志记录特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LoginLogAttribute : ActionFilterAttribute
{
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
        // 创建新的作用域来解析作用域服务
        using var scope = context.HttpContext.RequestServices.CreateScope();
        var loginLogService = scope.ServiceProvider.GetRequiredService<ILoginLogService>();

        // 记录登录日志
        await loginLogService.AddLoginLogAsync(userId, context.HttpContext);
      }
    }
    catch
    {
      // 记录日志失败不影响主流程
    }
  }
}