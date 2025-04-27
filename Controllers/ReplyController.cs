using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using PlantDiseaseDetection.Models.DTOs;

namespace PlantDiseaseDetection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReplyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReplyController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Reply
    
        // In your ReplyController.cs
        [HttpPost]
        public async Task<ActionResult<Reply>> PostReply(ReplyDto replyDto)
        {
            var reply = new Reply
            {
                PostId = replyDto.PostId,
                UserId = replyDto.UserId,
                Comment = replyDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            // Load the complete reply with user before returning
            var completeReply = await _context.Replies
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reply.Id);

            return completeReply;
        }

        // GET: api/Reply/post/{postId}

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<Reply>>> GetRepliesForPost(int postId)
        {
            var replies = await _context.Replies
        .Include(r => r.User) // Include User data
        .Where(r => r.PostId == postId)
        .OrderBy(r => r.CreatedAt)
        .ToListAsync();

            return replies;

        }
    }
}/*
  *     [HttpPost]
        public async Task<IActionResult> AddReply([FromBody] ReplyDto dto)
        {
            var reply = new Reply
            {
                Comment = dto.Comment,
                PostId = dto.PostId,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            return Ok(reply);
        }
return await _context.Replies
               .Where(r => r.PostId == postId)
               .Include(r => r.User)
               .ToListAsync();*/