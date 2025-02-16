using Microsoft.EntityFrameworkCore;
using PlantDiseaseDetection.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace PlantDiseaseDetection.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        // public DbSet<Recommendation> Recommendations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.PlantImages)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Disease>()
                .HasMany(p => p.PlantImages)
                .WithOne(r => r.Disease)
                .HasForeignKey(r => r.ImageId);
        }
    }
}
