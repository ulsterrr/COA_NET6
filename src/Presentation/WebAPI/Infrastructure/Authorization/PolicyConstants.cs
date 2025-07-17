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
    }
}
