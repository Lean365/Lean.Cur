using Lean.Cur.Domain.Entities;
using SqlSugar;
using System.ComponentModel;
using Lean.Cur.Common.Enums;

namespace Lean.Cur.Domain.Entities.Admin;

/// <summary>
/// 岗位实体
/// </summary>
/// <remarks>
/// 创建时间：2024-01-17
/// 修改时间：2024-01-17
/// 作者：Lean.Cur Team
/// 版本：v1.0.0
/// 描述：本实体用于管理系统中的岗位信息，包含以下功能：
/// 1. 基础信息管理（岗位名称、岗位编码等）
/// 2. 状态管理（启用/停用）
/// 3. 排序管理
/// 4. 与用户的关联关系
/// </remarks>
[SugarTable("lean_post")]
public class LeanPost : LeanBaseEntity
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 同一租户下唯一
    /// </remarks>
    [SugarColumn(ColumnName = "post_name", ColumnDescription = "岗位名称", Length = 50, IsNullable = false)]
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    /// <remarks>
    /// 1. 必填字段
    /// 2. 长度限制：2-50个字符
    /// 3. 只能包含字母、数字和下划线
    /// 4. 全局唯一，不区分大小写
    /// 5. 创建后不可修改
    /// </remarks>
    [SugarColumn(ColumnName = "post_code", ColumnDescription = "岗位编码", Length = 50, IsNullable = false)]
    public string PostCode { get; set; } = string.Empty;

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
    /// 备注
    /// </summary>
    /// <remarks>
    /// 1. 选填字段
    /// 2. 长度限制：最大500个字符
    /// </remarks>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 500, IsNullable = true)]
    public new string? Remark { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public LeanPost()
    {
        OrderNum = 0;
        Status = LeanStatus.Normal;
    }
} 