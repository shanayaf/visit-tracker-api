namespace VisitTracker.API.Dtos
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateStoreDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
