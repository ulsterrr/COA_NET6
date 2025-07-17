using Domain.Entities.Base;

namespace Domain.Entities
{
    public class User : BaseEntity<int>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserBranch> UserBranches { get; set; }
        public virtual ICollection<UserDepartment> UserDepartments { get; set; }
        public string EmailConfirmationCode { get; set; }
        public string EmailConfirmedCode { get; set; }
        public string ResetPasswordCode { get; set; }

        // Added Roles property to get list of role names
        public List<string> Roles => UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>();
    }
}