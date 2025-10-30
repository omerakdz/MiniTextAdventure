using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTextAdventure;

namespace MiniTextAdventureTests
{
    [TestClass]
    public class CombatTests
    {
        [TestMethod]
        public void Fight_Zonder_Zwaard_Geeft_GameOver()
        {
            Game game = new Game();

            // Ga naar beneden
            game.Move(Direction.South);
            // Ga naar monsterkamer
            game.Move(Direction.South);

            // Fight zonder zwaard
            game.Fight();

            Assert.IsTrue(game.GameOver, "GameOver moet true zijn als je vecht zonder zwaard");
        }

        [TestMethod]
        public void Fight_Met_Zwaard_Verslaat_Monster()
        {
            Game game = new Game();

            // Ga naar beneden
            game.Move(Direction.South);
            // Neem zwaard
            game.Take("sword");
            // Ga naar monsterkamer
            game.Move(Direction.South);

            // Fight met zwaard
            game.Fight();

            Assert.IsFalse(game.GameOver, "GameOver mag false zijn als je vecht met zwaard");
            Assert.IsFalse(game.Current.MonsterAlive, "MonsterAlive moet false zijn na overwinning");
        }

        [TestMethod]
        public void Na_Winst_Kan_Speler_Veilig_Terug()
        {
            Game game = new Game();

            // Ga naar beneden
            game.Move(Direction.South);
            // Neem zwaard
            game.Take("sword");
            // Ga naar monsterkamer
            game.Move(Direction.South);
            // Vecht
            game.Fight();

            // Terug naar beneden
            game.Move(Direction.North);

            Assert.AreEqual("Beneden", game.Current.Name, "Speler moet terug in Beneden zijn");
            Assert.IsFalse(game.GameOver, "GameOver mag false zijn na winst");
        }
    }
}
