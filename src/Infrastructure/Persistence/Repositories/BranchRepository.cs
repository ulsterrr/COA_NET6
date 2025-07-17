using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class BranchRepository : EfEntityRepository<Branch, CAContext, int>, IBranchRepository
    {
        public BranchRepository(CAContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _context.Branches.ToListAsync();
        }

        public async Task<Branch> GetBranchByIdAsync(int id)
        {
            return await _context.Branches.FindAsync(id);
        }
    }
}
