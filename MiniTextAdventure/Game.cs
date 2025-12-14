using MiniTextAdventure;

public class Game
{
    private readonly ApiClient _api;
    public Rooms room = new Rooms();
    public Inventory PlayerInventory = new Inventory();
    public bool running;
    public Game(ApiClient api)
    {
        _api = api;
    }
    public async Task Start()
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
                    await Look();
                    break;

                case "inventory":
                    PlayerInventory.Show();
                    break;

                case "take":
                    if (argument == "")
                        Console.WriteLine("Wat wil je oppakken?");
                    else
                        await Take(argument);
                    break;

                case "go":
                    if (argument == "")
                        Console.WriteLine("Waarheen? (n, e, s, w)");
                    else
                        await Move(argument);
                    break;

                case "fight":
                    await Fight();
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

    public async Task Look()
    {
        var room = await _api.GetCurrentRoom();
        if (room == null)
        {
            Console.WriteLine("Fout: kon kamer niet ophalen.");
            return;
        }
        Console.WriteLine($"[{room.Name}]");
        Console.WriteLine($"{room.Description}");
    
        if (room.Items.Count > 0)
        {
            Console.WriteLine("Items hier:");
            foreach (var item in room.Items)
                Console.WriteLine($"  - {item.Name}");
        }
    
        if (room.HasMonster && room.MonsterAlive)
            Console.WriteLine("Er is een monster hier!");
    }

    public async Task Move(string dir)
    {
        var result = await _api.MoveAsync(dir);
    
        if (result == null)
        {
            Console.WriteLine("Fout bij verplaatsen.");
            return;
        }
    
        if (!result.Success)
        {
            Console.WriteLine(result.Message);
            return;
        }
    
        Console.WriteLine(result.Message);
    
        if (result.CurrentRoom != null)
        {
            Console.WriteLine($"[{result.CurrentRoom.Name}]");
            Console.WriteLine($"{result.CurrentRoom.Description}");
        }
    
        if (result.PlayerDied)
        {
            Console.WriteLine("GAME OVER.");
            running = false;
        }
    
        if (result.PlayerWon)
        {
            Console.WriteLine("Je wint!");
            running = false;
        }
    }

    public async Task Take(string itemId)
    {
        var msg = await _api.TakeAsync(itemId);
        Console.WriteLine(msg);
    }

    public async Task Fight()
    {
        var result = await _api.FightAsync();
    
        if (result == null)
        {
            Console.WriteLine("Fout bij gevecht.");
            return;
        }
    
        Console.WriteLine(result.Message);
    }
}
