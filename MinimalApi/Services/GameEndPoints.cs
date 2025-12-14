using MiniApiTextAdv.Models;
using System.Security.Claims;
using MiniTextAdventure;

namespace MiniApiTextAdv
{
    public static class GameEndpoints
    {
        public static IResult GetCurrentRoom(ClaimsPrincipal user, GameState gameState)
        {
            var username = user.Identity?.Name;
            if (!gameState.PlayerRooms.ContainsKey(username))
                gameState.PlayerRooms[username] = gameState.StartRoom;

            var currentRoom = gameState.PlayerRooms[username];
            var dto = RoomToDto(currentRoom);

            return Results.Ok(dto);
        }

        public static IResult Move(ClaimsPrincipal user, GameState gameState, MoveRequest req)
        {
            var username = user.Identity?.Name;
            if (!gameState.PlayerRooms.ContainsKey(username))
                gameState.PlayerRooms[username] = gameState.StartRoom;

            var currentRoom = gameState.PlayerRooms[username];
            Direction direction = ParseDirection(req.Direction);

            var nextRoom = currentRoom.GetExit(direction);
            if (nextRoom == null)
                return Results.Ok(new MoveResultDto
                {
                    Success = false,
                    Message = "Je kunt hier niet heen.",
                    PlayerDied = false,
                    PlayerWon = false
                });

            // Check keyshare (client haalt dit op, maar we valideren hier)
            if (nextRoom.RequiresKeyshare && !gameState.UnlockedRooms.Contains(username + "_" + nextRoom.Name))
            {
                return Results.Ok(new MoveResultDto
                {
                    Success = false,
                    Message = "Deze kamer is beveiligd. Keyshare nodig.",
                    PlayerDied = false,
                    PlayerWon = false
                });
            }

            // Check lethal
            if (nextRoom.IsLethal && nextRoom.HasMonster && nextRoom.MonsterAlive)
            {
                gameState.PlayerRooms[username] = nextRoom;
                return Results.Ok(new MoveResultDto
                {
                    Success = false,
                    Message = "Je bent dood. GAME OVER.",
                    CurrentRoom = RoomToDto(nextRoom),
                    PlayerDied = true,
                    PlayerWon = false
                });
            }

            // Win condition (voorbeeld: specifieke kamer)
            if (nextRoom.Name == "Exit")
            {
                gameState.PlayerRooms[username] = nextRoom;
                return Results.Ok(new MoveResultDto
                {
                    Success = true,
                    Message = "Je ontsnapt! Je wint!",
                    CurrentRoom = RoomToDto(nextRoom),
                    PlayerDied = false,
                    PlayerWon = true
                });
            }

            gameState.PlayerRooms[username] = nextRoom;
            return Results.Ok(new MoveResultDto
            {
                Success = true,
                Message = $"Je gaat naar {nextRoom.Name}.",
                CurrentRoom = RoomToDto(nextRoom),
                PlayerDied = false,
                PlayerWon = false
            });
        }

        public static IResult Take(ClaimsPrincipal user, GameState gameState, TakeRequest req)
        {
            var username = user.Identity?.Name;
            if (!gameState.PlayerRooms.ContainsKey(username))
                return Results.BadRequest("Speler niet gevonden in game state.");

            var currentRoom = gameState.PlayerRooms[username];
            
            if (!currentRoom.Items.Contains(req.ItemId))
                return Results.Ok(new { message = "Dit item is hier niet." });

            currentRoom.Items.Remove(req.ItemId);
            
            if (!gameState.PlayerInventories.ContainsKey(username))
                gameState.PlayerInventories[username] = new List<string>();

            gameState.PlayerInventories[username].Add(req.ItemId);

            return Results.Ok(new { message = $"Je hebt {req.ItemId} opgepakt." });
        }

        public static IResult Fight(ClaimsPrincipal user, GameState gameState)
        {
            var username = user.Identity?.Name;
            if (!gameState.PlayerRooms.ContainsKey(username))
                return Results.BadRequest("Speler niet gevonden in game state.");

            var currentRoom = gameState.PlayerRooms[username];

            if (!currentRoom.HasMonster)
                return Results.Ok(new { success = false, message = "Er is hier geen monster." });

            if (!currentRoom.MonsterAlive)
                return Results.Ok(new { success = false, message = "Het monster is al dood." });

            // Check inventory for weapon
            var inventory = gameState.PlayerInventories.ContainsKey(username) 
                ? gameState.PlayerInventories[username] 
                : new List<string>();

            if (!inventory.Contains("sword"))
                return Results.Ok(new { success = false, message = "Je hebt geen zwaard! Het monster verslaat je. GAME OVER." });

            currentRoom.MonsterAlive = false;
            return Results.Ok(new { success = true, message = "Je verslaat het monster! De kamer is nu veilig." });
        }

        public static IResult GetInventory(ClaimsPrincipal user, GameState gameState)
        {
            var username = user.Identity?.Name;
            var inventory = gameState.PlayerInventories.ContainsKey(username)
                ? gameState.PlayerInventories[username]
                : new List<string>();

            return Results.Ok(new InventoryDto { Items = inventory.Select(id => new ItemDto { Id = id, Name = id }).ToList() });
        }

        private static RoomDto RoomToDto(Room room)
        {
            return new RoomDto
            {
                Name = room.Name,
                Description = room.Description,
                Items = room.Items.Select(id => new ItemDto { Id = id, Name = id, Description = "" }).ToList(),
                HasMonster = room.HasMonster,
                MonsterAlive = room.MonsterAlive,
                RequiresKeyshare = room.RequiresKeyshare,
                RequiredKeyId = room.RequiredKeyId
            };
        }

        private static Direction ParseDirection(string dir)
        {
            return dir.ToLower() switch
            {
                "n" => Direction.North,
                "e" => Direction.East,
                "s" => Direction.South,
                "w" => Direction.West,
                _ => Direction.North
            };
        }
    }
}
