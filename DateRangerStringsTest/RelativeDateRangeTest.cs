using System;
using DateRanger;
using Xunit;


namespace DateRangerTest
{
    public class RelativeDateRangeTest
    {
        [Fact]
        public void TryParseTest()
        {
            const string stringToParse = "14 Days Ago_Now";
            var now = DateTime.Now;

            RelativeDateRange outputRelDateRange;
            var tryParseSucceeded = RelativeDateRange.TryParse(stringToParse, out outputRelDateRange);

            Assert.True(tryParseSucceeded);
            Assert.Equal(now.Day, outputRelDateRange.StartDate.GetDateTime().AddDays(14).Day);
            Assert.Equal(now.Day, outputRelDateRange.EndDate.GetDateTime().Day);
        }

        [Fact]
        public void TryParse7DaysTest()
        {
            const string stringToParse = "7 Days Ago_Now";
            var now = DateTime.Now;

            RelativeDateRange outputRelDateRange;
            var tryParseSucceeded = RelativeDateRange.TryParse(stringToParse, out outputRelDateRange);

            Assert.True(tryParseSucceeded);
            Assert.Equal(now.Day, outputRelDateRange.StartDate.GetDateTime().AddDays(7).Day);
            Assert.Equal(now.Day, outputRelDateRange.EndDate.GetDateTime().Day);
        }
    }
}