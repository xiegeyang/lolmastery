using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lolmastery.Library
{
    public class SummonerLeague
    {

        public string queue { get; set; }
        public string name { get; set; }
        public List<Entries> entries { get; set; }
        public string tier { get; set; }
    }
}