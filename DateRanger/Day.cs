using System;

namespace DateRanger
{
    /// <summary>
    /// Day contains mostly static utility methods for working with start and end DateTime.
    /// </summary>
    public class Day
    {
        /// <summary>
        /// Returns the start of the day from the date provided. Start time is 0000 hrs.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOf(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns the last instant of the day from the date provided. End time is 23:59:59:999.
        /// This is the most precise time supported by the DateTime contructor.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOf(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        /// <summary>
        /// Start of the current day using DateTime.Now.
        /// </summary>
        /// <returns></returns>
        public static DateTime StartOfCurrent()
        {
            return StartOf(DateTime.Now);
        }

        /// <summary>
        /// End of the current day using DateTime.Now.
        /// </summary>
        /// <returns></returns>
        public static DateTime EndOfCurrent()
        {
            return EndOf(DateTime.Now);
        }

        /// <summary>
        /// Start of the yesterday using DateTime.Now as starting point.
        /// </summary>
        /// <returns></returns>
        public static DateTime StartOfYesterday()
        {
            return StartOf(DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// Enhd of the yesterday using DateTime.Now as starting point.
        /// </summary>
        /// <returns></returns>
        public static DateTime EndOfYesterday()
        {
            return EndOf(DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// Start of the tomorow using DateTime.Now as starting point.
        /// </summary>
        /// <returns></returns>
        public static DateTime StartOfTomorrow()
        {
            return StartOf(DateTime.Now.AddDays(1));
        }

        /// <summary>
        /// End of the tomorrow using DateTime.Now as starting point.
        /// </summary>
        /// <returns></returns>
        public static DateTime EndOfTomorrow()
        {
            return EndOf(DateTime.Now.AddDays(1));
        }

        /// <summary>
        /// Return the next date that matches this DayOfWeek
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static DateTime GetNextDayOfWeekOccurance(DayOfWeek dayOfWeek, DateTime startingDate)
        {
            var test = startingDate;

            while (true)
            {
                test = test.AddDays(1);
                if (test.DayOfWeek == dayOfWeek)
                    return test;
            }
        }
    }
}
