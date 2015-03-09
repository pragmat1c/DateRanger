using System;

namespace DateRanger
{

    /// <summary>
    /// A date range bounded by RelativeDateTimes.
    /// </summary>
    [Serializable]
    public class RelativeDateRange 
    {
        readonly RelativeDateTime _startDate;
        readonly RelativeDateTime _endDate;

        /// <summary>
        /// Set <see cref="StartDate"/> and  <see cref="EndDate"/>. If EndDate is before
        /// StartDate, the start date and end dates will be swapped, so the earlier date
        /// is always the start date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public RelativeDateRange(RelativeDateTime startDate, RelativeDateTime endDate)
        {
            if (startDate == null) throw new ArgumentNullException("startDate");
            if (endDate == null) throw new ArgumentNullException("endDate");
            
            if (startDate.GetDateTime() < endDate.GetDateTime())
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

        public TimeSpan TimeSpan
        {
            get { return _endDate.GetDateTime() - _startDate.GetDateTime(); }
        }

        public RelativeDateTime StartDate
        {
            get { return _startDate; }
        }

        public RelativeDateTime EndDate
        {
            get { return _endDate; }
        }

        public DateRange ToDateRange()
        {
            var start = StartDate.GetDateTime();
            var end = EndDate.GetDateTime();
            return new DateRange(start, end);
        }

        public override string ToString()
        {
            var str = (_startDate != null ? _startDate.ToString() : "-INFINITY") +
                " - " +
                (_endDate != null ? _endDate.ToString() : "INFINITY");

            return str;
        }

        /// <summary>
        /// Try to parse string with standard search date range format: 
        /// now_start of last month
        /// </summary>
        /// <param name="dateRangeString"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public static bool TryParse(string dateRangeString, out RelativeDateRange dateRange)
        {
            var items = dateRangeString.Split('_');
            if (items.Length == 2)
            {
                // ex: now_start of last month
                // should be 2 items parsable into RelativeDateTimes
                RelativeDateTime startDate;
                RelativeDateTime endDate;
                if (TryParseRelativeDate(items[0], out startDate) &&
                    TryParseRelativeDate(items[1], out endDate))
                {
                    dateRange = new RelativeDateRange(startDate, endDate);

                    return true;
                }
            }

            dateRange = null;
            return false;
        }

        private static bool TryParseRelativeDate(string p, out RelativeDateTime relDateTime)
        {
            RelativeDateTime testRelDateTime;
            if (RelativeDateTime.TryParse(p, out testRelDateTime))
            {
                relDateTime = testRelDateTime;
                return true;
            }
            relDateTime = null;
            return false;
        }
    }
}
