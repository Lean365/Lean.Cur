using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Security;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Entities.Authorization;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 用户服务实现
/// </summary>
public class LeanUserService : ILeanUserService
{
  private readonly ISqlSugarClient _db;
  private readonly IMemoryCache _cache;
  private readonly LeanExcelHelper _excel;

  public LeanUserService(ISqlSugarClient db, IMemoryCache cache, LeanExcelHelper excel)
  {
    _db = db;
    _cache = cache;
    _excel = excel;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<PagedResult<LeanUserDto>> GetPagedListAsync(LeanUserQueryDto queryDto)
  {
    var query = _db.Queryable<LeanUser>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), u => u.UserName!.Contains(queryDto.UserName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.NickName), u => u.NickName!.Contains(queryDto.NickName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.EnglishName), u => u.EnglishName!.Contains(queryDto.EnglishName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.Email), u => u.Email!.Contains(queryDto.Email!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.Phone), u => u.Phone!.Contains(queryDto.Phone!))
        .WhereIF(queryDto.Gender.HasValue, u => u.Gender == queryDto.Gender)
        .WhereIF(queryDto.Status.HasValue, u => u.Status == queryDto.Status)
        .WhereIF(queryDto.UserType.HasValue, u => u.UserType == queryDto.UserType)
        .WhereIF(queryDto.StartTime.HasValue, u => u.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, u => u.CreateTime <= queryDto.EndTime)
        .Where(u => u.IsDeleted == 0);

    var total = await query.CountAsync();
    var items = await query
        .OrderByDescending(u => u.CreateTime)
        .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
        .Take(queryDto.PageSize)
        .Select(u => new LeanUserDto
        {
          Id = u.Id,
          UserName = u.UserName,
          NickName = u.NickName,
          EnglishName = u.EnglishName,
          Avatar = u.Avatar,
          Gender = u.Gender,
          Email = u.Email,
          Phone = u.Phone,
          Status = u.Status,
          UserType = u.UserType,
          Remark = u.Remark,
          CreateTime = u.CreateTime,
          UpdateTime = u.UpdateTime
        })
        .ToListAsync();

