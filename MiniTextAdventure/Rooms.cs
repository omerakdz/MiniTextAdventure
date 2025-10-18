using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTextAdventure
{
    public enum MoveResult{ Moved, BlockedMissingKey, Died, Won,InvalidDirection }
    public class Rooms
    {
        public Dictionary<string, Room> AllRooms { get; private set; }
        public Room CurrentRoom { get; private set; }
        public bool PlayerHasKey { get; set; }
        public bool PlayerHasSword { get; set; }

        public Rooms()
        {
            AllRooms = new Dictionary<string, Room>();
            SetupWorld();
        }

        private void SetupWorld()
        {
            //  kamers
            var start = new Room("Start", "Je staat in de startkamer. Er zijn uitgangen in alle richtingen.");
            var left = new Room("Valkamer", "Je valt in een diepe put. Dood.") { IsLethal = true };
            var right = new Room("Sleutelkamer", "Er ligt een sleutel hier.");
            var up = new Room("Deur", "Een deur die naar vrijheid leidt.") { RequiresItem = true, RequiredItemId = "key" };
            var down = new Room("Kelder", "Een donkere kelder. Je ziet iets glinsteren: een zwaard.");
            var deep = new Room("Monsterkamer", "Een groot monster gromt in de schaduw.") { HasMonster = true, MonsterAlive = true };

            // Connecties
            start.Connect(Direction.West, left);
            start.Connect(Direction.East, right);
            start.Connect(Direction.North, up);
            start.Connect(Direction.South, down);

            down.Connect(Direction.South, deep);
            deep.Connect(Direction.North, down);

            // Items
            right.Items.Add("key");
            down.Items.Add("sword");

            // Toevoegen aan map
            AllRooms["start"] = start;
            AllRooms["left"] = left;
            AllRooms["right"] = right;
            AllRooms["up"] = up;
            AllRooms["down"] = down;
            AllRooms["deep"] = deep;

            CurrentRoom = start;
        }

        public MoveResult Go(Direction dir)
        {
            var target = CurrentRoom.GetExit(dir);

            if (target == null)
                return MoveResult.InvalidDirection;

            
            if (target.IsLethal)
                return MoveResult.Died;

           
            if (target.RequiresItem && !PlayerHasKey)
                return MoveResult.BlockedMissingKey;

            
            if (CurrentRoom.HasMonster && CurrentRoom.MonsterAlive && dir == Direction.North)
                return MoveResult.Died;

            
            CurrentRoom = target;

           
            if (target.Name == "Deur" && PlayerHasKey)
                return MoveResult.Won;

            return MoveResult.Moved;
        }

        public string Take(string itemId)
        {
            if (CurrentRoom.Items.Contains(itemId))
            {
                CurrentRoom.Items.Remove(itemId);
                if (itemId == "key") PlayerHasKey = true;
                if (itemId == "sword") PlayerHasSword = true;
                return $"Je hebt {itemId} opgepakt.";
            }
            return $"Er is geen {itemId} hier.";
        }

        public string Fight()
        {
            if (!CurrentRoom.HasMonster)
                return "Er is hier niets om te bevechten.";

            if (!PlayerHasSword)
                return "Je hebt geen zwaard! Het monster verslindt je. Dood.";

            CurrentRoom.MonsterAlive = false;
            return "Je hebt het monster verslagen!";
        }
    }
}
