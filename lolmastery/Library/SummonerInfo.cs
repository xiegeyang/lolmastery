using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolmastery.Library
{
    public class SummonerInfo
    {
        public Summoner Summoner { get; set; }
        public List<ChampionMastery> Champion { get; set; }
        public Int32[] SummonerStatistics { get; set; }
    }   
}
