using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MiniApiTextAdv.Models;
using MiniApiTextAdv.Services;

namespace MiniApiTextAdv
{
    public static class AuthEndpoints
    {
        public static IResult Register([FromBody] RegisterRequest request, UserRepository repo)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Results.BadRequest("Username en password zijn verplicht.");
            }

            if (repo.Exists(request.Username))
            {
                return Results.BadRequest("Gebruiker bestaat al.");
            }
            
            string hash = ComputeSha256(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                Role = request.Role ?? "Player"
            };

            repo.Add(user);

            return Results.Ok("Registratie gelukt.");
        }

 
        public static IResult Login([FromBody] LoginRequest request, UserRepository repo, string jwtKey)
        {
            var user = repo.GetByUsername(request.Username);
            if (user == null)
                return Results.BadRequest("Gebruiker bestaat niet.");

            var hash = ComputeSha256(request.Password);
            if (hash != user.PasswordHash)
                return Results.BadRequest("Ongeldige login.");

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { token = tokenString });
        }

            
        

        
        public static IResult Me(ClaimsPrincipal user)
        {
            var username = user.Identity?.Name;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            return Results.Ok(new { username, role });
        }
        private static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        private static string GenerateJwt(User user, string jwtKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

 
    public class RegisterRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string? Role { get; set; } = "Player";
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
