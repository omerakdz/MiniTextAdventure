using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTextAdventure;
using System.Threading;

namespace MiniTextAdventureTests
{
    [TestClass]
    public class MonsterTests
    {
        [TestMethod]
        public void NewMonster_IsAliveByDefault()
        {
            Monster monster = new Monster("Goblin");
            Assert.IsTrue(monster.IsAlive, "Monster moet levend zijn bij creatie");
        }

        [TestMethod]
        public void KillMonster_SetsIsAliveFalse()
        {
            Monster monster = new Monster("Goblin");
            monster.Kill();
            Assert.IsFalse(monster.IsAlive, "Monster moet dood zijn na Kill()");
        }

        [TestMethod]
        public void ToString_ReturnsStatus()
        {
            Monster monster = new Monster("Goblin");
            string aliveString = monster.ToString();
            Assert.IsTrue(aliveString.Contains("levend"), "ToString moet aangeven dat monster levend is");

            monster.Kill();
            string deadString = monster.ToString();
            Assert.IsTrue(deadString.Contains("dood"), "ToString moet aangeven dat monster dood is");
        }
    }
}
