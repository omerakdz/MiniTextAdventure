using MiniTextAdventure;

namespace MiniTestAdventureTesting
{
    [TestClass]
    public class RoomsTests
    {
        public Rooms rooms; 

        [TestInitialize]
        public void Setup()
        {
            rooms = new Rooms();
        }

        [TestMethod]
        public void GoingLeft_FromStart_ShouldCauseDeath()
        {
            var result = rooms.Go(Direction.West);
            Assert.AreEqual(MoveResult.Died, result);
        }

        [TestMethod]
        public void GoingUp_WithoutKey_ShouldBeBlocked()
        {
            var result = rooms.Go(Direction.North);
            Assert.AreEqual(MoveResult.BlockedMissingKey, result);
        }

        [TestMethod]
        public void PickingUpKey_ShouldAllowGoingUpToWin()
        {
            rooms.Go(Direction.East);
            rooms.Take("key");
            rooms.Go(Direction.West);
            var result = rooms.Go(Direction.North);
            Assert.AreEqual(MoveResult.Won, result);
        }

        [TestMethod]
        public void LeavingMonsterRoomAlive_ShouldKillPlayer()
        {
            rooms.Go(Direction.South);
            rooms.Go(Direction.South);
            var result = rooms.Go(Direction.North);
            Assert.AreEqual(MoveResult.Died, result);
        }

        [TestMethod]
        public void FightWithSword_ShouldKillMonster_AndAllowLeaving() // werkt niet
        {
            
            rooms.Go(Direction.South);
            rooms.Take("sword");
            
            rooms.Go(Direction.South);
            var fightResult = rooms.Fight();
            StringAssert.Contains(fightResult, "verslagen");
            
            var moveResult = rooms.Go(Direction.North);
            Assert.AreEqual(MoveResult.Moved, moveResult);
        }
    }
}