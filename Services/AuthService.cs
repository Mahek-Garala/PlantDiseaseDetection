using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantDiseaseDetection.Data;
using PlantDiseaseDetection.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<string> RegisterUser(string username,string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new Exception("Email already Exist!!");

            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                UserName = username,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };
            user.Password = hasher.HashPassword(user,password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async Task<string> LoginUser(string email,string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("Invalid details");

            var hasher = new PasswordHasher<User>();
            var res = hasher.VerifyHashedPassword(user,user.Password,password);
            if (res == PasswordVerificationResult.Failed)
                throw new Exception("Invalid password");

            return GenerateToken(user);

        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); //Secret key
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256); // hashing algo
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Subject (User Email)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) , // Unique Token ID
                new Claim("username", user.UserName) // Include username in token

            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"], // Who issued the token ; Issuer - backend api
                _config["Jwt:Audience"], // Who issued the token
                claims,  // Payload (user identity)
                expires: DateTime.UtcNow.AddDays(1), // Expiry Time
                signingCredentials: creds //Signing the token
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  // Convert token to string
        }


    }
}
