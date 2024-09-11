using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    public class Weapon : Item
    {
        public Weapon()
        {
            BaseDamage = 0;
        }

        public Weapon(string _name, int _value, int _baseDamage)
        {
            Name = _name;
            Value = _value;
            BaseDamage = _baseDamage;
        }

        public int BaseDamage { get; set; }

        protected override void OnUse()
        {
            Console.WriteLine("You equip the " + Name + "\n");
            Program.player.Equipped = this;
        }
    }
}
