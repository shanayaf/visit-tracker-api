namespace VisitTracker.API.Dtos
{
    public class VisitQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "VisitDate";
        public string? SortOrder { get; set; } = "desc";
        public string? Status { get; set; }
        public int? StoreId { get; set; }
    }
}
