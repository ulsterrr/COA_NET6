namespace Domain.Entities
{
    public class UserBranch
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public virtual User User { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
