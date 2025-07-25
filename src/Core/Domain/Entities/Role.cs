﻿using Domain.Entities.Base;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
