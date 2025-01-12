using Newtonsoft.Json;

namespace Lean.Cur.Common.Exceptions;

/// <summary>
/// 基础异常类
/// </summary>
[JsonObject]
public class LeanException : Exception
{
  /// <summary>
  /// 错误码
  /// </summary>
  [JsonProperty]
  public int Code { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="code">错误码</param>
  public LeanException(string message, int code = 400) : base(message)
  {
    Code = code;
  }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="innerException">内部异常</param>
  /// <param name="code">错误码</param>
  public LeanException(string message, Exception innerException, int code = 400) : base(message, innerException)
  {
    Code = code;
  }

  /// <summary>
  /// 转换为JSON字符串
  /// </summary>
  public override string ToString()
  {
    return JsonConvert.SerializeObject(new
    {
      Code,
      Message = Message,
      StackTrace = StackTrace
    });
  }
}

/// <summary>
/// 用户友好异常类
/// </summary>
[JsonObject]
public class LeanUserFriendlyException : LeanException
{
  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanUserFriendlyException() : base("未知错误")
  {
  }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="message">错误消息</param>
  public LeanUserFriendlyException(string message) : base(message)
  {
  }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="code">错误码</param>
  public LeanUserFriendlyException(string message, int code) : base(message, code)
  {
  }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="innerException">内部异常</param>
  public LeanUserFriendlyException(string message, Exception innerException) : base(message, innerException)
  {
  }
}