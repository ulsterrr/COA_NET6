using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class AssignDepartmentsToUserCommand : IRequest<IResponse>
    {
        public int UserId { get; set; }
        public List<int> DepartmentIds { get; set; }

        public class AssignDepartmentsToUserCommandHandler : IRequestHandler<AssignDepartmentsToUserCommand, IResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly IDepartmentRepository _departmentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AssignDepartmentsToUserCommandHandler(IUserRepository userRepository, IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
            {
                _userRepository = userRepository;
                _departmentRepository = departmentRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(AssignDepartmentsToUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    return new ErrorResponse(404, "User not found");
                }

                var departments = await _departmentRepository.GetAllDepartmentsAsync();
                var validDepartments = departments.Where(d => request.DepartmentIds.Contains(d.Id)).ToList();

                user.UserDepartments = validDepartments.Select(d => new UserDepartment { UserId = user.Id, DepartmentId = d.Id }).ToList();

                await _unitOfWork.SaveChangesAsync();

                return new SuccessResponse(200, "Departments assigned to user successfully");
            }
        }
    }
}
