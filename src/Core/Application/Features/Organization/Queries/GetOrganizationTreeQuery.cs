using Application.Dtos;
using Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Organization.Queries
{
    public class GetOrganizationTreeQuery : IRequest<OrganizationTreeDto>
    {
    }

    public class GetOrganizationTreeQueryHandler : IRequestHandler<GetOrganizationTreeQuery, OrganizationTreeDto>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        public GetOrganizationTreeQueryHandler(IBranchRepository branchRepository, IDepartmentRepository departmentRepository, IUserRepository userRepository)
        {
            _branchRepository = branchRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<OrganizationTreeDto> Handle(GetOrganizationTreeQuery request, CancellationToken cancellationToken)
        {
            var branches = await _branchRepository.GetAllAsync();
            var departments = await _departmentRepository.GetAllAsync();
            var users = await _userRepository.GetAllUsersWithRolesAsync();

            var tree = new OrganizationTreeDto
            {
                Branches = branches.Select(b => new Dtos.BranchNode
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList(),
                Departments = departments.Select(d => new Dtos.DepartmentNode
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList(),
                Users = users.Select(u => new Dtos.UserNode
                {
                    Id = u.Id,
                    Name = u.UserName
                }).ToList()
            };

            return tree;
        }
    }
}
