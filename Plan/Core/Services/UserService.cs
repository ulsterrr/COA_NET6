using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using System;

namespace Core.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService _cacheService;

        private const int CacheExpirationSeconds = 300; // 5 minutes

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles?.Select(r => r.Name).ToArray()
                });
            }

            return userDtos;
        }

        public async Task<bool> AssignRolesToUserAsync(int userId, List<int> roleIds)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var roles = new List<Role>();
            foreach (var roleId in roleIds)
            {
                var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
                if (role != null)
                {
                    roles.Add(role);
                }
            }

            user.Roles = roles;
            await _unitOfWork.CompleteAsync();

            // Refresh cache for user
            var cacheKey = $"user:{user.Username}";
            await _cacheService.RemoveAsync(cacheKey);

            return true;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var cacheKey = $"user:{username}";
            var cachedUser = await _cacheService.GetAsync<User>(cacheKey);
            if (cachedUser != null)
            {
                return cachedUser;
            }

            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                await _cacheService.SetAsync(cacheKey, user, CacheExpirationSeconds);
            }
            return user;
        }
    }
}
