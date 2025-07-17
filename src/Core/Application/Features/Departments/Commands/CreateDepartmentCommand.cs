using Application.Interfaces.Repositories;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;
using Domain.Entities;
using MediatR;

namespace Application.Features.Departments.Commands
{
    public class CreateDepartmentCommand : IRequest<IResponse>
    {
        public string Name { get; set; }

        public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, IResponse>
        {
            private readonly IDepartmentRepository _departmentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
            {
                _departmentRepository = departmentRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<IResponse> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
            {
                var department = new Department { Name = request.Name };
                await _departmentRepository.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();
                return new SuccessResponse(200, "Department created successfully");
            }
        }
    }
}
