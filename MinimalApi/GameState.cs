using MiniTextAdventure;

namespace MiniApiTextAdv.Models
{
    public class GameState
    {
        public Room StartRoom { get; set; }
        
        public Dictionary<string, Room> PlayerRooms { get; set; } = new();
        
        public Dictionary<string, List<string>> PlayerInventories { get; set; } = new();
        
        public HashSet<string> UnlockedRooms { get; set; } = new();


        private readonly Dictionary<string, Room> _playerRooms = new();

        public Room GetRoomForUser(string username)
        {
            if (!_playerRooms.ContainsKey(username))
                _playerRooms[username] = StartRoom;

            return _playerRooms[username];
        }

    }

}