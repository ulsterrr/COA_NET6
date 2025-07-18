using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Infrastructure.Persistence.Repositories
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly CAContext _context;

        public UserPermissionRepository(CAContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetPermissionsByUserIdAsync(int userId)
        {
            return await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .Select(up => up.Permission)
                .ToListAsync();
        }

        public async Task AddUserPermissionsAsync(int userId, List<int> permissionIds)
        {
            var existingUserPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId && permissionIds.Contains(up.PermissionId))
                .ToListAsync();

            var newPermissionIds = permissionIds.Except(existingUserPermissions.Select(up => up.PermissionId)).ToList();

            var newUserPermissions = newPermissionIds.Select(pid => new UserPermission
            {
                UserId = userId,
                PermissionId = pid
            });

            await _context.UserPermissions.AddRangeAsync(newUserPermissions);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserPermissionsAsync(int userId, List<int> permissionIds)
        {
            var userPermissionsToRemove = await _context.UserPermissions
                .Where(up => up.UserId == userId && permissionIds.Contains(up.PermissionId))
                .ToListAsync();

            _context.UserPermissions.RemoveRange(userPermissionsToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserPermission>> GetUserPermissionsAsync(int userId)
        {
            return await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .ToListAsync();
        }
    }
}
