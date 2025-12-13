using System;
using System.Collections.Generic;

namespace MiniTextAdventure
{
    public enum MoveResult { Moved, BlockedMissingKey, Died, Won, InvalidDirection }

    public class Rooms
    {
        public Dictionary<string, Room> AllRooms { get; set; }
        public Room CurrentRoom { get; set; }

        public bool PlayerHasKey { get; set; }
        public bool PlayerHasSword { get; set; }

        public Rooms()
        {
            AllRooms = new Dictionary<string, Room>();
            SetupWorld();
        }

        private void SetupWorld()
        {
            // Kamers
            var start = new Room("Startkamer", "Je staat in de startkamer. Er zijn uitgangen in alle richtingen.");

            var left = new Room("Valkamer", "Je valt in een diepe put. Dood.")
            {
                IsLethal = true
            };

            var right = new Room("Sleutelkamer", "Er ligt een sleutel hier.");

            var up = new Room("Deur", "Een deur die naar vrijheid leidt.")
            {
                RequiresItem = true,
                RequiredItemId = "key"
            };

            var down = new Room("Kelder", "Een donkere kelder. Je ziet iets glinsteren: een zwaard.");

            var deep = new Room("Monsterkamer", "Een groot monster gromt in de schaduw.")
            {
                HasMonster = true,
                MonsterAlive = true
            };

            // ✅ Voorbeeld beveiligde kamer (keyshare)
            var secret = new Room("Geheime Kamer", "Een kamer die beveiligd is met een keyshare.")
            {
                RequiresKeyshare = true,
                RequiredKeyId = "room2"
            };

            // Connecties
            start.Connect(Direction.West, left);
            left.Connect(Direction.East, start);

            start.Connect(Direction.East, right);
            right.Connect(Direction.West, start);

            start.Connect(Direction.North, up);
            up.Connect(Direction.South, start);

            start.Connect(Direction.South, down);
            down.Connect(Direction.North, start);

            down.Connect(Direction.South, deep);
            deep.Connect(Direction.North, down);

            // ✅ Geheime kamer achter monster
            deep.Connect(Direction.East, secret);
            secret.Connect(Direction.West, deep);

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
            AllRooms["secret"] = secret;

            CurrentRoom = start;
        }

        public MoveResult Go(Direction direction)
        {
            var target = CurrentRoom.GetExit(direction);

            if (target == null)
                return MoveResult.InvalidDirection;

            if (target.IsLethal)
                return MoveResult.Died;

            if (target.RequiresItem && !PlayerHasKey)
                return MoveResult.BlockedMissingKey;

            if (CurrentRoom.HasMonster && CurrentRoom.MonsterAlive && direction == Direction.North)
                return MoveResult.Died;

            CurrentRoom = target;

            if (target.Name == "Deur" && PlayerHasKey)
                return MoveResult.Won;

            return MoveResult.Moved;
        }

        public FightResult Fight(Inventory inventory)
        {
            var combat = new CombatService(inventory);
            return combat.Fight(CurrentRoom);
        }

        public string Take(string itemId, Inventory inventory)
        {
            if (!CurrentRoom.Items.Contains(itemId))
                return $"Er is geen {itemId} hier.";

            CurrentRoom.Items.Remove(itemId);
            inventory.Add(itemId);

            if (itemId == "key") PlayerHasKey = true;
            if (itemId == "sword") PlayerHasSword = true;

            return $"Je hebt {itemId} opgepakt.";
        }

        public Room? Peek(Direction dir)
        {
            if (!CurrentRoom.Exits.TryGetValue(dir, out var next))
                return null;

            return next;
        }
    }
}
