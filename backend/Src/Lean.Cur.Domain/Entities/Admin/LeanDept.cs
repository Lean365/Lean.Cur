using Lean.Cur.Common.Enums;
using SqlSugar;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 部门实体
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本实体用于管理系统中的部门信息，包含以下功能：
/// 1. 基础信息管理（部门名称、部门编码等）
/// 2. 状态管理（启用/停用）
/// 3. 排序管理
/// 4. 层级关系管理（上下级部门）
/// 5. 与用户的关联关系
/// </remarks>
[SugarTable("lean_dept")]
public class LeanDept : LeanBaseEntity
{
    /// <summary>
    /// 父级ID
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 默认值：0（表示顶级部门）
    /// 3. 用于构建部门层级关系
    /// </remarks>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", DefaultValue = "0", IsNullable = false)]
    public long ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一父级下唯一
    /// </remarks>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", Length = 50, IsNullable = false)]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// 5. 创建后不可修改
    /// </remarks>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", Length = 50, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 负责人
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大50个字符
    /// </remarks>
    [SugarColumn(ColumnName = "leader", ColumnDescription = "负责人", Length = 50, IsNullable = true)]
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大20个字符
    /// </remarks>
    [SugarColumn(ColumnName = "phone", ColumnDescription = "联系电话", Length = 20, IsNullable = true)]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大50个字符
    /// </remarks>
    [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", Length = 50, IsNullable = true)]
    public string? Email { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 值越小越靠前
    /// 3. 默认值：0
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "显示顺序", DefaultValue = "0", IsNullable = false)]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 默认值：正常
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", DefaultValue = "1", IsNullable = false)]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 导航属性：子部门列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    public List<LeanDept> Children { get; set; } = new();

    /// <summary>
    /// 导航属性：用户列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanUser.DeptId))]
    public List<LeanUser> Users { get; set; } = new();

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanDept()
    {
        ParentId = 0;
        OrderNum = 0;
        Status = LeanStatus.Normal;
    }
} 