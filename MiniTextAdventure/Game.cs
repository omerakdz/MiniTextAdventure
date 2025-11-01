using System;
using System.Collections.Generic;

namespace MiniTextAdventure
{
    public class Game
    {
        public Room CurrentRoom;
        public Inventory PlayerInventory = new Inventory();
        public bool GameOver = false;
        public bool GameWon = false;

        public Game()
        {
            // Rooms
            Room startRoom = new Room("Start", "Het middelpunt van de wereld.");
            Room leftRoom = new Room("Links", "Je stapt in een val. Dood!");
            leftRoom.IsLethal = true;

            Room rightRoom = new Room("Rechts", "Er ligt iets glimmends op de grond.");
            rightRoom.Items.Add("key");

            Room upperRoom = new Room("Boven", "Een deur die alleen opent met een sleutel.");
            upperRoom.RequiresItem = true;
            upperRoom.RequiredItemId = "key";

            Room lowerRoom = new Room("Beneden", "Een trap naar beneden.");
            lowerRoom.Items.Add("sword");

            Room deeperRoom = new Room("Dieper", "Een grot met een monster!");
            deeperRoom.HasMonster = true;
            deeperRoom.MonsterAlive = true;

            // Connections
            startRoom.Connect(Direction.West, leftRoom);
            startRoom.Connect(Direction.East, rightRoom);
            startRoom.Connect(Direction.North, upperRoom);
            startRoom.Connect(Direction.South, lowerRoom);

            lowerRoom.Connect(Direction.North, startRoom);
            lowerRoom.Connect(Direction.South, deeperRoom);

            deeperRoom.Connect(Direction.North, lowerRoom);

            CurrentRoom = startRoom;
        }

        public void Take(string itemId)
        {
            if (CurrentRoom.Items.Contains(itemId))
            {
                PlayerInventory.Add(itemId);
                CurrentRoom.Items.Remove(itemId);
                Console.WriteLine($"Je neemt de {itemId}.");
            }
            else
            {
                Console.WriteLine($"Er ligt geen {itemId} hier.");
            }
        }

        public void Move(Direction direction)
        {
            if (CurrentRoom.HasMonster && CurrentRoom.MonsterAlive)
            {
                GameOver = true;
                Console.WriteLine("Je probeert te vluchten, maar het monster grijpt je! GAME OVER.");
                return;
            }

            Room nextRoom = CurrentRoom.GetExit(direction);
            if (nextRoom == null)
            {
                Console.WriteLine("Je kunt daar niet heen.");
                return;
            }

            if (nextRoom.IsLethal)
            {
                GameOver = true;
                Console.WriteLine("Je bent dood. GAME OVER.");
                return;
            }

            if (nextRoom.RequiresItem && !PlayerInventory.Has(nextRoom.RequiredItemId))
            {
                Console.WriteLine($"Je hebt een {nextRoom.RequiredItemId} nodig om deze kamer te betreden.");
                return;
            }

            CurrentRoom = nextRoom;

            if (CurrentRoom.Name == "Boven" && PlayerInventory.Has("key"))
            {
                GameWon = true;
                Console.WriteLine("Je opent de deur met de sleutel en ontsnapt! Je wint!");
            }
            else
            {
                Console.WriteLine($"Je gaat naar {CurrentRoom.Name}.");
            }
        }

        public void Fight()
        {
            if (!CurrentRoom.HasMonster || !CurrentRoom.MonsterAlive)
            {
                Console.WriteLine("Er is hier geen monster.");
                return;
            }

            if (!PlayerInventory.Has("sword"))
            {
                GameOver = true;
                Console.WriteLine("Je hebt geen zwaard! Het monster verslaat je. GAME OVER.");
                return;
            }

            CurrentRoom.MonsterAlive = false;
            Console.WriteLine("Je gebruikt je zwaard en verslaat het monster! De kamer is nu veilig.");
        }
    }
}
