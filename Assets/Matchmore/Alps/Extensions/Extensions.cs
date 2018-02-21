using UnityEngine;
using System.Collections;
using System;
using Alps.Model;

namespace MatchmoreExtensions
{
    public static class TimeUtil
    {
        public static double Now()
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }

    public static class ModelExtensions
    {
        public static double? SecondsRemaining(this Publication pub)
        {
            var miliDelta = TimeUtil.Now() - pub.CreatedAt;
            return pub.Duration - (miliDelta / 1000f);
        }

        public static bool IsAlive(this Publication pub)
        {
            return pub.SecondsRemaining() >= 0;
        }


        public static double? SecondsRemaining(this Subscription sub)
        {
            var miliDelta = TimeUtil.Now() - sub.CreatedAt;
            return sub.Duration - (miliDelta / 1000f);
        }

        public static bool IsAlive(this Subscription sub)
        {
            return sub.SecondsRemaining() >= 0;
        }


        public static bool CreatedBy(this Match match, Publication pub = null, Subscription sub = null)
        {
            if (pub == null && sub == null)
            {
                return false;
            }

            var createdBy = true;
            if (pub != null)
            {
                createdBy &= match.Publication.Id == pub.Id;
            }
            if (sub != null)
            {
                createdBy &= match.Subscription.Id == sub.Id;
            }
            return createdBy;
        }

        public static double Distance(this Location loc1, Location loc2)
        {
            return DistanceTo((double)loc1.Latitude, (double)loc1.Longitude, (double)loc2.Latitude, (double)loc2.Longitude, 'M');
        }

        //https://stackoverflow.com/a/24712129
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        public static Location AsMatchmoreLocation(this LocationInfo locationInfo)
        {
            return new Location
            {
                Longitude = locationInfo.longitude,
                Latitude = locationInfo.latitude,
                Altitude = locationInfo.altitude
            };
        }

    }
}
