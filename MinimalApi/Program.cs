using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MiniApiTextAdv.Models;
using MiniApiTextAdv.Services;
using MiniTextAdventure;


namespace MiniApiTextAdv
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            var builder = WebApplication.CreateBuilder(args); 
            var jwtKey = builder.Configuration["JwtKey"] ?? "ThisIsA_VeryLongJwtKey_ForDevOnly_1234567890";

            builder.Services.AddSingleton<UserRepository>();
            
            // GameState toevoegen
            builder.Services.AddSingleton<GameState>(sp =>
            {
                var gs = new GameState();
                
                // Maak kamers
                var entrance = new Room("Ingang", "Je staat in een donkere gang.");
                var treasureRoom = new Room("Schatzkamer", "Goud en juwelen overal!");
                var monsterRoom = new Room("Monster Kamer", "ROOOAR!");
                var exit = new Room("Exit", "Vrijheid!");
                
                // Connect kamers
                entrance.Connect(Direction.North, treasureRoom);
                treasureRoom.Connect(Direction.South, entrance);
                treasureRoom.Connect(Direction.East, monsterRoom);
                monsterRoom.Connect(Direction.West, treasureRoom);
                monsterRoom.Connect(Direction.North, exit);
                
                // Items toevoegen
                entrance.Items.Add("key");
                treasureRoom.Items.Add("sword");
                
                // Monster setup
                monsterRoom.HasMonster = true;
                monsterRoom.MonsterAlive = true;
                monsterRoom.IsLethal = true;
                
                gs.StartRoom = entrance;
                return gs;
            });
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"[JWT] Auth failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine($"[JWT] Token valid! User: {context.Principal?.Identity?.Name}");
                            return Task.CompletedTask;
                        }
                    };

                });

            builder.Services.AddAuthorization();

            var app = builder.Build();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPost("/api/auth/register",
                (RegisterRequest req, UserRepository repo) =>
                    AuthEndpoints.Register(req, repo));

            app.MapPost("/api/auth/login",
                (LoginRequest req, UserRepository repo) =>
                    AuthEndpoints.Login(req, repo, jwtKey));

            app.MapGet("/api/auth/me",
                    (ClaimsPrincipal user) =>
                        AuthEndpoints.Me(user))
                .RequireAuthorization();

            app.MapGet("/api/keys/keyshare/{roomId}", KeyEndpoints.GetKeyshare).RequireAuthorization();

            // Game endpoints
            app.MapGet("/api/game/current-room",
                (ClaimsPrincipal user, GameState gameState) =>
                    GameEndpoints.GetCurrentRoom(user, gameState))
                .RequireAuthorization();

            app.MapPost("/api/game/move",
                (ClaimsPrincipal user, GameState gameState, MoveRequest req) =>
                    GameEndpoints.Move(user, gameState, req))
                .RequireAuthorization();

            app.MapPost("/api/game/take",
                (ClaimsPrincipal user, GameState gameState, TakeRequest req) =>
                    GameEndpoints.Take(user, gameState, req))
                .RequireAuthorization();

            app.MapPost("/api/game/fight",
                (ClaimsPrincipal user, GameState gameState) =>
                    GameEndpoints.Fight(user, gameState))
                .RequireAuthorization();

            app.MapGet("/api/game/inventory",
                (ClaimsPrincipal user, GameState gameState) =>
                    GameEndpoints.GetInventory(user, gameState))
                .RequireAuthorization();

            app.Run();
        }
    }
}
