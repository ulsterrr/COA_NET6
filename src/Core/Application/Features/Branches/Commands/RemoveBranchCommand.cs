using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using MediatR;

namespace Application.Features.Branches.Commands
{
    public class RemoveBranchCommand : IRequest<IResponse>
    {
        public int Id { get; set; }

        public class RemoveBranchCommandHandler : IRequestHandler<RemoveBranchCommand, IResponse>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IUnitOfWork _unitOfWork;

            public RemoveBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
            {
                _branchRepository = branchRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(RemoveBranchCommand request, CancellationToken cancellationToken)
            {
                var branch = await _branchRepository.GetByIdAsync(request.Id);
                if (branch == null)
                {
                    return new ErrorResponse(404, "Branch not found");
                }
                _branchRepository.Remove(branch);
                await _unitOfWork.SaveChangesAsync();
                return new SuccessResponse(200, "Branch removed successfully");
            }
        }
    }
}
