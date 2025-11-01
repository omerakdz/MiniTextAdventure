using MiniTextAdventure;
namespace MiniTestAdventureTesting
{
    [TestClass]
    public sealed class InventoryItemTest
    {
        Inventory playerInventory = new Inventory();

        [TestMethod]
        public void Add_ShouldAdd()
        {
            Item sword = new Item("sword", "Zwaard", "Een oud maar scherp zwaard.");

            playerInventory.Add(sword);

            Assert.IsTrue(playerInventory.Has(sword.Id), "Het zwaard zou in de inventaris moeten zitten.");
        }

        [TestMethod]
        public void Remove_ShouldRemove()
        {
            Item flashlight = new Item("flashlight", "Zaklamp", "Een lichtbron die helpt in het donker.");

            playerInventory.Add(flashlight);
            Assert.IsTrue(playerInventory.Has(flashlight.Id), $"De {flashlight.Name} is niet toegevoegd in je inventaris.");

            playerInventory.Remove(flashlight);
            Assert.IsFalse(playerInventory.Has(flashlight.Id), $"De {flashlight.Name} zit nog steeds in je inventaris.");
        }

        [TestMethod]
        public void ListItems_ShouldWork()
        {
            Item sandwich = new Item("sandwich", "Broodje", "Eten bestaande uit ingrediënten tussen twee sneetjes brood.");
            Item phone = new Item("phone", "Telefoon", "Een uitvinding om met mensen ver weg te spreken.");
            Item gum = new Item("gum", "Kauwgom", "Iets wat je kunt kauwen.");
            Item sword = new Item("sword", "Zwaard", "Een oud maar scherp zwaard.");

            playerInventory.Add(sandwich);
            playerInventory.Add(phone);
            playerInventory.Add(gum);

            Assert.IsNotNull(playerInventory.ListItems(), "Geeft null terug, dus de inventaris is leeg.");
            Assert.IsFalse(playerInventory.ListItems().Contains(sword.Name), "De inventaris zou 'Zwaard' niet moeten bevatten.");
        }
    }
}
