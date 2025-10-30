using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOpdracht; 

namespace ProjectOpdrachtTests
{
    [TestClass]
    public class CombatTests
    {
        // -------------------- Test 1 --------------------
        // Fight faalt zonder zwaard
        [TestMethod]
        public void Fight_Zonder_Zwaard_GameOver()
        {
            Game game = new Game();

            // Ga naar beneden en dan monsterkamer
            game.Move("s"); // naar beneden
            game.Move("s"); // naar dieper (monsterkamer)
            game.Fight();

           
            Assert.IsTrue(game.GameOver);
        }

        // -------------------- Test 2 --------------------
        // Fight met zwaard verslaat monster
        [TestMethod]
        public void Monster_Verslaan()
        {
            Game game = new Game();

           
            game.Move("s"); 
            game.Take("sword"); 
            game.Move("s"); 
            game.Fight();

           
            Assert.IsFalse(game.Current.MonsterAlive);
            Assert.IsFalse(game.GameOver);
        }

        // -------------------- Test 3 --------------------
        // Na overwinning veilig terugkeren
        [TestMethod]
        public void Na_Winst_Kan_Speler_Veilig_Terug()
        {
            Game game = new Game();

           
            game.Move("s"); 
            game.Take("sword"); 
            game.Move("s");
            game.Fight();
            game.Move("n"); 

            // Controleer dat speler veilig terug is en spel niet over is
            Assert.AreEqual("Beneden", game.Current.Name);
            Assert.IsFalse(game.GameOver);
        }
    }
}
