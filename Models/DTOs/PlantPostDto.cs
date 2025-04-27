

namespace PlantDiseaseDetection.Models.DTOs
{
    public class PlantPostDto
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string Description { get; set; }

        public string? ImageUrl { get; set; } // Make nullable

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public List<LikeDto>? Likes { get; set; } = new(); // Optional with default
        public List<ReplyDto>? Replies { get; set; } = new(); // Optional with default
    }

}

