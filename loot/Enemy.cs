using System;

namespace loot
{
    class Enemy
    {
        public Enemy()
        {
            Health = 0;
            for(int i = 0; i <= Program.player.Level; i++)
            {
                Health += (GameMaster.RollDice(8)*Program.player.Level);
            }

            if (Health >= 50) Name = "Dragon";
            else if (Health <= 49 && Health >= 40) Name = "Orc";
            else if (Health <= 39 && Health >= 25) Name = "Skeleton";
            else if (Health <= 24 && Health >= 15) Name = "Goblin";
            else Name = "Spider";
        }

        public static void Die()
        {
            Console.WriteLine("The enemy is defeated!\n");
            GameMaster.ObtainGold();
            Program.metaStats.EnemiesSlain++;
            GameMaster.ObtainEXP();
            Prompt.PromptUser();
        }

        public int Health { get; set; }
        public string Name { get; set; }
    }
}
