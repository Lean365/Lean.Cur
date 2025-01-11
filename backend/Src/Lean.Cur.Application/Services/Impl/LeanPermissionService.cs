using Lean.Cur.Domain.Entities.Admin;
using Lean.Cur.Domain.Repositories;

namespace Lean.Cur.Application.Services.Impl;

public class LeanPermissionService : ILeanPermissionService
{
  private readonly ILeanRepository<LeanPermission> _permissionRepository;
  private readonly ILeanUserRepository _userRepository;
  private readonly ILeanRoleRepository _roleRepository;

  public LeanPermissionService(
      ILeanRepository<LeanPermission> permissionRepository,
      ILeanUserRepository userRepository,
      ILeanRoleRepository roleRepository)
  {
    _permissionRepository = permissionRepository;
    _userRepository = userRepository;
    _roleRepository = roleRepository;
  }

  public async Task<List<LeanPermission>> GetPermissionListAsync(string? keyword)
  {
    var permissions = await _permissionRepository.GetListAsync();
    if (!string.IsNullOrEmpty(keyword))
    {
      permissions = permissions.Where(p =>
        p.PermissionName.Contains(keyword) ||
        p.PermissionCode.Contains(keyword)).ToList();
    }
    return permissions;
  }

  public async Task<LeanPermission?> GetPermissionAsync(long id)
  {
    return await _permissionRepository.GetByIdAsync(id);
  }

  public async Task<LeanPermission> CreatePermissionAsync(LeanPermission permission)
  {
    permission.CreateTime = DateTime.Now;
    return await _permissionRepository.CreateAsync(permission);
  }

  public async Task<LeanPermission> UpdatePermissionAsync(LeanPermission permission)
  {
    permission.UpdateTime = DateTime.Now;
    return await _permissionRepository.UpdateAsync(permission);
  }

  public async Task DeletePermissionAsync(long id)
  {
    await _permissionRepository.DeleteAsync(id);
  }

  public async Task<List<LeanPermission>> GetUserMenusAsync(long userId)
  {
    var permissions = await _userRepository.GetUserPermissionsAsync(userId);
    return permissions.Where(p => !p.IsButton).ToList();
  }

  public async Task<List<LeanPermission>> GetUserButtonsAsync(long userId)
  {
    var permissions = await _userRepository.GetUserPermissionsAsync(userId);
    return permissions.Where(p => p.IsButton).ToList();
  }

  public async Task<bool> ValidatePermissionAsync(long userId, string permissionCode)
  {
    return await _userRepository.HasPermissionAsync(userId, permissionCode);
  }

  public async Task<List<string>> GetUserPermissionCodesAsync(long userId)
  {
    var permissions = await _userRepository.GetUserPermissionsAsync(userId);
    return permissions.Select(p => p.PermissionCode).ToList();
  }

  public async Task AssignUserRolesAsync(long userId, IEnumerable<long> roleIds)
  {
    await _userRepository.AssignRolesAsync(userId, roleIds);
  }

  public async Task AssignRolePermissionsAsync(long roleId, IEnumerable<long> permissionIds)
  {
    await _roleRepository.AssignPermissionsAsync(roleId, permissionIds);
  }

  public async Task<List<LeanPermission>> GetPermissionTreeAsync()
  {
    var permissions = await _permissionRepository.GetListAsync();
    return BuildPermissionTree(permissions);
  }

  private List<LeanPermission> BuildPermissionTree(List<LeanPermission> permissions, long? parentId = null)
  {
    var tree = new List<LeanPermission>();
    var children = permissions.Where(x => x.ParentId == parentId && !x.IsButton).ToList();

    foreach (var child in children)
    {
      var node = child;
      var subChildren = permissions.Where(x => x.ParentId == child.Id && !x.IsButton).ToList();
      if (subChildren.Any())
      {
        node.Children = BuildPermissionTree(permissions, child.Id);
      }
      tree.Add(node);
    }

    return tree;
  }

  public async Task<bool> CheckPermissionCodeAsync(string code)
  {
    var permissions = await _permissionRepository.GetListAsync();
    return permissions.Any(x => x.PermissionCode == code);
  }
}