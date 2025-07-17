using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using MediatR;

namespace Application.Features.Departments.Commands
{
    public class RemoveDepartmentCommand : IRequest<IResponse>
    {
        public int Id { get; set; }

        public class RemoveDepartmentCommandHandler : IRequestHandler<RemoveDepartmentCommand, IResponse>
        {
            private readonly IDepartmentRepository _departmentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public RemoveDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
            {
                _departmentRepository = departmentRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(RemoveDepartmentCommand request, CancellationToken cancellationToken)
            {
                var department = await _departmentRepository.GetByIdAsync(request.Id);
                if (department == null)
                {
                    return new ErrorResponse(404, "Department not found");
                }
                _departmentRepository.Remove(department);
                await _unitOfWork.SaveChangesAsync();
                return new SuccessResponse(200, "Department removed successfully");
            }
        }
    }
}
