using MiniTextAdventure;

public enum FightResult { NoMonsterHere, NoWeapon, MonsterAlreadyDead, Victory }

public class CombatService
{
    private Inventory _inventory;
    public CombatService(Inventory inventory) => _inventory = inventory;

    public FightResult Fight(Room room)
    {
        if (!room.HasMonster) return FightResult.NoMonsterHere;
        if (!room.MonsterAlive) return FightResult.MonsterAlreadyDead;
        if (!_inventory.Has("sword")) return FightResult.NoWeapon;

        room.MonsterAlive = false;
        return FightResult.Victory;
    }
}
