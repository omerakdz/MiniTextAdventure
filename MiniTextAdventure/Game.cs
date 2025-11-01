using MiniTextAdventure;

public class Game
{
    public Rooms Room = new Rooms();
    public Inventory PlayerInventory = new Inventory();
    public bool GameOver = false;
    public bool GameWon = false;

    public void Move(Direction dir)
    {
        var result = Room.Go(dir);
        switch (result)
        {
            case MoveResult.Moved:
                Console.WriteLine($"Je gaat naar {Room.CurrentRoom.Name}.");
                break;
            case MoveResult.BlockedMissingKey:
                Console.WriteLine("Je hebt een sleutel nodig om deze kamer te betreden.");
                break;
            case MoveResult.Died:
                GameOver = true;
                Console.WriteLine("Je bent dood. GAME OVER.");
                break;
            case MoveResult.Won:
                GameWon = true;
                Console.WriteLine("Je opent de deur en ontsnapt! Je wint!");
                break;
        }
    }

    public void Take(string itemId)
    {
        string msg = Room.Take(itemId, PlayerInventory);
        Console.WriteLine(msg);
    }

    public void Fight()
    {
        var result = Room.Fight(PlayerInventory);
        switch (result)
        {
            case FightResult.NoMonsterHere:
                Console.WriteLine("Er is hier geen monster.");
                break;
            case FightResult.NoWeapon:
                GameOver = true;
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
