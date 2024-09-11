using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    class Blacksmith
    {
        public Blacksmith()
        {
            Inventory = new List<Item>()
            {
                new Weapon("Steel Sword", 10, 4),
                new Weapon("Iron Greatsword", 8, 3)
            };
            Gold = 100;
        }

        public List<Item> Inventory { get; set; }
        public int Gold { get; set; }
    }
}
