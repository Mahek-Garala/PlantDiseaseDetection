using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PlantDiseaseDetection.Controllers
{
    [Route("api/plant")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env; // For accessing wwwroot

        public HomeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // This endpoint matches your React app's second API call
        [HttpPost("save")]
        public async Task<IActionResult> SaveImage([FromForm] IFormFile image, [FromForm] string diseaseName)
        {
            try
            {
                // Check if required data is present
                if (image == null || image.Length == 0)
                {
                    return BadRequest(new { Message = "No image file provided" });
                }

                if (string.IsNullOrWhiteSpace(diseaseName))
                {
                    return BadRequest(new { Message = "Disease name is required" });
                }

                // Get user ID from the JWT token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    // Try alternate claim as fallback - depending on how your JWT is configured
                    var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(emailClaim))
                    {
                        return Unauthorized(new { Message = "User identification failed" });
                    }

                    // Look up user by email
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
                    if (user == null)
                    {
                        return Unauthorized(new { Message = "User not found" });
                    }

                    userId = user.UserId;
                }

                // Make sure uploads directory exists
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Create a unique filename to prevent conflicts
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the physical file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Create the URL path for the database (relative to website root)
                string imageUrl = $"/uploads/{uniqueFileName}";

                // Create and save the database record
                var plantImage = new PlantImage
                {
                    ImageUrl = imageUrl,
                    DiseaseName = diseaseName,
                    UserId = userId,
                    UploadedAt = DateTime.UtcNow
                };

                _context.PlantImages.Add(plantImage);
                await _context.SaveChangesAsync();

                // Return success with the created record data
                return Ok(new
                {
                    Message = "Image and disease data saved successfully",
                    Data = new
                    {
                        plantImage.ImageId,
                        plantImage.ImageUrl,
                        plantImage.DiseaseName,
                        plantImage.UploadedAt
                    }
                });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error saving image: {ex.Message}");
                return StatusCode(500, new { Message = "Error saving image", Error = ex.Message });
            }
        }

        // Get user's plant images (history)
        [HttpGet("history")]
        public async Task<IActionResult> GetUserHistory()
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId;

                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out userId))
                {
                    // Get user's plant images from database
                    var images = await _context.PlantImages
                        .Where(pi => pi.UserId == userId)
                        .OrderByDescending(pi => pi.UploadedAt)
                        .Select(pi => new
                        {
                            pi.ImageId,
                            pi.ImageUrl,
                            pi.DiseaseName,
                            pi.UploadedAt
                        })
                        .ToListAsync();

                    return Ok(images);
                }
                else
                {
                    // Try to get user by email as fallback
                    var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(emailClaim))
                    {
                        return Unauthorized(new { Message = "User identification failed" });
                    }

                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
                    if (user == null)
                    {
                        return Unauthorized(new { Message = "User not found" });
                    }

                    // Get user's plant images from database
                    var images = await _context.PlantImages
                        .Where(pi => pi.UserId == user.UserId)
                        .OrderByDescending(pi => pi.UploadedAt)
                        .Select(pi => new
                        {
                            pi.ImageId,
                            pi.ImageUrl,
                            pi.DiseaseName,
                            pi.UploadedAt
                        })
                        .ToListAsync();

                    return Ok(images);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving history", Error = ex.Message });
            }
        }

        // Get specific image details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageDetails(int id)
        {
            try
            {
                // Get plant image including related disease info
                var plantImage = await _context.PlantImages
                    .FirstOrDefaultAsync(pi => pi.ImageId == id);

                if (plantImage == null)
                {
                    return NotFound(new { Message = "Image not found" });
                }

                // Make sure the user owns this image
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    if (plantImage.UserId != userId)
                    {
                        return Forbid();
                    }
                }
                else
                {
                    // Try email claim as fallback
                    var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(emailClaim))
                    {
                        return Unauthorized(new { Message = "User identification failed" });
                    }

                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
                    if (user == null || plantImage.UserId != user.UserId)
                    {
                        return Forbid();
                    }
                }

                return Ok(new
                {
                    plantImage.ImageId,
                    plantImage.ImageUrl,
                    plantImage.DiseaseName,
                    plantImage.UploadedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving image details", Error = ex.Message });
            }
        }

        // Delete image
       
    }
}