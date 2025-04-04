using System.ComponentModel.DataAnnotations;

namespace VisitTracker.API.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
