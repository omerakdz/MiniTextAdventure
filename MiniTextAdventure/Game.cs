using System;
using System.Collections.Generic;

namespace MiniTextAdventure
{
    public class Game
    {
        public Room Current;
        public Inventory Inventory = new Inventory();
        public bool GameOver = false;
        public bool GameWon = false;

        public Game()
        {
            // Kamers
            Room start = new Room("Start", "Het middelpunt van de wereld.");
            Room links = new Room("Links", "Je stapt in een val. Dood!");
            links.IsLethal = true;

            Room rechts = new Room("Rechts", "Er ligt iets glimmends op de grond.");
            rechts.Items.Add("key");

            Room boven = new Room("Boven", "Een deur die alleen opent met een sleutel.");
            boven.RequiresItem = true;
            boven.RequiredItemId = "key";

            Room beneden = new Room("Beneden", "Een trap naar beneden.");
            beneden.Items.Add("sword");

            Room dieper = new Room("Dieper", "Een grot met een monster!");
            dieper.HasMonster = true;
            dieper.MonsterAlive = true;

            // Verbindingen
            start.Connect(Direction.West, links);
            start.Connect(Direction.East, rechts);
            start.Connect(Direction.North, boven);
            start.Connect(Direction.South, beneden);

            beneden.Connect(Direction.North, start);
            beneden.Connect(Direction.South, dieper);

            dieper.Connect(Direction.North, beneden);

            Current = start;
        }

        public void Take(string itemId)
        {
            if (Current.Items.Contains(itemId))
            {
                Inventory.Add(itemId);
                Current.Items.Remove(itemId);
                Console.WriteLine($"Je neemt de {itemId}.");
            }
            else
            {
                Console.WriteLine($"Er ligt geen {itemId} hier.");
            }
        }

        public void Move(Direction dir)
        {
            if (Current.HasMonster && Current.MonsterAlive)
            {
                GameOver = true;
                Console.WriteLine("Je probeert te vluchten, maar het monster grijpt je! GAME OVER.");
                return;
            }

            Room next = Current.GetExit(dir);
            if (next == null)
            {
                Console.WriteLine("Je kunt daar niet heen.");
                return;
            }

            if (next.IsLethal)
            {
                GameOver = true;
                Console.WriteLine("Je bent dood. GAME OVER.");
                return;
            }

            if (next.RequiresItem && !Inventory.Has(next.RequiredItemId))
            {
                Console.WriteLine($"Je hebt een {next.RequiredItemId} nodig om deze kamer te betreden.");
                return;
            }

            Current = next;

            if (Current.Name == "Boven" && Inventory.Has("key"))
            {
                GameWon = true;
                Console.WriteLine("Je opent de deur met de sleutel en ontsnapt! Je wint!");
            }
            else
            {
                Console.WriteLine($"Je gaat naar {Current.Name}.");
            }
        }

        public void Fight()
        {
            if (!Current.HasMonster || !Current.MonsterAlive)
            {
                Console.WriteLine("Er is hier geen monster.");
                return;
            }

            if (!Inventory.Has("sword"))
            {
                GameOver = true;
                Console.WriteLine("Je hebt geen zwaard! Het monster verslaat je. GAME OVER.");
                return;
            }

            Current.MonsterAlive = false;
            Console.WriteLine("Je gebruikt je zwaard en verslaat het monster! De kamer is nu veilig.");
        }
    }
}
