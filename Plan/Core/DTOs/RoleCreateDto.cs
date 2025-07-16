namespace Core.DTOs
{
    public class RoleCreateDto
    {
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
