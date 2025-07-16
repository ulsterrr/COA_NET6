namespace Core.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<Role> Roles { get; set; }
    }
}
