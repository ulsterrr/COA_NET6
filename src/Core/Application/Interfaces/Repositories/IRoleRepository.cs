using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        Task<IEnumerable<RoleDTO>> GetRolesByUserIdAsync(int userId);
        Task<IEnumerable<Permission>> GetPermissionsByRoleIdAsync(int roleId);
        Task<List<string>> GetPermissionsByUserIdAsync(int userId);
    }
}
        