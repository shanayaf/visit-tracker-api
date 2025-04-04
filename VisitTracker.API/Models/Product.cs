using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitTracker.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int? StoreId { get; set; }  


        // Navigation
        [ForeignKey("StoreId")]
        public Store Store { get; set; } = null!;
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
