using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTextAdventure;

namespace MiniTextAdventureTests
{
    [TestClass]
    public class CombatTests
    {
        [TestMethod]
        public void Fight_WithoutSword_PlayerDies()
        {
            Game game = new Game();

            // naar kelder
            game.Move("s");
            // naar monsterkamer
            game.Move("s");

            // vecht zonder zwaard
            game.Fight();

            // speler hoort dood te zijn (running = false)
            Assert.IsFalse(game.running, "Speler zou dood moeten zijn na vechten zonder zwaard.");
        }

        [TestMethod]
        public void Fight_WithSword_DefeatsMonster()
        {
            Game game = new Game();

            // naar kelder
            game.Move("s");
            // zwaard pakken
            game.Take("sword");
            // naar monsterkamer
            game.Move("s");

            // vecht met zwaard
            game.Fight();

            Assert.IsTrue(game.room.CurrentRoom.MonsterAlive == false, "MonsterAlive moet false zijn na overwinning.");
            Assert.IsTrue(game.running, "Spel moet nog lopen na winst op monster.");
        }

        [TestMethod]
        public void AfterVictory_PlayerCanReturnSafely()
        {
            Game game = new Game();

            // zwaard ophalen
            game.Move("s");
            game.Take("sword");

            // naar monsterkamer
            game.Move("s");
            game.Fight();

            // terug naar beneden
            game.Move("n");

            Assert.AreEqual("Kelder", game.room.CurrentRoom.Name, "Speler moet terug zijn in de kelder.");
            Assert.IsTrue(game.running, "Spel moet nog actief zijn na veilige terugkeer.");
        }
    }
}