namespace Core.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
