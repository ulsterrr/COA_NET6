using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using Domain.Entities;
using MediatR;

namespace Application.Features.Branches.Commands
{
    public class CreateBranchCommand : IRequest<IResponse>
    {
        public string Name { get; set; }

        public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, IResponse>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IUnitOfWork _unitOfWork;

            public CreateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
            {
                _branchRepository = branchRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
            {
                var branch = new Branch { Name = request.Name };
                await _branchRepository.AddAsync(branch);
                await _unitOfWork.SaveChangesAsync();
                return new SuccessResponse(200, "Branch created successfully");
            }
        }
    }
}
