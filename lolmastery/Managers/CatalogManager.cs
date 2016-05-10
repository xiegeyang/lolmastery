using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lolmastery.Library;
using lolmastery.Managers.ResourceMgr;
using System.Resources;
using System.Reflection;
using System.Configuration;

namespace lolmastery.Manager
{ 
    public class CatalogManager
    {
        /// <summary>
        /// Build API Call.
        /// </summary>
        /// <param name="requestCall">Requested API KEY</param>
        /// <param name="clientParameter">Generic Parameters.</param>
        /// <returns></returns>
        public string BuildApiString(string requestCall, ClientParameters clientParameter)
        {
            //Get API Address from the Resource Catalog (RiotAPICatalog.resx)
            string response =  RiotApiCatalog.ResourceManager.GetString(requestCall);

            Type type = clientParameter.GetType();
            PropertyInfo[] properties = type.GetProperties();

            //Replace Parameters in API Address.
            foreach (PropertyInfo propertyInfo in properties)
            {
                //Get Parameter Name from Resource Catalog (ClientParameterCatalog.resx)
                string parameterName = ClientParameterCatalog.ResourceManager.GetString(propertyInfo.Name);
                string parameterValue = propertyInfo.GetValue(clientParameter, null).ToString();

                if (!string.IsNullOrEmpty(parameterValue))
                {
                    response = response.Replace(parameterName, parameterValue);
                }
            }

            //SET API Key.
            response = response + ConfigurationManager.AppSettings.GetValues("ApiKey")[0].ToString();

            return response;
        }

        /// <summary>
        /// Regions Catalog.
        /// </summary>
        /// <returns></returns>
        public List<Region> GetRegionCatalog()
        {
            List<Region> regions = new List<Region>();
            regions.Add(new Region("br", "BR1", "https://br.api.pvp.net/"));
            regions.Add(new Region("eune", "EUN1", "https://eune.api.pvp.net/"));
            regions.Add(new Region("euw", "EUW1", "https://euw.api.pvp.net/"));
            regions.Add(new Region("jp", "JP1", "https://jp.api.pvp.net/"));
            regions.Add(new Region("kr", "KR", "https://kr.api.pvp.net/"));
            regions.Add(new Region("lan", "LA1", "https://lan.api.pvp.net/"));
            regions.Add(new Region("las", "LA2", "https://las.api.pvp.net/"));
            regions.Add(new Region("na", "NA1", "https://na.api.pvp.net/"));
            regions.Add(new Region("oce", "OC1", "https://oce.api.pvp.net/"));
            regions.Add(new Region("tr", "TR1", "https://tr.api.pvp.net/"));
            regions.Add(new Region("ru", "RU", "https://ru.api.pvp.net/"));
            regions.Add(new Region("Global", "", "https://global.api.pvp.net"));

            return regions;
        }
    }
}
