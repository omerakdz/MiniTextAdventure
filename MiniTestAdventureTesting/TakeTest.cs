
using MiniTextAdventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiniTestAdventureTesting
{

    [TestClass]
    public class TakeTest
    {

        [TestMethod]
        public void Take_ItemExists_ShouldMoveToInventory()
        {

            Rooms rooms = new Rooms();
            Inventory inv = new Inventory();

            rooms.CurrentRoom = rooms.AllRooms["right"];
            string itemId = "key";

            if (rooms.CurrentRoom.Items.Contains(itemId))
            {
                Item item = new Item(itemId, "Sleutel", "Een kleine bronzen sleutel.");
                inv.Add(item);
                rooms.CurrentRoom.Items.Remove(itemId);
            }
 
            Assert.IsTrue(inv.Has(itemId), "De sleutel zou in de inventaris moeten zitten.");
            Assert.IsFalse(rooms.CurrentRoom.Items.Contains(itemId), "De sleutel zou niet meer in de kamer mogen liggen.");

        }
        [TestMethod]
        public void Take_ItemDoesNotExist_ShouldNotThrow()
        {
            
            Rooms rooms = new Rooms();
            Inventory inv = new Inventory();

            rooms.CurrentRoom = rooms.AllRooms["start"];
            string itemId = "key";

            
            if (rooms.CurrentRoom.Items.Contains(itemId))
            {
                Item item = new Item(itemId, "Sleutel", "Een kleine bronzen sleutel.");
                inv.Add(item);
                rooms.CurrentRoom.Items.Remove(itemId);
            }

            
            Assert.IsFalse(inv.Has(itemId), "De sleutel zou niet in de inventaris mogen zitten, want hij lag niet in deze kamer.");
        }


    }
}
