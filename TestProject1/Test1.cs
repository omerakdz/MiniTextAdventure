using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projectopdracht;
namespace TestProject1
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Fight_Zonder_Zwaard_Geeft_GameOver()
        {
            Game game = new Game();

            // Ga naar monsterkamer zonder zwaard
            game.Move("s"); // naar beneden
            game.Move("s"); // naar dieper (monsterkamer)
            game.Fight();

            Assert.IsTrue(game.GameOver);
        }

        [TestMethod]
        public void Fight_Met_Zwaard_Verslaat_Monster()
        {
            Game game = new Game();

            // Pak zwaard en ga vechten
            game.Move("s"); // naar beneden
            game.Take("sword");
            game.Move("s"); // monsterkamer
            game.Fight();

            Assert.IsFalse(game.GameOver);
            Assert.IsFalse(game.Current.MonsterAlive);
        }

        [TestMethod]
        public void Na_Winst_Kan_Speler_Veilig_Terug()
        {
            Game game = new Game();

            // Pak zwaard, vecht met monster, en ga terug
            game.Move("s"); // beneden
            game.Take("sword");
            game.Move("s"); // monsterkamer
            game.Fight();
            game.Move("n"); // terug naar beneden

            Assert.AreEqual("Beneden", game.Current.Name);
            Assert.IsFalse(game.GameOver);
        }
        }
}
