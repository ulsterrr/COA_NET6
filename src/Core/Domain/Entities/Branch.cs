using Domain.Entities.Base;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Branch : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<UserBranch> UserBranches { get; set; }
    }
}
