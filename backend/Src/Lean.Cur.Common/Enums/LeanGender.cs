/**
 * @description 性别枚举
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 性别枚举
/// </summary>
public enum LeanGender
{
  /// <summary>
  /// 未知
  /// </summary>
  [Description("未知")]
  Unknown = 0,

  /// <summary>
  /// 男
  /// </summary>
  [Description("男")]
  Male = 1,

  /// <summary>
  /// 女
  /// </summary>
  [Description("女")]
  Female = 2
}