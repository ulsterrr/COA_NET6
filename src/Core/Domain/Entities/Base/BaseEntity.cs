using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base
{
    public class BaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
        public bool? Enable { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
