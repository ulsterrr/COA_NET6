using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Features.Roles.Commands
{
    public class AddRolePermissionCommand : IRequest
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public AddRolePermissionCommand(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }

    public class AddRolePermissionCommandHandler : IRequestHandler<AddRolePermissionCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ICacheService _cacheService;

        public AddRolePermissionCommandHandler(IRoleRepository roleRepository, ICacheService cacheService)
        {
            _roleRepository = roleRepository;
            _cacheService = cacheService;
        }

        public async Task<Unit> Handle(AddRolePermissionCommand request, CancellationToken cancellationToken)
        {
            await _roleRepository.AddPermissionToRoleAsync(request.RoleId, request.PermissionId);

            // Remove cache by prefix as before
            await _cacheService.RemoveByPrefixAsync("GetAuthenticatedUserWithRoles");

            // Define cache key for role permissions
            string cacheKey = $"RolePermissions:{request.RoleId}";

            // Check if cache key exists
            var cachedRolePermissions = await _cacheService.GetAsync<RoleWithPermissionsDTO>(cacheKey);
            if (cachedRolePermissions != null)
            {
                // Fetch updated permissions
                var permissions = await _roleRepository.GetPermissionsByRoleIdAsync(request.RoleId);
                var permissionNames = permissions.Select(p => p.Name).ToList();

                // Update cache with fresh data
                var updatedRolePermissions = new RoleWithPermissionsDTO
                {
                    Id = request.RoleId,
                    Permissions = permissionNames
                };
                await _cacheService.SetAsync(cacheKey, updatedRolePermissions);
            }
            else
            {
                // Cache key does not exist, add cache with fresh data
                var permissions = await _roleRepository.GetPermissionsByRoleIdAsync(request.RoleId);
                var permissionNames = permissions.Select(p => p.Name).ToList();

                var newRolePermissions = new RoleWithPermissionsDTO
                {
                    Id = request.RoleId,
                    Permissions = permissionNames
                };
                await _cacheService.SetAsync(cacheKey, newRolePermissions);
            }

            return Unit.Value;
        }
    }
}
