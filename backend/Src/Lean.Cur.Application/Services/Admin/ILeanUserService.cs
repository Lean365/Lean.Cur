using Lean.Cur.Application.DTOs.Admin;

namespace Lean.Cur.Application.Services.Admin;

/// <summary>
/// 用户服务接口
/// </summary>
public interface ILeanUserService
{
  /// <summary>
  /// 获取用户列表
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <returns>用户列表</returns>
  Task<List<LeanUserDto>> GetUserListAsync(LeanUserQueryDto query);

  /// <summary>
  /// 获取用户详情
  /// </summary>
  /// <param name="id">用户ID</param>
  /// <returns>用户详情</returns>
  Task<LeanUserDto?> GetUserAsync(long id);

  /// <summary>
  /// 创建用户
  /// </summary>
  /// <param name="dto">用户创建信息</param>
  /// <returns>用户ID</returns>
  Task<long> CreateUserAsync(LeanUserCreateDto dto);

  /// <summary>
  /// 更新用户
  /// </summary>
  /// <param name="dto">用户更新信息</param>
  Task UpdateUserAsync(LeanUserUpdateDto dto);

  /// <summary>
  /// 删除用户
  /// </summary>
  /// <param name="id">用户ID</param>
  Task DeleteUserAsync(long id);

  /// <summary>
  /// 重置用户密码
  /// </summary>
  /// <param name="dto">密码重置信息</param>
  Task ResetPasswordAsync(LeanUserResetPasswordDto dto);

  /// <summary>
  /// 更新用户状态
  /// </summary>
  /// <param name="dto">状态更新信息</param>
  Task UpdateUserStatusAsync(LeanUserStatusUpdateDto dto);

  /// <summary>
  /// 检查用户名是否存在
  /// </summary>
  /// <param name="userName">用户名</param>
  /// <param name="excludeId">排除的用户ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckUserNameExistAsync(string userName, long? excludeId = null);

  /// <summary>
  /// 检查手机号是否存在
  /// </summary>
  /// <param name="phone">手机号</param>
  /// <param name="excludeId">排除的用户ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckPhoneExistAsync(string phone, long? excludeId = null);

  /// <summary>
  /// 检查邮箱是否存在
  /// </summary>
  /// <param name="email">邮箱</param>
  /// <param name="excludeId">排除的用户ID</param>
  /// <returns>true:存在,false:不存在</returns>
  Task<bool> CheckEmailExistAsync(string email, long? excludeId = null);

  /// <summary>
  /// 获取用户角色ID列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>角色ID列表</returns>
  Task<List<long>> GetUserRoleIdsAsync(long userId);

  /// <summary>
  /// 更新用户角色关联
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="roleIds">角色ID列表</param>
  Task UpdateUserRolesAsync(long userId, List<long> roleIds);
}