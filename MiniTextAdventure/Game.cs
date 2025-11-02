using MiniTextAdventure;

public class Game
{
    public Rooms room = new Rooms();
    public Inventory PlayerInventory = new Inventory();
    public bool running; 

    public void Start()
    {
        running = true;

        Console.WriteLine("Welkom bij het avontuur!");
        Console.WriteLine("Typ 'help' voor commando's.\n");
        Look();

        while (running)
        {
            Console.Write("> ");
            string input = Console.ReadLine().Trim().ToLower();
            string[] parts = input.Split(' ');

            if (parts.Length == 0) continue;

            string command = parts[0];
            string argument = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;

                case "look":
                    Look();
                    break;

                case "inventory":
                    PlayerInventory.Show();
                    break;

                case "take":
                    if (argument == "")
                        Console.WriteLine("Wat wil je oppakken?");
                    else
                        Console.WriteLine(room.Take(argument, PlayerInventory));
                    break;

                case "go":
                    if (argument == "")
                        Console.WriteLine("Waarheen? (n, e, s, w)");
                    else
                        Move(argument);
                    break;

                case "fight":
                    Fight();
                    break;

                case "quit":
                    running = false;
                    Console.WriteLine("Spel afgesloten.");
                    break;

                default:
                    Console.WriteLine("Onbekend commando. Typ 'help' voor hulp.");
                    break;
            }
        }
    }
    
    private void ShowHelp()
    {
        Console.WriteLine("Beschikbare commando's:");
        Console.WriteLine("look - bekijk huidige kamer");
        Console.WriteLine("go n|e|s|w - ga naar een richting");
        Console.WriteLine("take <id> - pak een item op");
        Console.WriteLine("inventory - bekijk je items");
        Console.WriteLine("fight - vecht met een monster (indien aanwezig)");
        Console.WriteLine("quit - sluit het spel af");
    }

    public void Look()
    {
        Console.WriteLine($"[{room.CurrentRoom.Name}]");
        Console.WriteLine($"{room.CurrentRoom.Description}");

    }

    public void Move(string dir)
    {
        Direction direction;
        switch (dir)
        {
            case "n": direction = Direction.North; break;
            case "e": direction = Direction.East; break;
            case "s": direction = Direction.South; break;
            case "w": direction = Direction.West; break;
            default:
                Console.WriteLine("Ongeldige richting.");
                return;
        }

        
        var result = room.Go(direction);
        switch (result)
        {
            case MoveResult.Moved:
                Console.WriteLine($"Je gaat naar {room.CurrentRoom.Name}.");
                break;
            case MoveResult.BlockedMissingKey:
                Console.WriteLine("Je hebt een sleutel nodig om deze kamer te betreden.");
                break;
            case MoveResult.Died:
                if (room.CurrentRoom.Name == "Start")
                {
                    Console.WriteLine();
                }
                Console.WriteLine("Je bent dood. GAME OVER.");
                running = false;
                break;
            case MoveResult.Won:
                Console.WriteLine($"Je gaat naar {room.CurrentRoom.Name}.");
                Console.WriteLine("Je opent de deur en ontsnapt! Je wint!");
                running = false;
                break;
            case MoveResult.InvalidDirection:
                Console.WriteLine("Je kunt hier niet heen.");
                break;
        }
    }

    public void Take(string itemId)
    {
        string msg = room.Take(itemId, PlayerInventory);
        Console.WriteLine(msg);
    }

    public void Fight()
    {
        var result = room.Fight(PlayerInventory);
        switch (result)
        {
            case FightResult.NoMonsterHere:
                Console.WriteLine("Er is hier geen monster.");
                break;
            case FightResult.NoWeapon:
                Console.WriteLine("Je hebt geen zwaard! Het monster verslaat je. GAME OVER.");
                break;
            case FightResult.MonsterAlreadyDead:
                Console.WriteLine("Het monster is al verslagen.");
                break;
            case FightResult.Victory:
                Console.WriteLine("Je verslaat het monster! De kamer is nu veilig.");
                break;
        }
    }
}
