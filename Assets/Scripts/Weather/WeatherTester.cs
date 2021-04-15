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

    private void Awake()
    {
        weather = WeatherAPI.GetCurrentWeather("c7ac86f3c41189c78302f6e0a4f27c7c");
        //weather = await WeatherAPI.GetCurrentWeatherAsync("c7ac86f3c41189c78302f6e0a4f27c7c", info.Loc);
    }

    void Update()
    {
        if (!printed && weather != null)
        {
            for (int iWeather = 0; iWeather < weather.weather.Length; iWeather++)
            {
                Debug.Log($"<b>[Weather Test]</b> {weather.weather[iWeather].main}");
            }
        }
    }
}
