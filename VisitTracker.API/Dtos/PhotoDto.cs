namespace VisitTracker.API.Dtos
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int ProductId { get; set; }
        public string Base64Image { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }

  /*  public class CreatePhotoDto
    {
        public int ProductId { get; set; }
        public string Base64Image { get; set; } = string.Empty;
    }*/
}
