using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlantDiseaseDetection.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public HomeController(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] int userId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                // Convert image to base64 string
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                string base64Image = Convert.ToBase64String(imageBytes);

                // Send image to ML API for disease detection
                var response = await _httpClient.PostAsync("http://localhost:5000/api/detect",
                    new StringContent(JsonSerializer.Serialize(new { image = base64Image }), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "ML API error.");

                var result = JsonSerializer.Deserialize<DiseasePrediction>(await response.Content.ReadAsStringAsync());

                // Store in database
                var plantImage = new PlantImage
                {
                    ImageUrl = $"data:image/jpeg;base64,{base64Image}",
                    UploadedAt = DateTime.UtcNow,
                    ConfidenceScore = result?.ConfidenceScore ?? 0.0,
                    UserId = userId,
                    DiseaseId = result?.DiseaseId
                };

                _context.PlantImages.Add(plantImage);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Image processed successfully.", plantImage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class DiseasePrediction
    {
        public int? DiseaseId { get; set; }
        public double ConfidenceScore { get; set; }
    }
}
