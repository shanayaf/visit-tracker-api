namespace VisitTracker.API.Dtos
{
    public class CreatePhotoDto
    {
        public int ProductId { get; set; }
        public string Base64Image { get; set; } = string.Empty;
    }
}
