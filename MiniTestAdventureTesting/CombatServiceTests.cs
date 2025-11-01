using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTextAdventure;

namespace MiniTextAdventureTests
{
    [TestClass]
    public class CombatServiceTests
    {
        [TestMethod]
        public void Fight_NoMonster_ReturnsNoMonsterHere()
        {
            Inventory inventory = new Inventory();
            CombatService combatService = new CombatService(inventory);
            Room emptyRoom = new Room("Empty", "Hier is niets te vinden");

            var result = combatService.Fight(emptyRoom);

            Assert.AreEqual(FightResult.NoMonsterHere, result);
        }

        [TestMethod]
        public void Fight_WithoutSword_ReturnsNoWeapon()
        {
            Inventory inventory = new Inventory();
            CombatService combatService = new CombatService(inventory);

            Room monsterRoom = new Room("Monster", "Brr")
            {
                HasMonster = true,
                MonsterAlive = true
            };

            var result = combatService.Fight(monsterRoom);

            Assert.AreEqual(FightResult.NoWeapon, result);
        }

        [TestMethod]
        public void Fight_WithSword_ReturnsVictory()
        {
            Inventory inventory = new Inventory();
            inventory.Add(new Item("sword", "Zwaard", "Een scherp zwaard"));

            CombatService combatService = new CombatService(inventory);

            Room monsterRoom = new Room("Monster", "Brr")
            {
                HasMonster = true,
                MonsterAlive = true
            };

            var result = combatService.Fight(monsterRoom);

            Assert.AreEqual(FightResult.Victory, result);
            Assert.IsFalse(monsterRoom.MonsterAlive);
        }

        [TestMethod]
        public void Fight_AlreadyDeadMonster_ReturnsMonsterAlreadyDead()
        {
            Inventory inventory = new Inventory();
            inventory.Add(new Item("sword", "Zwaard", "Een scherp zwaard"));

            CombatService combatService = new CombatService(inventory);

            Room monsterRoom = new Room("Monster", "Brr")
            {
                HasMonster = true,
                MonsterAlive = false
            };

            var result = combatService.Fight(monsterRoom);

            Assert.AreEqual(FightResult.MonsterAlreadyDead, result);
        }
    }
}
