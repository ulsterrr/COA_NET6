using Domain.Entities.Base;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Department : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<UserDepartment> UserDepartments { get; set; }
    }
}
