using System.Security.Cryptography;
using System.Text;
using Lean.Cur.Application.DTOs.Admin;
using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Lean.Cur.Application.Services.Admin.Impl;

/// <summary>
/// 用户服务实现
/// </summary>
public class LeanUserService : ILeanUserService
{
  private readonly ILeanBaseRepository<LeanUser> _userRepository;
  private readonly ILeanBaseRepository<LeanUserRole> _userRoleRepository;
  private readonly ILeanBaseRepository<LeanRole> _roleRepository;
  private readonly ILeanBaseRepository<LeanDept> _deptRepository;
  private readonly ILeanBaseRepository<LeanPosition> _positionRepository;
  private readonly ILogger<LeanUserService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanUserService(
      ILeanBaseRepository<LeanUser> userRepository,
      ILeanBaseRepository<LeanUserRole> userRoleRepository,
      ILeanBaseRepository<LeanRole> roleRepository,
      ILeanBaseRepository<LeanDept> deptRepository,
      ILeanBaseRepository<LeanPosition> positionRepository,
      ILogger<LeanUserService> logger)
  {
    _userRepository = userRepository;
    _userRoleRepository = userRoleRepository;
    _roleRepository = roleRepository;
    _deptRepository = deptRepository;
    _positionRepository = positionRepository;
    _logger = logger;
  }

  /// <inheritdoc/>
  public async Task<List<LeanUserDto>> GetUserListAsync(LeanUserQueryDto query)
  {
    try
    {
      var queryable = _userRepository.AsQueryable();

      // 构建查询条件
      if (!string.IsNullOrEmpty(query.UserName))
      {
        queryable = queryable.Where(x => x.UserName.Contains(query.UserName));
      }
      if (!string.IsNullOrEmpty(query.EnglishName))
      {
        queryable = queryable.Where(x => x.EnglishName != null && x.EnglishName.Contains(query.EnglishName));
      }
      if (!string.IsNullOrEmpty(query.NickName))
      {
        queryable = queryable.Where(x => x.NickName.Contains(query.NickName));
      }
      if (!string.IsNullOrEmpty(query.Phone))
      {
        queryable = queryable.Where(x => x.Phone != null && x.Phone.Contains(query.Phone));
      }
      if (query.Status.HasValue)
      {
        queryable = queryable.Where(x => x.Status == query.Status.Value);
      }
      if (query.DeptId.HasValue)
      {
        queryable = queryable.Where(x => x.DeptId == query.DeptId.Value);
      }
      if (query.StartTime.HasValue)
      {
        queryable = queryable.Where(x => x.CreateTime >= query.StartTime.Value);
      }
      if (query.EndTime.HasValue)
      {
        queryable = queryable.Where(x => x.CreateTime <= query.EndTime.Value);
      }

      // 按创建时间倒序排序
      queryable = queryable.OrderByDescending(x => x.CreateTime);

      var users = await queryable.ToListAsync();
      var userDtos = users.Adapt<List<LeanUserDto>>();

      // 填充部门和岗位信息
      foreach (var userDto in userDtos)
      {
        var dept = await _deptRepository.GetByIdAsync(userDto.DeptId);
        if (dept != null)
        {
          userDto.DeptName = dept.DeptName;
        }

        var position = await _positionRepository.GetByIdAsync(userDto.PositionId);
        if (position != null)
        {
          userDto.PositionName = position.PositionName;
        }

        // 填充角色信息
        var roleIds = await GetUserRoleIdsAsync(userDto.Id);
        userDto.RoleIds = roleIds;
        var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));
        userDto.RoleNames = roles.Select(x => x.RoleName).ToList();
      }

