/**
 * @description 用户类型枚举
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.ComponentModel;

namespace Lean.Cur.Domain.Enums;

/// <summary>
/// 用户类型枚举
/// </summary>
public enum UserType
{
  /// <summary>
  /// 超级用户
  /// </summary>
  [Description("超级用户")]
  SuperAdmin = 0,

  /// <summary>
  /// 管理员
  /// </summary>
  [Description("管理员")]
  Admin = 1,

  /// <summary>
  /// 注册用户
  /// </summary>
  [Description("注册用户")]
  User = 2
}