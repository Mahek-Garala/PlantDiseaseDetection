using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlantDiseaseDetection.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Hashed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
        public ICollection<PlantPost> PlantPosts { get; set; } = new List<PlantPost>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    }
}
