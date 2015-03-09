using System;
using DateRanger;
using Xunit;

namespace DateRangerTest
{
    public class DayTests
    {
        [Fact()]
        public void TestStartOf()
        {
            var now = DateTime.Now;
            var want = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            var got = Day.StartOf(now);
            Assert.Equal(want, got);
        }

        [Fact()]
        public void TestEndOf()
        {
            var now = DateTime.Now;
            var want = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);
            var got = Day.EndOf(now);
            Assert.Equal(want, got);
        }
    }
}
