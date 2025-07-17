using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class DepartmentRepository : EfEntityRepository<Department, CAContext, int>, IDepartmentRepository
    {
        public DepartmentRepository(CAContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }
    }
}