    return new PagedResult<LeanUserDto>
    {
      Total = total,
      Items = items
    };
  }

  /// <inheritdoc/>
  public async Task<LeanUserDto> GetByIdAsync(long id)
  {
    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == id && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    return user.Adapt<LeanUserDto>();
  }

  /// <inheritdoc/>
  public async Task<long> CreateAsync(LeanUserCreateDto createDto)
  {
    // 检查用户名是否已存在
    if (await _db.Queryable<LeanUser>().AnyAsync(u => u.UserName == createDto.UserName && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("用户名已存在");
    }

    // 检查手机号是否已存在
    if (await _db.Queryable<LeanUser>().AnyAsync(u => u.Phone == createDto.Phone && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("手机号已存在");
    }

    // 检查邮箱是否已存在
    if (await _db.Queryable<LeanUser>().AnyAsync(u => u.Email == createDto.Email && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("邮箱已存在");
    }

    var user = createDto.Adapt<LeanUser>();
    user.PasswordSalt = LeanPassword.GenerateSalt();
    user.Password = LeanPassword.HashPassword(createDto.Password, user.PasswordSalt);

    return await _db.Insertable(user).ExecuteReturnIdentityAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateAsync(LeanUserUpdateDto updateDto)
  {
    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == updateDto.Id && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    // 检查手机号是否已存在
    if (await _db.Queryable<LeanUser>().AnyAsync(u => u.Phone == updateDto.Phone && u.Id != updateDto.Id && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("手机号已存在");
    }

    // 检查邮箱是否已存在
    if (await _db.Queryable<LeanUser>().AnyAsync(u => u.Email == updateDto.Email && u.Id != updateDto.Id && u.IsDeleted == 0))
    {
      throw new LeanUserFriendlyException("邮箱已存在");
    }

    updateDto.Adapt(user);
    return await _db.Updateable(user).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> DeleteAsync(long id)
  {
    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == id && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    if (user.UserType == UserType.Admin)
    {
      throw new LeanUserFriendlyException("管理员用户不能删除");
    }

    return await _db.Deleteable(user).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdateStatusAsync(LeanUserStatusDto statusDto)
  {
    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == statusDto.Id && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    if (user.UserType == UserType.Admin)
    {
      throw new LeanUserFriendlyException("管理员用户状态不能修改");
    }

    user.Status = statusDto.Status;
    return await _db.Updateable(user).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> ResetPasswordAsync(LeanUserResetPasswordDto resetDto)
  {
    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == resetDto.Id && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    user.PasswordSalt = LeanPassword.GenerateSalt();
    user.Password = LeanPassword.HashPassword(resetDto.NewPassword, user.PasswordSalt);

    return await _db.Updateable(user).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> UpdatePasswordAsync(long userId, LeanUserUpdatePasswordDto updateDto)
  {
    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == userId && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    var oldPasswordHash = LeanPassword.HashPassword(updateDto.OldPassword, user.PasswordSalt);
    if (oldPasswordHash != user.Password)
    {
      throw new LeanUserFriendlyException("旧密码错误");
    }

    user.PasswordSalt = LeanPassword.GenerateSalt();
    user.Password = LeanPassword.HashPassword(updateDto.NewPassword, user.PasswordSalt);

    return await _db.Updateable(user).ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<bool> BatchDeleteAsync(List<long> ids)
  {
    if (ids == null || !ids.Any())
    {
      throw new LeanUserFriendlyException("请选择要删除的用户");
    }

    var users = await _db.Queryable<LeanUser>()
        .Where(u => ids.Contains(u.Id) && u.IsDeleted == 0)
        .ToListAsync();

    if (!users.Any())
    {
      throw new LeanUserFriendlyException("未找到要删除的用户");
    }

    // 检查是否包含管理员用户
    if (users.Any(u => u.UserType == UserType.Admin))
    {
      throw new LeanUserFriendlyException("不能删除管理员用户");
    }

    return await _db.Deleteable<LeanUser>()
        .Where(u => ids.Contains(u.Id))
        .ExecuteCommandHasChangeAsync();
  }

  #endregion 基础操作

  #region 导入导出

  /// <inheritdoc/>
  public Task<byte[]> GetImportTemplateAsync()
  {
    var template = new List<LeanUserTempleteDto>
    {
        new()
        {
            UserName = "示例：zhangsan",
            NickName = "示例：张三",
            EnglishName = "示例：Zhang San",
            Gender = Gender.Male,
            Email = "示例：zhangsan@lean.cur",
            Phone = "示例：13800138000",
            UserType = UserType.User,
            Remark = "示例：备注信息"
        }
    };

    return Task.FromResult(_excel.Export(template));
  }

  /// <inheritdoc/>
  public async Task<(int total, int success, List<string> errors)> ImportAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      throw new LeanUserFriendlyException("请选择要导入的文件");
    }

    var result = await _excel.Import<LeanUserImportDto>(file.OpenReadStream(), "import.xlsx");
    if (result?.Data == null || !result.Data.Any())
    {
      throw new LeanUserFriendlyException("导入的数据为空");
    }

    var errors = new List<string>();
    var success = 0;
    var total = result.Data.Count;

    foreach (var importDto in result.Data)
    {
      try
      {
        // 检查用户名是否已存在
        var existUser = await _db.Queryable<LeanUser>()
            .Where(u => u.UserName == importDto.UserName && u.IsDeleted == 0)
            .FirstAsync();
        if (existUser != null)
        {
          errors.Add($"用户名 {importDto.UserName} 已存在");
          continue;
        }

        // 检查手机号是否已存在
        existUser = await _db.Queryable<LeanUser>()
            .Where(u => u.Phone == importDto.Phone && u.IsDeleted == 0)
            .FirstAsync();
        if (existUser != null)
        {
          errors.Add($"手机号 {importDto.Phone} 已存在");
          continue;
        }

        // 检查邮箱是否已存在
        existUser = await _db.Queryable<LeanUser>()
            .Where(u => u.Email == importDto.Email && u.IsDeleted == 0)
            .FirstAsync();
        if (existUser != null)
        {
          errors.Add($"邮箱 {importDto.Email} 已存在");
          continue;
        }

        var user = importDto.Adapt<LeanUser>();
        user.Status = LeanStatus.Normal;
        user.PasswordSalt = LeanPassword.GenerateSalt();
        user.Password = LeanPassword.HashPassword(user.Phone, user.PasswordSalt); // 默认使用手机号作为密码

        await _db.Insertable(user).ExecuteCommandAsync();
        success++;
      }
      catch (Exception ex)
      {
        errors.Add($"导入用户 {importDto.UserName} 失败：{ex.Message}");
      }
    }

    return (total, success, errors);
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(LeanUserQueryDto queryDto)
  {
    var query = _db.Queryable<LeanUser>()
        .WhereIF(!string.IsNullOrEmpty(queryDto.UserName), u => u.UserName!.Contains(queryDto.UserName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.NickName), u => u.NickName!.Contains(queryDto.NickName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.EnglishName), u => u.EnglishName!.Contains(queryDto.EnglishName!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.Phone), u => u.Phone!.Contains(queryDto.Phone!))
        .WhereIF(!string.IsNullOrEmpty(queryDto.Email), u => u.Email!.Contains(queryDto.Email!))
        .WhereIF(queryDto.Status.HasValue, u => u.Status == queryDto.Status)
        .WhereIF(queryDto.UserType.HasValue, u => u.UserType == queryDto.UserType)
        .WhereIF(queryDto.StartTime.HasValue, u => u.CreateTime >= queryDto.StartTime)
        .WhereIF(queryDto.EndTime.HasValue, u => u.CreateTime <= queryDto.EndTime)
        .Where(u => u.IsDeleted == 0);

    var users = await query.OrderByDescending(u => u.CreateTime).ToListAsync();

    var exportDtos = users.Select(u => new LeanUserExportDto
    {
      UserName = u.UserName,
      NickName = u.NickName,
      EnglishName = u.EnglishName,
      Gender = u.Gender.GetDescription(),
      Email = u.Email,
      Phone = u.Phone,
      Status = u.Status.GetDescription(),
      UserType = u.UserType.GetDescription(),
      Remark = u.Remark,
      CreateTime = u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
      UpdateTime = u.UpdateTime?.ToString("yyyy-MM-dd HH:mm:ss")
    }).ToList();

    return _excel.Export(exportDtos);
  }

  #endregion 导入导出

  /// <inheritdoc/>
  public async Task<string> GetUserRoleCodeAsync(long userId)
  {
    var roleCode = await _db.Queryable<LeanUserRole>()
        .LeftJoin<LeanRole>((ur, r) => ur.RoleId == r.Id)
        .Where(ur => ur.UserId == userId && ur.IsDeleted == 0)
        .Select((ur, r) => r.RoleCode)
        .FirstAsync();

    return roleCode ?? string.Empty;
  }

  /// <inheritdoc/>
  public async Task<List<string>> GetUserPermissionsAsync(long userId)
  {
    // 从缓存获取权限列表
    var cacheKey = $"user_permissions_{userId}";
    if (_cache.TryGetValue<List<string>>(cacheKey, out var permissions))
    {
      return permissions ?? new List<string>();
    }

    // 查询用户的所有权限
    permissions = await _db.Queryable<LeanUserRole>()
        .LeftJoin<LeanRole>((ur, r) => ur.RoleId == r.Id)
        .LeftJoin<LeanRolePermission>((ur, r, rp) => r.Id == rp.RoleId)
        .LeftJoin<LeanPermission>((ur, r, rp, p) => rp.PermissionId == p.Id)
        .Where((ur, r, rp, p) => ur.UserId == userId
            && ur.IsDeleted == 0
            && r.Status == 1
            && r.IsDeleted == 0
            && p.Status == 1
            && p.IsDeleted == 0)
        .Select((ur, r, rp, p) => p.PermissionCode)
        .ToListAsync();

    // 缓存权限列表（1小时过期）
    _cache.Set(cacheKey, permissions, TimeSpan.FromHours(1));

    return permissions ?? new List<string>();
  }
}