using System.Collections;
using System.Collections.Generic;
using System;
//using System.Device.Location;

namespace VRolijk.Weather.Localization
{
    public static class GeoLocalizer
    {
        public static string GetLocation()
        {
            //GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            //UnityEngine.Debug.Log("Permission: "+ watcher.Permission.ToString());

            /*
            _ = watcher.Permission switch
            {
                GeoPositionPermission.Granted => watcher.TryStart(true, TimeSpan.FromSeconds(1f)),
                GeoPositionPermission.Denied => throw new Exception("Geo Position Permission Denied"),
                GeoPositionPermission.Unknown => watcher.TryStart(false, TimeSpan.FromSeconds(1f)),
                _ => throw new Exception("Unknown excpetion"),
            };
            */
            return null;
            //return watcher.Position.Location;
        }
    }
}