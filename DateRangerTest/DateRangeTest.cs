using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DateRanger;
using Xunit;


namespace DateRangerTest
{
   public class DateRangeTest
    {
        [Fact]
        public void GetDateRangeTest()
        {
            var refTime = DateTime.Now;
            const int magnitude = 1;
            const TimeUnit interval = TimeUnit.Minute;
            var actual = DateRange.DateRangeFrom(refTime, magnitude, interval);
            Assert.NotNull(actual);
        }

        [Fact]
        public void GetDateRangeTestMinutes()
        {
            var refTime = DateTime.Now;
            DateRangeTestVaryBy(refTime, TimeUnit.Minute, refTime.AddMinutes);
        }

        [Fact]
        public void GetDateRangeTestHours()
        {
            DateTime refTime = DateTime.Now;
            DateRangeTestVaryBy(refTime, TimeUnit.Hour, refTime.AddHours);
        }

        [Fact]
        public void GetDateRangeTestDays()
        {
            DateTime refTime = DateTime.Now;
            DateRangeTestVaryBy(refTime, TimeUnit.Day, refTime.AddDays);
        }


        [Fact]
        public void GetDateRangeTestMonths()
        {
            DateTime refTime = DateTime.Now;
            DateRangeTestVaryByInt(refTime, TimeUnit.Month, refTime.AddMonths);
        }

        [Fact]
        public void GetDateRangeTestYears()
        {
            DateTime refTime = DateTime.Now;
            DateRangeTestVaryByInt(refTime, TimeUnit.Year, refTime.AddYears);
        }

        private static void DateRangeTestVaryByInt(DateTime refTime, TimeUnit interval, Func<int, DateTime> dateFunc)
        {
            DateRangeTestVaryBy(refTime, interval, x => dateFunc(Convert.ToInt32(x)));
        }


        private static void DateRangeTestVaryBy(DateTime refTime, TimeUnit interval, Func<double, DateTime> dateFunc)
        {
            for (int magnitude = 1; magnitude < 1000; magnitude++)
            {
                var expected = new DateRange(refTime, dateFunc(magnitude));
                DateRange actual = DateRange.DateRangeFrom(refTime, magnitude, interval);

                Assert.True(expected.Equals(actual));
            }

            for (int magnitude = -1; magnitude > -1000; magnitude--)
            {
                var expected = new DateRange(dateFunc(magnitude), refTime);
                DateRange actual = DateRange.DateRangeFrom(refTime, magnitude, interval);

                Assert.True(expected.Equals(actual));
            }
        }

        /// <summary>
        ///     A test for Yesterday
        /// </summary>
        [Fact]
        public void YesterdayTest()
        {
            DateRange actual = DateRange.Yesterday;

            Assert.NotNull(actual.StartDate);

            Assert.Equal(DateTime.Today.AddDays(-1), actual.StartDate);
            Assert.Equal(DateTime.Today.AddMilliseconds(-1), actual.EndDate);

            Assert.True(actual.EndDate < DateTime.Today);
            Assert.True(actual.StartDate < DateTime.Today);
        }

        /// <summary>
        ///     A test for Today
        /// </summary>
        [Fact]
        public void TodayTest()
        {
            DateRange actual = DateRange.Today;

            Assert.NotNull(actual.StartDate);

            Assert.Equal(DateTime.Today, actual.StartDate);
            Assert.Equal(DateTime.Today.AddDays(1).AddMilliseconds(-1), actual.EndDate);

            Assert.True(actual.EndDate > DateTime.Today);
            Assert.True(actual.StartDate == DateTime.Today);
        }

        /// <summary>
        ///     A test for Tomorrow
        /// </summary>
        [Fact]
        public void TomorrowTest()
        {
            DateRange actual = DateRange.Tomorrow;

            Assert.NotNull(actual.StartDate);

            Assert.Equal(DateTime.Today.AddDays(1), actual.StartDate);
            Assert.Equal(DateTime.Today.AddDays(2).AddMilliseconds(-1), actual.EndDate);

            Assert.True(actual.EndDate > DateTime.Today);
        }

