namespace MiniApiTextAdv.Models;

public class MoveResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public RoomDto? CurrentRoom { get; set; }
    public bool PlayerDied { get; set; }
    public bool PlayerWon { get; set; }
}
