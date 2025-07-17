using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands
{
    public class AssignRolesToUserWithCacheCommand : IRequest<IResponse>
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }

        public class AssignRolesToUserWithCacheCommandHandler : IRequestHandler<AssignRolesToUserWithCacheCommand, IResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly IRoleRepository _roleRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICacheService _cacheService;
            private readonly ILogger<AssignRolesToUserWithCacheCommandHandler> _logger;

            public AssignRolesToUserWithCacheCommandHandler(
                IUserRepository userRepository,
                IRoleRepository roleRepository,
                IUnitOfWork unitOfWork,
                ICacheService cacheService,
                ILogger<AssignRolesToUserWithCacheCommandHandler> logger)
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _unitOfWork = unitOfWork;
                _cacheService = cacheService;
                _logger = logger;
            }

            public async Task<IResponse> Handle(AssignRolesToUserWithCacheCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User with id {UserId} not found", request.UserId);
                    return new ErrorResponse(404, "User not found");
                }

                var roles = new List<Domain.Entities.Role>();
                foreach (var roleId in request.RoleIds)
                {
                    var role = await _roleRepository.GetByIdAsync(roleId);
                    if (role != null)
                    {
                        roles.Add(role);
                    }
                }

                user.UserRoles = roles.Select(role => new Domain.Entities.UserRole { UserId = user.Id, RoleId = role.Id }).ToList();
                await _unitOfWork.SaveChangesAsync();

                var cacheKey = $"user:{user.UserName}";
                await _cacheService.RemoveAsync(cacheKey);

                _logger.LogInformation("Assigned roles to user {UserId} and cleared cache", request.UserId);
                return new SuccessResponse(200, "Roles assigned successfully");
            }
        }
    }
}
