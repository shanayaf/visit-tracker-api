namespace VisitTracker.API.Dtos
{
    public class VisitDto
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string Status { get; set; } = "In Progress";
        public int StoreId { get; set; }
        public int UserId { get; set; }
    }
}
