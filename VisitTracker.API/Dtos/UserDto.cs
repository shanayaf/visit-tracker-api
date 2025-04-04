using VisitTracker.API.Models;

namespace VisitTracker.API.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<VisitDto> Visits { get; set; } = new();
    }
}