using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 菜单实体类
/// </summary>
/// <remarks>
/// 菜单管理模块的核心实体，包含菜单的基本信息和权限关联
/// 
/// 数据库映射说明：
/// 1. 表名：lean_menu
/// 2. 主键：id (自增长)
/// 3. 唯一索引：menu_code (菜单编码)
/// 4. 索引：parent_id (父级ID)
/// 
/// 业务规则：
/// 1. 菜单编码全局唯一，不区分大小写
/// 2. 菜单名称在同一父级下唯一
/// 3. 所有字段都必填（备注除外）
/// 4. 状态默认为启用
/// 5. 排序值越小越靠前
/// 6. 支持无限级菜单
/// 7. 菜单类型包括：目录、菜单、按钮
/// </remarks>
/// <author>CodeGenerator</author>
/// <date>2024-01-17</date>
/// <version>1.0.0</version>
/// <copyright>© 2024 Lean. All rights reserved</copyright>
[SugarTable("lean_menu", "菜单表")]
public class LeanMenu : LeanBaseEntity
{
    /// <summary>
    /// 父级ID
    /// </summary>
    /// <remarks>
    /// 1. 顶级菜单的父级ID为0
    /// 2. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", ColumnDataType = "bigint", IsNullable = false)]
    [Description("父级ID")]
    public long ParentId { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：2-50个字符
    /// 2. 允许中文、字母、数字
    /// 3. 同一父级下唯一
    /// 4. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "menu_name", ColumnDescription = "菜单名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    [Description("菜单名称")]
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：2-50个字符
    /// 2. 只能包含字母、数字、下划线
    /// 3. 全局唯一，不区分大小写
    /// 4. 创建后不允许修改
    /// 5. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "menu_code", ColumnDescription = "菜单编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    [Description("菜单编码")]
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 默认值：目录
    /// </remarks>
    [SugarColumn(ColumnName = "menu_type", ColumnDescription = "菜单类型", ColumnDataType = "int", IsNullable = false)]
    public LeanMenuType MenuType { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：最大200个字符
    /// 2. 以/开头
    /// 3. 菜单类型为菜单时必填
    /// </remarks>
    [SugarColumn(ColumnName = "path", ColumnDescription = "路由地址", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
    [Description("路由地址")]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：最大200个字符
    /// 2. 菜单类型为菜单时必填
    /// </remarks>
    [SugarColumn(ColumnName = "component", ColumnDescription = "组件路径", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
    [Description("组件路径")]
    public string? Component { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：最大100个字符
    /// 2. 格式：模块:操作，如：system:user:list
    /// 3. 菜单类型为按钮时必填
    /// </remarks>
    [SugarColumn(ColumnName = "permission", ColumnDescription = "权限标识", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    [Description("权限标识")]
    public string? Permission { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：最大100个字符
    /// 2. 可选字段
    /// </remarks>
    [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    [Description("图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 1. 默认值：0
    /// 2. 值越小越靠前
    /// 3. 用于自定义显示顺序
    /// 4. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "显示顺序", ColumnDataType = "int", IsNullable = false)]
    [Description("显示顺序")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 菜单状态，使用LeanStatus枚举
    /// 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false)]
    [Description("状态")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    /// <remarks>
    /// 1. 1：可见
    /// 2. 0：隐藏
    /// 3. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "visible", ColumnDescription = "是否可见", ColumnDataType = "int", IsNullable = false)]
    [Description("是否可见")]
    public int Visible { get; set; }

    /// <summary>
    /// 是否缓存
    /// </summary>
    /// <remarks>
    /// 1. 1：缓存
    /// 2. 0：不缓存
    /// 3. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "cache", ColumnDescription = "是否缓存", ColumnDataType = "int", IsNullable = false)]
    [Description("是否缓存")]
    public int Cache { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    /// <remarks>
    /// 1. 1：是
    /// 2. 0：否
    /// 3. 必填字段
    /// </remarks>
    [SugarColumn(ColumnName = "external", ColumnDescription = "是否外链", ColumnDataType = "int", IsNullable = false)]
    [Description("是否外链")]
    public int External { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    /// <remarks>
    /// 1. 长度限制：最大100个字符
    /// 2. 可选字段
    /// </remarks>
    [SugarColumn(ColumnName = "trans_key", ColumnDescription = "翻译键", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    [Description("翻译键")]
    public string? TransKey { get; set; }

    /// <summary>
    /// 子菜单列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    public List<LeanMenu>? Children { get; set; }

    /// <summary>
    /// 角色菜单关联
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanRoleMenu.MenuId))]
    public List<LeanRoleMenu>? RoleMenus { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanMenu()
    {
        MenuName = string.Empty;
        MenuCode = string.Empty;
        MenuType = LeanMenuType.Directory;
        OrderNum = 0;
        Status = LeanStatus.Normal;
        Visible = 1;
        Cache = 0;
        External = 0;
        IsDeleted = 0;
    }
} 