using System;
using System.Data;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _dbConnection;

        public UnitOfWork(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            Users = new DapperUserRepository(_dbConnection);
            Roles = new DapperRoleRepository(_dbConnection);
            Permissions = new DapperPermissionRepository(_dbConnection);
            Menus = new DapperMenuRepository(_dbConnection);
            Branches = new DapperBranchRepository(_dbConnection);
            Departments = new DapperDepartmentRepository(_dbConnection);
        }

        public IRepository<User> Users { get; private set; }
        public IRepository<Role> Roles { get; private set; }
        public IRepository<Permission> Permissions { get; private set; }
        public IRepository<Menu> Menus { get; private set; }
        public IRepository<Branch> Branches { get; private set; }
        public IRepository<Department> Departments { get; private set; }

        public Task<int> CompleteAsync()
        {
            // Implement transaction commit if needed
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Dispose();
            }
        }
    }
}
