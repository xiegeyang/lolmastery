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


namespace lolmastery.Manager
{
    public class RiotManager
    {
        /// <summary>
        /// Async Method to call Riot API.
        /// </summary>
        /// <param name="requestCall">Requested Key(RiotApiCatalog.resx)</param>
        /// <param name="clientParameter">Generic Client Parameters</param>
        /// <returns></returns>
        public static HttpResponseMessage RiotCall(string requestCall, ClientParameters clientParameter)
        {
            return RunAsync(requestCall, clientParameter).Result;
        }

        /// <summary>
        /// Async Method to call Riot API.
        /// </summary>
        /// <param name="requestCall">Requested Key(RiotApiCatalog.resx)</param>
        /// <param name="clientParameter">Generic Client Parameters</param>
        /// <returns></returns>
        static async Task<HttpResponseMessage> RunAsync(string requestCall, ClientParameters clientParameter)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            using (var client = new HttpClient())
            {
                //Get Region Catalog.
                Region regions = new CatalogManager().GetRegionCatalog().Where(a => a.RegionCode == clientParameter.Region).FirstOrDefault();
                client.BaseAddress = new Uri(regions.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string call = new CatalogManager().BuildApiString(requestCall, clientParameter).Replace("static-data/Global/", "static-data/na/");
                response = Task.Run(() => client.GetAsync(call)).Result;
            }

            return response;
        }
    }
}
