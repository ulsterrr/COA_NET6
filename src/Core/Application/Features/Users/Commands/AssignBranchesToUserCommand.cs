using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class AssignBranchesToUserCommand : IRequest<IResponse>
    {
        public int UserId { get; set; }
        public List<int> BranchIds { get; set; }

        public class AssignBranchesToUserCommandHandler : IRequestHandler<AssignBranchesToUserCommand, IResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly IBranchRepository _branchRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AssignBranchesToUserCommandHandler(IUserRepository userRepository, IBranchRepository branchRepository, IUnitOfWork unitOfWork)
            {
                _userRepository = userRepository;
                _branchRepository = branchRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(AssignBranchesToUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ErrorResponse(404, "User not found");
                }

                var branches = await _branchRepository.GetAllBranchesAsync();
                var validBranches = branches.Where(b => request.BranchIds.Contains(b.Id)).ToList();

                user.UserBranches = validBranches.Select(b => new UserBranch { UserId = user.Id, BranchId = b.Id }).ToList();

                await _unitOfWork.SaveChangesAsync();

                return new SuccessResponse(200, "Branches assigned to user successfully");
            }
        }
    }
}
