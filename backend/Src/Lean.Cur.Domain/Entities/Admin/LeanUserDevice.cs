using SqlSugar;
using System.ComponentModel;

namespace Lean.Cur.Domain.Entities.Admin
{
  /// <summary>
  /// 用户设备信息表
  /// </summary>
  /// <remarks>
  /// 记录用户的设备登录信息和状态
  /// 
  /// 数据库映射说明：
  /// 1. 表名：lean_user_device
  /// 2. 主键：Id (自增长)
  /// 3. 联合唯一索引：user_id, device_id (用户ID, 设备标识)
  /// 4. 索引：
  ///    - IX_UserId (用户ID)
  ///    - IX_LastLoginTime (最后登录时间)
  ///    - IX_OnlineStatus (在线状态)
  /// 
  /// 业务规则：
  /// 1. 每个用户最多允许同时在线设备数由配置决定
  /// 2. 新设备登录时，如果超过限制，强制下线最早登录的设备
  /// 3. 设备信息包括操作系统、浏览器等详细信息
  /// 4. 记录设备的地理位置信息用于安全审计
  /// 5. 支持设备登录状态的实时监控
  /// 6. Token过期后自动设置设备为离线状态
  /// </remarks>
  [SugarTable("lean_user_device", "用户设备信息表")]
  public class LeanUserDevice : LeanBaseEntity
  {
    /// <summary>
    /// 用户ID
    /// </summary>
    /// <remarks>
    /// 1. 关联用户表的主键
    /// 2. 必填字段
    /// 3. 建立索引提高查询性能
    /// </remarks>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [Description("用户ID")]
    public long UserId { get; set; }

