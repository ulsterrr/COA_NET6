namespace Application.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum DataScope
    {
        Own,
        Department,
        Branch,
        All
    }

    public interface IDataPermissionService
    {
        Task<DataScope> GetUserDataScopeAsync(int userId, string permissionName);
        Task<DataScope> GetUserDataScopeAsync(int userId, IEnumerable<string> permissionNames);
        Task<bool> HasAccessToDataAsync(int userId, string permissionName, int dataOwnerUserId, int dataDepartmentId, int dataBranchId);
        Task<bool> HasAccessToDataAsync(int userId, IEnumerable<string> permissionNames, int dataOwnerUserId, int dataDepartmentId, int dataBranchId);
    }

    public class DataPermissionService : IDataPermissionService
    {
        // This example assumes you have access to user info like department and branch
        // You may need to inject repositories or services to get user details

        public async Task<DataScope> GetUserDataScopeAsync(int userId, string permissionName)
        {
            // TODO: Implement logic to get user's data scope for the given permission
            // For example, query user roles or permissions that define the scope level
            // Return DataScope.All if user has full access, etc.

            // Placeholder implementation:
            return await Task.FromResult(DataScope.Own);
        }

        public async Task<DataScope> GetUserDataScopeAsync(int userId, IEnumerable<string> permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any())
            {
                return DataScope.Own;
            }

            var scopes = new List<DataScope>();
            foreach (var permission in permissionNames)
            {
                var scope = await GetUserDataScopeAsync(userId, permission);
                scopes.Add(scope);
            }

            // Merge scopes to the highest level
            return MergeScopes(scopes);
        }

        private DataScope MergeScopes(IEnumerable<DataScope> scopes)
        {
            if (scopes.Contains(DataScope.All))
                return DataScope.All;
            if (scopes.Contains(DataScope.Branch))
                return DataScope.Branch;
            if (scopes.Contains(DataScope.Department))
                return DataScope.Department;
            return DataScope.Own;
        }

        public async Task<bool> HasAccessToDataAsync(int userId, string permissionName, int dataOwnerUserId, int dataDepartmentId, int dataBranchId)
        {
            var scope = await GetUserDataScopeAsync(userId, permissionName);

            return await CheckAccessByScope(userId, scope, dataOwnerUserId, dataDepartmentId, dataBranchId);
        }

        public async Task<bool> HasAccessToDataAsync(int userId, IEnumerable<string> permissionNames, int dataOwnerUserId, int dataDepartmentId, int dataBranchId)
        {
            var scope = await GetUserDataScopeAsync(userId, permissionNames);

            return await CheckAccessByScope(userId, scope, dataOwnerUserId, dataDepartmentId, dataBranchId);
        }

        private async Task<bool> CheckAccessByScope(int userId, DataScope scope, int dataOwnerUserId, int dataDepartmentId, int dataBranchId)
        {
            switch (scope)
            {
                case DataScope.All:
                    return true;
                case DataScope.Branch:
                    // TODO: Check if user's branch matches dataBranchId
                    // Placeholder: assume true for now
                    return true;
                case DataScope.Department:
                    // TODO: Check if user's department matches dataDepartmentId
                    // Placeholder: assume true for now
                    return true;
                case DataScope.Own:
                    return userId == dataOwnerUserId;
                default:
                    return false;
            }
        }
    }
}
