using Domain.Entities.Base;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Permission : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
