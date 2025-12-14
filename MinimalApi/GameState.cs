using MiniTextAdventure;

namespace MiniApiTextAdv.Models
{
    public class GameState
    {
        public Room StartRoom { get; set; }
        
        public Dictionary<string, Room> PlayerRooms { get; set; } = new();
        
        public Dictionary<string, List<string>> PlayerInventories { get; set; } = new();
        
        public HashSet<string> UnlockedRooms { get; set; } = new();
    }
}