using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lolmastery.Library
{
    public class Entries
    {
        public int leaguePoints { get; set; }
        public string isFreshBlood { get; set; }
        public string isHotStreak { get; set; }
        public string division { get; set; }
        public string isInactive { get; set; }
        public string isVeteran { get; set; }
        public int losses { get; set; }
        public string playerOrTeamName { get; set; }
        public string playerOrTeamId { get; set; }
        public int wins { get; set; }
    }
}