using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTextAdventure
{
    public class Item
    {
        public string Id;
        public string Description;

        public Item(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
