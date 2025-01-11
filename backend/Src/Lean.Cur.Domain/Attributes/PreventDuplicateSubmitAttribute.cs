using Microsoft.AspNetCore.Mvc.Filters;

namespace Lean.Cur.Domain.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PreventDuplicateSubmitAttribute : ActionFilterAttribute
{
  private readonly int _interval;

  public PreventDuplicateSubmitAttribute(int interval = 5)
  {
    _interval = interval;
  }

  public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    // TODO: 实现防重复提交逻辑
    await next();
  }
}