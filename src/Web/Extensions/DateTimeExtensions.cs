using System;

namespace Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        public static TimeSpan Hours(this double hours)
        {
            return TimeSpan.FromHours(hours);
        }
    }
}