using System;

namespace DateRanger
{
    public enum TimeDirection { 
        /// <summary>
        /// Previous, Past, History
        /// </summary>
        Last = 1, 
        /// <summary>
        /// Future
        /// </summary>
        Next = 2
    } 

    /// <summary>
    /// vec·tor [vek-ter] noun
    /// 1. a quantity possessing both magnitude and direction...
    ///     
    /// A vector indicating a date range relative to now.
    /// </summary>
    public class TimeVector
    {
        public TimeDirection TimeDirection { get; private set; }
        public int Magnitude { get; private set; }
        public TimeInterval TimeInterval { get; private set; }

        public TimeVector(TimeDirection timeDirection, int magnitude, TimeInterval timeInterval)
        {
            TimeDirection = timeDirection;
            Magnitude = magnitude;
            TimeInterval = timeInterval;
        }

        /// <summary>
        /// Try to parse string with standard search date range format. ex: Next_5_Days
        /// </summary>
        /// <param name="dateRangeString"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public static bool TryParse(string dateRangeString, out TimeVector dateRange)
        {
            var items = dateRangeString.Split('_');

            if (items.Length == 3)
            {
                // ex: Next_5_Days
                // should be 3 items
                // first = [Last|Next]
                // second = int (the magnitude)
                // third = interval [Hour|Minute|etc]

                var td = items[0].ToLower() == "next" ? TimeDirection.Next : TimeDirection.Last;
                int magnitude = int.Parse(items[1]);

                TimeInterval ti;
                if (TimeInterval.TryParse(items[2], out ti))
                {
                    dateRange = new TimeVector(td, magnitude, ti);
                    return true;
                }
            }

            dateRange = null;
            return false;
                
        }
        /// <summary>
        /// ex: Next_5_Days
        /// should be 3 items
        /// first = [Last|Next]
        /// second = int (the magnitude)
        /// third = interval [Hour|Minute|etc]
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string dir = Enum.GetName(typeof(TimeDirection), this.TimeDirection);
            string mag = Magnitude.ToString();
            string interval = TimeInterval.ToString();

            return dir + "_" + mag + "_" + interval;
        }
        
    }
}
