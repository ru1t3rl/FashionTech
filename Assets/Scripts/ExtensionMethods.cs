using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class ExtensionMethods
{

    public static int TotalSeconds(this DateTime date)
    {
        int totalTimeInSeconds = 0;

        //totalTimeInSeconds += Mathf.FloorToInt(date.Year * 365.25f * 12 * 24 * 60 * 60);
        totalTimeInSeconds += date.Month * 12 * 24 * 60 * 60;
        totalTimeInSeconds += date.Day * 24 * 60 * 60;
        totalTimeInSeconds += date.Hour * 60 * 60;
        totalTimeInSeconds += date.Minute * 60;
        totalTimeInSeconds += date.Second;

        return totalTimeInSeconds;
    }

}
