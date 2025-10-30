using System;
using System.Collections.Generic;

namespace MiniTextAdventure
{
    public class Inventory
    {
        private List<string> items = new List<string>();

        public void Add(string item) => items.Add(item);
        public bool Has(string item) => items.Contains(item);

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
