using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth
{
    public class JwtAuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public JwtAuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _unitOfWork.Users.GetAllAsync();
            var matchedUser = default(User);
            foreach (var u in user)
            {
                if (u.Username == username && u.PasswordHash == password) // In real app, use hashed password check
                {
                    matchedUser = u;
                    break;
                }
            }

            if (matchedUser == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, matchedUser.Username)
            };

foreach (var role in matchedUser.Roles)
{
    claims.Add(new Claim(ClaimTypes.Role, role.Name));

    // Add permission claims for each role
    if (role.Permissions != null)
    {
        foreach (var permission in role.Permissions)
        {
            claims.Add(new Claim("permission", permission.Name));
        }
    }
}

            // Add branch and department claims
            if (matchedUser.Branches != null)
            {
                foreach (var branch in matchedUser.Branches)
                {
                    claims.Add(new Claim("branch", branch.Name));
                }
            }

            if (matchedUser.Departments != null)
            {
                foreach (var department in matchedUser.Departments)
                {
                    claims.Add(new Claim("department", department.Name));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            foreach (var user in users)
            {
                if (user.Username == username)
                    return user;
            }
            return null;
        }
    }
}
