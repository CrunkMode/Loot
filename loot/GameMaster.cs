using System;
using System.Reflection;

namespace loot
{
    class GameMaster
    {
        public static void ObtainGold()
        {
            Random rand = new Random();
            int chance = rand.Next(10);
            int totalGold = chance + (1 * Program.player.Level);
            Program.player.Gold += totalGold;
            Program.metaStats.GoldObtained += totalGold;
            Console.WriteLine("You've obtained " + totalGold + " gold.\n");
        }

        public static void ObtainGold(int amount)
        {
            Program.player.Gold += amount;
            Program.metaStats.GoldObtained += amount;
            Console.WriteLine("You've obtained " + amount + " gold.\n");
        }

        public static int RollDice(int sides)
        {
            Random rand = new Random();
            int chance = rand.Next(sides);
            return chance++;
        }

        public static void CopyData<T>(T source, T destination)
        {
            try
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (property.CanWrite)
                    {
                        var value = property.GetValue(source);
                        property.SetValue(destination, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press enter to continue");
                Console.Read();
            }
        }

        //This method activates if the player finds a chest while exploring
        public static void FindChest()
        {
            Random rand = new Random();
            int chance = rand.Next(100);
            Console.Clear();

            //Health Crystal
            if (chance >= 0 && chance <= 5)
            {
                Crystal crystal = new Crystal();
                crystal.Name = "Health Crystal";
                crystal.Value = 100;
                Console.WriteLine("You find a health crystal.\n");
                Program.player.Inventory.Add(crystal);
            }
            //Potion
            else if (chance >= 11 && chance <= 40)
            {
                Potion potion = new Potion();
                potion.Name = "Potion";
                potion.Value = 50;
                Console.WriteLine("You find a potion.\n");
                Program.player.Inventory.Add(potion);
            }
            //Gold
            else
            {
                int chestGold = (rand.Next(5) + Program.player.Level);
                Console.WriteLine("You found " + chestGold + " gold.\n");
                Program.player.Gold += chestGold;
            }
        }

        //This method activates if the player finds an enemy while exploring
        public static void FindEnemy()
        {
            Enemy enemy = new Enemy();
            Console.Clear();
            Random rand = new Random();
            int chance = rand.Next(30);

            InitiateCombat(enemy);
        }

        //This method activates if the player encounters a trap.
        public static void FindTrap()
        {
            Console.Clear();
            Console.WriteLine("You accidentally trip a wire trap! Arrows shoot out and hit you for 2 health.\n");
            Program.player.Health -= 2;
        }

        //This method activates if the player finds nothing while adventuring
        public static void FindNothing()
        {
            Console.Clear();
            Console.WriteLine("You delve further into the dungeon.\n");
            Prompt.PromptUser();
        }

        //This method activated when the player fights an enemy.
        public static void InitiateCombat(Enemy enemy)
        {
            Console.WriteLine("You start a fight with an enemy " + enemy.Name + "!");

            Random rand = new Random();
            int chance = rand.Next(50);

            if (chance >= 0 && chance <= 25)
            {
                Console.WriteLine("Your natural speed lets you attack first.");
                int value = 0;
                value = Program.player.Equipped.BaseDamage;
                Console.WriteLine("You hit your enemy for " + value + " damage!");
                enemy.Health -= value;
                Prompt.PromptBattle(enemy);
            }
            else
            {
                Console.WriteLine("The enemy's speed gives it an advantage.");
                Console.WriteLine("You take 1 point of damage\n");
                Program.player.Health--;

                if (Program.player.Health <= 0)
                    Player.Die();

                Prompt.PromptBattle(enemy);
            }
        }

        //This method activates when the player obtains exp
        public static void ObtainEXP()
        {
            Console.WriteLine("You obtained 50 EXP.\n");
            Program.player.Experience += 50;

            if (Program.player.Experience >= Program.metaStats.NextLevel)
            {
                Console.WriteLine("Congratulations! You have leveled up.\n");
                Program.player.Level++;
                Program.metaStats.NextLevel = Program.levels[Program.player.Level + 1];
                Prompt.PromptUser();
            }
        }

    }
}