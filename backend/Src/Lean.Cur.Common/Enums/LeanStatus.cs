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
public enum LeanStatus
{
  /// <summary>
  /// 正常
  /// </summary>
  Normal = 0,

  /// <summary>
  /// 禁用
  /// </summary>
  Disabled = 1,

  /// <summary>
  /// 删除
  /// </summary>
  Deleted = 2
}