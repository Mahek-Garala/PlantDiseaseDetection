using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using PlantDiseaseDetection.Models.DTOs;

namespace PlantDiseaseDetection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikeController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Like
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] LikeDto dto)
        {
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == dto.PostId && l.UserId == dto.UserId);

            if (existingLike != null)
            {
                return Conflict("User has already liked this post.");
            }

            var like = new Like
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok(like);
        }
        // DELETE: api/Like
        [HttpDelete]
        public async Task<IActionResult> UnlikePost([FromBody] LikeDto dto)
        {
            var existing = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == dto.UserId && l.PostId == dto.PostId);

            if (existing == null)
            {
                return NotFound("Like not found.");
            }

            _context.Likes.Remove(existing);
            await _context.SaveChangesAsync();
            return Ok("Like removed.");
        }


    }
}
