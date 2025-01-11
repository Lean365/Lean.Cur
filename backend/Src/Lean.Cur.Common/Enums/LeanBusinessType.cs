/**
 * @description 业务类型枚举
 * @author AI
 * @date 2024-01-17
 * @version 1.0.0
 * @copyright © 2024 Lean. All rights reserved
 */

using System.ComponentModel;

namespace Lean.Cur.Common.Enums;

/// <summary>
/// 业务操作类型
/// </summary>
public enum LeanBusinessType
{
  /// <summary>
  /// 其他
  /// </summary>
  Other = 0,

  /// <summary>
  /// 查询
  /// </summary>
  Query = 1,

  /// <summary>
  /// 新增
  /// </summary>
  Insert = 2,

  /// <summary>
  /// 修改
  /// </summary>
  Update = 3,

  /// <summary>
  /// 删除
  /// </summary>
  Delete = 4,

  /// <summary>
  /// 授权
  /// </summary>
  Grant = 5,

  /// <summary>
  /// 导出
  /// </summary>
  Export = 6,

  /// <summary>
  /// 导入
  /// </summary>
  Import = 7,

  /// <summary>
  /// 强制退出
  /// </summary>
  ForceLogout = 8,

  /// <summary>
  /// 生成代码
  /// </summary>
  GenCode = 9,

  /// <summary>
  /// 清空数据
  /// </summary>
  Clean = 10,

  /// <summary>
  /// 登录
  /// </summary>
  Login = 11,

  /// <summary>
  /// 退出
  /// </summary>
  Logout = 12,

  /// <summary>
  /// 注册
  /// </summary>
  Register = 13,

  /// <summary>
  /// 重置密码
  /// </summary>
  ResetPassword = 14,

  /// <summary>
  /// 修改密码
  /// </summary>
  ChangePassword = 15
}