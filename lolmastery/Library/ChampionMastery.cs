using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolmastery.Library
{
    public class ChampionMastery
    {
        public string title { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string highestGrade { get; set; }
        public int championPoints { get; set; }
        public int playerId { get; set; }
        public int championPointsUntilNextLevel { get; set; }
        public bool chestGranted { get; set; }
        public int championLevel { get; set; }
        public int championId { get; set; }
        public int championPointsSinceLastLevel { get; set; }
        public Int64 lastPlayTime { get; set; }
    }
}
