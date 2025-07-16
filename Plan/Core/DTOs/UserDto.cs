namespace Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Branches { get; set; }
        public List<string> Departments { get; set; }
    }
}
