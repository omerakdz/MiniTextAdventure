using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MiniApiTextAdv.Models;
using MiniApiTextAdv.Services;


namespace MiniApiTextAdv
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); 
            var jwtKey = builder.Configuration["JwtKey"] ?? "supersecretkey12345";

            builder.Services.AddSingleton<UserRepository>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.MapPost("/api/auth/register", AuthEndpoints.Register);
            app.MapPost("/api/auth/login", AuthEndpoints.Login);
            app.MapGet("/api/auth/me", AuthEndpoints.Me).RequireAuthorization();
            app.MapGet("/api/keys/keyshare/{roomId}", KeyEndpoints.GetKeyshare).RequireAuthorization();

            app.Run();
        }
    }
}
