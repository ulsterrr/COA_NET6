using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IDepartmentRepository : IRepository<Department, int>
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
    }
}
