using Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands
{
    public class AddUserPermissionCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public List<int> PermissionIds { get; set; }
    }

    public class AddUserPermissionCommandHandler : IRequestHandler<AddUserPermissionCommand, bool>
    {
        private readonly IUserPermissionRepository _userPermissionRepository;

        public AddUserPermissionCommandHandler(IUserPermissionRepository userPermissionRepository)
        {
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<bool> Handle(AddUserPermissionCommand request, CancellationToken cancellationToken)
        {
            await _userPermissionRepository.AddUserPermissionsAsync(request.UserId, request.PermissionIds);
            return true;
        }
    }
}
