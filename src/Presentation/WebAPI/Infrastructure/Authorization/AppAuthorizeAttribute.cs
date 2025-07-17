using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roleRepository = context.HttpContext.RequestServices.GetService<IRoleRepository>();
            var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();

            if (roleRepository == null || cacheService == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userId = int.Parse(userIdClaim.Value);
            var cacheKey = $"UserPermissions_{userId}";
            var userPermissions = await cacheService.GetAsync<List<string>>(cacheKey);

            if (userPermissions == null)
            {
                userPermissions = await roleRepository.GetPermissionsByUserIdAsync(userId);
                if (userPermissions != null)
                {
                    await cacheService.SetAsync(cacheKey, userPermissions, TimeSpan.FromMinutes(30));
                }
            }

            if (userPermissions == null)
            {
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
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
