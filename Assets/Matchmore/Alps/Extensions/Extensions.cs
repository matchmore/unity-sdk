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

        public static bool HasTopic(this Match match, string topic)
        {
            return match.Subscription.Topic == topic || match.Publication.Topic == topic;
        }

        public static double Distance(this Location loc1, Location loc2)
        {
            return DistanceTo((double)loc1.Latitude, (double)loc1.Longitude, (double)loc2.Latitude, (double)loc2.Longitude, 'K');
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
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
