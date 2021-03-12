using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System.Threading.Tasks;
using DG.Tweening;

namespace VRolijk.Lighting
{
    [ExecuteAlways]
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] bool useSystemTime;

        [Tooltip("Time in miliseconds")]
        [SerializeField] int updateInterval = 1000;
        [Tooltip("Time increment in seconds! This value will only be used when \"useSystemTime\" is disabled")]
        [SerializeField] int timeIncrement = 1;

        Timer timer;

        [Header("Lighting")]
        [SerializeField] [Range(0, 24)] int hours = 12;
        [SerializeField] [Range(0, 60)] int minutes = 0;
        int seconds;
        [SerializeField] Light sun;
        GameObject sunPivot;
        [SerializeField] Light moon;
        GameObject moonPivot;

        [Tooltip("X: Hour | Y: Minut")]
        [SerializeField] Vector2Int sunRiseTime, sunSetTime;

        [Tooltip("X: Sunrise | Y: Sunset")]
        [SerializeField] Vector2 sunYRotation, sunXRotation;
        float dayDuration, currentTime, elapsedDayTime;

        bool moveLights = false;
        bool isDay = true;

        void Awake()
        {
            timer = new Timer(updateInterval);
            // UpdateLight is called everytime the given "updateInterval" has passed
            timer.Elapsed += async (sender, e) => await UpdateLightEvent();
            timer.Start();

            // Calculate the totalTime as a float x.xf where decimals are minutes
            dayDuration = (sunSetTime.x - sunRiseTime.x) + ((sunSetTime.y - sunRiseTime.y ) / 60);

            SetupSunNMoon();
        }

        void SetupSunNMoon()
        {
            sun.gameObject.transform.rotation = Quaternion.Euler(sunXRotation.x, sunYRotation.x, 0);


            moon.gameObject.transform.rotation = sun.gameObject.transform.rotation;
            moon.transform.Rotate(180, 180, 0);
        }

        private void Update()
        {
            if (moveLights)
            {
                SetSunRotation();

                if(hours >= sunRiseTime.x && hours <= sunSetTime.x &&
                   minutes >= sunRiseTime.y && minutes <= sunRiseTime.y && !isDay)
                {
                    RenderSettings.sun = sun;
                    isDay = true;
                } 
                else if (hours < sunRiseTime.x && hours > sunSetTime.x &&
                   minutes < sunRiseTime.y && minutes > sunRiseTime.y && isDay)
                {
                    RenderSettings.sun = moon;
                    isDay = false;
                }
            }
        }

        // Update the sun and moon position
        Task UpdateLightEvent()
        {
            if (useSystemTime)
            {
                hours = System.DateTime.Now.Hour;
                minutes = System.DateTime.Now.Minute;
            }
            else
            {
                seconds += timeIncrement;
                minutes += (int)(seconds / 60f);
                hours += (int)(minutes / 60f);

                seconds %= 60;
                minutes %= 60;

                if (hours >= 25)
                {
                    hours %= 24;
                }
            }

            CalculateCurrentNElapsedTime();

            moveLights = true;
            return null;
        }

        /// <summary>
        /// currentTime: Turns the hour and time variables in a single float
        /// ElapsedDayTime: Current time minus sunRistTime.ToFloat()
        /// </summary>
        void CalculateCurrentNElapsedTime()
        {
            currentTime = hours + (minutes / 60f);

            // The percentage of time elapsed during day time
            elapsedDayTime = dayDuration - ((sunSetTime.x + (sunSetTime.y / 60f)) - currentTime);
            //elapsedDayTime = (currentTime - (sunRiseTime.x + (sunRiseTime.y / 60f)));
        }

        /// <summary>
        /// Reset the hour and minute variables based on System.DateTime.Now
        /// </summary>
        void SyncWithSystem()
        {
            hours = System.DateTime.Now.Hour;
            minutes = System.DateTime.Now.Minute;

            CalculateCurrentNElapsedTime();
            moveLights = true;
        }

        public void SetSunRotation()
        {
            Vector3 rotation = new Vector3(sunXRotation.x + elapsedDayTime / dayDuration * (sunXRotation.y - sunXRotation.x),
                                               sunYRotation.x + elapsedDayTime / dayDuration * (sunYRotation.y - sunYRotation.x),
                                               0);

            sun.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            moon.transform.rotation = Quaternion.Euler(rotation.x + 180, rotation.y + 180, 0);
        }

        void SetSunRotation(Quaternion quaternion)
        {
            sun.transform.rotation = quaternion;
            moon.gameObject.transform.rotation = sun.gameObject.transform.rotation;
            moon.transform.Rotate(180, 180, 0);
        }
    }
}