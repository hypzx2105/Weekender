using System.Globalization;
using Weekender.Helpers;

namespace Weekender.Tests
{
    public class DateHelperTests
    {
        private static DateTime Day(string isoDate)
        {
            return DateTime.ParseExact(isoDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        [Theory]
        [InlineData("2026-10-12", "2026-10-16")]
        [InlineData("2026-10-17", "2026-10-23")]
        public void GetNextFriday_ReturnsTheComingFriday(string today, string expectedFriday)
        {
            DateTime result = DateHelper.GetNextFriday(Day(today));

            Assert.Equal(Day(expectedFriday), result);
            Assert.Equal(DayOfWeek.Friday, result.DayOfWeek);
        }

        [Fact]
        public void ValidateTripDates_ReturnsError_WhenReturnIsNotAfterDeparture()
        {
            string? error = DateHelper.ValidateTripDates(Day("2026-10-18"), Day("2026-10-17"), Day("2026-10-15"));

            Assert.Equal("The return date must be after the departure date.", error);
        }

        [Theory]
        [InlineData("2026-10-30", true)]
        [InlineData("2026-10-31", false)]
        public void IsWithinForecastRange_ChecksTheSixteenDayLimit(string tripEnd, bool expected)
        {
            bool result = DateHelper.IsWithinForecastRange(Day(tripEnd), Day("2026-10-15"));

            Assert.Equal(expected, result);
        }
    }
}