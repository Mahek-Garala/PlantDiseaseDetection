using Microsoft.AspNetCore.Mvc;
using PlantDiseaseDetection.Services;
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
        /* public IActionResult Index()
         {
             return View();
         }*/

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "All fields are required." });
            try
            {
                var token = await _authservice.RegisterUser(request.UserName,request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "Email and Password are required." });
            try
            {
                var token = await _authservice.LoginUser(request.Email, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
    public class AuthRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
