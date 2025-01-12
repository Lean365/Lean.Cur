using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 用户扩展信息表
/// </summary>
/// <remarks>
/// 记录用户的登录历史、在线状态和相关统计信息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_user_extend
/// 2. 主键：Id (自增长)
/// 3. 外键：UserId (关联用户表)
/// 4. 索引：
///    - IX_UserId (用户ID)
///    - IX_LastLoginTime (末次登录时间)
///    - IX_LastActiveTime (最后活跃时间)
///    - IX_OnlineStatus (在线状态)
/// 
/// 业务规则：
/// 1. 用户首次登录时创建记录
/// 2. 每次登录时更新末次登录信息
/// 3. 登录次数自动累加
/// 4. 登出时更新末次登出时间
/// 5. 定时检查心跳更新在线状态
/// 6. 每次请求更新活跃时间
/// 7. 统计在线时长
/// 8. 记录登录错误信息
/// 9. 支持账户锁定机制
/// </remarks>
[SugarTable("lean_user_extend", "用户扩展信息表")]
public class LeanUserExtend : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// 3. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false)]
  public long UserId { get; set; }

  /// <summary>
  /// 首次登录时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户第一次登录的时间
  /// 2. 可选字段
  /// 3. 一旦设置不再更改
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_time", ColumnDescription = "首次登录时间", IsNullable = true)]
  public DateTime? FirstLoginTime { get; set; }

  /// <summary>
  /// 首次登录IP
  /// </summary>
  /// <remarks>
  /// 1. 记录用户第一次登录的IP地址
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_ip", ColumnDescription = "首次登录IP", Length = 50, IsNullable = true)]
  public string? FirstLoginIp { get; set; }

  /// <summary>
  /// 首次登录地点
  /// </summary>
  /// <remarks>
  /// 1. 记录用户第一次登录的地理位置
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 格式：省份-城市-区县
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_location", ColumnDescription = "首次登录地点", Length = 100, IsNullable = true)]
  public string? FirstLoginLocation { get; set; }

  /// <summary>
  /// 首次登录设备
  /// </summary>
  /// <remarks>
  /// 1. 记录用户第一次登录的设备信息
  /// 2. 可选字段
  /// 3. 最大长度：200个字符
  /// 4. 包含设备类型、型号等信息
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_device", ColumnDescription = "首次登录设备", Length = 200, IsNullable = true)]
  public string? FirstLoginDevice { get; set; }

  /// <summary>
  /// 首次登录浏览器
  /// </summary>
  /// <remarks>
  /// 1. 记录用户第一次登录的浏览器信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 包含浏览器名称和版本
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_browser", ColumnDescription = "首次登录浏览器", Length = 100, IsNullable = true)]
  public string? FirstLoginBrowser { get; set; }

  /// <summary>
  /// 首次登录操作系统
  /// </summary>
  /// <remarks>
  /// 1. 记录用户第一次登录的操作系统信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 包含操作系统名称和版本
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_os", ColumnDescription = "首次登录操作系统", Length = 100, IsNullable = true)]
  public string? FirstLoginOs { get; set; }

  /// <summary>
  /// 末次登录时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录的时间
  /// 2. 可选字段
  /// 3. 每次登录时更新
  /// 4. 建立索引提高查询性能
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_time", ColumnDescription = "末次登录时间", IsNullable = true)]
  public DateTime? LastLoginTime { get; set; }

  /// <summary>
  /// 末次登录IP
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录的IP地址
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_ip", ColumnDescription = "末次登录IP", Length = 50, IsNullable = true)]
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 末次登录地点
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录的地理位置
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 格式：省份-城市-区县
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_location", ColumnDescription = "末次登录地点", Length = 100, IsNullable = true)]
  public string? LastLoginLocation { get; set; }

  /// <summary>
  /// 末次登录设备
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录的设备信息
  /// 2. 可选字段
  /// 3. 最大长度：200个字符
  /// 4. 包含设备类型、型号等信息
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_device", ColumnDescription = "末次登录设备", Length = 200, IsNullable = true)]
  public string? LastLoginDevice { get; set; }

  /// <summary>
  /// 末次登录浏览器
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录的浏览器信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 包含浏览器名称和版本
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_browser", ColumnDescription = "末次登录浏览器", Length = 100, IsNullable = true)]
  public string? LastLoginBrowser { get; set; }

  /// <summary>
  /// 末次登录操作系统
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录的操作系统信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// 4. 包含操作系统名称和版本
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_os", ColumnDescription = "末次登录操作系统", Length = 100, IsNullable = true)]
  public string? LastLoginOs { get; set; }

  /// <summary>
  /// 末次登出时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登出的时间
  /// 2. 可选字段
  /// 3. 用户主动登出或会话过期时更新
  /// </remarks>
  [SugarColumn(ColumnName = "last_logout_time", ColumnDescription = "末次登出时间", IsNullable = true)]
  public DateTime? LastLogoutTime { get; set; }

  /// <summary>
  /// 登录次数
  /// </summary>
  /// <remarks>
  /// 1. 记录用户累计登录次数
  /// 2. 必填字段
  /// 3. 默认值：0
  /// 4. 每次登录时自动加1
  /// </remarks>
  [SugarColumn(ColumnName = "login_count", ColumnDescription = "登录次数", IsNullable = false)]
  public int LoginCount { get; set; }

  /// <summary>
  /// 登录错误次数
  /// </summary>
  /// <remarks>
  /// 1. 记录用户当前登录错误次数
  /// 2. 必填字段
  /// 3. 默认值：0
  /// 4. 登录失败时自动加1
  /// 5. 登录成功时重置为0
  /// </remarks>
  [SugarColumn(ColumnName = "error_count", ColumnDescription = "登录错误次数", IsNullable = false)]
  public int ErrorCount { get; set; }

  /// <summary>
  /// 末次错误时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次登录失败的时间
  /// 2. 可选字段
  /// 3. 登录失败时更新
  /// 4. 用于判断是否需要显示验证码或锁定账户
  /// </remarks>
  [SugarColumn(ColumnName = "last_error_time", ColumnDescription = "末次错误时间", IsNullable = true)]
  public DateTime? LastErrorTime { get; set; }

  /// <summary>
  /// 锁定结束时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户账户锁定的结束时间
  /// 2. 可选字段
  /// 3. 当错误次数超过限制时设置
  /// 4. 为空表示未锁定
  /// </remarks>
  [SugarColumn(ColumnName = "lock_end_time", ColumnDescription = "锁定结束时间", IsNullable = true)]
  public DateTime? LockEndTime { get; set; }

  /// <summary>
  /// 累计错误次数
  /// </summary>
  /// <remarks>
  /// 1. 记录用户累计登录错误次数
  /// 2. 必填字段
  /// 3. 默认值：0
  /// 4. 登录失败时自动加1
  /// 5. 用于统计分析
  /// </remarks>
  [SugarColumn(ColumnName = "total_error_count", ColumnDescription = "累计错误次数", IsNullable = false)]
  public int TotalErrorCount { get; set; }

  /// <summary>
  /// 在线状态（0-离线，1-在线，2-忙碌，3-离开）
  /// </summary>
  /// <remarks>
  /// 1. 记录用户当前在线状态
  /// 2. 必填字段
  /// 3. 默认值：0（离线）
  /// 4. 用于即时通讯和在线状态显示
  /// </remarks>
  [SugarColumn(ColumnName = "online_status", ColumnDescription = "在线状态", IsNullable = false)]
  public int OnlineStatus { get; set; }

  /// <summary>
  /// 最后心跳时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次心跳包的时间
  /// 2. 可选字段
  /// 3. 用于判断用户是否真实在线
  /// 4. 超过指定时间未更新则自动设置为离线
  /// </remarks>
  [SugarColumn(ColumnName = "last_heartbeat_time", ColumnDescription = "最后心跳时间", IsNullable = true)]
  public DateTime? LastHeartbeatTime { get; set; }

  /// <summary>
  /// 当前会话ID
  /// </summary>
  /// <remarks>
  /// 1. 记录用户当前的会话标识
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// 4. 用于判断是否多地登录
  /// </remarks>
  [SugarColumn(ColumnName = "session_id", ColumnDescription = "当前会话ID", Length = 50, IsNullable = true)]
  public string? SessionId { get; set; }

  /// <summary>
  /// 最后活跃时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次操作的时间
  /// 2. 可选字段
  /// 3. 用于统计用户活跃度
  /// 4. 每次请求时更新
  /// </remarks>
  [SugarColumn(ColumnName = "last_active_time", ColumnDescription = "最后活跃时间", IsNullable = true)]
  public DateTime? LastActiveTime { get; set; }

  /// <summary>
  /// 最后活跃IP
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次操作的IP地址
  /// 2. 可选字段
  /// 3. 最大长度：50个字符
  /// 4. 用于安全审计
  /// </remarks>
  [SugarColumn(ColumnName = "last_active_ip", ColumnDescription = "最后活跃IP", Length = 50, IsNullable = true)]
  public string? LastActiveIp { get; set; }

  /// <summary>
  /// 累计在线时长（分钟）
  /// </summary>
  /// <remarks>
  /// 1. 记录用户累计在线时长
  /// 2. 必填字段
  /// 3. 默认值：0
  /// 4. 用于统计用户使用情况
  /// </remarks>
  [SugarColumn(ColumnName = "total_online_time", ColumnDescription = "累计在线时长", IsNullable = false)]
  public int TotalOnlineTime { get; set; }
}