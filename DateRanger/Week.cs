using System;

namespace DateRanger
{
    public class Week 
    {
        public static DateTime StartOf(DateTime day)
        {
            int DaysToSubtract = (int)day.DayOfWeek;
            DateTime dt = DateTime.Now.Subtract(System.TimeSpan.FromDays(DaysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime EndOf(DateTime day)
        {
            DateTime dt = StartOf(day).AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfCurrent()
        {
            return StartOf(DateTime.Now);
        }

        public static DateTime EndOfCurrent()
        {
            return EndOf(DateTime.Now);
        }

        public static DateTime StartOfLast()
        {
            return StartOf(DateTime.Now).AddDays(-7);
        }

        public static DateTime EndOfLast()
        {
            return EndOfCurrent().AddDays(-7);
        }

        public static DateTime StartOfNext()
        {
            return StartOfCurrent().AddDays(7);
        }

        public static DateTime EndOfNext()
        {
            return EndOfCurrent().AddDays(7);
        }
    }
}
