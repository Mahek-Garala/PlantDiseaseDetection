using System.ComponentModel.DataAnnotations;

namespace PlantDiseaseDetection.Models
{
    public class PlantImage
    {
        [Key]
        public int ImageId { get; set; } //pk
        public string ImageUrl { get; set; } //from firebase
        public DateTime UploadedAt { get; set; }
        public double ConfidenceScore { get; set; } //from ML model

        public int UserId { get; set; } //many to one - who uploads
        public User User { get; set; }

        public int? DiseaseId { get; set; } //many to one - disease classification if detected(?)
        public Disease Disease { get; set; }

    }
}
