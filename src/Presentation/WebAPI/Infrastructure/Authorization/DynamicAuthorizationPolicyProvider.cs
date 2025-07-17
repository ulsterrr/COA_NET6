using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.Services;
using WebAPI.Infrastructure.Authorization;

namespace Infrastructure.Authorization
{
    public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private readonly ICacheService _cacheService;

        public DynamicAuthorizationPolicyProvider(Microsoft.Extensions.Options.IOptions<AuthorizationOptions> options, ICacheService cacheService)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _cacheService = cacheService;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _fallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return _fallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = _cacheService.Get<AuthorizationPolicy>(policyName);
            if (policy != null)
            {
                return Task.FromResult(policy);
            }

            // Dynamically create policy based on policyName
            var dynamicPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            // Cache the policy for future use
            _cacheService.Set(policyName, dynamicPolicy, TimeSpan.FromMinutes(30));

            return Task.FromResult(dynamicPolicy);
        }
    }
}
