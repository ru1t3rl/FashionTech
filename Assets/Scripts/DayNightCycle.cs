using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] bool useSystemTime = true;

    [Header("Lighting")]
    [SerializeField] [Range(0, 24)] int hour = 12;
    [SerializeField] [Range(0, 60)] int minutes = 0;

    [SerializeField] Light sun;
    [SerializeField] Light moon;
}