      return userDtos;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取用户列表失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<LeanUserDto?> GetUserAsync(long id)
  {
    try
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
      {
        return null;
      }

      var userDto = user.Adapt<LeanUserDto>();

      // 填充部门和岗位信息
      var dept = await _deptRepository.GetByIdAsync(userDto.DeptId);
      if (dept != null)
      {
        userDto.DeptName = dept.DeptName;
      }

      var position = await _positionRepository.GetByIdAsync(userDto.PositionId);
      if (position != null)
      {
        userDto.PositionName = position.PositionName;
      }

      // 填充角色信息
      var roleIds = await GetUserRoleIdsAsync(userDto.Id);
      userDto.RoleIds = roleIds;
      var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));
      userDto.RoleNames = roles.Select(x => x.RoleName).ToList();

      return userDto;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取用户详情失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<long> CreateUserAsync(LeanUserCreateDto dto)
  {
    try
    {
      // 检查用户名是否已存在
      if (await CheckUserNameExistAsync(dto.UserName))
      {
        throw new Exception($"用户名 {dto.UserName} 已存在");
      }

      // 检查手机号是否已存在
      if (!string.IsNullOrEmpty(dto.Phone) && await CheckPhoneExistAsync(dto.Phone))
      {
        throw new Exception($"手机号 {dto.Phone} 已存在");
      }

      // 检查邮箱是否已存在
      if (!string.IsNullOrEmpty(dto.Email) && await CheckEmailExistAsync(dto.Email))
      {
        throw new Exception($"邮箱 {dto.Email} 已存在");
      }

      // 检查部门是否存在
      var dept = await _deptRepository.GetByIdAsync(dto.DeptId);
      if (dept == null)
      {
        throw new Exception($"部门ID {dto.DeptId} 不存在");
      }

      // 检查岗位是否存在
      var position = await _positionRepository.GetByIdAsync(dto.PositionId);
      if (position == null)
      {
        throw new Exception($"岗位ID {dto.PositionId} 不存在");
      }

      // 检查角色是否存在
      if (dto.RoleIds.Any())
      {
        var roles = await _roleRepository.GetListAsync(x => dto.RoleIds.Contains(x.Id));
        if (roles.Count != dto.RoleIds.Count)
        {
          throw new Exception("部分角色ID不存在");
        }
      }

      // 生成密码盐值和加密密码
      var salt = GenerateSalt();
      var hashedPassword = HashPassword(dto.Password, salt);

      var user = dto.Adapt<LeanUser>();
      user.Salt = salt;
      user.Password = hashedPassword;

      await _userRepository.AddAsync(user);

      // 创建用户角色关联
      if (dto.RoleIds.Any())
      {
        var userRoles = dto.RoleIds.Select(roleId => new LeanUserRole
        {
          UserId = user.Id,
          RoleId = roleId
        }).ToList();
        await _userRoleRepository.AddRangeAsync(userRoles);
      }

      return user.Id;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "创建用户失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateUserAsync(LeanUserUpdateDto dto)
  {
    try
    {
      var user = await _userRepository.GetByIdAsync(dto.Id);
      if (user == null)
      {
        throw new Exception($"用户ID {dto.Id} 不存在");
      }

      // 检查用户名是否已存在
      if (await CheckUserNameExistAsync(dto.UserName, dto.Id))
      {
        throw new Exception($"用户名 {dto.UserName} 已存在");
      }

      // 检查手机号是否已存在
      if (!string.IsNullOrEmpty(dto.Phone) && await CheckPhoneExistAsync(dto.Phone, dto.Id))
      {
        throw new Exception($"手机号 {dto.Phone} 已存在");
      }

      // 检查邮箱是否已存在
      if (!string.IsNullOrEmpty(dto.Email) && await CheckEmailExistAsync(dto.Email, dto.Id))
      {
        throw new Exception($"邮箱 {dto.Email} 已存在");
      }

      // 检查部门是否存在
      var dept = await _deptRepository.GetByIdAsync(dto.DeptId);
      if (dept == null)
      {
        throw new Exception($"部门ID {dto.DeptId} 不存在");
      }

      // 检查岗位是否存在
      var position = await _positionRepository.GetByIdAsync(dto.PositionId);
      if (position == null)
      {
        throw new Exception($"岗位ID {dto.PositionId} 不存在");
      }

      // 检查角色是否存在
      if (dto.RoleIds.Any())
      {
        var roles = await _roleRepository.GetListAsync(x => dto.RoleIds.Contains(x.Id));
        if (roles.Count != dto.RoleIds.Count)
        {
          throw new Exception("部分角色ID不存在");
        }
      }

      // 更新密码
      if (!string.IsNullOrEmpty(dto.Password))
      {
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(dto.Password, salt);
        user.Salt = salt;
        user.Password = hashedPassword;
      }

      // 更新基本信息
      user.UserName = dto.UserName;
      user.EnglishName = dto.EnglishName;
      user.NickName = dto.NickName;
      user.Email = dto.Email;
      user.Phone = dto.Phone;
      user.Gender = dto.Gender;
      user.Avatar = dto.Avatar;
      user.Status = dto.Status;
      user.DeptId = dto.DeptId;
      user.PositionId = dto.PositionId;
      user.UserType = dto.UserType;
      user.Remark = dto.Remark;

      await _userRepository.UpdateAsync(user);

      // 更新用户角色关联
      await UpdateUserRolesAsync(user.Id, dto.RoleIds);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新用户失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task DeleteUserAsync(long id)
  {
    try
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
      {
        throw new Exception($"用户ID {id} 不存在");
      }

      // 删除用户角色关联
      await _userRoleRepository.DeleteAsync(x => x.UserId == id);

      // 删除用户
      await _userRepository.DeleteAsync(id);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "删除用户失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task ResetPasswordAsync(LeanUserResetPasswordDto dto)
  {
    try
    {
      var user = await _userRepository.GetByIdAsync(dto.Id);
      if (user == null)
      {
        throw new Exception($"用户ID {dto.Id} 不存在");
      }

      var salt = GenerateSalt();
      var hashedPassword = HashPassword(dto.Password, salt);
      user.Salt = salt;
      user.Password = hashedPassword;

      await _userRepository.UpdateAsync(user);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "重置用户密码失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateUserStatusAsync(LeanUserStatusUpdateDto dto)
  {
    try
    {
      var user = await _userRepository.GetByIdAsync(dto.Id);
      if (user == null)
      {
        throw new Exception($"用户ID {dto.Id} 不存在");
      }

      user.Status = dto.Status;
      await _userRepository.UpdateAsync(user);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新用户状态失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> CheckUserNameExistAsync(string userName, long? excludeId = null)
  {
    try
    {
      var queryable = _userRepository.AsQueryable()
          .Where(x => x.UserName == userName);

      if (excludeId.HasValue)
      {
        queryable = queryable.Where(x => x.Id != excludeId.Value);
      }

      return await queryable.AnyAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查用户名是否存在失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> CheckPhoneExistAsync(string phone, long? excludeId = null)
  {
    try
    {
      var queryable = _userRepository.AsQueryable()
          .Where(x => x.Phone == phone);

      if (excludeId.HasValue)
      {
        queryable = queryable.Where(x => x.Id != excludeId.Value);
      }

      return await queryable.AnyAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查手机号是否存在失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<bool> CheckEmailExistAsync(string email, long? excludeId = null)
  {
    try
    {
      var queryable = _userRepository.AsQueryable()
          .Where(x => x.Email == email);

      if (excludeId.HasValue)
      {
        queryable = queryable.Where(x => x.Id != excludeId.Value);
      }

      return await queryable.AnyAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "检查邮箱是否存在失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task<List<long>> GetUserRoleIdsAsync(long userId)
  {
    try
    {
      var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == userId);
      return userRoles.Select(x => x.RoleId).ToList();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取用户角色ID列表失败");
      throw;
    }
  }

  /// <inheritdoc/>
  public async Task UpdateUserRolesAsync(long userId, List<long> roleIds)
  {
    try
    {
      // 删除原有的用户角色关联
      await _userRoleRepository.DeleteAsync(x => x.UserId == userId);

      // 创建新的用户角色关联
      if (roleIds.Any())
      {
        var userRoles = roleIds.Select(roleId => new LeanUserRole
        {
          UserId = userId,
          RoleId = roleId
        }).ToList();
        await _userRoleRepository.AddRangeAsync(userRoles);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "更新用户角色关联失败");
      throw;
    }
  }

  /// <summary>
  /// 生成密码盐值
  /// </summary>
  private static string GenerateSalt()
  {
    var bytes = new byte[10];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(bytes);
    return Convert.ToBase64String(bytes);
  }

  /// <summary>
  /// 密码加密
  /// </summary>
  private static string HashPassword(string password, string salt)
  {
    using var md5 = MD5.Create();
    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
    return BitConverter.ToString(hash).Replace("-", "").ToLower();
  }
}