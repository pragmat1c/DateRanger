using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;

namespace DateRanger
{
    /// <summary>
    /// Common units of time used throught this library.
    /// </summary>
    public enum TimeUnit
    {
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4,
        Minute = 5,
        Hour = 6
    };

    /// <summary>
    ///     A DateRange class able to handle all ranges attainable with the DateTime structure.
    ///     The start date should precede the end date, if it does not, the dates are
    ///     automatically switched.
    ///     The Start and End dates can be null (=/-infinity).
    ///     When start date is null, it is equivalent to negative infinity. When end date
    ///     is null, it is equivalent to positive infinity.
    ///     Given two date ranges, it is able to calculate whether the ranges intersect
    ///     and what the range of intersection is between the two ranges.
    ///     Unlike the TimeSpan structure, this class retains the start and end date components
    ///     of the range (hence the choice for the term "Range" instead of "Span").
    ///     Original code :
    ///     http://huntjason.wordpress.com/2005/10/28/net-c-date-range-implementation-using-generics/
    /// </summary>
    [Serializable]
    public class DateRange : IEquatable<DateRange>
    {
        /// <summary>
        ///     The empty value. Indicates an empty or invalid DateRange.
        /// </summary>
        public static readonly DateRange EmptyValue = new DateRange(DateTime.MinValue, DateTime.MinValue);

        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
       
        /// <summary>
        ///     Creates a new data range object. If the endDate is before the startDate, the dates will be swapped.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate < endDate)
            {
                _startDate = startDate;
                _endDate = endDate;
            }
            else
            {
                _startDate = endDate;
                _endDate = startDate;
            }
        }

        /// <summary>
        ///     The TimeSpan of the DateRange.
        /// </summary>
        public TimeSpan TimeSpan
        {
            get { return _endDate - _startDate; }
        }

