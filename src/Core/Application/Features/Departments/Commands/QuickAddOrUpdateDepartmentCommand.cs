using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;

namespace Application.Features.Departments.Commands
{
    public class QuickAddOrUpdateDepartmentCommand : IRequest<IResponse>
    {
        public int? Id { get; set; } // null for add, set for update
        public string Name { get; set; }
    }

    public class QuickAddOrUpdateDepartmentCommandHandler : IRequestHandler<QuickAddOrUpdateDepartmentCommand, IResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public QuickAddOrUpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IResponse> Handle(QuickAddOrUpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            Department department;
            if (request.Id.HasValue)
            {
                department = await _departmentRepository.GetByIdAsync(request.Id.Value);
                if (department == null)
                {
                    return new ErrorResponse(404, "Department not found");
                }
                department.Name = request.Name;
                _departmentRepository.Update(department);
            }
            else
            {
                department = new Department
                {
                    Name = request.Name
                };
                await _departmentRepository.AddAsync(department);
            }
            return new SuccessResponse(200, "Department saved successfully");
        }
    }
}
