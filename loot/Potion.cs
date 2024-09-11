using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    class Potion : Item
    {
        public Potion()
        {
            Name = "Potion";
            Value = 20;
        }

        protected override void OnUse()
        {
            Console.WriteLine("You drink the potion and feel your wounds begin to heal immediately." +
                                                  "\nYour health is restored to " + Program.player.MaxHealth + "\n");
            Program.player.Health = Program.player.MaxHealth;
            Program.player.Inventory.Remove(this);
            Program.metaStats.PotionsDrank++;
        }
    }
}
