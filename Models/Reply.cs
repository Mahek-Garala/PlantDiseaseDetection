using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlantDiseaseDetection.Models
{
    public class Reply
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        [JsonIgnore]
        public PlantPost Post { get; set; }

        public int UserId { get; set; }
      
        public User User { get; set; }
    }

}
