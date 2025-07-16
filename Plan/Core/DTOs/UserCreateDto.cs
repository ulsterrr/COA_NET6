namespace Core.DTOs
{
    public class UserCreateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> BranchIds { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}
