using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using PlantDiseaseDetection.Models.DTOs;

namespace PlantDiseaseDetection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantPostController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantPostController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        /*

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlantPost>>> GetPosts()
        {
            return await _context.PlantPosts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Replies)
                    .ThenInclude(r => r.User) // 🛠 Include reply author
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        } */
         // GET: api/PlantPost
         [HttpGet]
         public async Task<ActionResult<IEnumerable<PlantPost>>> GetPosts()
         {
             return await _context.PlantPosts
                 .Include(p => p.User)
                 .Include(p => p.Likes)
                 .Include(p => p.Replies)
                     .ThenInclude(r => r.User)
                 .ToListAsync();
         }

         // POST: api/PlantPost
         /* [HttpPost]
          public async Task<ActionResult<PlantPost>> CreatePost([FromBody] PlantPost post)
          {

              _context.PlantPosts.Add(post);
              await _context.SaveChangesAsync();

              return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
          } 
          [HttpPost]
          public async Task<IActionResult> CreatePost([FromBody] PlantPost post)

          {

              if (!ModelState.IsValid)
                  return BadRequest(ModelState);

              foreach (var modelState in ModelState)
              {
                  Console.WriteLine($"{modelState.Key}: {string.Join(",", modelState.Value.Errors.Select(e => e.ErrorMessage))}");
              }

              post.CreatedAt = DateTime.UtcNow;

              _context.PlantPosts.Add(post);
              await _context.SaveChangesAsync();

              return Ok(post); // Or CreatedAtAction(...)
          }
         [HttpPost]
         public async Task<IActionResult> CreatePost([FromBody] PlantPostDto dto)
         {
             var newPost = new PlantPost
             {
                 PlantName = dto.PlantName,
                 Description = dto.Description,
                 ImageUrl = dto.ImageUrl,
                 CreatedAt = DateTime.UtcNow,
                 UserId = dto.UserId
             };

             _context.PlantPosts.Add(newPost);
             await _context.SaveChangesAsync();

             return Ok(newPost);
         }
         */
        [HttpPost("upload")]
        public async Task<IActionResult> CreatePost([FromForm] PlantPostDto postDto, [FromForm] IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { errors });
            }

            string imageUrl = null;

            if (imageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/{uniqueFileName}";
            }

            var post = new PlantPost
            {
                PlantName = postDto.PlantName,
                Description = postDto.Description,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow,
                UserId = postDto.UserId
            };

            _context.PlantPosts.Add(post);
            await _context.SaveChangesAsync();

            return Ok(post);
        }


    }
}
