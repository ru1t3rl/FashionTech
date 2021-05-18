using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VRolijk.Weather.Data;
using VRolijk.Weather.Localization;
using VRolijk.Weather.Type;

namespace VRolijk.Weather
{
    public class WeatherManager : MonoBehaviour
    {
        [SerializeField] bool playOnAwake = true;

        [SerializeField] BaseWeather[] weatherConditions;
        public WeatherType currentWeather { get; private set; }
        System.Action OnStopAllWeather;

        void Awake()
        {
            currentWeather = WeatherType.None;

            for (int iWeather = 0; iWeather < weatherConditions.Length; iWeather++)
            {
                OnStopAllWeather += weatherConditions[iWeather].Stop;
            }

            if (playOnAwake)
            {
                SetWeather();
            }
        }

        /// <summary>
        /// Set the active weather to a specific condition
        /// </summary>
        /// <param name="type">Weather Condition</param>
        public void SetWeather(WeatherType type)
        {
            StopAllWeather();

            currentWeather = type;

            for (int iWeather = 0; iWeather < weatherConditions.Length; iWeather++)
            {
                if (weatherConditions[iWeather].Weather == type)
                {
                    weatherConditions[iWeather].gameObject.SetActive(true);
                    weatherConditions[iWeather].Play();
                    break;
                }
            }
        }

        /// <summary>
        /// Set the active weather to the users current real-life weather
        /// </summary>
        public void SetWeather()
        {
            SetWeather(GetWeatherType());
        }

        /// <summary>
        /// Gets the current weather conditions
        /// </summary>
        /// <returns>Current weather of type WeatherType</returns>
        WeatherType GetWeatherType()
        {
            
            CurrentWeather weather = WeatherAPI.GetCurrentWeather("c7ac86f3c41189c78302f6e0a4f27c7c");
            return weather.weather[0].main.ToLower() switch
            {
                "clouds" => WeatherType.Clouds,
                "clear" => WeatherType.Clear,
                "snow" => WeatherType.Snow,
                "rain" => WeatherType.Rain,
                "drizzle" => WeatherType.Rain,
                "thunderstorm" => WeatherType.Thunder,
                "fog" => WeatherType.Fog,
                "mist" => WeatherType.Fog,
                _ => WeatherType.None
            };
        }

        public void StopAllWeather()
        {
            OnStopAllWeather?.Invoke();
            currentWeather = WeatherType.None;
        }
    }
}
