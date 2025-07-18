using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetUserPermissionsQuery : IRequest<List<Permission>>
    {
        public int UserId { get; set; }
    }

    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, List<Permission>>
    {
        private readonly IUserPermissionRepository _userPermissionRepository;

        public GetUserPermissionsQueryHandler(IUserPermissionRepository userPermissionRepository)
        {
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<List<Permission>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _userPermissionRepository.GetPermissionsByUserIdAsync(request.UserId);
        }
    }
}
