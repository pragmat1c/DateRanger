using System;
using System.Collections.Generic;

namespace DateRanger
{
    public enum QuarterOfYear
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public class Quarter : IComparable<Quarter>, IEquatable<Quarter>
    {
        public QuarterOfYear QuarterOfYear { get; set; }
        public int Year { get; set; }

        public Quarter(DateTime dateTime)
        {
            Year = dateTime.Year;
            QuarterOfYear = GetQuarterOfYear((MonthOfYear)dateTime.Month);
        }

        public Quarter(QuarterOfYear quarterOfYear, int year)
        {
            Year = year;
            QuarterOfYear = quarterOfYear;
        }

        public static DateTime StartOf(QuarterOfYear quarter, int year)
        {
            switch (quarter)
            {
                case QuarterOfYear.First:
                    return new DateTime(year, 1, 1, 0, 0, 0, 0);
                case QuarterOfYear.Second:
                    return new DateTime(year, 4, 1, 0, 0, 0, 0);
                case QuarterOfYear.Third:
                    return new DateTime(year, 7, 1, 0, 0, 0, 0);
                default:
                    return new DateTime(year, 10, 1, 0, 0, 0, 0);
            }
        }

        public static DateTime EndOf(QuarterOfYear quarter, int year)
        {
            switch (quarter)
            {
                case QuarterOfYear.First:
                    return new DateTime(year, 3, DateTime.DaysInMonth(year, 3), 23, 59, 59, 999);
                case QuarterOfYear.Second:
                    return new DateTime(year, 6, DateTime.DaysInMonth(year, 6), 23, 59, 59, 999);
                case QuarterOfYear.Third:
                    return new DateTime(year, 9, DateTime.DaysInMonth(year, 9), 23, 59, 59, 999);
                default:
                    return new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59, 999);
            }
        }

        public static DateTime StartOf(DateTime date)
        {
            return StartOf(GetQuarterOfYear((MonthOfYear)date.Month), date.Year);
        }

        public static DateTime EndOf(DateTime date)
        {
            
            return EndOf(GetQuarterOfYear((MonthOfYear)date.Month), date.Year);
        }

        public static DateTime EndOfLast()
        {
            return DateTime.Now.Month <= (int) MonthOfYear.March ? 
                EndOf(GetQuarterOfYear(MonthOfYear.December), DateTime.Now.Year - 1) : 
                EndOf(GetQuarterOfYear((MonthOfYear)DateTime.Now.Month), DateTime.Now.Year);
        }

        public static DateTime StartOfLast()
        {
            return DateTime.Now.Month <= 3 ? 
                StartOf(GetQuarterOfYear(MonthOfYear.December), DateTime.Now.Year - 1) : 
                StartOf(GetQuarterOfYear((MonthOfYear)DateTime.Now.Month), DateTime.Now.Year);
        }

        public static Quarter CurrentQuarter()
        {
            return new Quarter(GetQuarterOfYear((MonthOfYear)DateTime.Now.Month), DateTime.Now.Year);
        }

        public static DateTime StartOfCurrent()
        {
            return StartOf(GetQuarterOfYear((MonthOfYear)DateTime.Now.Month), DateTime.Now.Year);
        }

        public static DateTime EndOfCurrent()
        {
            return EndOf(GetQuarterOfYear((MonthOfYear)DateTime.Now.Month), DateTime.Now.Year);
        }

        public static QuarterOfYear GetQuarterOfYear(MonthOfYear month)
        {
            if (month <= MonthOfYear.March)	// 1st Quarter = January 1 to March 31
                return QuarterOfYear.First;
            if ((month >= MonthOfYear.April) && (month <= MonthOfYear.June)) // 2nd Quarter = April 1 to June 30
                return QuarterOfYear.Second;
            if ((month >= MonthOfYear.July) && (month <= MonthOfYear.September)) // 3rd Quarter = July 1 to September 30
                return QuarterOfYear.Third;
            return QuarterOfYear.Fourth; // 4th Quarter = October 1 to December 31
        }

        /// <summary>
        /// Returns the next quarter
        /// </summary>
        /// <returns></returns>
        public Quarter Next()
        {
            if (QuarterOfYear == QuarterOfYear.Fourth)
            {
                return new Quarter(QuarterOfYear.First, Year + 1);
            }
            var newQuarter = (QuarterOfYear)((int)QuarterOfYear + 1);
            return new Quarter(newQuarter,Year);
        }

        public static IEnumerable<Quarter> GetQuartersByDateRange(DateTime startDate, DateTime endDate)
        {
            var currentQuarter = new Quarter(startDate);

            var lastQuarter = new Quarter(endDate);

            while (currentQuarter.CompareTo(lastQuarter) < 0)
            {
                yield return currentQuarter;
                currentQuarter = currentQuarter.Next();
            }
        }


        #region IComparable<Quarter> Members

        /// <summary>
        /// Compares two objects and returns a 32-bit signed integer that indicates the relative order 
        /// of the objects being compared. The return value has these meanings: 
        /// Less than zero - This instance is less than obj. 
        /// Zero - This instance is equal to obj. 
        /// Greater than zero - This instance is greater than obj. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///</returns>
        public int CompareTo(Quarter other)
        {
            // convert this into easily comparable values
            // make them look like YYYYQ or 20091
            var thisValue = int.Parse((Year + ((int)QuarterOfYear).ToString()));
            var otherValue = int.Parse(other.Year + ((int)other.QuarterOfYear).ToString());

            return thisValue.CompareTo(otherValue);
        }

        #endregion

        #region IEquatable<Quarter> Members

        public bool Equals(Quarter other)
        {
            return CompareTo(other) == 0;
        }

        #endregion

        /// <summary>
        /// Get quarter in "YYYYQ#" format.  Example: 2009Q1
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}Q{1}", Year, (int)QuarterOfYear);
        }

        /// <summary>
        /// Get quarter in "Quarter # YYYY" format.  Example: Quarter 1 2009
        /// </summary>
        /// <returns></returns>
        public string ToLongString()
        {
            return string.Format("Quarter {0}, {1}", QuarterOfYear, Year);
        }

        /// <summary>
        /// Parses a string in YYYYQ# format
        /// </summary>
        /// <param name="s">a string in YYYYQ# format</param>
        /// <param name="quarter"></param>
        /// <returns>true if parse succeeded, false if failed.</returns>
        public static bool TryParse(string s, out Quarter quarter)
        {
            if (s != null)
            {
                var items = s.Split('Q');
                if (items.Length == 2)
                {
                    int year;
                    if (int.TryParse(items[0], out year))
                    {
                        int q;
                        if (int.TryParse(items[1], out q))
                        {
                            if (q > 0 && q < 5)
                            {
                                quarter = new Quarter((QuarterOfYear) q, year);
                                return true;
                            }
                        }
                    }
                }
            }
            quarter = null;
            return false;
            
        }
    }
}
