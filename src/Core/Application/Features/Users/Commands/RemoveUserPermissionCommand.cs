using Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands
{
    public class RemoveUserPermissionCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public List<int> PermissionIds { get; set; }
    }

    public class RemoveUserPermissionCommandHandler : IRequestHandler<RemoveUserPermissionCommand, bool>
    {
        private readonly IUserPermissionRepository _userPermissionRepository;

        public RemoveUserPermissionCommandHandler(IUserPermissionRepository userPermissionRepository)
        {
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<bool> Handle(RemoveUserPermissionCommand request, CancellationToken cancellationToken)
        {
            await _userPermissionRepository.RemoveUserPermissionsAsync(request.UserId, request.PermissionIds);
            return true;
        }
    }
}
