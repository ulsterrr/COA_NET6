using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Permission> Permissions { get; }
        IRepository<Menu> Menus { get; }
        IRepository<Branch> Branches { get; }
        IRepository<Department> Departments { get; }
        Task<int> CompleteAsync();
    }
}
