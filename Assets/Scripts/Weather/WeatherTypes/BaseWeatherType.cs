using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BaseWeatherType : MonoBehaviour
{
    [SerializeField] private VisualEffect weatherEffect;
    public VisualEffect WeatherEffect => weatherEffect;

    public System.Action OnPlay, OnStop;

    public virtual void Play()
    {
        OnPlay?.Invoke();
    }

    public virtual void Stop()
    {
        OnStop?.Invoke();
    }
}
