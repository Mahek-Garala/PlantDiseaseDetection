using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlantDiseaseDetection.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ✅ Register New User
        public async Task<string> RegisterUser(string username, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new Exception("Email already exists. Please log in.");

            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                UserName = username,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                Password = hasher.HashPassword(null, password) // Hashing password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateToken(user);
        }

        // ✅ Login User
        public async Task<string> LoginUser(string email, string password)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
                throw new Exception("Invalid email or password.");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid email or password.");

            return GenerateToken(user);
        }

        // ✅ Get User by Email (Helper Method)
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // ✅ Generate JWT Token
        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Email as subject
        new Claim(JwtRegisteredClaimNames.Email, user.Email), // Ensure email claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("username", user.UserName)
    };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
