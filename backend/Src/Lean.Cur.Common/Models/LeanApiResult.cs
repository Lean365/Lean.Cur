using System.Text.Json.Serialization;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Common.Models;

/// <summary>
/// API响应模型
/// </summary>
public class LeanApiResult
{
  /// <summary>
  /// 错误代码
  /// </summary>
  [JsonPropertyName("code")]
  public LeanErrorCode Code { get; set; } = LeanErrorCode.Success;

  /// <summary>
  /// 错误消息
  /// </summary>
  [JsonPropertyName("message")]
  public string Message { get; set; } = string.Empty;

  /// <summary>
  /// 业务类型
  /// </summary>
  [JsonPropertyName("businessType")]
  public LeanBusinessType BusinessType { get; set; } = LeanBusinessType.Other;

  /// <summary>
  /// 请求路径
  /// </summary>
  [JsonPropertyName("path")]
  public string Path { get; set; } = string.Empty;

  /// <summary>
  /// 请求方法
  /// </summary>
  [JsonPropertyName("method")]
  public string Method { get; set; } = string.Empty;

  /// <summary>
  /// 请求参数
  /// </summary>
  [JsonPropertyName("params")]
  public string? Params { get; set; }

  /// <summary>
  /// 执行时间(毫秒)
  /// </summary>
  [JsonPropertyName("duration")]
  public long Duration { get; set; }

  /// <summary>
  /// 请求时间
  /// </summary>
  [JsonPropertyName("timestamp")]
  public DateTime Timestamp { get; set; } = DateTime.Now;

  /// <summary>
  /// 请求ID
  /// </summary>
  [JsonPropertyName("requestId")]
  public string RequestId { get; set; } = Guid.NewGuid().ToString("N");

  /// <summary>
  /// 服务器IP
  /// </summary>
  [JsonPropertyName("serverIp")]
  public string ServerIp { get; set; } = string.Empty;

  /// <summary>
  /// 客户端IP
  /// </summary>
  [JsonPropertyName("clientIp")]
  public string ClientIp { get; set; } = string.Empty;

  /// <summary>
  /// 用户代理
  /// </summary>
  [JsonPropertyName("userAgent")]
  public string UserAgent { get; set; } = string.Empty;

  /// <summary>
  /// 操作人ID
  /// </summary>
  [JsonPropertyName("operatorId")]
  public long? OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  [JsonPropertyName("operatorName")]
  public string? OperatorName { get; set; }

  /// <summary>
  /// 堆栈跟踪
  /// </summary>
  [JsonPropertyName("stackTrace")]
  public string? StackTrace { get; set; }
}

/// <summary>
/// API响应模型（泛型）
/// </summary>
public class LeanApiResult<T> : LeanApiResult
{
  /// <summary>
  /// 响应数据
  /// </summary>
  [JsonPropertyName("data")]
  public T? Data { get; set; }

  /// <summary>
  /// 创建成功响应
  /// </summary>
  public static LeanApiResult<T> Success(T data)
  {
    return new LeanApiResult<T>
    {
      Code = LeanErrorCode.Success,
      Message = "操作成功",
      Data = data
    };
  }

  /// <summary>
  /// 创建失败响应
  /// </summary>
  public static LeanApiResult<T> Error(LeanErrorCode code, string message)
  {
    return new LeanApiResult<T>
    {
      Code = code,
      Message = message
    };
  }
}