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
/// 业务类型枚举
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
  /// 邮件附件
  /// </summary>
  [Description("邮件附件")]
  EmailAttachment = 6,

  /// <summary>
  /// 用户头像
  /// </summary>
  [Description("用户头像")]
  UserAvatar = 7,

  /// <summary>
  /// 系统文档
  /// </summary>
  [Description("系统文档")]
  SystemDocument = 8,

  /// <summary>
  /// 导入文件
  /// </summary>
  [Description("导入文件")]
  Import = 9,

  /// <summary>
  /// 导出文件
  /// </summary>
  [Description("导出文件")]
  Export = 10,

  /// <summary>
  /// 强制退出
  /// </summary>
  ForceLogout = 11,

  /// <summary>
  /// 生成代码
  /// </summary>
  GenCode = 12,

  /// <summary>
  /// 清空数据
  /// </summary>
  Clean = 13,

  /// <summary>
  /// 登录
  /// </summary>
  Login = 14,

  /// <summary>
  /// 退出
  /// </summary>
  Logout = 15,

  /// <summary>
  /// 注册
  /// </summary>
  Register = 16,

  /// <summary>
  /// 重置密码
  /// </summary>
  ResetPassword = 17,

  /// <summary>
  /// 修改密码
  /// </summary>
  ChangePassword = 18,

  /// <summary>
  /// 临时文件
  /// </summary>
  [Description("临时文件")]
  Temporary = 19
}