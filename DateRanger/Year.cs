
using System;
namespace DateRanger
{
    public class Year
    {
        public static DateTime StartOf(int Year)
        {
            return new DateTime(Year, 1, 1, 0, 0, 0, 0);
        }

        public static DateTime EndOf(int Year)
        {
            return new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12), 23, 59, 59, 999);
        }

        public static DateTime StartOf(DateTime date)
        {
            return StartOf(date.Year);
        }

        public static DateTime EndOf(DateTime date)
        {
            return EndOf(date.Year);
        }

        public static DateTime StartOfLast()
        {
            return StartOf(DateTime.Now.Year - 1);
        }

        public static DateTime EndOfLast()
        {
            return EndOf(DateTime.Now.Year - 1);
        }

        public static DateTime StartOfNext()
        {
            return StartOf(DateTime.Now.Year + 1);
        }

        public static DateTime EndOfNext()
        {
            return EndOf(DateTime.Now.Year + 1);
        }

        public static DateTime StartOfCurrent()
        {
            return StartOf(DateTime.Now.Year);
        }

        public static DateTime EndOfCurrent()
        {
            return EndOf(DateTime.Now.Year);
        }
    }
}
