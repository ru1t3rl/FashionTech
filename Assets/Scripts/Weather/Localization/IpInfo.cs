using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Net;
using UnityEngine;
using Valve.Newtonsoft.Json;

namespace VRolijk.Weather.Localization
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }

        private WebClient client;
        public void SyncWithExternalIp()
        {
            client = new WebClient();

            string response = client.DownloadString(new System.Uri("http://icanhazip.com"));

            if (string.IsNullOrEmpty(response))
            {
                var externalIp = IPAddress.Parse(response.Replace("\\r\\n", "").Replace("\\n", "").Trim());

                string info = new WebClient().DownloadString("http://ipinfo.io/" + externalIp);
                IpInfo locationInfo = JsonConvert.DeserializeObject<IpInfo>(info);

                Ip = locationInfo.Ip;
                Hostname = locationInfo.Hostname;
                City = locationInfo.City;
                Region = locationInfo.Region;
                Country = locationInfo.Country;
                Loc = locationInfo.Loc;
                Org = locationInfo.Org;
                Postal = locationInfo.Postal;
            }

        }
    }
}
