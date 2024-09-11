using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    class Crystal : Item
    {
        public Crystal()
        {
            Name = "Crystal";
            Value = 50;
        }

        protected override void OnUse()
        {
            Console.WriteLine("The crystal responds to your desire for greatness.");
            Console.WriteLine("Your max health is increased by 1.\n");
            Program.player.MaxHealth++;
            Program.player.Inventory.Remove(this);
            Program.metaStats.CrystalsUsed++;
        }
    }
}
