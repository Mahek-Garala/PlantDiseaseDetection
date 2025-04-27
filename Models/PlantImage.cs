using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantDiseaseDetection.Models
{
    public class PlantImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string DiseaseName { get; set; }

        [Required]
        public DateTime UploadedAt { get; set; }

        [Required]
        public int UserId { get; set; }

        // Uncomment if you want to create a navigation property to User
        // [ForeignKey("UserId")]
        // public User User { get; set; }
    }
}
