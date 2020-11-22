using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncoderDecoder.Utilities
{
    public class EpochHelper
    {
        public static string TimeZoneOffsetToTimeZone(TimeSpan offset)
        {
            return $"GMT {(offset.Hours >= 0 && offset.Minutes >= 0 ? "+" : string.Empty)}{offset.Hours.ToString("D2")}:{offset.Minutes.ToString("D2")}";
        }

        public static string GetTimeDiffMessage(TimeSpan timeSpan, bool isPast)
        {
            if (timeSpan.TotalSeconds < 60)
                return isPast ? "A few seconds ago" : "In a few seconds";

            if (timeSpan.TotalMinutes < 2)
                return isPast ? $"A minute ago" : $"In a minute";

            if (timeSpan.TotalMinutes < 60)
                return isPast ? $"{(int)timeSpan.TotalMinutes} minutes ago" : $"In {(int)timeSpan.TotalMinutes} minutes";

            if (timeSpan.TotalHours < 2)
                return isPast ? $"A hour ago" : $"In a hour";

            if (timeSpan.TotalHours < 24)
                return isPast ? $"{(int)timeSpan.TotalHours} hours ago" : $"In {(int)timeSpan.TotalHours} hours";

            if (timeSpan.TotalDays < 2)
                return isPast ? $"A day ago" : $"In a day";

            if (timeSpan.TotalDays < 30)
                return isPast ? $"{(int)timeSpan.TotalDays} days ago" : $"In {(int)timeSpan.TotalDays} days";

            if (timeSpan.TotalDays < 60)
                return isPast ? $"A month ago" : $"In a month";

            if (timeSpan.TotalDays < 365)
                return isPast ? $"{Math.Round(timeSpan.TotalDays / 30)} months ago" : $"In {Math.Round(timeSpan.TotalDays / 30)} months";

            if (timeSpan.TotalDays < 730)
                return isPast ? $"A year ago" : $"In a year";


            return isPast ? $"{Math.Round(timeSpan.TotalDays / 365)} years ago" : $"In {Math.Round(timeSpan.TotalDays / 365)} years";
        }



    }
}
