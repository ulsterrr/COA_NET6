using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    public class DepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentService(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<object>> GetDepartmentsGroupedByBranchAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();

            var grouped = departments
                .GroupBy(d => d.BranchId)
                .Select(g => new
                {
                    BranchId = g.Key,
                    Departments = g.Select(d => new { d.Id, d.Name }).ToList()
                });

            return grouped;
        }
    }
}
