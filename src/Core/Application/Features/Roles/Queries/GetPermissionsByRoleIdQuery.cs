using Application.Dtos;
using Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Roles.Queries
{
    public class GetPermissionsByRoleIdQuery : IRequest<RoleWithPermissionsDTO>
    {
        public int RoleId { get; set; }

        public GetPermissionsByRoleIdQuery(int roleId)
        {
            RoleId = roleId;
        }
    }

    public class GetPermissionsByRoleIdQueryHandler : IRequestHandler<GetPermissionsByRoleIdQuery, RoleWithPermissionsDTO>
    {
        private readonly IRoleRepository _roleRepository;

        public GetPermissionsByRoleIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleWithPermissionsDTO> Handle(GetPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _roleRepository.GetPermissionsByRoleIdAsync(request.RoleId);
            var permissionNames = permissions.Select(p => p.Name).ToList();

            // Assuming RoleDTO is fetched elsewhere or minimal info here
            return new RoleWithPermissionsDTO
            {
                Id = request.RoleId,
                Permissions = permissionNames
            };
        }
    }
}
