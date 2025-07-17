using System.Collections.Generic;

namespace Application.Dtos
{
    public class RoleWithPermissionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
}
