using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Cache;
using System;
using System.Linq;

namespace Core.Services
{
    public class RoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        private const int CacheExpirationSeconds = 300; // 5 minutes

        public RoleService(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.CompleteAsync();
            await RefreshRoleCache(role.Id);
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.Roles.GetAllAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var cacheKey = $"role:{id}";
            var cachedRole = await _cacheService.GetAsync<Role>(cacheKey);
            if (cachedRole != null)
            {
                return cachedRole;
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role != null)
            {
                await _cacheService.SetAsync(cacheKey, role, CacheExpirationSeconds);
            }
            return role;
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            var existingRole = await _unitOfWork.Roles.GetByIdAsync(role.Id);
            if (existingRole == null)
            {
                return false;
            }
            existingRole.Name = role.Name;
            await _unitOfWork.CompleteAsync();
            await RefreshRoleCache(role.Id);
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
            {
                return false;
            }
            _unitOfWork.Roles.Delete(role);
            await _unitOfWork.CompleteAsync();
            await _cacheService.RemoveAsync($"role:{id}");
            return true;
        }

        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            if (role == null)
            {
                return false;
            }

            var permissions = new List<Permission>();
            foreach (var permissionId in permissionIds)
            {
                var permission = await _unitOfWork.Permissions.GetByIdAsync(permissionId);
                if (permission != null)
                {
                    permissions.Add(permission);
                }
            }

            role.Permissions = permissions;
            await _unitOfWork.CompleteAsync();
            await RefreshRoleCache(roleId);
            return true;
        }

        public async Task<IEnumerable<object>> GetRolePermissionsByMenuAsync()
        {
            var menus = await _unitOfWork.Menus.GetAllAsync();

            var result = new List<object>();

            foreach (var menu in menus)
            {
                var roles = menu.Roles ?? new List<Role>();

                var rolePermissions = roles.Select(role => new
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Permissions = role.Permissions?.Select(p => new { p.Id, p.Name }).ToList() ?? new List<object>()
                }).ToList();

                result.Add(new
                {
                    MenuId = menu.Id,
                    MenuTitle = menu.Title,
                    Roles = rolePermissions
                });
            }

            return result;
        }

        private async Task RefreshRoleCache(int roleId)
        {
            await _cacheService.RemoveAsync($"role:{roleId}");
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            if (role != null)
            {
                await _cacheService.SetAsync($"role:{roleId}", role, CacheExpirationSeconds);
            }
        }
    }
}
