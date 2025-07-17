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
    }
}
