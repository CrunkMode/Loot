using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace loot
{
    class Player
    {
        public Player()
        {
            MaxHealth = 15;
            Health = 15;
            Gold = 0;
            Level = 1;
            Experience = 0;
            Name = "";
            Inventory = new List<Item>();
            Location = "dungeon";
        }

        public List<Item> Inventory { get; set; }

        public int Health { get; set; }

        public int Gold { get; set; }

        public int MaxHealth { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

        public Weapon Equipped { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public static void Die()
        {
            Console.WriteLine("Your days of adventuring are cut short as you collapse to the floor.");
            if (Program.metaStats.CurrentDepth <= 10)
                Console.WriteLine("And at the very beginning of the exploration, too.\n\n");
            Console.WriteLine("Press enter to continue.");
            Console.Read();
            Program.MainMenu();
        }
    }

    
}
