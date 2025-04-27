<<<<<<< HEAD
﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
﻿using System.ComponentModel.DataAnnotations;
>>>>>>> 4d38ebd0c3e8e8964ac57243b815b8e427606332

namespace PlantDiseaseDetection.Models
{
    public class PlantImage
    {
        [Key]
<<<<<<< HEAD
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
=======
        public int ImageId { get; set; } //pk
        public string ImageUrl { get; set; } //from firebase
        public DateTime UploadedAt { get; set; }
        public double ConfidenceScore { get; set; } //from ML model

        public int UserId { get; set; } //many to one - who uploads
        public User User { get; set; }

        public int? DiseaseId { get; set; } //many to one - disease classification if detected(?)
        public Disease Disease { get; set; }

>>>>>>> 4d38ebd0c3e8e8964ac57243b815b8e427606332
    }
}
