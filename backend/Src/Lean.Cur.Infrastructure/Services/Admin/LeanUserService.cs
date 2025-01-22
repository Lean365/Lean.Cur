using Lean.Cur.Application.Dtos.Admin;
using Lean.Cur.Application.Services.Admin;
using Lean.Cur.Common.Enums;
using Lean.Cur.Common.Excel;
using Lean.Cur.Common.Exceptions;
using Lean.Cur.Common.Extensions;
using Lean.Cur.Common.Pagination;
using Lean.Cur.Common.Security;
using Lean.Cur.Domain.Cache;
using Lean.Cur.Domain.Entities.Admin;
using Mapster;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace Lean.Cur.Infrastructure.Services.Admin;

/// <summary>
/// 用户服务实现
/// </summary>
public class LeanUserService : ILeanUserService
{
  private readonly SqlSugarClient _db;
  private readonly ILeanCache _cache;
  private readonly LeanExcelHelper _excel;

  public LeanUserService(SqlSugarClient db, ILeanCache cache, LeanExcelHelper excel)
  {
    _db = db;
    _cache = cache;
    _excel = excel;
  }

  #region 基础操作

  /// <inheritdoc/>
  public async Task<LeanPagedResult<LeanUserDto>> GetPagedListAsync(LeanUserQueryDto queryDto)
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

    return new LeanPagedResult<LeanUserDto>
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

    if (user.UserType == LeanUserType.Admin)
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

    if (user.UserType == LeanUserType.Admin)
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
    if (users.Any(u => u.UserType == LeanUserType.Admin))
    {
      throw new LeanUserFriendlyException("不能删除管理员用户");
    }

