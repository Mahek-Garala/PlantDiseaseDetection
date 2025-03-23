using Microsoft.EntityFrameworkCore;

namespace PlantDiseaseDetection.Models
{
    public class Disease
    {
        public int DiseaseId { get; set; } //pk
        public string DiseaseName { get; set; }
        public string PlantName { get; set; }

        //public string Symptoms { get; set; }
        public string Solution { get; set; }


        // Ensure uniqueness in DB
       // [Index(IsUnique = true)]
       // public string UniqueKey => $"{PlantName}_{DiseaseName}".ToLower(); // Unique key

        public List<PlantImage> PlantImages { get; set; } = new List<PlantImage>(); // one  to many - can have multiple classified plant images 
                                                                                    //  public ICollection<Recommendation> Recommendations { get; set; }// one to many - can have multiple recomm.

    }
}
