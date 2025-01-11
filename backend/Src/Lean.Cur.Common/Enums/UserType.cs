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
  /// 普通用户
  /// </summary>
  [Description("普通用户")]
  User = 2,

  /// <summary>
  /// 访客
  /// </summary>
  [Description("访客")]
  Guest = 3,

  /// <summary>
  /// 系统用户
  /// </summary>
  [Description("系统用户")]
  System = 4,

  /// <summary>
  /// 测试用户
  /// </summary>
  [Description("测试用户")]
  Test = 5,

  /// <summary>
  /// VIP用户
  /// </summary>
  [Description("VIP用户")]
  VIP = 6,

  /// <summary>
  /// 企业用户
  /// </summary>
  [Description("企业用户")]
  Enterprise = 7,

  /// <summary>
  /// 开发者
  /// </summary>
  [Description("开发者")]
  Developer = 8,

  /// <summary>
  /// 运维人员
  /// </summary>
  [Description("运维人员")]
  Operator = 9,

  /// <summary>
  /// 审计人员
  /// </summary>
  [Description("审计人员")]
  Auditor = 10
}