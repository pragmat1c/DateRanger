using DateRanger;
using Xunit;

namespace DateRangerTest
{
    public class QuarterTest
    {
        [Fact]
        public void ParseTest()
        {
            var badStrings = new[]
            {
                "2014QQ",
                "2014-1"
            };


            Quarter quarter;
            foreach (var s in badStrings)
            {
                Assert.False(Quarter.TryParse(s, out quarter));
            }

            var goodStrings = new[]
            {
                "2014Q1",
                "2009Q4"
            };
            foreach (var s in goodStrings)
            {
                Assert.True(Quarter.TryParse(s, out quarter));
            }
        }
    }
}