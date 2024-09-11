using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loot
{
    class MetaStatistics
    {
        public MetaStatistics()
        {
            CurrentDepth = 0;
            ExplorationsLasted = 0;
            EnemiesSlain = 0;
            PotionsDrank = 0;
            CrystalsUsed = 0;
            GoldObtained = 0;
            NextLevel = Program.levels[Program.player.Level + 1];
        }

        public int CurrentDepth { get; set; }
        public int ExplorationsLasted { get; set; }
        public int EnemiesSlain { get; set; }
        public int PotionsDrank { get; set; }
        public int CrystalsUsed { get; set; }
        public int GoldObtained { get; set; }
        public int NextLevel { get; set; }
    }
}
