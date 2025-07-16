﻿using Application.Constants;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using MediatR;

namespace Application.Features.Roles.Commands
{
    public class RemoveRoleCommand : IRequest<IResponse>
    {
        public Guid Id { get; set; }

        public RemoveRoleCommand(Guid id)
        {
            Id = id;
        }

        public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, IResponse>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ICacheService _cacheService;

            public RemoveRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
            {
                _roleRepository = roleRepository;
                _unitOfWork = unitOfWork;
                _cacheService = cacheService;
            }

            public async Task<IResponse> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
            {
                var exisrole = await _roleRepository.GetByIdAsync(request.Id);
                if (exisrole == null)
                {
                    return new ErrorResponse(404,Messages.RoleNameAlreadyExist);
                }
                _roleRepository.Remove(exisrole);
                await _unitOfWork.SaveChangesAsync();
                await _cacheService.RemoveByPrefixAsync("GetAuthenticatedUserWithRoles");
                return new SuccessResponse(200, Messages.DeletedSuccessfully);
            }
        }
    }
}