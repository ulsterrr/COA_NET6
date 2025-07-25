﻿using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Wrappers.Concrete;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Queries
{
    public class GetAllUsersWithRolesQuery : IRequest<DataResponse<IEnumerable<UserDTO>>>
    {
        public class GetAllUsersWithRolesQueryHandler : IRequestHandler<GetAllUsersWithRolesQuery, DataResponse<IEnumerable<UserDTO>>>
        {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<GetAllUsersWithRolesQueryHandler> _logger;
            private readonly ICacheService _cacheService;

            public GetAllUsersWithRolesQueryHandler(IUserRepository userRepository, ILogger<GetAllUsersWithRolesQueryHandler> logger, ICacheService cacheService)
            {
                _userRepository = userRepository;
                _logger = logger;
                _cacheService = cacheService;
            }

            public async Task<DataResponse<IEnumerable<UserDTO>>> Handle(GetAllUsersWithRolesQuery request, CancellationToken cancellationToken)
            {
                var cacheKey = "all_users_with_roles";
                var cachedUsers = await _cacheService.GetAsync<IEnumerable<UserDTO>>(cacheKey);
                if (cachedUsers != null)
                {
                    _logger.LogInformation("Returning cached users with roles");
                    return new DataResponse<IEnumerable<UserDTO>>(cachedUsers, 200);
                }

                var userswithroles = await _userRepository.GetAllUsersWithRolesAsync();
                await _cacheService.SetAsync(cacheKey, userswithroles, TimeSpan.FromSeconds(300)); // Cache for 5 minutes
                _logger.LogInformation("GetAllUserWithRoles = {@GetAllUserWithRoles}", userswithroles);
                return new DataResponse<IEnumerable<UserDTO>>(userswithroles, 200);
            }
        }
    }
}