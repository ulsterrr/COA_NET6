﻿using Application.Dtos;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UserRepository : EfEntityRepository<User, CAContext, int>, IUserRepository
    {
        public UserRepository(CAContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<UserDTO>> GetAllUsersWithRolesAsync()
        {
            return await _context.Users.AsNoTracking().Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToListAsync();
        }

        public async Task<User> GetUserRolesByUserIdAsync(int userid)
        {
            return await _context.Users.Include(u => u.UserRoles).SingleOrDefaultAsync(u => u.Id == userid);
        }

        public async Task<UserDTO> GetUserWithRolesAsync(int userid)
        {
            return await _context.Users.AsNoTracking().Select(user => new UserDTO
            {
                Id = user.Id,
                UserName=user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).FirstOrDefaultAsync(user => user.Id == userid);
        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }
    }
}
