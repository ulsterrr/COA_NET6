using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
