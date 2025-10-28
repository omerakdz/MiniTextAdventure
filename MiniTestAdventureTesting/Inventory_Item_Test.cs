using Mini_Text_Adventure;
namespace TEstingAdventureGame
{
    [TestClass]
    public sealed class Test1
    {
        Inventory inv = new Inventory();
        [TestMethod]
        public void Add_ShouldAdd()
        {
            
            Item sword = new Item("sword", "Zwaard", "Een oud maar scherp zwaard.");

            inv.Add(sword);

            Assert.IsTrue(inv.HasItem(sword.Id),"You should have a sword in je inventory");
            
        }
        [TestMethod]
        public void Remove_ShouldRemove()
        {
            
            Item light = new Item("flashlight", "Zaklamp", "Its a light source, helping you see in the dark.");

            inv.Add(light);
            Assert.IsTrue(inv.HasItem(light.Id),$"De {light.Name} is niet toegevoegd in je inventaris.");

            inv.Remove(light);
            Assert.IsFalse(inv.HasItem(light.Id), $"De {light.Name} zit nog steeds in je inventory.");
        }
        [TestMethod]
        public void ListItem_Shouldwork()
        {
            Item sandwich = new Item("sandwich", "Sandwich", "Food thats made of ingredients between two bread slices");
            Item phone = new Item("phone", "Phone", "An invention that will make you be able to speak to people from far away");
            Item gum = new Item("gum", "gum", "something you can chew on");
            Item sword = new Item("sword", "Zwaard", "Een oud maar scherp zwaard.");

            inv.Add(sandwich);
            inv.Add(phone);
            inv.Add(gum); 

            Assert.IsNotNull(inv.ListItems(),"Geeft null terug, dus leeg.");
            Assert.IsFalse(inv.ListItems().Contains(sword.Name), "Inventory should contain 'Zwaard'");
        }                                                                
    }       
}
