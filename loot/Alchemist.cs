using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    class Alchemist
    {
        public Alchemist()
        {
            Inventory = new List<Item>() {
                new Potion(),
                new Crystal()
            };
            Gold = 100;
        }
        public List<Item> Inventory { get; set; }
        public int Gold { get; set; }
    }
}
