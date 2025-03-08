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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); // Secret key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Hashing algorithm
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Subject (User Email)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique Token ID
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Store User ID
                new Claim(ClaimTypes.Name, user.UserName) // Store Username
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"], // Who issued the token
                audience: _config["Jwt:Audience"], // Who can use the token
                claims: claims, // Payload (user identity)
                expires: DateTime.UtcNow.AddDays(1), // Expiry time
                signingCredentials: creds // Signing the token
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Convert token to string
        }
    }
}
