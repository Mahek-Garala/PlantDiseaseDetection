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
        public DbSet<PlantPost> PlantPosts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
      
        // public DbSet<Recommendation> Recommendations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Post
            modelBuilder.Entity<User>()
                .HasMany(u => u.PlantPosts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // User - Like
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post - Like
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint: One like per post per user
            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.PostId })
                .IsUnique();

            // Post - Reply
            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Reply
            modelBuilder.Entity<Reply>()
                .HasOne(r => r.User)
                .WithMany(u => u.Replies)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
