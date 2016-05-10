using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lolmastery.Library
{
    public class Region
    {
        public Region(string mRegionCode, string mPlatformID, string mHost)
        {
            RegionCode = mRegionCode;
            Host = mHost;
            PlatformID = mPlatformID;
        }
        public string RegionCode { get; set; }
        public string PlatformID { get; set; }
        public string Host { get; set; }
        
    }
}