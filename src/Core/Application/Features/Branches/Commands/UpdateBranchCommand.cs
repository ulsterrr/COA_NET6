using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using MediatR;

namespace Application.Features.Branches.Commands
{
    public class UpdateBranchCommand : IRequest<IResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, IResponse>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IUnitOfWork _unitOfWork;

            public UpdateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
            {
                _branchRepository = branchRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
            {
                var branch = await _branchRepository.GetByIdAsync(request.Id);
                if (branch == null)
                {
                    return new ErrorResponse(404, "Branch not found");
                }
                branch.Name = request.Name;
                await _unitOfWork.SaveChangesAsync();
                return new SuccessResponse(200, "Branch updated successfully");
            }
        }
    }
}
