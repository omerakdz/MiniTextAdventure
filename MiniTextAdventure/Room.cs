using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTextAdventure
{
    public enum Direction {North,East,South,West}
    public class Room
    {
        public string Name { get; }
        public string Description { get; }
        public bool IsLethal { get; set; }
        public bool RequiresItem { get; set; }
        public string RequiredItemId { get; set; }
        public bool HasMonster { get; set; }
        public bool MonsterAlive { get; set; }
        public Dictionary<Direction, Room> Exits { get; }
        public List<string> Items { get; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            Exits = new Dictionary<Direction, Room>();
            Items = new List<string>();
        }

        public void Connect(Direction dir, Room target)
        {
            Exits[dir] = target;
        }

        public Room GetExit(Direction dir)
        {
            if (Exits.ContainsKey(dir))
            {
                return Exits[dir];
            }
            else
            {
                return null;
            }
        }
    }
}