        /// <summary>
        /// The start or earliest data in the range.
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
        }

        /// <summary>
        /// The end or latest data in the range.
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
        }

        /// <summary>
        ///     Create a data create a date range spanning the day specified in <paramref name="dateTime" />.
        ///     Useful for selecting an entire day's data from a database.
        /// </summary>
        /// <param name="dateTime"></param>
        public static DateRange Day(DateTime dateTime)
        {
            return new DateRange(DateRanger.Day.StartOf(dateTime), DateRanger.Day.EndOf(dateTime));
        }

        /// <summary>
        ///     Create a data create a date range spanning the week specified in <paramref name="dateTime" />.
        ///     Useful for selecting an entire week's data from a database.
        /// </summary>
        /// <param name="dateTime"></param>
        public static DateRange Week(DateTime dateTime)
        {
            return new DateRange(DateRanger.Week.StartOf(dateTime), DateRanger.Week.EndOf(dateTime));
        }

        /// <summary>
        ///     Create a data create a date range spanning the month specified in <paramref name="dateTime" />.
        ///     Useful for selecting an entire month's data from a database.
        /// </summary>
        /// <param name="dateTime"></param>
        public static DateRange Month(DateTime dateTime)
        {
            return new DateRange(DateRanger.Month.StartOf(dateTime), DateRanger.Month.EndOf(dateTime));
        }

        /// <summary>
        ///     Create a data create a date range spanning the month specified in <paramref name="dateTime" />.
        ///     Useful for selecting an entire quarter's data from a database.
        /// </summary>
        /// <param name="dateTime"></param>
        public static DateRange Quarter(DateTime dateTime)
        {
            return new DateRange(DateRanger.Quarter.StartOf(dateTime), DateRanger.Quarter.EndOf(dateTime));
        }

        /// <summary>
        ///     Create a data create a date range spanning the month specified in <paramref name="dateTime" />.
        ///     Useful for selecting an entire year's data from a database.
        /// </summary>
        /// <param name="dateTime"></param>
        public static DateRange Year(DateTime dateTime)
        {
            return new DateRange(DateRanger.Year.StartOf(dateTime), DateRanger.Year.EndOf(dateTime));
        }

        /// <summary>
        ///     Given a starting DateTime go forward/backward from that point by the magnitude and
        ///     interval specified.
        /// </summary>
        /// <example>
        ///     GetDateRangeWithin(DateTime.Now, 5, Interval.Minute)
        ///     returns a DateRange starting now and ending 5 minutes in the future.
        ///     GetDateRangeWithin(DateTime.Now, -1, Interval.Hour)
        ///     returns a DateRange starting one(1) hour in the past and ending now.
        /// </example>
        /// <param name="refTime"></param>
        /// <param name="magnitude"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static DateRange DateRangeFrom(DateTime refTime, int magnitude, TimeUnit interval)
        {
            DateRange dateRange;

            switch (interval)
            {
                case TimeUnit.Minute:
                    dateRange = new DateRange(refTime, refTime.AddMinutes(magnitude));
                    break;
                case TimeUnit.Hour:
                    dateRange = new DateRange(refTime, refTime.AddHours(magnitude));
                    break;
                case TimeUnit.Day:
                    dateRange = new DateRange(refTime, refTime.AddDays(magnitude));
                    break;
                case TimeUnit.Month:
                    dateRange = new DateRange(refTime, refTime.AddMonths(magnitude));
                    break;
                case TimeUnit.Year:
                    dateRange = new DateRange(refTime, refTime.AddYears(magnitude));
                    break;
                case TimeUnit.Week:
                    dateRange = new DateRange(refTime, refTime.AddDays(magnitude*7));
                    break;
                default:
                    dateRange = EmptyValue;
                    break;
            }

            return dateRange;
        }

        /// <summary>
        ///     Enumerate each Date Time between this DateRange's StartDate and EndDate.
        ///     The stepBy Interval controls the span between each returned DateTime. The first
        ///     value returned by the enumeration is always the StartDate.
        ///     Starting date must not be null, as there is no way to increment from -infinity.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     If the StartDate is null (negative infinity) there is no way to increment time
        ///     from that point.  StartDate must not be null.
        /// </exception>
        /// <param name="stepBy"></param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        public IEnumerable<DateTime> EnumerateDateTimes(TimeUnit stepBy)
        {
            if (StartDate == null)
            {
                throw new InvalidOperationException("StartDate cannot be null");
            }

            var current = StartDate;

            while (current <= EndDate)
            {
                switch (stepBy)
                {
                    case TimeUnit.Minute:
                        yield return current;
                        current = current.AddMinutes(1);
                        break;
                    case TimeUnit.Hour:
                        yield return current;
                        current = current.AddHours(1);
                        break;
                    case TimeUnit.Day:
                        yield return current;
                        current = current.AddDays(1);
                        break;
                    case TimeUnit.Week:
                        yield return current;
                        current = current.AddDays(7);
                        break;
                    case TimeUnit.Month:
                        yield return current;
                        current = current.AddMonths(1);
                        break;
                    case TimeUnit.Year:
                        yield return current;
                        current = current.AddYears(1);
                        break;
                    default:
                        throw new NotImplementedException("This interval is not implemented");
                }
            }
        }

        /// <summary>
        ///     Enumerate each Date Time between this DateRange's StartDate and EndDate.
        ///     The stepBy Interval controls the span between each returned DateTime. The first
        ///     value returned by the enumeration is always the StartDate.
        ///     Starting date must not be null, as there is no way to increment from -infinity.
        /// </summary>
        /// <param name="stepBy"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EnumerateDateTimes(DateTime startDate, TimeUnit stepBy)
        {
            var current = startDate;

            switch (stepBy)
            {
                case TimeUnit.Minute:
                    while (true)
                    {
                        yield return current;
                        current = current.AddMinutes(1);
                    }
                case TimeUnit.Hour:
                    while (true)
                    {
                        yield return current;
                        current = current.AddHours(1);
                    }
                case TimeUnit.Day:
                    while (true)
                    {
                        yield return current;
                        current = current.AddDays(1);
                    }
                case TimeUnit.Week:
                    while (true)
                    {
                        yield return current;
                        current = current.AddDays(7);
                    }
                case TimeUnit.Month:
                    while (true)
                    {
                        yield return current;
                        current = current.AddMonths(1);
                    }
                case TimeUnit.Year:
                    while (true)
                    {
                        yield return current;
                        current = current.AddYears(1);
                    }
            }
        }

        public bool Intersects(DateRange other)
        {
            if ((other.EndDate < _startDate) ||
                ( other.StartDate > _endDate) ||
                ( _endDate < other.StartDate) ||
                ( _startDate > other.EndDate))
            {
                return false;
            }
            return true;
        }

        public bool Contains(DateTime other)
        {
            if ((_startDate <= other) && (_endDate >= other))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var str = _startDate  + " - " + _endDate;

            return str;
        }

        /// <summary>
        ///     Returns a string in the format: yyyy-MM-dd_yyyy-MM-dd
        ///     If either the start or end date is null (open) then the value will be blank.
        ///     If the range is 2010-02-01 to Infinity then the return value will be
        ///     2010-02-01_ to indicate that the end date is open (infinite).
        /// </summary>
        public string ToShortDateString()
        {
            var str = _startDate.ToString("yyyy-MM-dd") +
                         "_" +
                         _endDate.ToString("yyyy-MM-dd");

            return str;
        }

        /// <summary>
        ///     Try to parse string with standard search date range format: yyyy-MM-dd_yyyy-MM-dd
        /// </summary>
        /// <param name="dateRangeString"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public static bool TryParseShortDateRange(string dateRangeString, out DateRange dateRange)
        {
            string[] items = dateRangeString.Split('_');
            if (items.Length == 2)
            {
                // ex: yyyy-MM-dd_yyyy-MM-dd
                // should be 2 items with yyyy-MM-dd format
                DateTime startDate;
                DateTime endDate;
                if (TryGetDateFromDayKey(items[0], out startDate) && TryGetDateFromDayKey(items[1], out endDate))
                {
                    dateRange = new DateRange(startDate, endDate);

                    return true;
                }
            }

            dateRange = EmptyValue;
            return false;
        }

        private static bool TryGetDateFromDayKey(string dateInKeyFormat, out DateTime date)
        {
            return DateTime.TryParseExact(dateInKeyFormat, "yyyy-MM-dd", null, DateTimeStyles.None, out date);
        }

        public DateRange GetIntersection(DateRange other)
        {
            if (!Intersects(other)) throw new InvalidOperationException("DateRanges do not intersect");
            return new DateRange(LatestStartDate(other.StartDate), EarliestEndDate(other.EndDate));
        }

        private DateTime LatestStartDate(DateTime other)
        {
            return (DateTime.Compare(_startDate, other) >= 0) ? _startDate : other;
        }

        private DateTime EarliestEndDate(DateTime other)
        {
            return (DateTime.Compare(_endDate, other) >= 0) ? other : _endDate;
        }

        #region equality

        public bool Equals(DateRange other)
        {
            return ((_startDate == other.StartDate) && (_endDate == other.EndDate));
        }

        public override bool Equals(Object obj)
        {
            var range = obj as DateRange;
            if (range != null)
            {
                return ((_startDate == range.StartDate) && (_endDate == range.EndDate));
            }
            return false;
        }

        public override int GetHashCode()
        {
            // from the great Jon Skeet
            // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash*23 + _startDate.GetHashCode();
                hash = hash*23 + _endDate.GetHashCode();
                return hash;
            }
        }

        #endregion

        #region Predefined DateRanges

        /// <summary>
        ///     Yesterday range starting at 12:00:00 AM ending at 11:59:59 PM.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange Yesterday
        {
            get
            {
                var startDate = DateTime.Today.AddDays(-1);
                var endDate = DateTime.Today.AddMilliseconds(-1);

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Today range starting at 12:00:00 AM ending at 11:59:59 PM.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange Today
        {
            get
            {
                var startDate = DateTime.Today;
                var endDate = DateTime.Today.AddDays(1).AddMilliseconds(-1);

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Tomorrow range starting at 12:00:00 AM ending at 11:59:59 PM.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange Tomorrow
        {
            get
            {
                var startDate = DateTime.Today.AddDays(1);
                var endDate = DateTime.Today.AddDays(2).AddMilliseconds(-1);

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     This week range starting at 12:00:00 AM Sunday Morning ending at 11:59:59 PM Saturday Night.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange ThisWeek
        {
            get
            {
                var startDate = DateRanger.Week.StartOfCurrent();
                var endDate = DateRanger.Week.EndOfCurrent();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Last week range starting at 12:00:00 AM Sunday Morning ending at 11:59:59 PM Saturday Night.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange LastWeek
        {
            get
            {
                var startDate = DateRanger.Week.StartOfLast();
                var endDate = DateRanger.Week.EndOfLast();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Next week range starting at 12:00:00 AM Sunday Morning ending at 11:59:59 PM Saturday Night.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange NextWeek
        {
            get
            {
                var startDate = DateRanger.Week.StartOfNext();
                var endDate = DateRanger.Week.EndOfNext();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Last month's range starting at 12:00:00 AM 1st day of month ending at 11:59:59 PM last day of month.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange LastMonth
        {
            get
            {
                var startDate = DateRanger.Month.StartOfLast();
                var endDate = DateRanger.Month.EndOfLast();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     This month's range starting at 12:00:00 AM 1st day of month ending at 11:59:59 PM last day of month.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange ThisMonth
        {
            get
            {
                var startDate = DateRanger.Month.StartOfCurrent();
                var endDate = DateRanger.Month.EndOfCurrent();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Next month's range starting at 12:00:00 AM 1st day of month ending at 11:59:59 PM last day of month.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange NextMonth
        {
            get
            {
                var startDate = DateRanger.Month.StartOfNext();
                var endDate = DateRanger.Month.EndOfNext();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Last year's range starting at 12:00:00 AM 1st day of yead and ending at 11:59:59 PM last day of year.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange LastYear
        {
            get
            {
                var startDate = DateRanger.Year.StartOfLast();
                var endDate = DateRanger.Year.EndOfLast();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Next year's range starting at 12:00:00 AM 1st day of yead and ending at 11:59:59 PM last day of year.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange NextYear
        {
            get
            {
                var startDate = DateRanger.Year.StartOfNext();
                var endDate = DateRanger.Year.EndOfNext();

                return new DateRange(startDate, endDate);
            }
        }


        /// <summary>
        ///     This year's range starting at 12:00:00 AM 1st day of yead and ending at 11:59:59 PM last day of year.
        ///     Suitable for use with the SQL BETWEEN clause.
        /// </summary>
        public static DateRange ThisYear
        {
            get
            {
                var startDate = DateRanger.Year.StartOfCurrent();
                var endDate = DateRanger.Year.EndOfCurrent();

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Start at DateTime.MinValue. End at the moment that is 
        ///     right before <see cref="DateTime.Now" />
        ///     (DateTime.Now.AddMilliseconds(-1)).
        /// </summary>
        public static DateRange ThePast
        {
            get
            {
                var startDate = DateTime.MinValue;
                var endDate = DateTime.Now.AddMilliseconds(-1);

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Start at the moment that is right after <see cref="DateTime.Now" />
        ///     (DateTime.Now.AddMilliseconds(1)).
        ///     End at DateTime.MaxValue.
        /// </summary>
        public static DateRange TheFuture
        {
            get
            {
                var startDate = DateTime.Now.AddMilliseconds(1);
                var endDate = DateTime.MaxValue;

                return new DateRange(startDate, endDate);
            }
        }


        /// <summary>
        ///     Returns the maximum valid DateRange that is compatible with SqlDateTime type.
        /// </summary>
        public static DateRange MaxSqlDateRange
        {
            get
            {
                var startDate = SqlDateTime.MinValue.Value;
                var endDate = SqlDateTime.MaxValue.Value;

                return new DateRange(startDate, endDate);
            }
        }

        /// <summary>
        ///     Returns the DateRange that is compatible with SqlDateTime type.
        ///     Includes every valid DateTime in the past until now.
        /// </summary>
        public static DateRange PastSqlDateRange
        {
            get
            {
                var startDate = SqlDateTime.MinValue.Value;
                var endDate = DateTime.Now;

                return new DateRange(startDate, endDate);
            }
        }

        #endregion
    }
}