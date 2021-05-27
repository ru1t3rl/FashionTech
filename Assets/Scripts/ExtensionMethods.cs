using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class ExtensionMethods
{
    public static int TotalSeconds(this DateTime date)
    {
        int totalTimeInSeconds = 0;

        totalTimeInSeconds += date.Month * 12 * 24 * 60 * 60;
        totalTimeInSeconds += date.Day * 24 * 60 * 60;
        totalTimeInSeconds += date.Hour * 60 * 60;
        totalTimeInSeconds += date.Minute * 60;
        totalTimeInSeconds += date.Second;

        return totalTimeInSeconds;
    }


}

namespace UnityEngine
{
    public static class Random2
    {
        public static Vector3 Range(Vector3 minimun, Vector3 maximum) => new Vector3(
            Random.Range(minimun.x, maximum.x),
            Random.Range(minimun.y, maximum.y),
            Random.Range(minimun.z, maximum.z)
        );
    }
}
