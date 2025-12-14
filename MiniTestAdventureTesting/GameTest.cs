using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTextAdventure;

namespace MiniTextAdventure.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Player_Wins_After_Right_Down_Deep_Fight_Up()
        {
            
            Game game = new Game();

            
            game.Move("e"); // sleutel
            game.Take("key");
            game.Move("s"); // kelder
            game.Take("sword");
            game.Move("s"); // monsterkamer
            game.Fight();
            game.Move("n"); // terug omhoog
            game.Move("n"); // deur

            
            Assert.IsFalse(game.running, "Spel zou moeten stoppen na overwinning.");
        }

        [TestMethod]
        public void Player_Dies_When_Going_Left()
        {
            
            Game game = new Game();

            
            game.Move("w");

            
            Assert.IsFalse(game.running, "Spel zou moeten stoppen bij dood.");
        }

        [TestMethod]
        public void Player_Dies_When_Leaving_MonsterRoom_Alive()
        {
            
            Game game = new Game();

            game.Move("s"); 
            game.Move("s"); 

            
            game.Move("n");

            
            Assert.IsFalse(game.running, "Spel zou moeten eindigen bij dood door monster.");
        }
    }
}