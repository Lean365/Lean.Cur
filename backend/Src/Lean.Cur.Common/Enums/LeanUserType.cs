/**
 * @description 用户类型枚举
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 用户类型枚举
/// </summary>
public enum LeanUserType
{
  /// <summary>
  /// 系统管理员
  /// </summary>
  [Description("系统管理员")]
  Admin = 0,

  /// <summary>
  /// 普通用户
  /// </summary>
  [Description("普通用户")]
  User = 1,

  /// <summary>
  /// 访客用户
  /// </summary>
  [Description("访客用户")]
  Guest = 2
}