using System;
using System.Collections.Generic;
namespace PlantDiseaseDetection.Models
{
    public class User
    {
        public int UserId { get; set; } //pk
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // will be hashed
        public DateTime CreatedAt { get; set; }
        public List<PlantImage> PlantImages { get; set; } = new List<PlantImage>(); //for one to many - upload many images

    }
}
