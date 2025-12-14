namespace MiniApiTextAdv.Models;

public class RoomDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ItemDto> Items { get; set; }
    public bool HasMonster { get; set; }
    public bool MonsterAlive { get; set; }
    public bool RequiresKeyshare { get; set; }
    public string? RequiredKeyId { get; set; }
}