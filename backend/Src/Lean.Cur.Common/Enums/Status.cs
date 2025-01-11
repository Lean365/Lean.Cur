/**
 * @description 状态枚举
 * @author CodeGenerator
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 状态枚举
/// </summary>
public enum Status
{
  /// <summary>
  /// 禁用
  /// </summary>
  [Description("禁用")]
  Disabled = 0,

  /// <summary>
  /// 正常
  /// </summary>
  [Description("正常")]
  Normal = 1
}