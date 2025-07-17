using System.Collections.Generic;

namespace Application.Dtos
{
    public class OrganizationTreeDto
    {
        public List<BranchNode> Branches { get; set; }
        public List<DepartmentNode> Departments { get; set; }
    }

    public class BranchNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
