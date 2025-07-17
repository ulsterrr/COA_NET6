using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;

namespace Application.Features.Branches.Commands
{
    public class QuickAddOrUpdateBranchCommand : IRequest<IResponse>
    {
        public int? Id { get; set; } // null for add, set for update
        public string Name { get; set; }
    }

    public class QuickAddOrUpdateBranchCommandHandler : IRequestHandler<QuickAddOrUpdateBranchCommand, IResponse>
    {
        private readonly IBranchRepository _branchRepository;

        public QuickAddOrUpdateBranchCommandHandler(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<IResponse> Handle(QuickAddOrUpdateBranchCommand request, CancellationToken cancellationToken)
        {
            Branch branch;
            if (request.Id.HasValue)
            {
                branch = await _branchRepository.GetByIdAsync(request.Id.Value);
                if (branch == null)
                {
                    return new ErrorResponse(404, "Branch not found");
                }
                branch.Name = request.Name;
                _branchRepository.Update(branch);
            }
            else
            {
                branch = new Branch
                {
                    Name = request.Name
                };
                await _branchRepository.AddAsync(branch);
            }
            return new SuccessResponse(200, "Branch saved successfully");
        }
    }
}
