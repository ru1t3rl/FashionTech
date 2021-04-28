using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace VRolijk.Weather.Type
{
    public class BaseWeather : MonoBehaviour
    {
        [SerializeField] protected VisualEffect weatherEffect;
        public VisualEffect WeatherEffect => weatherEffect;

        [SerializeField] protected WeatherType weather;
        public WeatherType Weather => weather;

        public bool IsPlaying { get; private set; }

        public UnityEvent OnPlay, OnStop;

        private void Awake()
        {
            IsPlaying = false;
            weatherEffect.Stop();

            gameObject.SetActive(false);
        }

        public virtual void Play()
        {
            IsPlaying = true;
            OnPlay?.Invoke();

            Debug.Log($"<b>[{Weather}]</b> Whoop Whoop... I have been activated");
        }

        public virtual void Stop()
        {
            IsPlaying = false;
            OnStop?.Invoke();

            gameObject.SetActive(false);
        }
    }

    public enum WeatherType
    {
        None = 0,
        Clouds = 1,
        Rain = 3,
        Drizzle = 4,
        Thunder = 6,
        Snow = 7,
        Clear = 8,
        Fog = 9
    }
}
