namespace Lean.Cur.Common.Enums;

/// <summary>
/// 权限审计类型
/// </summary>
public enum LeanPermissionAuditType
{
  /// <summary>
  /// 登录
  /// </summary>
  Login = 0,

  /// <summary>
  /// 登出
  /// </summary>
  Logout = 1,

  /// <summary>
  /// 访问
  /// </summary>
  Access = 2,

  /// <summary>
  /// 操作
  /// </summary>
  Operation = 3,

  /// <summary>
  /// 异常
  /// </summary>
  Exception = 4
}