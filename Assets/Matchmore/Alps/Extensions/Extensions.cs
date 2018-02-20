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

    }
}
