using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTextAdventure
{
    public class Inventory
    {
        private List<Item> items = new List<Item>();

        public void Add(Item item)
        {
            if (item != null) return;
            bool bestaat = false;

            foreach(Item i in items)
            {
                if(i.Id == item.Id)
                {
                    bestaat = true;
                    break;
                }

            }

            if (!bestaat)
            {
                items.Add(item);
            }
            else
            {
                Console.WriteLine("Deze item zit al in je inventaris");
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
        public bool HasItem(string id)
        {
            bool hasIt = false;

            foreach (Item i in items)
            {
                if(i.Id == id)
                {
                    hasIt = true;
                }
            }

            return hasIt;
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
    }
}
