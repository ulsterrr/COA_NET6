using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        TokenDTO CreateToken(User user,List<string> roles, List<string> permissions);
    }
}
