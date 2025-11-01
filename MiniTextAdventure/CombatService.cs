using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTextAdventure
{

    public enum FightResult{ Victory, NoWeapon, NoMonsterHere, MonsterAlreadyDead, PlayerDied}
    public class CombatService
    {
        private readonly Inventory _inventory;

        public CombatService(Inventory inventory)
        {
            _inventory = inventory;
        }

        public FightResult Fight(Room currentRoom)
        {
            if (currentRoom == null)
            {
                throw new ArgumentNullException(nameof(currentRoom));
            }
                

            if (!currentRoom.HasMonster)
            {
                return FightResult.NoMonsterHere;
            }
               
            if (!currentRoom.MonsterAlive)
            {
                return FightResult.MonsterAlreadyDead;
            }
                

            if (!_inventory.Has("sword"))
            {
                return FightResult.NoWeapon;
            }
                

            currentRoom.MonsterAlive = false;
            Console.WriteLine("Je gebruikt je zwaard en verslaat het monster! De kamer is nu veilig.");
            return FightResult.Victory;
        }
    }
}

