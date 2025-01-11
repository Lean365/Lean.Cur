namespace Lean.Cur.Application.DTOs.Admin;

/// <summary>
/// 菜单查询DTO
/// </summary>
public class LeanMenuQueryDto
{
  /// <summary>
  /// 菜单名称
  /// </summary>
  public string? MenuName { get; set; }

  /// <summary>
  /// 菜单类型(M:目录,C:菜单,F:按钮)
  /// </summary>
  public string? MenuType { get; set; }

  /// <summary>
  /// 显示状态(0:隐藏,1:显示)
  /// </summary>
  public int? Visible { get; set; }

  /// <summary>
  /// 菜单状态(0:禁用,1:正常)
  /// </summary>
  public int? Status { get; set; }
}

/// <summary>
/// 菜单DTO
/// </summary>
public class LeanMenuDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string MenuName { get; set; } = null!;

  /// <summary>
  /// 翻译键
  /// </summary>
  public string? TransKey { get; set; }

  /// <summary>
  /// 父菜单ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  public string? Component { get; set; }

  /// <summary>
  /// 是否为外链(0:否,1:是)
  /// </summary>
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存(0:否,1:是)
  /// </summary>
  public int IsCache { get; set; }

  /// <summary>
  /// 菜单类型(M:目录,C:菜单,F:按钮)
  /// </summary>
  public string MenuType { get; set; } = "M";

  /// <summary>
  /// 显示状态(0:隐藏,1:显示)
  /// </summary>
  public int Visible { get; set; } = 1;

  /// <summary>
  /// 菜单状态(0:禁用,1:正常)
  /// </summary>
  public int Status { get; set; } = 1;

  /// <summary>
  /// 权限标识
  /// </summary>
  public string? Perms { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  public string? Icon { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 子菜单
  /// </summary>
  public List<LeanMenuDto> Children { get; set; } = new();
}

/// <summary>
/// 菜单创建DTO
/// </summary>
public class LeanMenuCreateDto
{
  /// <summary>
  /// 菜单名称
  /// </summary>
  public string MenuName { get; set; } = null!;

  /// <summary>
  /// 翻译键
  /// </summary>
  public string? TransKey { get; set; }

  /// <summary>
  /// 父菜单ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  public string? Component { get; set; }

  /// <summary>
  /// 是否为外链(0:否,1:是)
  /// </summary>
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存(0:否,1:是)
  /// </summary>
  public int IsCache { get; set; }

  /// <summary>
  /// 菜单类型(M:目录,C:菜单,F:按钮)
  /// </summary>
  public string MenuType { get; set; } = "M";

  /// <summary>
  /// 显示状态(0:隐藏,1:显示)
  /// </summary>
  public int Visible { get; set; } = 1;

  /// <summary>
  /// 菜单状态(0:禁用,1:正常)
  /// </summary>
  public int Status { get; set; } = 1;

  /// <summary>
  /// 权限标识
  /// </summary>
  public string? Perms { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  public string? Icon { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }
}

/// <summary>
/// 菜单更新DTO
/// </summary>
public class LeanMenuUpdateDto : LeanMenuCreateDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }
}