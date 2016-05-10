using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lolmastery.Library
{
    public class ClientParameters
    {
        public int SummonerID { get; set; } = 0;
        public string SummonerName { get; set; } = "";
        public string Region { get; set; } = "";
        public int ChampionID { get; set; } = 0;
        public int TeamID { get; set; } = 0;
        public int MatchID { get; set; } = 0;
        public string PlatformID { get; set; } = "";
        public int PlayerID { get; set; } = 0; 

    }
}
