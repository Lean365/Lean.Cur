using Lean.Cur.Common.Models;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Lean.Cur.WebApi.Controllers;

/// <summary>
/// 控制器基类
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class LeanBaseController : ControllerBase
{
  /// <summary>
  /// 返回成功响应
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="data">数据</param>
  /// <param name="message">消息</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<T> Success<T>(T data, string message = "操作成功")
  {
    return new LeanApiResult<T>
    {
      Code = LeanErrorCode.Success,
      Message = message,
      Data = data,
      Path = Request.Path,
      Method = Request.Method
    };
  }

  /// <summary>
  /// 返回成功响应（无数据类型）
  /// </summary>
  /// <param name="message">成功消息</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<object> Success(string message = "操作成功")
  {
    return new LeanApiResult<object>
    {
      Code = LeanErrorCode.Success,
      Message = message,
      Data = new { },
      Path = Request.Path,
      Method = Request.Method
    };
  }

  /// <summary>
  /// 返回错误响应
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="message">错误消息</param>
  /// <param name="data">数据</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<T> Error<T>(string message, T data = default)
  {
    return new LeanApiResult<T>
    {
      Code = LeanErrorCode.BadRequest,
      Message = message,
      Data = data,
      Path = Request.Path,
      Method = Request.Method
    };
  }

  /// <summary>
  /// 返回错误响应（无数据类型）
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<object> Error(string message)
  {
    return Error<object>(message);
  }

  /// <summary>
  /// 返回参数验证错误响应
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="message">错误消息</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<T> ValidateError<T>(string message = "参数验证失败")
  {
    return new LeanApiResult<T>
    {
      Code = LeanErrorCode.BadRequest,
      Message = message,
      Path = Request.Path,
      Method = Request.Method
    };
  }

  /// <summary>
  /// 返回未授权错误响应
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="message">错误消息</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<T> Unauthorized<T>(string message = "未授权的访问")
  {
    return new LeanApiResult<T>
    {
      Code = LeanErrorCode.Unauthorized,
      Message = message,
      Path = Request.Path,
      Method = Request.Method
    };
  }

  /// <summary>
  /// 返回业务错误响应
  /// </summary>
  /// <typeparam name="T">数据类型</typeparam>
  /// <param name="message">错误消息</param>
  /// <returns>API响应</returns>
  protected LeanApiResult<T> BusinessError<T>(string message)
  {
    return new LeanApiResult<T>
    {
      Code = LeanErrorCode.BadRequest,
      Message = message,
      Path = Request.Path,
      Method = Request.Method
    };
  }

  /// <summary>
  /// 返回文件响应
  /// </summary>
  /// <param name="fileBytes">文件字节数组</param>
  /// <param name="fileName">文件名</param>
  /// <param name="contentType">内容类型</param>
  /// <returns>文件响应</returns>
  protected IActionResult FileResponse(byte[] fileBytes, string fileName, string contentType = "application/octet-stream")
  {
    return File(fileBytes, contentType, fileName);
  }

  /// <summary>
  /// 返回Excel文件响应
  /// </summary>
  /// <param name="fileBytes">文件字节数组</param>
  /// <param name="fileName">文件名</param>
  /// <returns>Excel文件响应</returns>
  protected IActionResult ExcelResponse(byte[] fileBytes, string fileName)
  {
    return FileResponse(fileBytes, fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
  }
}