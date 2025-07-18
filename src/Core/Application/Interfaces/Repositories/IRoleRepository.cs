using Application.Dtos;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        Task AddPermissionsToRoleAsync(int roleId, List<string> permissionNames);
        Task<IEnumerable<RoleDTO>> GetRolesByUserIdAsync(int userId);
        Task<List<string>> GetPermissionsByUserIdAsync(int userId);
        Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId);
        Task AddPermissionToRoleAsync(int roleId, int permissionId);
        Task RemovePermissionFromRoleAsync(int roleId, int permissionId);
    }
}
