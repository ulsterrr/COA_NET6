namespace Core.Entities
{
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public List<Role> Roles { get; set; }
    public List<Branch> Branches { get; set; }
    public List<Department> Departments { get; set; }
}
}
