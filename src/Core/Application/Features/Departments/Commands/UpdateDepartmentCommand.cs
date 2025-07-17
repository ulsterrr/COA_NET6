using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using MediatR;

namespace Application.Features.Departments.Commands
{
    public class UpdateDepartmentCommand : IRequest<IResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, IResponse>
        {
            private readonly IDepartmentRepository _departmentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
            {
                _departmentRepository = departmentRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
            {
                var department = await _departmentRepository.GetByIdAsync(request.Id);
                if (department == null)
                {
                    return new ErrorResponse(404, "Department not found");
                }
                department.Name = request.Name;
                await _unitOfWork.SaveChangesAsync();
                return new SuccessResponse(200, "Department updated successfully");
            }
        }
    }
}
