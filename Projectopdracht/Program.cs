using System;
using System.Collections.Generic;

namespace ProjectOpdracht
{
    
    public class Item
    {
        public string Id;
        public string Description;

        public Item(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }

   
    public class Inventory
    {
        public List<Item> Items = new List<Item>();

        public void Add(Item item) => Items.Add(item);

        public bool Has(string id)
        {
            foreach (var i in Items)
                if (i.Id == id) return true;
            return false;
        }

        public void Show()
        {
            if (Items.Count == 0) Console.WriteLine("Je draagt niets bij je.");
            else
            {
                Console.WriteLine("Je draagt bij je:");
                foreach (var i in Items) Console.WriteLine("- " + i.Id);
            }
        }
    }

    
    public class Room
    {
        public string Name;
        public string Description;
        public List<Item> Items = new List<Item>();
        public Dictionary<string, Room> Exits = new Dictionary<string, Room>();
        public bool MonsterAlive = false;

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void Describe()
        {
            Console.WriteLine("Je bent in: " + Name);
            Console.WriteLine(Description);

            if (Items.Count > 0)
            {
                Console.WriteLine("Je ziet hier:");
                foreach (var i in Items) Console.WriteLine("- " + i.Id);
            }

            if (MonsterAlive) Console.WriteLine("Er staat een monster!");

            Console.WriteLine("Uitgangen:");
            foreach (var e in Exits.Keys) Console.Write(e + " ");
            Console.WriteLine();
        }
    }

    
    public class Game
    {
        public Room Current;
        public Inventory Inventory = new Inventory();
        public bool GameOver = false;
        public bool GameWon = false;

        public Game()
        {
            
            Room start = new Room("Start", "Het middelpunt van de wereld.");
            Room links = new Room("Links", "Je stapt in een val. Dood!");
            Room rechts = new Room("Rechts", "Er ligt iets glimmends op de grond.");
            Room boven = new Room("Boven", "Een deur die alleen opent met een sleutel.");
            Room beneden = new Room("Beneden", "Een trap naar beneden.");
            Room dieper = new Room("Dieper", "Een grot met een monster!");
            dieper.MonsterAlive = true;

            
            start.Exits["w"] = links;
            start.Exits["e"] = rechts;
            start.Exits["n"] = boven;
            start.Exits["s"] = beneden;

            beneden.Exits["n"] = start;
            beneden.Exits["s"] = dieper;
            dieper.Exits["n"] = beneden;

         
            rechts.Items.Add(new Item("key", "Een oude sleutel"));
            beneden.Items.Add(new Item("sword", "Een scherp zwaard"));

            Current = start;
        }

        
        public void Take(string itemId)
        {
            Item found = null;
            foreach (var i in Current.Items)
            {
                if (i.Id == itemId) { found = i; break; }
            }
            if (found == null) Console.WriteLine("Er ligt geen " + itemId + " hier.");
            else
            {
                Inventory.Add(found);
                Current.Items.Remove(found);
                Console.WriteLine("Je neemt de " + itemId + ".");
            }
        }

        
        public void Move(string dir)
        {
            if (Current.MonsterAlive)
            {
                GameOver = true;
                Console.WriteLine("Je probeert te vluchten, maar het monster grijpt je! GAME OVER.");
                return;
            }

            if (!Current.Exits.ContainsKey(dir))
            {
                Console.WriteLine("Je kunt daar niet heen.");
                return;
            }

            Current = Current.Exits[dir];

            if (Current.Name == "Links")
            {
                GameOver = true;
                Console.WriteLine("Je bent dood. GAME OVER.");
            }
            else if (Current.Name == "Boven" && Inventory.Has("key"))
            {
                GameWon = true;
                Console.WriteLine("Je opent de deur met de sleutel en ontsnapt! Je wint!");
            }
            else
            {
                Console.WriteLine("Je gaat naar " + Current.Name + ".");
            }
        }

       
        public void Fight()
        {
            if (!Current.MonsterAlive)
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

   
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            Console.WriteLine("Welkom bij het mini Text Adventure!");
            Console.WriteLine("Typ 'help' voor een lijst met commando's.\n");

            while (!game.GameOver && !game.GameWon)
            {
                Console.Write("> ");
                string input = Console.ReadLine().Trim().ToLower();
                string[] parts = input.Split(' ');
                string cmd = parts[0];

                switch (cmd)
                {
                    case "help":
                        Console.WriteLine("Commando's: help, look, inventory, go <richting>, take <item>, fight, quit");
                        break;

                    case "look":
                        game.Current.Describe();
                        break;

                    case "inventory":
                        game.Inventory.Show();
                        break;

                    case "go":
                        if (parts.Length < 2) Console.WriteLine("Gebruik: go <richting>");
                        else game.Move(parts[1]);
                        break;

                    case "take":
                        if (parts.Length < 2) Console.WriteLine("Gebruik: take <item>");
                        else game.Take(parts[1]);
                        break;

                    case "fight":
                        game.Fight();
                        break;

                    case "quit":
                        game.GameOver = true;
                        Console.WriteLine("Spel gestopt.");
                        break;

                    default:
                        Console.WriteLine("Onbekend commando. Typ 'help' voor hulp.");
                        break;
                }
            }

            Console.WriteLine("\nEinde van het spel.");
        }
    }
}
