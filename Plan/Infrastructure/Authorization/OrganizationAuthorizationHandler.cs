using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization
{
    public class OrganizationAuthorizationHandler : AuthorizationHandler<OrganizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrganizationRequirement requirement)
        {
            var userBranches = context.User.FindAll("branch").Select(c => c.Value);
            var userDepartments = context.User.FindAll("department").Select(c => c.Value);

            if (userBranches.Contains(requirement.RequiredBranch) || userDepartments.Contains(requirement.RequiredDepartment))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class OrganizationRequirement : IAuthorizationRequirement
    {
        public string RequiredBranch { get; }
        public string RequiredDepartment { get; }

        public OrganizationRequirement(string requiredBranch, string requiredDepartment)
        {
            RequiredBranch = requiredBranch;
            RequiredDepartment = requiredDepartment;
        }
    }
}
