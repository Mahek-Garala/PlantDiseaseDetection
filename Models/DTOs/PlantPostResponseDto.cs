
namespace PlantDiseaseDetection.Models.DTOs
{
    public class PlantPostResponseDto
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }

        public List<string> Replies { get; set; } = new();
        public int LikeCount { get; set; }
    }
}