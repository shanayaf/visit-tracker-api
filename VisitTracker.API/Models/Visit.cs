using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VisitTracker.API.Models
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        public string Status { get; set; } = "In Progress";

        // Foreign keys
        public int UserId { get; set; }
        public int StoreId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [ForeignKey("StoreId")]
        public Store Store { get; set; } = null!;

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
