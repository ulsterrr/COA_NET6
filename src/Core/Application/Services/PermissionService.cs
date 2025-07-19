using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public interface IPermissionService
    {
        Task<List<string>> GetUserPermissionsAsync(int userId);
    }

    public class PermissionService : IPermissionService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IRoleRepository roleRepository, IUserPermissionRepository userPermissionRepository, ICacheService cacheService, ILogger<PermissionService> logger)
        {
            _roleRepository = roleRepository;
            _userPermissionRepository = userPermissionRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            var cacheKey = $"UserPermissions_{userId}";
            var userPermissions = await _cacheService.GetAsync<List<string>>(cacheKey);

            if (userPermissions == null)
            {
                try
                {
                    // Get role permissions
                    var rolePermissions = await _roleRepository.GetPermissionsByUserIdAsync(userId);

                    // Get user-specific permissions
                    var userPermissionsEntities = await _userPermissionRepository.GetPermissionsByUserIdAsync(userId);
                    var userSpecificPermissions = userPermissionsEntities?.Select(p => p.Name).ToList() ?? new List<string>();

                    // Combine role and user-specific permissions
                    userPermissions = rolePermissions != null ? rolePermissions.Union(userSpecificPermissions).Distinct().ToList() : userSpecificPermissions;

                    if (userPermissions != null)
                    {
                        await _cacheService.SetAsync(cacheKey, userPermissions, TimeSpan.FromMinutes(30));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching permissions for userId {UserId}", userId);
                    return new List<string>();
                }
            }

            return userPermissions ?? new List<string>();
        }
    }
}
