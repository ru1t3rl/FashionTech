using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.DayNight
{
    public class MoonModule : DNModuleBase
    {
        [SerializeField] Light moon;
        [SerializeField] Gradient moonColor;
        [SerializeField] float baseIntensity, intensityVariation;
        [SerializeField] float minActiveIntensity;

        public override void UpdateModule(float intensity)
        {
            moon.color = moonColor.Evaluate(1 - intensity);
            moon.intensity = baseIntensity + intensity * intensityVariation;
        }
    }
}
