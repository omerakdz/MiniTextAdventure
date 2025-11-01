using System;
using System.Collections.Generic;

namespace MiniTextAdventure
{
    public class Inventory
    {
        private readonly List<Item> items = new List<Item>();

        public bool Has(string id)
        {
            bool hasItem = false;

            foreach (Item item in items)
            {
                if (item.Id == id)
                {
                    hasItem = true;
                }
            }

            return hasItem;
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
            Item itemToRemove = null;

            foreach (Item i in items)
            {
                if (item.Id == i.Id)
                {
                    itemToRemove = i;
                    items.Remove(item);
                    Console.WriteLine(item.Name + " verwijderd van je inventaris");
                }

                if (itemToRemove != null)
                {
                    items.Remove(itemToRemove);
                }
            }
        }

        public string ListItems()
        {
            if (items.Count == 0)
                return "Je inventaris is leeg";

            string itemList = "Items in je inventaris: \n";

            foreach (Item item in items)
            {
                itemList += "- " + item.Name + "\n";
            }

            return itemList;
        }

        public void Show()
        {
            if (items.Count == 0)
                Console.WriteLine("Je draagt niets bij je.");
            else
            {
                Console.WriteLine("Je draagt bij je:");
                foreach (var item in items)
                    Console.WriteLine("- " + item);
            }
        }
    }
}
