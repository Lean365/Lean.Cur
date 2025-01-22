using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Domain.Entities.Admin;

namespace Lean.Cur.Domain.Entities.Routine;

/// <summary>
/// 在线用户实体
/// </summary>
/// <remarks>
/// 记录系统在线用户信息
/// 
/// 数据库映射说明：
/// 1. 表名：lean_online_user
/// 2. 主键：user_id
/// 3. 索引：
///    - IX_ConnectionId (连接ID)
///    - IX_LastActiveTime (最后活动时间)
/// 
/// 业务规则：
/// 1. 用户建立SignalR连接时创建记录
/// 2. 定期更新最后活动时间
/// 3. 连接断开时删除记录
/// 4. 支持按最后活动时间清理过期记录
/// </remarks>
[SugarTable("lean_online_user", "在线用户表")]
public class LeanOnlineUser : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 1. 关联用户表的主键
  /// 2. 必填字段
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsPrimaryKey = true)]
  [Description("用户ID")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 1. 记录当前的用户名称
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
  [Description("用户名")]
  public string UserName { get; set; } = null!;

  /// <summary>
  /// SignalR连接ID
  /// </summary>
  /// <remarks>
  /// 1. 用于标识用户的SignalR连接
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "connection_id", ColumnDescription = "连接ID", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  [Description("连接ID")]
  public string ConnectionId { get; set; } = null!;

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的IP地址
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("IP地址")]
  public string IpAddress { get; set; } = null!;

  /// <summary>
  /// 浏览器信息
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的浏览器信息
  /// 2. 可选字段
  /// 3. 最大长度：200个字符
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器信息", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
  [Description("浏览器信息")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统信息
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的操作系统信息
  /// 2. 可选字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统信息", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
  [Description("操作系统信息")]
  public string? Os { get; set; }

  /// <summary>
  /// 最后活动时间
  /// </summary>
  /// <remarks>
  /// 1. 记录用户最后一次活动的时间
  /// 2. 必填字段
  /// 3. 用于判断用户是否在线
  /// </remarks>
  [SugarColumn(ColumnName = "last_active_time", ColumnDescription = "最后活动时间", ColumnDataType = "datetime", IsNullable = false)]
  [Description("最后活动时间")]
  public DateTime LastActiveTime { get; set; }

  /// <summary>
  /// 当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户当天的总在线时长(所有设备)
  /// 2. 必填字段
  /// 3. 每天零点重置为0
  /// 4. 用于统计用户当天总活跃度
  /// </remarks>
  [SugarColumn(ColumnName = "today_online_time", ColumnDescription = "当天在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("当天在线时长")]
  public int TodayOnlineTime { get; set; }

  /// <summary>
  /// 累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的历史累计总在线时长(所有设备)
  /// 2. 必填字段
  /// 3. 持续累加,不会重置
  /// 4. 用于统计用户总体活跃度
  /// </remarks>
  [SugarColumn(ColumnName = "total_online_time", ColumnDescription = "累计在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("累计在线时长")]
  public int TotalOnlineTime { get; set; }

  /// <summary>
  /// PC端当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户PC端当天的在线时长
  /// 2. 必填字段
  /// 3. 每天零点重置为0
  /// </remarks>
  [SugarColumn(ColumnName = "pc_today_online_time", ColumnDescription = "PC端当天在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("PC端当天在线时长")]
  public int PcTodayOnlineTime { get; set; }

  /// <summary>
  /// PC端累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户PC端的历史累计在线时长
  /// 2. 必填字段
  /// 3. 持续累加,不会重置
  /// </remarks>
  [SugarColumn(ColumnName = "pc_total_online_time", ColumnDescription = "PC端累计在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("PC端累计在线时长")]
  public int PcTotalOnlineTime { get; set; }

  /// <summary>
  /// 移动端当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户移动端当天的在线时长
  /// 2. 必填字段
  /// 3. 每天零点重置为0
  /// </remarks>
  [SugarColumn(ColumnName = "mobile_today_online_time", ColumnDescription = "移动端当天在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("移动端当天在线时长")]
  public int MobileTodayOnlineTime { get; set; }

  /// <summary>
  /// 移动端累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户移动端的历史累计在线时长
  /// 2. 必填字段
  /// 3. 持续累加,不会重置
  /// </remarks>
  [SugarColumn(ColumnName = "mobile_total_online_time", ColumnDescription = "移动端累计在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("移动端累计在线时长")]
  public int MobileTotalOnlineTime { get; set; }

  /// <summary>
  /// 平板端当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户平板端当天的在线时长
  /// 2. 必填字段
  /// 3. 每天零点重置为0
  /// </remarks>
  [SugarColumn(ColumnName = "tablet_today_online_time", ColumnDescription = "平板端当天在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("平板端当天在线时长")]
  public int TabletTodayOnlineTime { get; set; }

  /// <summary>
  /// 平板端累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户平板端的历史累计在线时长
  /// 2. 必填字段
  /// 3. 持续累加,不会重置
  /// </remarks>
  [SugarColumn(ColumnName = "tablet_total_online_time", ColumnDescription = "平板端累计在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("平板端累计在线时长")]
  public int TabletTotalOnlineTime { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的设备类型(PC、Mobile、Tablet等)
  /// 2. 必填字段
  /// 3. 最大长度：50个字符
  /// </remarks>
  [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
  [Description("设备类型")]
  public string DeviceType { get; set; } = null!;

  /// <summary>
  /// 设备名称
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的设备名称
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// </remarks>
  [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
  [Description("设备名称")]
  public string DeviceName { get; set; } = null!;

  /// <summary>
  /// 登录地点
  /// </summary>
  /// <remarks>
  /// 1. 记录用户的登录地理位置
  /// 2. 必填字段
  /// 3. 最大长度：100个字符
  /// 4. 格式：国家-省份-城市
  /// </remarks>
  [SugarColumn(ColumnName = "location", ColumnDescription = "登录地点", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
  [Description("登录地点")]
  public string Location { get; set; } = null!;

  /// <summary>
  /// 其他设备当天在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户其他设备当天的在线时长
  /// 2. 必填字段
  /// 3. 每天零点重置为0
  /// 4. 用于统计未知或其他类型设备的使用时长
  /// </remarks>
  [SugarColumn(ColumnName = "other_today_online_time", ColumnDescription = "其他设备当天在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("其他设备当天在线时长")]
  public int OtherTodayOnlineTime { get; set; }

  /// <summary>
  /// 其他设备累计在线时长(分钟)
  /// </summary>
  /// <remarks>
  /// 1. 记录用户其他设备的历史累计在线时长
  /// 2. 必填字段
  /// 3. 持续累加,不会重置
  /// 4. 用于统计未知或其他类型设备的总使用时长
  /// </remarks>
  [SugarColumn(ColumnName = "other_total_online_time", ColumnDescription = "其他设备累计在线时长", ColumnDataType = "int", IsNullable = false)]
  [Description("其他设备累计在线时长")]
  public int OtherTotalOnlineTime { get; set; }

  /// <summary>
  /// 导航属性：用户信息
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = null!;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanOnlineUser()
  {
    UserName = string.Empty;
    ConnectionId = string.Empty;
    IpAddress = string.Empty;
    DeviceType = string.Empty;
    DeviceName = string.Empty;
    Location = string.Empty;
    LastActiveTime = DateTime.Now;
    TodayOnlineTime = 0;
    TotalOnlineTime = 0;
    PcTodayOnlineTime = 0;
    PcTotalOnlineTime = 0;
    MobileTodayOnlineTime = 0;
    MobileTotalOnlineTime = 0;
    TabletTodayOnlineTime = 0;
    TabletTotalOnlineTime = 0;
    OtherTodayOnlineTime = 0;
    OtherTotalOnlineTime = 0;
  }
}