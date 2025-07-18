using Application.Dtos;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class RoleRepository : EfEntityRepository<Role, CAContext, int>, IRoleRepository
    {
        public RoleRepository(CAContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesByUserIdAsync(int userId)
        {
            return await _context.Roles.Where(r => r.UserRoles.Any(ur => ur.UserId == userId)).Select(role => new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            }).ToListAsync();
        }

        public async Task<List<string>> GetPermissionsByUserIdAsync(int userId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.Role.UserRoles.Any(ur => ur.UserId == userId))
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task AddPermissionToRoleAsync(int roleId, int permissionId)
        {
            var exists = await _context.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (!exists)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                };
                await _context.RolePermissions.AddAsync(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPermissionsToRoleAsync(int roleId, List<string> permissionNames)
        {
            var existingPermissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.Name))
                .ToListAsync();

            var existingRolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && permissionNames.Contains(rp.Permission.Name))
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            var newPermissions = existingPermissions
                .Where(p => !existingRolePermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in newPermissions)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permission.Id
                };
                await _context.RolePermissions.AddAsync(rolePermission);
            }

            await _context.SaveChangesAsync();
        }
    }
}
