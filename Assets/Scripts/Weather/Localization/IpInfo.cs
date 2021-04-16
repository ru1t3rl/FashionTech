using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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

        #region SyncWithIp
        private HttpWebRequest request;
        private Regex ipRegex = new Regex(@"(?:[0-9]{1,3}\.){3}[0-9]{1,3}");

        public void SyncWithExternalIp()
        {
            request = (HttpWebRequest)HttpWebRequest.Create("http://checkip.dyndns.org");
            request.Method = "GET";

            try
            {
                // Get the External IP
                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                string result = reader.ReadToEnd();

                var externalIp = ipRegex.Matches(result)[0];
                Debug.Log($"<b>[Weather API - IpInfo]</b>  IP: {externalIp}");

                // Get the IP Information
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

                Debug.Log($"<b>[Weather API - IpInfo]</b>  City: {City} | Cords: {Loc}");

                reader.Close();
                dataStream.Close();
            }
            catch (WebException e)
            {
                Debug.LogError($"<b>[Weather API - IpInfo]</b> {e.Message}");
            }
        }
        #endregion
    }
}
