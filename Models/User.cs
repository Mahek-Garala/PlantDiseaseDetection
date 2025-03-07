using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlantDiseaseDetection.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } //pk
        [Required]
        public string UserName { get; set; }
        [Required , EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } // will be hashed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<PlantImage> PlantImages { get; set; } = new List<PlantImage>(); //for one to many - upload many images

    }
}
