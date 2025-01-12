using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lean.Cur.WebApi.Filters;

/// <summary>
/// API结果过滤器
/// </summary>
public class ApiResultFilter : IResultFilter
{
  /// <summary>
  /// 结果执行中
  /// </summary>
  /// <param name="context">结果执行上下文</param>
  public void OnResultExecuting(ResultExecutingContext context)
  {
    if (context.Result is ObjectResult objectResult)
    {
      var response = new
      {
        code = 200,
        message = "success",
        data = objectResult.Value
      };

      context.Result = new ObjectResult(response);
    }
  }

  /// <summary>
  /// 结果执行后
  /// </summary>
  /// <param name="context">结果执行上下文</param>
  public void OnResultExecuted(ResultExecutedContext context)
  {
  }
}