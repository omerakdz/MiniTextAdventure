using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Text_Adventure
{
    internal class Item
    {
		private string id;

		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string description;

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public Item(string id1,string name1,string description1)
		{
			id = id1;
			name = name1; 
			description = description1;
		}

        public override string ToString()
        {
            return $"{Name}: {Description}";
        }


	}
}
