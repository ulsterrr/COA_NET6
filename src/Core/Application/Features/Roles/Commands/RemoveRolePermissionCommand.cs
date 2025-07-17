using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Roles.Commands
{
    public class RemoveRolePermissionCommand : IRequest
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public RemoveRolePermissionCommand(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }

    public class RemoveRolePermissionCommandHandler : IRequestHandler<RemoveRolePermissionCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ICacheService _cacheService;

        public RemoveRolePermissionCommandHandler(IRoleRepository roleRepository, ICacheService cacheService)
        {
            _roleRepository = roleRepository;
            _cacheService = cacheService;
        }

        public async Task<Unit> Handle(RemoveRolePermissionCommand request, CancellationToken cancellationToken)
        {
            await _roleRepository.RemovePermissionFromRoleAsync(request.RoleId, request.PermissionId);
            await _cacheService.RemoveByPrefixAsync("GetAuthenticatedUserWithRoles");
            return Unit.Value;
        }
    }
}
