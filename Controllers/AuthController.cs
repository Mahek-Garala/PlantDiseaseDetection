using Microsoft.AspNetCore.Mvc;
using PlantDiseaseDetection.Services;
using System;
using System.Threading.Tasks;

namespace PlantDiseaseDetection.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authservice;

        public AuthController(AuthService authservice)
        {
            _authservice = authservice;
        }

        // ✅ Register (Sign-Up) Endpoint
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
                // Check if user already exists
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
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ✅ Login Endpoint
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
                    return BadRequest(new { Message = "Invalid email or password." });
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    // ✅ Single DTO for both Login and Register
    public class AuthRequest
    {
        public string? UserName { get; set; } // Used only for registration
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
