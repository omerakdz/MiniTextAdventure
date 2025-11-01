using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniTextAdventure;

namespace MiniTextAdventureTests
{
    [TestClass]
    public class CombatTests
    {
        [TestMethod]
        public void Fight_WithoutSword_SetsGameOver()
        {
            Game game = new Game();

            game.Move(Direction.South);
       
            game.Move(Direction.South);

            // Fight without sword
            game.Fight();

            Assert.IsTrue(game.GameOver, "GameOver mag alleen true zijn als de speler zonder een zwaard vecht.");
        }

        [TestMethod]
        public void Fight_WithSword_DefeatsMonster()
        {
            Game game = new Game();

            
            game.Move(Direction.South);
            
            game.Take("sword");
            // monster kamer
            game.Move(Direction.South);

            game.Fight();

            Assert.IsFalse(game.GameOver, "GameOver zou vals moeten zijn als de speler met een zwaard vecht");
            Assert.IsFalse(game.Room.CurrentRoom.MonsterAlive, "MonsterAlive zou na de overwinning vals moeten zijn");
        }

        [TestMethod]
        public void AfterVictory_PlayerCanReturnSafely()
        {
            Game game = new Game();

        
            game.Move(Direction.South);
          
            game.Take("sword");
            // monster kamer
            game.Move(Direction.South);
       
            game.Fight();

         
            game.Move(Direction.North);

            Assert.AreEqual("Beneden", game.Room.CurrentRoom.Name, "Speler moet terug in Beneden zijn");
            Assert.IsFalse(game.GameOver, "GameOver mag false zijn na winst");
        }
    }
}
