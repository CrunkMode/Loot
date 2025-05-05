using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.XPath;

namespace loot
{
    class Program
    {

        public static IDictionary<int, int> levels = new Dictionary<int, int>()
        {
            {1, 0}, {2, 300}, {3, 900}, {4, 2700}, {5, 6500}, {6, 14000}, {7, 23000}, {8, 34000}, {9, 48000}, {10, 64000}
        };

        //Player Data
        public static Player player = new Player();
        public static SaveData saveData = new SaveData();
        public static MetaStatistics metaStats = new MetaStatistics();

        //NPC Data
        public static Alchemist Alchemist = new Alchemist();
        public static Blacksmith Blacksmith = new Blacksmith();

        //Inventory Pointer
        public static Pointer pointer = new Pointer();

        //Main 
        static void Main(string[] args)
        {
            MainMenu();
        }

        //Fancy menu screen
        public static void MainMenu()
        {
            Console.Clear();
            player.Health = 15;
            player.MaxHealth = 15;
            player.Inventory.Clear();
            Weapon starterWeapon = new Weapon("Iron Shortsword", 10, 1);
            player.Inventory.Add(starterWeapon);
            player.Equipped = starterWeapon;
            GameMaster.ObtainGold(10000);
            string title = "==============================================\n" +
                           "||||||||||||||||||||||||||||||||||||||||||||||\n" +
                           "==============================================\n\n" +
                           "  L           OOO          OOO       TTTTTTT  \n" +
                           "  L          O   O        O   O         T     \n" +
                           "  L         O     O      O     O        T     \n" +
                           "  L         O     O      O     O        T     \n" +
                           "  L          O   O        O   O         T     \n" +
                           "  LLLLLL      OOO          OOO          T     \n\n" +
                           "==============================================\n" +
                           "||||||||||||||||||||||||||||||||||||||||||||||\n" +
                           "==============================================\n";

            Console.WriteLine(title);
            Prompt.PromptMenu();
        }
    }
}