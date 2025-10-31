using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiniTextAdventure
{
    public class Inventory
    {

        private readonly List<Item> items = new List<Item>();

        public bool Has(string id)
        {
            bool hasIt = false;

            foreach (Item i in items)
            {
                if (i.Id == id)
                {
                    hasIt = true;
                }
            }

            return hasIt;
        }

        public void Add(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("Kan geen ongeldig item toevoegen (item is null).");
                return;
            }

            if (Has(item.Id))
            {
                Console.WriteLine($"Het item '{item.Name}' zit al in je inventaris.");
            }
            else
            {
                items.Add(item);
                Console.WriteLine($"'{item.Name}' toegevoegd aan je inventaris.");
            }
        }
        public void Add(string id)
        {
            if (!Has(id))
            {
                items.Add(new Item(id));
            }
        }

        public void Remove(Item item)
        {
            Item teVerwijderen = null;
            foreach (Item i in items)
            {
                if(item.Id == i.Id)
                {
                    teVerwijderen = i;
                    items.Remove(item);
                    Console.WriteLine(item.Name + " verwijderd van je inventaris");
                }
                if (teVerwijderen != null)
                {
                    items.Remove(teVerwijderen);
                }
            }
        }
        
        public string ListItems()
        {
            string lijstItems = "";
            if(items.Count == 0)
            {
                return "Je inventaris is leeg";
            }
            string begin = "Items in je inventaris: \n";
            foreach (Item i in items)
            {
                lijstItems += "- " + i.Name + "\n";
            }

            return begin + lijstItems;
        }

        public void Show()
        {
            if (items.Count == 0)
                Console.WriteLine("Je draagt niets bij je.");
            else
            {
                Console.WriteLine("Je draagt bij je:");
                foreach (var i in items)
                    Console.WriteLine("- " + i);
            }
        }

    }
}
