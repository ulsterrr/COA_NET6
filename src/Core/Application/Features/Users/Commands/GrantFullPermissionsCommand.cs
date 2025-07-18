using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MediatR;
using WebAPI.Infrastructure.Authorization;

namespace Application.Features.Users.Commands
{
    public class GrantFullPermissionsCommand : IRequest<bool>
    {
        public int UserId { get; set; }
    }

    public class GrantFullPermissionsCommandHandler : IRequestHandler<GrantFullPermissionsCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;

        public GrantFullPermissionsCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository, ICacheService cacheService)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(GrantFullPermissionsCommand request, CancellationToken cancellationToken)
        {
            // Get all permissions dynamically from PolicyConstants class
            var allPermissions = PolicyConstants.GetPermissions().Keys.ToList();

            // Get user roles
            var userRoles = await _userRepository.GetUserRolesAsync(request.UserId);
            if (userRoles == null || !userRoles.Any())
            {
                return false;
            }

            // For each role, add missing permissions
            foreach (var role in userRoles)
            {
                var existingPermissions = await _roleRepository.GetPermissionsByRoleIdAsync(role.Id);
                var missingPermissions = allPermissions.Except(existingPermissions.Select(p => p.Name)).ToList();

                if (missingPermissions.Any())
                {
                    await _roleRepository.AddPermissionsToRoleAsync(role.Id, missingPermissions);
                }
            }

            // Update cache for user permissions
            var cacheKey = $"UserPermissions_{request.UserId}";
            var updatedPermissions = await _roleRepository.GetPermissionsByUserIdAsync(request.UserId);
            await _cacheService.SetAsync(cacheKey, updatedPermissions, System.TimeSpan.FromMinutes(30));

            return true;
        }
    }
}
