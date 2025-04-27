using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlantDiseaseDetection.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK to Post
        public int PostId { get; set; }
        [JsonIgnore]
        public PlantPost Post { get; set; }

        // FK to User
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }

}
