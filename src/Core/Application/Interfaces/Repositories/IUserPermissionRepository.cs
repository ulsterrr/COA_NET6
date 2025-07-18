using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserPermissionRepository
    {
        Task<List<Permission>> GetPermissionsByUserIdAsync(int userId);
        Task AddUserPermissionsAsync(int userId, List<int> permissionIds);
        Task RemoveUserPermissionsAsync(int userId, List<int> permissionIds);
        Task<List<UserPermission>> GetUserPermissionsAsync(int userId);
    }
}
