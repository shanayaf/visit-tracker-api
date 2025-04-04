using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VisitTracker.API.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Base64Image { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int VisitId { get; set; }
        public int ProductId { get; set; }

        
        [JsonIgnore]
        [ForeignKey("VisitId")]
        public Visit Visit { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;
    }
}
