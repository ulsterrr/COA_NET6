using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User,int>
    {
        Task<List<Role>> GetUserRolesAsync(int userId);
        Task<UserDTO> GetUserWithRolesAsync(int userid);
        Task<IEnumerable<UserDTO>> GetAllUsersWithRolesAsync();
        Task<User> GetUserRolesByUserIdAsync(int userid);
    }
}
