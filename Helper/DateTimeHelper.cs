using System;

namespace Helper
{
    public static class DateTimeHelper
    {
        public static DateTime ToSiteLocal(this DateTime dtUtc)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(SettingsManager.TimezoneName);
            return tz == null ? dtUtc.ToLocalTime() : TimeZoneInfo.ConvertTimeFromUtc(dtUtc, tz);
        }

        public static string ToAmPmString(this TimeSpan span)
        {
            DateTime time = DateTime.Today.Add(span);
            return time.ToString("hh:mm tt");
        }
    }
}
