using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly CAContext _context;
        private readonly ICacheService _cacheService;
        private const string CacheKey = "all_permissions_cache";

        public PermissionRepository(CAContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            var cachedPermissions = await _cacheService.GetAsync<IEnumerable<Permission>>(CacheKey);
            if (cachedPermissions != null)
            {
                return cachedPermissions;
            }

            var permissions = await _context.Permissions.ToListAsync();
            await _cacheService.SetAsync(CacheKey, permissions, TimeSpan.FromMinutes(10));
            return permissions;
        }

        public async Task<Permission> GetPermissionByIdAsync(int id)
        {
            var permissions = await GetAllPermissionsAsync();
            foreach (var permission in permissions)
            {
                if (permission.Id == id)
                {
                    return permission;
                }
            }
            return null;
        }
    }
}
