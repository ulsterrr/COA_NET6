using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Interfaces.Repositories
{
    public interface IBranchRepository : IRepository<Branch, int>
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<Branch> GetBranchByIdAsync(int id);
    }
}
