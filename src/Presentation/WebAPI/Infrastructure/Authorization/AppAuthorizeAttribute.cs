using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace WebAPI.Infrastructure.Authorization
{
    /// <summary>
    /// Usage example:
    /// [AppAuthorize("Permission1", "Permission2", RequireAll = false)] // user needs any one permission
    /// [AppAuthorize("Permission1", "Permission2", RequireAll = true)] // user needs all permissions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AppAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;
        public bool RequireAll { get; set; } = false;

        public AppAuthorizeAttribute(params string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<AppAuthorizeAttribute>>();
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                logger?.LogWarning("Unauthorized access attempt: user not authenticated.");
                context.Result = new UnauthorizedResult();
                return;
            }

            var roleRepository = context.HttpContext.RequestServices.GetService<IRoleRepository>();
            var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();

            if (roleRepository == null || cacheService == null)
            {
                logger?.LogError("RoleRepository or CacheService not available in DI container.");
                context.Result = new ForbidResult();
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                logger?.LogWarning("Unauthorized access attempt: user ID claim missing.");
                context.Result = new ForbidResult();
                return;
            }

            var userId = int.Parse(userIdClaim.Value);
            var cacheKey = $"UserPermissions_{userId}";
            var userPermissions = await cacheService.GetAsync<List<string>>(cacheKey);

            if (userPermissions == null)
            {
                // Get role permissions
                var rolePermissions = await roleRepository.GetPermissionsByUserIdAsync(userId);

                // Get user-specific permissions
                var userPermissionRepository = context.HttpContext.RequestServices.GetService<IUserPermissionRepository>();
                var userSpecificPermissions = new List<string>();
                if (userPermissionRepository != null)
                {
                    var userPermissionsEntities = await userPermissionRepository.GetPermissionsByUserIdAsync(userId);
                    if (userPermissionsEntities != null)
                    {
                        userSpecificPermissions = userPermissionsEntities.Select(p => p.Name).ToList();
                    }
                }

                // Combine role and user-specific permissions
                userPermissions = rolePermissions != null ? rolePermissions.Union(userSpecificPermissions).Distinct().ToList() : userSpecificPermissions;

                if (userPermissions != null)
                {
                    await cacheService.SetAsync(cacheKey, userPermissions, TimeSpan.FromMinutes(30));
                }
            }

            if (userPermissions == null)
            {
                logger?.LogWarning("Unauthorized access attempt: user permissions not found for userId {UserId}.", userId);
                context.Result = new ForbidResult();
                return;
            }

            bool authorized;
            if (RequireAll)
            {
                authorized = _requiredPermissions.All(rp => userPermissions.Contains(rp));
            }
            else
            {
                authorized = _requiredPermissions.Any(rp => userPermissions.Contains(rp));
            }

            // If user has role "Admin", they are automatically authorized
            if (user.IsInRole("Admin"))
            {
                authorized = true;
            }

            if (!authorized)
            {
                var missingPermissions = RequireAll
                    ? _requiredPermissions.Except(userPermissions).ToArray()
                    : _requiredPermissions.Where(rp => !userPermissions.Contains(rp)).ToArray();

                logger?.LogWarning("Unauthorized access attempt by userId {UserId}. Missing permissions: {MissingPermissions}", userId, string.Join(", ", missingPermissions));

                context.Result = new ObjectResult(new { message = "You do not have the required permissions to access this resource.", missingPermissions })
                {
                    StatusCode = 403
                };
                return;
            }
        }
    }
}
