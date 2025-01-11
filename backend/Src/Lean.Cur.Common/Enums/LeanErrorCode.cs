/**
 * @description 错误代码枚举
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 错误代码枚举
/// </summary>
public enum LeanErrorCode
{
  /// <summary>
  /// 成功
  /// </summary>
  [Description("成功")]
  Success = 200,

  /// <summary>
  /// 参数错误
  /// </summary>
  [Description("参数错误")]
  BadRequest = 400,

  /// <summary>
  /// 未授权
  /// </summary>
  [Description("未授权")]
  Unauthorized = 401,

  /// <summary>
  /// 禁止访问
  /// </summary>
  [Description("禁止访问")]
  Forbidden = 403,

  /// <summary>
  /// 资源不存在
  /// </summary>
  [Description("资源不存在")]
  NotFound = 404,

  /// <summary>
  /// 请求方法不允许
  /// </summary>
  [Description("请求方法不允许")]
  MethodNotAllowed = 405,

  /// <summary>
  /// 请求超时
  /// </summary>
  [Description("请求超时")]
  RequestTimeout = 408,

  /// <summary>
  /// 请求频率超限
  /// </summary>
  [Description("请求频率超限")]
  TooManyRequests = 429,

  /// <summary>
  /// 服务器错误
  /// </summary>
  [Description("服务器错误")]
  ServerError = 500,

  /// <summary>
  /// 服务不可用
  /// </summary>
  [Description("服务不可用")]
  ServiceUnavailable = 503,

  /// <summary>
  /// 网关超时
  /// </summary>
  [Description("网关超时")]
  GatewayTimeout = 504,

  /// <summary>
  /// 用户名或密码错误
  /// </summary>
  [Description("用户名或密码错误")]
  InvalidCredentials = 1001,

  /// <summary>
  /// 账号已被禁用
  /// </summary>
  [Description("账号已被禁用")]
  AccountDisabled = 1002,

  /// <summary>
  /// 账号已过期
  /// </summary>
  [Description("账号已过期")]
  AccountExpired = 1003,

  /// <summary>
  /// 无效的刷新令牌
  /// </summary>
  [Description("无效的刷新令牌")]
  InvalidRefreshToken = 1004,

  /// <summary>
  /// 无效的访问令牌
  /// </summary>
  [Description("无效的访问令牌")]
  InvalidAccessToken = 1005,

  /// <summary>
  /// 权限不足
  /// </summary>
  [Description("权限不足")]
  InsufficientPermissions = 1006,

  /// <summary>
  /// 数据不存在
  /// </summary>
  [Description("数据不存在")]
  DataNotFound = 2001,

  /// <summary>
  /// 数据已存在
  /// </summary>
  [Description("数据已存在")]
  DataAlreadyExists = 2002,

  /// <summary>
  /// 数据验证失败
  /// </summary>
  [Description("数据验证失败")]
  ValidationFailed = 2003,

  /// <summary>
  /// 数据关联错误
  /// </summary>
  [Description("数据关联错误")]
  DataRelationError = 2004,

  /// <summary>
  /// 数据状态错误
  /// </summary>
  [Description("数据状态错误")]
  DataStatusError = 2005,

  /// <summary>
  /// 文件上传失败
  /// </summary>
  [Description("文件上传失败")]
  FileUploadFailed = 3001,

  /// <summary>
  /// 文件下载失败
  /// </summary>
  [Description("文件下载失败")]
  FileDownloadFailed = 3002,

  /// <summary>
  /// 文件格式错误
  /// </summary>
  [Description("文件格式错误")]
  InvalidFileFormat = 3003,

  /// <summary>
  /// 文件大小超限
  /// </summary>
  [Description("文件大小超限")]
  FileSizeExceeded = 3004,

  /// <summary>
  /// 系统配置错误
  /// </summary>
  [Description("系统配置错误")]
  SystemConfigError = 4001,

  /// <summary>
  /// 系统维护中
  /// </summary>
  [Description("系统维护中")]
  SystemMaintenance = 4002,

  /// <summary>
  /// 系统资源不足
  /// </summary>
  [Description("系统资源不足")]
  InsufficientResources = 4003,

  /// <summary>
  /// 第三方服务调用失败
  /// </summary>
  [Description("第三方服务调用失败")]
  ThirdPartyServiceError = 5001,

  /// <summary>
  /// 网络连接失败
  /// </summary>
  [Description("网络连接失败")]
  NetworkError = 5002,

  /// <summary>
  /// 数据库操作失败
  /// </summary>
  [Description("数据库操作失败")]
  DatabaseError = 5003,

  /// <summary>
  /// 缓存操作失败
  /// </summary>
  [Description("缓存操作失败")]
  CacheError = 5004,

  /// <summary>
  /// 消息队列操作失败
  /// </summary>
  [Description("消息队列操作失败")]
  MessageQueueError = 5005
}