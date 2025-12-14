using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MiniApiTextAdv.Services;

namespace MiniApiTextAdv
{
    public static class KeyEndpoints
    {
        // Simpele keyshares per kamer (student-level)
        private static readonly Dictionary<string, string> Keyshares = new()
        {
            { "room1", "KEYSHARE-R1" },
            { "room2", "KEYSHARE-R2!" }
        };

        [Authorize]
        public static IResult GetKeyshare(string roomId, ClaimsPrincipal user)
        {
            if (!Keyshares.ContainsKey(roomId))
                return Results.NotFound("Onbekende kamer.");
            
            var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "Player";
            
            if (role == "Admin")
                return Results.Ok(new { keyshare = Keyshares[roomId] });
            
            if (role == "Player" && roomId == "room1")
                return Results.Ok(new { keyshare = Keyshares[roomId] });
            
            return Results.Forbid();
        }
    }
}
