using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PlantDiseaseDetection.Services;
using PlantDiseaseDetection.Data;

namespace PlantDiseaseDetection.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authservice;
        private readonly AppDbContext _context; // Database context

        public AuthController(AuthService authservice, AppDbContext context)
        {
            _authservice = authservice;
            _context = context;
        }

        // ✅ Register (Sign-Up)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "All fields are required." });
            }

            try
            {
                var existingUser = await _authservice.GetUserByEmail(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "User already exists. Please log in." });
                }

                var token = await _authservice.RegisterUser(request.UserName, request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while registering.", Error = ex.Message });
            }
        }

        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Email and Password are required." });
            }

            try
            {
                var token = await _authservice.LoginUser(request.Email, request.Password);
                if (token == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while logging in.", Error = ex.Message });
            }
        }

        // ✅ Fetch User Profile (Protected)
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized(new { Message = "User not authenticated." });

                var user = await _context.Users
                    .Where(u => u.Email == userEmail)
                    .Select(u => new { u.UserName, u.Email }) // Fetch only necessary fields
                    .FirstOrDefaultAsync();

                if (user == null)
                    return NotFound(new { Message = "User not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching profile.", Error = ex.Message });
            }
        }

        // ✅ Update User Profile (Protected)
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized(new { Message = "User not authenticated." });

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                if (user == null)
                    return NotFound(new { Message = "User not found." });

                // Update user data
                user.UserName = request.UserName;
                user.Email = request.Email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Profile updated successfully!", user.UserName, user.Email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating profile.", Error = ex.Message });
            }
        }
    }

    // ✅ DTO for Login & Registration
    public class AuthRequest
    {
        public string? UserName { get; set; } // Used only for registration
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // ✅ DTO for Profile Update
    public class UpdateProfileRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
