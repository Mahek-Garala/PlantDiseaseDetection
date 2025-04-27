using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlantDiseaseDetection.Models
{
    public class PlantPost
    {
        [Key]
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }


      //  [JsonIgnore]
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Reply> Replies { get; set; } = new List<Reply>();

    }
    }
