using System;

namespace Web.Models
{
    public class Time
    {
        public static DateTime Now()
        {
            return FakeTime.nowFactory();
        }

        public static DateTime Today()
        {
            return Now().Date;
        }

        public static TimeSpan TimeOfDay()
        {
            return Now().TimeOfDay;
        }
    }

    public class FakeTime
    {
        public static Func<DateTime> nowFactory = GetNowFactory();

        public static void Fake(DateTime timeToFake)
        {
            nowFactory = () => timeToFake;
        }

        public static void Reset()
        {
            nowFactory = GetNowFactory();
        }

        static Func<DateTime> GetNowFactory()
        {
            return () => DateTime.UtcNow;
        }
    }
}