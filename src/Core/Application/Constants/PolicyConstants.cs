using System.ComponentModel;
using System.Reflection;

namespace WebAPI.Infrastructure.Authorization
{
    public static class PolicyConstants
    {
        // User policies
        public const string UserView = "User.View";
        public const string UserUpdate = "User.Update";
        public const string UserDelete = "User.Delete";

        // Branch policies
        public const string BranchView = "Branch.View";
        public const string BranchCreate = "Branch.Create";
        public const string BranchUpdate = "Branch.Update";
        public const string BranchDelete = "Branch.Delete";

        // Department policies
        public const string DepartmentView = "Department.View";
        public const string DepartmentCreate = "Department.Create";
        public const string DepartmentUpdate = "Department.Update";
        public const string DepartmentDelete = "Department.Delete";

        // Organization policies
        public const string OrganizationView = "Organization.View";

        // Role policies
        public const string RoleView = "Role.View";
        public const string RoleCreate = "Role.Create";
        public const string RoleUpdate = "Role.Update";
        public const string RoleDelete = "Role.Delete";

        public static IDictionary<string, string> GetPermissions()
        {
            return typeof(PolicyConstants)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => new
                {
                    Key = f.GetValue(null).ToString(),
                    Description = f.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "lorem ipsum dolor sit amet"
                })
                .ToDictionary(f => f.Key, f => f.Description);
        }
    }
}