    return await _db.Deleteable<LeanUser>()
        .Where(u => ids.Contains(u.Id))
        .ExecuteCommandHasChangeAsync();
  }

  /// <inheritdoc/>
  public async Task<LeanUserDto> GetCurrentUserAsync()
  {
    var userId = LeanUser.GetCurrentUserId();
    if (userId <= 0)
    {
      throw new LeanUserFriendlyException("用户未登录");
    }

    var user = await _db.Queryable<LeanUser>()
        .FirstAsync(u => u.Id == userId && u.IsDeleted == 0)
        ?? throw new LeanUserFriendlyException("用户不存在");

    return user.Adapt<LeanUserDto>();
  }

  #endregion 基础操作

  #region 导入导出

  /// <inheritdoc/>
  public async Task<byte[]> GetImportTemplateAsync()
  {
    var headers = new Dictionary<string, string>
    {
        { "UserName", "用户名" },
        { "NickName", "昵称" },
        { "EnglishName", "英文名称" },
        { "Gender", "性别" },
        { "Email", "邮箱" },
        { "Phone", "手机号" },
        { "UserType", "用户类型" },
        { "Remark", "备注" }
    };

    return await _excel.GenerateTemplateAsync<LeanUserImportDto>(headers);
  }

  /// <inheritdoc/>
  public async Task<LeanUserImportResultDto> ImportAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      throw new LeanUserFriendlyException("请选择要导入的文件");
    }

    var result = _excel.Import<LeanUserImportDto>(file.OpenReadStream(), "import.xlsx");
    if (result == null || !result.Any())
    {
      throw new LeanUserFriendlyException("导入的数据为空");
    }

    var importResult = new LeanUserImportResultDto
    {
      TotalCount = result.Count,
      SuccessCount = 0,
      FailureCount = 0,
      FailureItems = new List<LeanUserImportDto>()
    };

    foreach (var importDto in result)
    {
      try
      {
        // 检查用户名是否已存在
        var existUser = await _db.Queryable<LeanUser>()
            .Where(u => u.UserName == importDto.UserName && u.IsDeleted == 0)
            .FirstAsync();
        if (existUser != null)
        {
          importDto.ErrorMessage = "用户名已存在";
          importResult.FailureItems.Add(importDto);
          importResult.FailureCount++;
          continue;
        }

        // 创建用户
        var salt = LeanPassword.GenerateSalt();
        var password = LeanPassword.HashPassword("123456", salt);

        var user = new LeanUser
        {
          UserName = importDto.UserName,
          NickName = importDto.NickName,
          EnglishName = importDto.EnglishName,
          Gender = importDto.Gender,
          Email = importDto.Email,
          Phone = importDto.Phone,
          UserType = importDto.UserType,
          Status = LeanStatus.Normal,
          Remark = importDto.Remark,
          Password = password,
          PasswordSalt = salt
        };

        await _db.Insertable(user).ExecuteCommandAsync();
        importResult.SuccessCount++;
      }
      catch (Exception)
      {
        importDto.ErrorMessage = "导入失败";
        importResult.FailureItems.Add(importDto);
        importResult.FailureCount++;
      }
    }

    return importResult;
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
    var roleCodes = await _db.Queryable<LeanUserRole>()
        .LeftJoin<LeanRole>((ur, r) => ur.RoleId == r.Id)
        .Where(ur => ur.UserId == userId && ur.IsDeleted == 0)
        .Where((ur, r) => r.Status == LeanStatus.Normal && r.IsDeleted == 0)
        .Select((ur, r) => r.RoleCode)
        .ToListAsync();

    return string.Join(",", roleCodes.Where(x => !string.IsNullOrEmpty(x)));
  }

  /// <inheritdoc/>
  public async Task<List<string>> GetUserPermissionsAsync(long userId)
  {
    // 从缓存中获取权限列表
    var cacheKey = $"user_permissions:{userId}";
    var permissions = await _cache.GetAsync<List<string>>(cacheKey);
    if (permissions != null)
    {
      return permissions;
    }

    // 查询用户的所有权限
    permissions = await _db.Queryable<LeanUserRole>()
        .InnerJoin<LeanRoleMenu>((ur, rm) => ur.RoleId == rm.RoleId)
        .Where(ur => ur.UserId == userId && ur.IsDeleted == 0)
        .Where((ur, rm) => rm.Status == LeanStatus.Normal && rm.IsDeleted == 0)
        .Select((ur, rm) => rm.Permission)
        .Where(p => !string.IsNullOrEmpty(p))
        .Distinct()
        .ToListAsync();

    // 缓存权限列表
    if (permissions.Any())
    {
      await _cache.SetAsync(cacheKey, permissions, TimeSpan.FromMinutes(30));
    }

    return permissions;
  }

  /// <inheritdoc/>
  public async Task<List<string>> GetUserMenuPermissionsAsync(long userId)
  {
    // 从缓存中获取权限列表
    var cacheKey = $"user_menu_permissions:{userId}";
    var permissions = await _cache.GetAsync<List<string>>(cacheKey);
    if (permissions != null)
    {
      return permissions;
    }

    // 查询用户的菜单权限
    permissions = await _db.Queryable<LeanUserRole>()
        .LeftJoin<LeanRole>((ur, r) => ur.RoleId == r.Id)
        .LeftJoin<LeanRoleMenu>((ur, r, rm) => r.Id == rm.RoleId)
        .Where(ur => ur.UserId == userId && ur.IsDeleted == 0)
        .Where((ur, r) => r.Status == LeanStatus.Normal && r.IsDeleted == 0)
        .Where((ur, r, rm) => rm.Status == LeanStatus.Normal && rm.IsDeleted == 0)
        .Where((ur, r, rm) => !string.IsNullOrEmpty(rm.Permission))
        .Select((ur, r, rm) => rm.Permission)
        .Distinct()
        .ToListAsync();

    // 缓存权限列表
    if (permissions.Any())
    {
      await _cache.SetAsync(cacheKey, permissions, TimeSpan.FromMinutes(30));
    }

    return permissions;
  }

  private void HandleImportError(LeanUserImportDto importDto, string message)
  {
    importDto.ErrorMessage = message;
  }
}