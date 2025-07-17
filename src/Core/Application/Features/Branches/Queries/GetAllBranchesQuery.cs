using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using Domain.Entities;
using MediatR;

namespace Application.Features.Branches.Queries
{
    public class GetAllBranchesQuery : IRequest<IResponse>
    {
        public class GetAllBranchesQueryHandler : IRequestHandler<GetAllBranchesQuery, IResponse>
        {
            private readonly IBranchRepository _branchRepository;

            public GetAllBranchesQueryHandler(IBranchRepository branchRepository)
            {
                _branchRepository = branchRepository;
            }

            public async Task<IResponse> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
            {
                var branches = await _branchRepository.GetAllBranchesAsync();
                return new DataResponse<IEnumerable<Branch>>(branches, 200);
            }
        }
    }
}
