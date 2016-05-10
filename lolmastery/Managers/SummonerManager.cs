using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using lolmastery.Library;
using System.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace lolmastery.Manager
{
    public class SummonerManager
    {
        /// <summary>
        /// Get Mastery information by Summoner Name
        /// </summary>
        /// <param name="summonerSearchParam">summmoner Search param (SummonerName, Region)</param>
        /// <returns>ActionResponse</returns>
        public static ActionResponse GetSummnonerInfoBySummonerName(List<SummonerSearch> summonerSearchParam)
        {
            ActionResponse response = new ActionResponse();
            List<SummonerInfo> summonerInfoList = new List<SummonerInfo>();
            HttpResponseMessage championPool = new HttpResponseMessage();
            HttpResponseMessage championCatalog = new HttpResponseMessage();
            HttpResponseMessage summonerScore = new HttpResponseMessage();
            HttpResponseMessage summonerLeague = new HttpResponseMessage();

            //iterate thru the summonerSearchParam list.
            foreach(SummonerSearch summonerParam in summonerSearchParam)
            {
                HttpResponseMessage summonersInfo = new HttpResponseMessage();
                ClientParameters clientParameters = new ClientParameters();
                clientParameters.SummonerName = summonerParam.SummonerName;
                clientParameters.Region = summonerParam.Region;

                //Get Summoner Info by Summoner Name.
                summonersInfo = RiotManager.RiotCall("SM_GET_SUMMONERINFO_BY_NAME", clientParameters);

                if (summonersInfo.IsSuccessStatusCode)
                {
                
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    string summonerJSON = summonersInfo.Content.ReadAsStringAsync().Result;

                    var o = JObject.Parse(summonerJSON);

                    //Iteration to remove Root Keys.
                    foreach (JToken child in o.Children())
                    {
                    
                        Int32 score = 0, totalChampUsed = 0, totalLvl5Champ = 0, totalSRankChamp = 0,
                              bestChampScore = 0;
                        Summoner summoner = new Summoner();
                        SummonerInfo summonerInfo = new SummonerInfo();
                        ClientParameters summonerClientParameters = new ClientParameters();
                        ClientParameters champParameters = new ClientParameters();
                        List<Champion> championCatalogList = new List<Champion>();
                        List<ChampionMastery> championList = new List<ChampionMastery>();

                        foreach (JToken grandChild in child)
                        {
                        
                            //Summoner Information.
                            summoner = json_serializer.Deserialize<Summoner>(grandChild.ToString());
                            summonerClientParameters = clientParameters;
                            summonerClientParameters.PlayerID = summoner.id;
                            summonerClientParameters.SummonerID = summoner.id;
                            summonerClientParameters.PlatformID = new CatalogManager().GetRegionCatalog().Where(a => a.RegionCode == summonerClientParameters.Region).FirstOrDefault().PlatformID;

                            //Get Summoner Score.
                            summonerScore = RiotManager.RiotCall("CH_GET_CHAMPIONMASTERY_SCORE", summonerClientParameters);

                            if (championPool.IsSuccessStatusCode)
                            {
                                string summonerScorelJSON = summonerScore.Content.ReadAsStringAsync().Result;
                                score = json_serializer.Deserialize<int>(summonerScorelJSON);
                            }
                            else
                            {
                                response.ActionState = false;
                                response.ErrorMessage = "Couldn't get the champion mastery score, Please refresh the page.";
                                return response;
                            }

                            //Get Summoner Tier.
                            summonerClientParameters.SummonerID = summoner.id;
                            summonerLeague = RiotManager.RiotCall("LG_GET_SUMMONERLEAGUEBYSUMMONERNAME", summonerClientParameters);

                            if (summonerLeague.IsSuccessStatusCode)
                            {
                                string summonerLeaguelJSON = summonerLeague.Content.ReadAsStringAsync().Result;

                                var b = JObject.Parse(summonerLeaguelJSON);

                                foreach (JToken ChampRoot in b.Children())
                                {
                                    foreach (JToken Champions in ChampRoot)
                                    {
                                       var summonerLeagueInfo = json_serializer.Deserialize<List<SummonerLeague>>(Champions.ToString());
                                       summoner.tier = summonerLeagueInfo.FirstOrDefault().tier;
                                    }
                                }
                            }
                            else if (summonerLeague.ReasonPhrase == "Not Found")
                            {
                                summoner.tier = "UNRANKED";
                            }
                            else
                            {
                                response.ActionState = false;
                                response.ErrorMessage = "Couldn't get the champion mastery score, Please refresh the page.";
                                return response;
                            }

                            summonerInfo.Summoner = summoner;

                            //Get Champion Catalog.
                            champParameters.Region = "Global";
                            championCatalog = RiotManager.RiotCall("CH_GET_CHAMPIONCATALOG", champParameters);
                       
                            if (championCatalog.IsSuccessStatusCode)
                            {
                           
                                string championCatalogJSON = championCatalog.Content.ReadAsStringAsync().Result;
                                championCatalogJSON = championCatalogJSON.Substring(championCatalogJSON.IndexOf("data"));

                                championCatalogJSON = @"{""" + championCatalogJSON;

                                var c = JObject.Parse(championCatalogJSON);

                                foreach (JToken ChampRoot in c.Children())
                                {
                                    foreach (JToken Champions in ChampRoot.Children())
                                    {
                                        foreach (JToken Champion in Champions.Children())
                                        {
                                            foreach (JToken Championdata in Champion.Children())
                                            {
                                                Champion SingleChampion = new Champion();
                                                SingleChampion = json_serializer.Deserialize<Champion>(Championdata.ToString());
                                                championCatalogList.Add(SingleChampion);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                response.ActionState = false;
                                response.ErrorMessage = "Couldn't get the champion catalog, Please refresh the page.";
                                return response;
                            }

                            //Get Champion Mastery By Platform and PlayerID
                            championPool = RiotManager.RiotCall("CH_GET_CHAMPIONMASTERY_BY_PLATFORM_AND_PLAYERID", summonerClientParameters);

                            if (championPool.IsSuccessStatusCode)
                            {
                                string championPoolJSON = championPool.Content.ReadAsStringAsync().Result;
                                List<ChampionMastery> championList_tmp = json_serializer.Deserialize<List<ChampionMastery>>(championPoolJSON);

                                foreach (ChampionMastery champ in championList_tmp)
                                {
                                    ChampionMastery champTmp = new ChampionMastery();
                                    champTmp = champ;
                                    champTmp.title = championCatalogList.Where(a => a.id == champTmp.championId).FirstOrDefault().title;
                                    champTmp.name = championCatalogList.Where(a => a.id == champTmp.championId).FirstOrDefault().name;
                                    champTmp.key = championCatalogList.Where(a => a.id == champTmp.championId).FirstOrDefault().key;
                                    championList.Add(champTmp);
                                }
                            }
                            else
                            {
                                response.ActionState = false;
                                response.ErrorMessage = "Couldn't get the champion mastery by summoner, Please refresh the page.";
                                return response;
                            }

                            summonerInfo.Champion = championList;
                        }

                        //Get Summoner Statistics.
                        totalChampUsed = championList.Count();
                        totalLvl5Champ = championList.Where(a => a.championLevel == 5).Count();
                        totalSRankChamp = championList.Where(a => a.highestGrade == "S" || a.highestGrade == "S-" || a.highestGrade == "S+").Count();
                        bestChampScore = championList.OrderByDescending(a => a.championPoints).FirstOrDefault().championPoints;

                        summonerInfo.SummonerStatistics = new Int32[5] { score, totalChampUsed, totalLvl5Champ, totalSRankChamp, bestChampScore };
                        summonerInfoList.Add(summonerInfo);
                    }
                }
                else
                {
                    response.ActionState = false;
                    response.ErrorMessage = "Couldn't get the Summoner Information, Please refresh the page.";
                    return response;
                }
            }

            response.ActionResult = JsonConvert.SerializeObject(summonerInfoList);
            response.ActionState = true;

            return response;
        }
    }
}