        /// <summary>
        ///     A test for ThisWeek
        /// </summary>
        [Fact]
        public void ThisWeekTest()
        {
            DayOfWeek day = DateTime.Today.DayOfWeek;
            int days = day - DayOfWeek.Sunday;
            DateTime start = DateTime.Today.AddDays(-days);
            DateTime end = start.AddDays(7).AddMilliseconds(-1);

            var actual = DateRange.ThisWeek;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for LastWeek
        /// </summary>
        [Fact]
        public void LastWeekTest()
        {
            DayOfWeek day = DateTime.Today.DayOfWeek;
            int days = day - DayOfWeek.Sunday;
            DateTime start = DateTime.Today.AddDays(-days).AddDays(-7);
            DateTime end = start.AddDays(7).AddMilliseconds(-1);

            DateRange actual;
            actual = DateRange.LastWeek;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for NextWeek
        /// </summary>
        [Fact]
        public void NextWeekTest()
        {
            DayOfWeek day = DateTime.Today.DayOfWeek;
            int days = day - DayOfWeek.Sunday;
            DateTime start = DateTime.Today.AddDays(-days).AddDays(7);
            DateTime end = start.AddDays(7).AddMilliseconds(-1);

            DateRange actual;
            actual = DateRange.NextWeek;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for ThisMonth
        /// </summary>
        [Fact]
        public void ThisMonthTest()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            var start = new DateTime(year, month, 1);

            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month))
                .AddDays(1)
                .AddMilliseconds(-1);

            DateRange actual = DateRange.ThisMonth;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for NextMonth
        /// </summary>
        [Fact]
        public void NextMonthTest()
        {
            DateTime now = DateTime.Today;

            DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(1);

            int year = start.Year;
            int month = start.Month;

            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month))
                .AddDays(1)
                .AddMilliseconds(-1);

