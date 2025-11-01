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
            // ga sleutelkamer en pak sleutel
            rooms.Go(Direction.East);
            rooms.Take("key", new Inventory());

            // terug  Start
            rooms.Go(Direction.West);

            //ga naar Deur
            var result = rooms.Go(Direction.North);

            // Controleer dat speler gewonnen heeft
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
        public void FightWithSword_ShouldKillMonster_AndAllowLeaving()
        {
            var inventory = new Inventory();

            rooms.Go(Direction.South); // naar Kelder
            rooms.Take("sword", inventory); // pak zwaard

            rooms.Go(Direction.South); // naar Monsterkamer
            var fightResult = rooms.Fight(inventory);

            Assert.AreEqual(FightResult.Victory, fightResult, "Het monster moet verslagen zijn.");

            var moveResult = rooms.Go(Direction.North); // terug naar Kelder
            Assert.AreEqual(MoveResult.Moved, moveResult, "Speler moet veilig kunnen teruggaan.");
        }
    }
}