using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using VRolijk.Weather.Localization;
using Valve.Newtonsoft.Json;
using System.Globalization;
using VRolijk.Weather;
using VRolijk.Weather.Data;
using System.Threading.Tasks;

public class WeatherTester : MonoBehaviour
{
    CurrentWeather weather;
    bool printed = false;

    private async void Awake()
    {        
        IpInfo info = new IpInfo();
        info.SyncWithExternalIp();

        weather = await WeatherAPI.GetCurrentWeatherAsync("c7ac86f3c41189c78302f6e0a4f27c7c", info.City);
        Debug.Log($"<b>[Weather Test]</b> {weather.weather[0].main}: {weather.weather[0].description}");
    }
}