            DateRange actual = DateRange.NextMonth;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for LastMonth
        /// </summary>
        [Fact]
        public void LastMonthTest()
        {
            DateTime now = DateTime.Today;

            DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(-1);

            int year = start.Year;
            int month = start.Month;

            DateTime end = new DateTime(year, month, DateTime.DaysInMonth(year, month))
                .AddDays(1)
                .AddMilliseconds(-1);

            DateRange actual = DateRange.LastMonth;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for ThisYear
        /// </summary>
        [Fact]
        public void ThisYearTest()
        {
            int year = DateTime.Today.Year;

            var start = new DateTime(year, 1, 1);


            DateTime end = new DateTime(year, 12, DateTime.DaysInMonth(year, 12))
                .AddDays(1)
                .AddMilliseconds(-1);

            DateRange actual = DateRange.ThisYear;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for LastYear
        /// </summary>
        [Fact]
        public void LastYearTest()
        {
            int year = DateTime.Today.AddYears(-1).Year;

            var start = new DateTime(year, 1, 1);

            DateTime end = new DateTime(year, 12, DateTime.DaysInMonth(year, 12))
                .AddDays(1)
                .AddMilliseconds(-1);

            DateRange actual = DateRange.LastYear;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }

        /// <summary>
        ///     A test for NextYear
        /// </summary>
        [Fact]
        public void NextYearTest()
        {
            int year = DateTime.Today.AddYears(1).Year;

            var start = new DateTime(year, 1, 1);

            DateTime end = new DateTime(year, 12, DateTime.DaysInMonth(year, 12))
                .AddDays(1)
                .AddMilliseconds(-1);

            DateRange actual = DateRange.NextYear;

            Trace.WriteLine(actual);

            Assert.Equal(start, actual.StartDate);
            Assert.Equal(end, actual.EndDate);
        }


        /// <summary>
        ///     A test for Intersects
        /// </summary>
        [Fact]
        public void IntersectsTest()
        {
            DateRange target = DateRange.ThisWeek;
            DateRange other = DateRange.Yesterday;

            Assert.True(target.Intersects(other));

            DateRange t2 = DateRange.ThisWeek;
            DateRange o2 = DateRange.NextWeek;

            Assert.False(t2.Intersects(o2));
        }

        /// <summary>
        ///     A test for Contains
        /// </summary>
        [Fact]
        public void ContainsTest()
        {
            DateRange target = DateRange.ThisWeek;
            DateTime now = DateTime.Now;

            Assert.True(target.Contains(now));

            DateRange o2 = DateRange.NextWeek;

            Assert.False(o2.Contains(now));

            var negInf = new DateRange(DateTime.MinValue, now.AddDays(1));

            Assert.True(negInf.Contains(now));

            var posInf = new DateRange(now.AddDays(-1), DateTime.MaxValue);

            Assert.True(posInf.Contains(now));

            var futureInf = new DateRange(now.AddDays(10), DateTime.MaxValue);

            Assert.False(futureInf.Contains(now));
        }

        /// <summary>
        ///     A test for DateRange Constructor
        /// </summary>
        [Fact]
        public void DateRangeConstructorTest()
        {
            var target = new DateRange(DateTime.MinValue, DateTime.MaxValue);
            
            Assert.Equal(target.StartDate, DateTime.MinValue);
            Assert.Equal(target.EndDate, DateTime.MaxValue);
            
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void EnumerateDateTimesByMinutesTest()
        {
            DateRange target = DateRange.Today;
            const TimeUnit stepBy = TimeUnit.Minute;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            Assert.Equal(1440, actual.Count());
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void EnumerateDateTimesByHoursTest()
        {
            DateRange target = DateRange.Today;
            const TimeUnit stepBy = TimeUnit.Hour;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            Assert.Equal(24, actual.Count());
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void EnumerateDateTimesByDaysTest()
        {
            DateRange target = DateRange.ThisWeek;
            const TimeUnit stepBy = TimeUnit.Day;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            Assert.Equal(7, actual.Count());
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void EnumerateDateTimesByWeeksTest()
        {
            DateRange target = DateRange.ThisYear;
            const TimeUnit stepBy = TimeUnit.Week;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            // 53 entries b/c last week wil only be a partial (so 52 and some fraction of a week)
            Assert.Equal(53, actual.Count());
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void EnumerateDateTimesByMonthsTest()
        {
            DateRange target = DateRange.ThisYear;
            const TimeUnit stepBy = TimeUnit.Month;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            Assert.Equal(12, actual.Count());
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void SameDateTest()
        {
            var target = new DateRange(Month.StartOfCurrent(), Month.StartOfCurrent());
            const TimeUnit stepBy = TimeUnit.Month;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            Assert.Equal(1, actual.Count());
        }

        /// <summary>
        ///     A test for EnumerateDateTimes
        /// </summary>
        [Fact]
        public void EnumerateDateTimesByYearsTest()
        {
            var target = new DateRange(new DateTime(2001, 1, 1), new DateTime(2010, 12, 31));
            var stepBy = TimeUnit.Year;

            IEnumerable<DateTime> actual = target.EnumerateDateTimes(stepBy);

            Assert.Equal(10, actual.Count());
        }

        [Fact]
        public void ToShortDateStringTest()
        {
            DateRange infiniteDateRange = DateRange.EmptyValue;

            Assert.Equal("0001-01-01_0001-01-01", infiniteDateRange.ToShortDateString());

            DateTime now = DateTime.Now;
            string nowAsString = now.ToString("yyyy-MM-dd");

            var openStartDateRange = new DateRange(DateTime.MinValue, now);

            Assert.Equal("0001-01-01_" + nowAsString, openStartDateRange.ToShortDateString());

            var openEndDateRange = new DateRange(now, DateTime.MaxValue);

            Assert.Equal(nowAsString + "_9999-12-31", openEndDateRange.ToShortDateString());

            var singleDateRange = new DateRange(now, now);

            Assert.Equal(nowAsString + "_" + nowAsString, singleDateRange.ToShortDateString());
        }

        [Fact]
        public void TryParseShortDateStringTest()
        {
            DateTime now = DateTime.Now;

            var today = new DateTime(now.Year, now.Month, now.Day);
            string nowAsString = today.ToString("yyyy-MM-dd");

            var singleDateRange = new DateRange(today, today);

            DateRange outputDateRange;
            bool tryparseResult = DateRange.TryParseShortDateRange(nowAsString + "_" + nowAsString, out outputDateRange);

            Assert.True(tryparseResult);

            Assert.Equal(singleDateRange, outputDateRange);
        }
    }
}