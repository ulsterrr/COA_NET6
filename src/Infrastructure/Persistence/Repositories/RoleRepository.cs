using Application.Dtos;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

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
    }
}
