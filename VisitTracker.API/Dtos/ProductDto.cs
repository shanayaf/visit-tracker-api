namespace VisitTracker.API.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int StoreId { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

       
    }
}
