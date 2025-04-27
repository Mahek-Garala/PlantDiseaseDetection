using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlantDiseaseDetection.Models
{
    public class User
    {
        [Key]
<<<<<<< HEAD
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
=======
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
>>>>>>> 4d38ebd0c3e8e8964ac57243b815b8e427606332
