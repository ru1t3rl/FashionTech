using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Valve.Newtonsoft.Json;
using VRolijk.Weather.Data;
using VRolijk.Weather.Localization;

namespace VRolijk.Weather
{
    public class WeatherAPI
    {
        private static HttpWebRequest request;

        /// <summary>
        /// Request the current weather from openweathermap api
        /// </summary>
        /// <param name="key">Openweather API key</param>        
        /// <returns>Returns the current weather of your current location; based on your IP Address</returns>
        public static CurrentWeather GetCurrentWeather(string apiKey)
        {
            IpInfo ipInfo = new IpInfo();
            ipInfo.SyncWithExternalIp();

            return GetCurrentWeather(apiKey, ipInfo.Loc);
        }

        /// <summary>
        /// Request the current weather from openweathermap api
        /// </summary>
        /// <param name="key">Openweather API key</param>
        /// <param name="cityName">City Name</param>
        /// <returns>Returns the current weather of cityName</returns>
        public static CurrentWeather GetCurrentWeather(string apiKey, string cityName)
        {
            string result = String.Empty;
            request = (HttpWebRequest)HttpWebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&units=imperial&appid={apiKey}");
            request.Method = "GET";
            request.Accept = "application/json";

            try
            {
                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                result = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
            }
            catch (WebException e)
            {
                Debug.Log($"<b>[Weather API]</b> {e.Message}");
            }

            return JsonConvert.DeserializeObject<CurrentWeather>(result);
        }

        /// <summary>
        /// Request the current weather from openweathermap api
        /// </summary>
        /// <param name="key">Openweather API key</param>        
        /// <returns>Returns the current weather of your current location; based on your IP Address</returns>
        public static async Task<CurrentWeather> GetCurrentWeatherAsync(string apiKey)
        {
            IpInfo ipInfo = new IpInfo();
            ipInfo.SyncWithExternalIp();

            return await GetCurrentWeatherAsync(apiKey, ipInfo.Loc);
        }

        /// <summary>
        /// Request the current weather from openweathermap api
        /// </summary>
        /// <param name="key">Openweather API key</param>
        /// <param name="cityName">Zip code...</param>
        /// <returns>Returns the current weather of cityName</returns>
        public static async Task<CurrentWeather> GetCurrentWeatherAsync(string apiKey, string cityName)
        {
            string result = String.Empty;
            request = (HttpWebRequest)HttpWebRequest.Create($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&units=imperial&appid={apiKey}");
            request.Method = "GET";
            request.Accept = "application/json";

            try
            {
                await request.GetResponseAsync().ContinueWith(async task =>
                {
                    Stream dataStream = task.Result.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);

                    result = await reader.ReadToEndAsync();

                    reader.Close();
                    dataStream.Close();
                });                
            }
            catch (WebException e)
            {
                Debug.Log($"<b>[Weather API] {e.Message}");
            }

            return JsonConvert.DeserializeObject<CurrentWeather>(result);
        }
    }
}
