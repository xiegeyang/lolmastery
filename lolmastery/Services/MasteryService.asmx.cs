using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using lolmastery.Library;
using lolmastery.Manager;
using Newtonsoft.Json;

namespace lolmastery.Services
{
    /// <summary>
    /// Summary description for MasteryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MasteryService : System.Web.Services.WebService
    {
        /// <summary>
        /// Get Mastery information by Summoner Name
        /// </summary>
        /// <param name="summonerSearchParam">summmoner Search param (SummonerName, Region)</param>
        /// <returns>ActionResponse</returns>
        [WebMethod]
        public ActionResponse MasteryInfoBySummonerName(List<SummonerSearch> summonerSearchParam)
        {
            ActionResponse response = new ActionResponse();

            try
            {
                response = SummonerManager.GetSummnonerInfoBySummonerName(summonerSearchParam);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.ActionState = false;
            }

            return response;
        }
    }
}
