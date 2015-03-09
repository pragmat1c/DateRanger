using System;

namespace DateRanger
{
    public enum MonthOfYear
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class Month
    {
        public static DateTime StartOf(int Month, int Year)
        {
            return new DateTime(Year, Month, 1, 0, 0, 0, 0);
        }

        public static DateTime EndOf(int Month, int Year)
        {
            return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 23, 59, 59, 999);
        }

        public static DateTime StartOf(DateTime date)
        {
            return StartOf(date.Month, date.Year);
        }

        public static DateTime EndOf(DateTime date)
        {
            return EndOf(date.Month, date.Year);
        }

        public static DateTime StartOfLast()
        {
            if (DateTime.Now.Month == 1)
                return StartOf(12, DateTime.Now.Year - 1);
            else
                return StartOf(DateTime.Now.Month - 1, DateTime.Now.Year);
        }

        public static DateTime EndOfLast()
        {
            if (DateTime.Now.Month == 1)
                return EndOf(12, DateTime.Now.Year - 1);
            else
                return EndOf(DateTime.Now.Month - 1, DateTime.Now.Year);
        }

        public static DateTime StartOfCurrent()
        {
            return StartOf(DateTime.Now.Month, DateTime.Now.Year);
        }

        public static DateTime EndOfCurrent()
        {
            return EndOf(DateTime.Now.Month, DateTime.Now.Year);
        }

        public static DateTime StartOfNext()
        {
            if (DateTime.Now.Month == 12)
                return StartOf(1, DateTime.Now.Year + 1);
            else
                return StartOf(DateTime.Now.Month + 1, DateTime.Now.Year);
        }

        public static DateTime EndOfNext()
        {
            if (DateTime.Now.Month == 12)
                return EndOf(1, DateTime.Now.Year + 1);
            else
                return EndOf(DateTime.Now.Month + 1, DateTime.Now.Year);
        }

        /// <summary>
        /// Return the next date that matches this MonthOfYear.
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime NextMonthOccurance(MonthOfYear month, DateTime startingDate)
        {
            var test = startingDate;

            while (true)
            {
                test = test.AddMonths(1);
                if (test.Month == (int)month)
                    return test;
            }
        }
    }
}
