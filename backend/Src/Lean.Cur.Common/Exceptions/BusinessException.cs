using Lean.Cur.Common.Enums;

namespace Lean.Cur.Common.Exceptions;

/// <summary>
/// 业务异常
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public LeanErrorCode ErrorCode { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="errorCode">错误代码</param>
    public BusinessException(string message, LeanErrorCode errorCode = LeanErrorCode.ValidationFailed) : base(message)
    {
        ErrorCode = errorCode;
    }
} 