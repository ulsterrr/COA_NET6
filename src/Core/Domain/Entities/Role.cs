using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
