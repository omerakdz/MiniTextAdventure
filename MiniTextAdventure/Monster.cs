using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTextAdventure
{
    public class Monster
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private bool isAlive;
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public Monster(string name)
        {
            this.name = name;
            this.isAlive = true;
        }

        public void Kill()
        {
            isAlive = false;
        }

        public override string ToString()
        {
            return IsAlive ? $"{Name} (levend)" : $"{Name} (dood)";
        }
    }
}
