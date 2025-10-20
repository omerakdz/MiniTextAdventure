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
        private string name;
        public string Name 
        {
            get {  return name; } 
        }

        private string description;
        public string Description 
        {
            get { return description; }
        }

        private bool isLethal;
        public bool IsLethal 
        
        {
            get { return isLethal;}
            set { isLethal = value;}
        }

        private bool requiresItem;
        public bool RequiresItem 
        {
            get { return requiresItem;}
            set { requiresItem = value;}
        }

        private string requiredItemId;
        public string RequiredItemId 
        {
            get { return requiredItemId;}
            set { requiredItemId = value;}
        }

        private bool hasMonster;
        public bool HasMonster 
        {
            get { return hasMonster;}
            set {  hasMonster = value;}
        }

        public bool monsterAlive;
        public bool MonsterAlive 
        {
            get { return monsterAlive;}
            set { monsterAlive = value;}
        }

        private Dictionary<Direction, Room> exits;
        public Dictionary<Direction, Room> Exits 
        {
            get { return  exits;}
        }

        private List<string> items;
        public List<string> Items 
        {
            get { return items;}
        }

        public Room(string name, string description)
        {
            this.name = name;
            this.description = description;
            exits = new Dictionary<Direction, Room>();
            this.items = new List<string>();
        }

        public void Connect(Direction direction, Room target)
        {
            Exits[direction] = target;
        }

        public Room GetExit(Direction direction)
        {
            if (Exits.ContainsKey(direction))
            {
                return Exits[direction];
            }
            else
            {
                return null;
            }
        }
    }
}