    /// <summary>
    /// 设备标识
    /// </summary>
    /// <remarks>
    /// 1. 设备的唯一标识符
    /// 2. 必填字段，最大长度100
    /// 3. 可以是硬件标识或浏览器指纹
    /// </remarks>
    [SugarColumn(ColumnName = "device_id", ColumnDescription = "设备标识", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    [Description("设备标识")]
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// 设备类型
    /// </summary>
    /// <remarks>
    /// 1. 设备的类型，如PC、Mobile、Tablet等
    /// 2. 必填字段，最大长度50
    /// </remarks>
    [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    [Description("设备类型")]
    public string DeviceType { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    /// <remarks>
    /// 1. 设备的友好名称
    /// 2. 必填字段，最大长度100
    /// 3. 可以是用户自定义的名称
    /// </remarks>
    [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    [Description("设备名称")]
    public string DeviceName { get; set; } = string.Empty;

    /// <summary>
    /// 操作系统
    /// </summary>
    /// <remarks>
    /// 1. 设备的操作系统信息
    /// 2. 必填字段，最大长度50
    /// 3. 包含系统名称和版本号
    /// </remarks>
    [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    [Description("操作系统")]
    public string OS { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器
    /// </summary>
    /// <remarks>
    /// 1. 浏览器信息
    /// 2. 必填字段，最大长度50
    /// 3. 包含浏览器名称和版本号
    /// </remarks>
    [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    [Description("浏览器")]
    public string Browser { get; set; } = string.Empty;

    /// <summary>
    /// IP地址
    /// </summary>
    /// <remarks>
    /// 1. 设备的IP地址
    /// 2. 必填字段，最大长度50
    /// 3. 支持IPv4和IPv6
    /// </remarks>
    [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    [Description("IP地址")]
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 地理位置
    /// </summary>
    /// <remarks>
    /// 1. 设备的地理位置信息
    /// 2. 可选字段，最大长度200
    /// 3. 格式：国家-省份-城市-区县
    /// </remarks>
    [SugarColumn(ColumnName = "location", ColumnDescription = "地理位置", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    [Description("地理位置")]
    public string? Location { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    /// <remarks>
    /// 1. 设备最后一次登录的时间
    /// 2. 必填字段
    /// 3. 每次登录时更新
    /// </remarks>
    [SugarColumn(ColumnName = "last_login_time", ColumnDescription = "最后登录时间", ColumnDataType = "datetime", IsNullable = false)]
    [Description("最后登录时间")]
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    /// <remarks>
    /// 1. 设备最后一次活动的时间
    /// 2. 必填字段
    /// 3. 用于判断设备是否在线
    /// </remarks>
    [SugarColumn(ColumnName = "last_active_time", ColumnDescription = "最后活动时间", ColumnDataType = "datetime", IsNullable = false)]
    [Description("最后活动时间")]
    public DateTime LastActiveTime { get; set; }

    /// <summary>
    /// 在线状态（0-离线，1-在线）
    /// </summary>
    /// <remarks>
    /// 1. 设备的当前在线状态
    /// 2. 必填字段
    /// 3. 默认为离线(0)
    /// </remarks>
    [SugarColumn(ColumnName = "online_status", ColumnDescription = "在线状态", ColumnDataType = "int", IsNullable = false)]
    [Description("在线状态")]
    public int OnlineStatus { get; set; }

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <remarks>
    /// 1. 用于刷新访问令牌的Token
    /// 2. 必填字段，最大长度100
    /// 3. 用于自动续期会话
    /// </remarks>
    [SugarColumn(ColumnName = "refresh_token", ColumnDescription = "刷新Token", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    [Description("刷新Token")]
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Token过期时间
    /// </summary>
    /// <remarks>
    /// 1. 访问令牌的过期时间
    /// 2. 必填字段
    /// 3. 用于判断是否需要刷新Token
    /// </remarks>
    [SugarColumn(ColumnName = "token_expires", ColumnDescription = "Token过期时间", ColumnDataType = "datetime", IsNullable = false)]
    [Description("Token过期时间")]
    public DateTime TokenExpires { get; set; }

    /// <summary>
    /// 是否锁定（0-未锁定，1-已锁定）
    /// </summary>
    /// <remarks>
    /// 1. 设备的锁定状态
    /// 2. 必填字段
    /// 3. 默认为未锁定(0)
    /// 4. 锁定后需要管理员手动解锁
    /// </remarks>
    [SugarColumn(ColumnName = "is_locked", ColumnDescription = "是否锁定", ColumnDataType = "int", IsNullable = false)]
    [Description("是否锁定")]
    public int IsLocked { get; set; }

    /// <summary>
    /// 锁定时间
    /// </summary>
    /// <remarks>
    /// 1. 设备被锁定的时间
    /// 2. 可选字段
    /// 3. 用于审计和统计
    /// </remarks>
    [SugarColumn(ColumnName = "lock_time", ColumnDescription = "锁定时间", ColumnDataType = "datetime", IsNullable = true)]
    [Description("锁定时间")]
    public DateTime? LockTime { get; set; }

    /// <summary>
    /// 锁定操作人ID
    /// </summary>
    /// <remarks>
    /// 1. 执行锁定操作的管理员ID
    /// 2. 可选字段
    /// 3. 0表示系统自动锁定
    /// </remarks>
    [SugarColumn(ColumnName = "lock_by", ColumnDescription = "锁定操作人ID", ColumnDataType = "bigint", IsNullable = true)]
    [Description("锁定操作人ID")]
    public long? LockBy { get; set; }

    /// <summary>
    /// 锁定原因
    /// </summary>
    /// <remarks>
    /// 1. 记录设备被锁定的具体原因
    /// 2. 可选字段，最大长度500
    /// 3. 包含详细的锁定说明
    /// </remarks>
    [SugarColumn(ColumnName = "lock_reason", ColumnDescription = "锁定原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    [Description("锁定原因")]
    public string? LockReason { get; set; }

    /// <summary>
    /// 解锁时间
    /// </summary>
    /// <remarks>
    /// 1. 设备被解锁的时间
    /// 2. 可选字段
    /// 3. 用于审计和统计
    /// </remarks>
    [SugarColumn(ColumnName = "unlock_time", ColumnDescription = "解锁时间", ColumnDataType = "datetime", IsNullable = true)]
    [Description("解锁时间")]
    public DateTime? UnlockTime { get; set; }

    /// <summary>
    /// 解锁操作人ID
    /// </summary>
    /// <remarks>
    /// 1. 执行解锁操作的管理员ID
    /// 2. 可选字段
    /// </remarks>
    [SugarColumn(ColumnName = "unlock_by", ColumnDescription = "解锁操作人ID", ColumnDataType = "bigint", IsNullable = true)]
    [Description("解锁操作人ID")]
    public long? UnlockBy { get; set; }

    /// <summary>
    /// 解锁原因
    /// </summary>
    /// <remarks>
    /// 1. 记录设备被解锁的具体原因
    /// 2. 可选字段，最大长度500
    /// 3. 包含详细的解锁说明
    /// </remarks>
    [SugarColumn(ColumnName = "unlock_reason", ColumnDescription = "解锁原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    [Description("解锁原因")]
    public string? UnlockReason { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanUserDevice()
    {
      DeviceId = string.Empty;
      DeviceType = string.Empty;
      DeviceName = string.Empty;
      OS = string.Empty;
      Browser = string.Empty;
      IpAddress = string.Empty;
      RefreshToken = string.Empty;
      OnlineStatus = 0;
      IsLocked = 0;
      LastLoginTime = DateTime.Now;
      LastActiveTime = DateTime.Now;
      TokenExpires = DateTime.Now;
      IsDeleted = 0;
    }
  }
}